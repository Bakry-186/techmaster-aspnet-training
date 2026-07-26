using EfCoreDrills.Api.Data;
using EfCoreDrills.Api.DTOs;
using EfCoreDrills.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDrills.Api.Services;

public class TrackService(AppDbContext context)
{
    public async Task<TrackDetailsDto?> GetByIdAsync(int id)
    {
        return await context.TrainingTracks
            .AsNoTracking()
            .Where(track => track.Id == id && !track.IsDeleted)
            .Select(track => new TrackDetailsDto
            {
                Id = track.Id,
                Title = track.Title,
                InstructorName = track.Instructor.FullName,
                EnrolledStudentCount = track.Enrollments.Count,
                CreatedAt = track.CreatedAt,
                UpdatedAt = track.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<(TrackDetailsDto? Track, string? Error)> CreateAsync(CreateTrackRequest request)
    {
        var instructorExists = await context.Instructors.AnyAsync(i => i.Id == request.InstructorId);
        if (!instructorExists)
        {
            return (null, "Instructor not found.");
        }

        var track = new TrainingTrack
        {
            Title = request.Title,
            InstructorId = request.InstructorId,
            CreatedAt = DateTime.UtcNow
        };

        context.TrainingTracks.Add(track);
        await context.SaveChangesAsync();

        var details = await GetByIdAsync(track.Id);
        return (details, null);
    }
}
