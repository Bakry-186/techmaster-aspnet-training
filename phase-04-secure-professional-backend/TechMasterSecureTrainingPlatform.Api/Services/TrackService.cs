using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class TrackService(AppDbContext context)
{
    public async Task<(IReadOnlyList<TrackListItemResponse>? Data, string? Error)> GetAllAsync(
        string? keyword, string? level, string? status, int? instructorId)
    {
        var filterError = FilterValidation.ValidateTrackFilters(level, status);
        if (filterError is not null) return (null, filterError);

        var query = context.TrainingTracks.AsNoTracking()
            .Where(t => !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var term = keyword.Trim().ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(term) ||
                t.Code.ToLower().Contains(term) ||
                (t.Description != null && t.Description.ToLower().Contains(term)));
        }
        if (!string.IsNullOrWhiteSpace(level) && Enum.TryParse<TrackLevel>(level, true, out var parsedLevel))
            query = query.Where(t => t.Level == parsedLevel);
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TrackStatus>(status, true, out var parsedStatus))
            query = query.Where(t => t.Status == parsedStatus);
        if (instructorId.HasValue)
            query = query.Where(t => t.InstructorId == instructorId.Value);

        return (await query.OrderBy(t => t.TrainingTrackId)
            .Select(t => new TrackListItemResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Code = t.Code,
                Level = t.Level.ToString(),
                Status = t.Status.ToString(),
                Capacity = t.Capacity,
                InstructorName = t.Instructor.FullName
            }).ToListAsync(), null);
    }

    public async Task<TrackDetailsResponse?> GetByIdAsync(int id)
    {
        var track = await context.TrainingTracks.AsNoTracking()
            .Include(t => t.Enrollments)
            .FirstOrDefaultAsync(t => t.TrainingTrackId == id && !t.IsDeleted);
        if (track is null) return null;

        var active = EnrollmentHelper.CountActiveEnrollments(track);
        return new TrackDetailsResponse
        {
            TrainingTrackId = track.TrainingTrackId,
            Title = track.Title,
            Code = track.Code,
            Description = track.Description,
            Level = track.Level.ToString(),
            Status = track.Status.ToString(),
            Capacity = track.Capacity,
            InstructorName = (await context.Instructors.AsNoTracking()
                .Where(i => i.InstructorId == track.InstructorId)
                .Select(i => i.FullName).FirstAsync()),
            StartDate = track.StartDate,
            EndDate = track.EndDate,
            ActiveEnrollments = active,
            RemainingSeats = Math.Max(0, track.Capacity - active)
        };
    }

    public async Task<(TrackDetailsResponse? Data, string? Error)> CreateAsync(CreateTrackRequest request)
    {
        var error = await ValidateTrackAsync(request.Title, request.Code, request.Level,
            request.Capacity, request.Fee, request.StartDate, request.EndDate, request.InstructorId, null);
        if (error is not null) return (null, error);

        var track = new TrainingTrack
        {
            Title = request.Title,
            Code = request.Code,
            Description = request.Description,
            Level = Enum.Parse<TrackLevel>(request.Level, true),
            Capacity = request.Capacity,
            Fee = request.Fee,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            InstructorId = request.InstructorId,
            Status = TrackStatus.Open,
            CreatedAt = DateTime.UtcNow
        };
        context.TrainingTracks.Add(track);
        await context.SaveChangesAsync();
        return (await GetByIdAsync(track.TrainingTrackId), null);
    }

    public async Task<(TrackDetailsResponse? Data, string? Error)> UpdateAsync(int id, UpdateTrackRequest request)
    {
        var track = await context.TrainingTracks.FirstOrDefaultAsync(t => t.TrainingTrackId == id && !t.IsDeleted);
        if (track is null) return (null, "Track not found.");

        var error = await ValidateTrackAsync(request.Title, request.Code, request.Level,
            request.Capacity, request.Fee, request.StartDate, request.EndDate, request.InstructorId, id);
        if (error is not null) return (null, error);
        if (!Enum.TryParse<TrackStatus>(request.Status, true, out var status))
            return (null, "Invalid track status.");

        track.Title = request.Title;
        track.Code = request.Code;
        track.Description = request.Description;
        track.Level = Enum.Parse<TrackLevel>(request.Level, true);
        track.Capacity = request.Capacity;
        track.Fee = request.Fee;
        track.StartDate = request.StartDate;
        track.EndDate = request.EndDate;
        track.Status = status;
        track.InstructorId = request.InstructorId;
        await context.SaveChangesAsync();
        return (await GetByIdAsync(id), null);
    }

    public async Task<(bool Success, string? Error)> SoftDeleteAsync(int id)
    {
        var track = await context.TrainingTracks
            .Include(t => t.Enrollments)
            .FirstOrDefaultAsync(t => t.TrainingTrackId == id && !t.IsDeleted);
        if (track is null) return (false, "Track not found.");
        if (track.Enrollments.Any(e => EnrollmentHelper.IsActiveEnrollment(e.Status)))
            return (false, "Cannot delete track with active enrollments.");

        track.IsDeleted = true;
        track.DeletedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return (true, null);
    }

    private async Task<string?> ValidateTrackAsync(
        string title, string code, string level, int capacity, decimal fee,
        DateTime startDate, DateTime endDate, int instructorId, int? excludeId)
    {
        if (string.IsNullOrWhiteSpace(title)) return "Title is required.";
        if (fee <= 0) return "Fee must be greater than 0.";
        if (capacity <= 0) return "Capacity must be greater than 0.";
        if (startDate >= endDate) return "StartDate must be before EndDate.";
        if (!await context.Instructors.AnyAsync(i => i.InstructorId == instructorId))
            return "Instructor is required.";
        if (!Enum.TryParse<TrackLevel>(level, true, out _))
            return "Invalid level.";
        if (await context.TrainingTracks.AnyAsync(t =>
                t.Code == code && !t.IsDeleted && (excludeId == null || t.TrainingTrackId != excludeId)))
            return "Code must be unique.";
        return null;
    }
}
