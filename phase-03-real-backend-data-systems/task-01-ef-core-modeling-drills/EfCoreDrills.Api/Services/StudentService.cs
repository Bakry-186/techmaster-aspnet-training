using EfCoreDrills.Api.Common;
using EfCoreDrills.Api.Data;
using EfCoreDrills.Api.DTOs;
using EfCoreDrills.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDrills.Api.Services;

public class StudentService(AppDbContext context)
{
    public async Task<PaginationResult<StudentListItemDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        bool includeDeleted = false)
    {
        var query = context.Students.AsQueryable();

        if (!includeDeleted)
        {
            query = query.Where(student => !student.IsDeleted);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(student => student.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(student => new StudentListItemDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                IsActive = student.IsActive,
                CreatedAt = student.CreatedAt
            })
            .ToListAsync();

        return new PaginationResult<StudentListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<StudentDetailsDto?> GetByIdAsync(int id)
    {
        return await context.Students
            .AsNoTracking()
            .Where(student => student.Id == id && !student.IsDeleted)
            .Select(student => new StudentDetailsDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                IsActive = student.IsActive,
                CreatedAt = student.CreatedAt,
                UpdatedAt = student.UpdatedAt,
                Profile = student.Profile == null
                    ? null
                    : new StudentProfileDto
                    {
                        NationalId = student.Profile.NationalId,
                        Address = student.Profile.Address,
                        EmergencyPhone = student.Profile.EmergencyPhone,
                        DateOfBirth = student.Profile.DateOfBirth
                    },
                Enrollments = student.Enrollments
                    .Select(enrollment => new EnrollmentSummaryDto
                    {
                        EnrollmentId = enrollment.Id,
                        TrackTitle = enrollment.TrainingTrack.Title,
                        Status = enrollment.Status.ToString(),
                        EnrollmentDate = enrollment.EnrollmentDate
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StudentDetailsDto?> CreateAsync(CreateStudentRequest request)
    {
        var student = new Student
        {
            FullName = request.FullName,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();

        return await GetByIdAsync(student.Id);
    }

    public async Task<StudentDetailsDto?> UpdateAsync(int id, UpdateStudentRequest request)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (student is null)
        {
            return null;
        }

        student.FullName = request.FullName;
        student.Email = request.Email;
        student.IsActive = request.IsActive;
        student.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return await GetByIdAsync(student.Id);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        if (student is null)
        {
            return false;
        }

        student.IsDeleted = true;
        student.DeletedAt = DateTime.UtcNow;
        student.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }
}
