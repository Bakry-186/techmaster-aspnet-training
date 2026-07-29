using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class InstructorService(AppDbContext context)
{
    public async Task<IReadOnlyList<InstructorResponse>> GetAllAsync() =>
        await context.Instructors.AsNoTracking()
            .OrderBy(i => i.InstructorId)
            .Select(i => new InstructorResponse
            {
                InstructorId = i.InstructorId,
                FullName = i.FullName,
                Email = i.Email,
                Specialization = i.Specialization,
                Bio = i.Bio,
                IsActive = i.IsActive
            }).ToListAsync();

    public async Task<InstructorResponse?> GetByIdAsync(int id) =>
        await context.Instructors.AsNoTracking()
            .Where(i => i.InstructorId == id)
            .Select(i => new InstructorResponse
            {
                InstructorId = i.InstructorId,
                FullName = i.FullName,
                Email = i.Email,
                Specialization = i.Specialization,
                Bio = i.Bio,
                IsActive = i.IsActive
            }).FirstOrDefaultAsync();

    public async Task<(InstructorResponse? Data, string? Error)> CreateAsync(CreateInstructorRequest request)
    {
        if (await context.Instructors.AnyAsync(i => i.Email == request.Email))
            return (null, "Email must be unique.");

        var instructor = new Instructor
        {
            FullName = request.FullName,
            Email = request.Email,
            Specialization = request.Specialization,
            Bio = request.Bio,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        context.Instructors.Add(instructor);
        await context.SaveChangesAsync();
        return (await GetByIdAsync(instructor.InstructorId), null);
    }

    public async Task<(InstructorResponse? Data, string? Error)> UpdateAsync(int id, UpdateInstructorRequest request)
    {
        var instructor = await context.Instructors.FirstOrDefaultAsync(i => i.InstructorId == id);
        if (instructor is null) return (null, "Instructor not found.");
        if (await context.Instructors.AnyAsync(i => i.Email == request.Email && i.InstructorId != id))
            return (null, "Email must be unique.");

        instructor.FullName = request.FullName;
        instructor.Email = request.Email;
        instructor.Specialization = request.Specialization;
        instructor.Bio = request.Bio;
        instructor.IsActive = request.IsActive;
        await context.SaveChangesAsync();
        return (await GetByIdAsync(id), null);
    }

    public async Task<IReadOnlyList<TrackListItemResponse>> GetTracksAsync(int id) =>
        await context.TrainingTracks.AsNoTracking()
            .Where(t => t.InstructorId == id && !t.IsDeleted)
            .Select(t => new TrackListItemResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Code = t.Code,
                Level = t.Level.ToString(),
                Status = t.Status.ToString(),
                Capacity = t.Capacity,
                InstructorName = t.Instructor.FullName
            }).ToListAsync();
}
