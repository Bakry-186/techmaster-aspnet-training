using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class StudentService(AppDbContext context)
{
    public async Task<PaginationResult<StudentListItemResponse>> GetPagedAsync(
        string? search, bool? isActive, bool includeDeleted,
        int pageNumber, int pageSize)
    {
        var query = context.Students.AsNoTracking().AsQueryable();
        if (!includeDeleted) query = query.Where(s => !s.IsDeleted);
        if (isActive.HasValue) query = query.Where(s => s.IsActive == isActive.Value);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s =>
                s.FullName.ToLower().Contains(term) ||
                s.Email.ToLower().Contains(term) ||
                (s.PhoneNumber != null && s.PhoneNumber.Contains(term)));
        }

        var total = await query.CountAsync();
        var items = await query.OrderBy(s => s.StudentId)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .Select(s => new StudentListItemResponse
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsActive = s.IsActive
            }).ToListAsync();

        return new PaginationResult<StudentListItemResponse>
        {
            Items = items, TotalCount = total, PageNumber = pageNumber,
            PageSize = pageSize, TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public async Task<StudentDetailsResponse?> GetByIdAsync(int id)
    {
        return await context.Students.AsNoTracking()
            .Where(s => s.StudentId == id && !s.IsDeleted)
            .Select(s => new StudentDetailsResponse
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                ActiveEnrollmentCount = s.Enrollments.Count(e =>
                    e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active)
            }).FirstOrDefaultAsync();
    }

    public async Task<(StudentDetailsResponse? Data, string? Error)> CreateAsync(CreateStudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return (null, "FullName is required.");
        if (await context.Students.AnyAsync(s => s.Email == request.Email && !s.IsDeleted))
            return (null, "Email must be unique.");

        var student = new Student
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return (await GetByIdAsync(student.StudentId), null);
    }

    public async Task<(StudentDetailsResponse? Data, string? Error)> UpdateAsync(int id, UpdateStudentRequest request)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.StudentId == id && !s.IsDeleted);
        if (student is null) return (null, "Student not found.");
        if (await context.Students.AnyAsync(s => s.Email == request.Email && s.StudentId != id && !s.IsDeleted))
            return (null, "Email must be unique.");

        student.FullName = request.FullName;
        student.Email = request.Email;
        student.PhoneNumber = request.PhoneNumber;
        student.IsActive = request.IsActive;
        student.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return (await GetByIdAsync(id), null);
    }

    public async Task<(bool Success, string? Error)> SoftDeleteAsync(int id)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.StudentId == id && !s.IsDeleted);
        if (student is null) return (false, "Student not found.");
        student.IsDeleted = true;
        student.DeletedAt = DateTime.UtcNow;
        student.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return (true, null);
    }
}
