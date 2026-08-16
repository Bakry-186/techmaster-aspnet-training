using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class InstructorPortalService(
    AppDbContext context,
    ICurrentUserService currentUser,
    EnrollmentService enrollmentService,
    AuditService auditService)
{
    public async Task<IReadOnlyList<TrackListItemResponse>> GetMyTracksAsync()
    {
        var instructorId = await ResolveInstructorIdAsync();
        if (instructorId is null) return [];

        return await context.TrainingTracks.AsNoTracking()
            .Include(t => t.Instructor)
            .Where(t => !t.IsDeleted && t.InstructorId == instructorId)
            .OrderBy(t => t.Title)
            .Select(t => new TrackListItemResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Code = t.Code,
                Level = t.Level.ToString(),
                Status = t.Status.ToString(),
                Capacity = t.Capacity,
                InstructorName = t.Instructor.FullName
            })
            .ToListAsync();
    }

    public async Task<(IReadOnlyList<TrackStudentResponse>? Data, string? Error)> GetTrackStudentsAsync(int trackId)
    {
        if (!await OwnsTrackAsync(trackId))
            return (null, "You can only view students in your assigned tracks.");

        if (await context.TrainingTracks.AsNoTracking().AllAsync(t => t.TrainingTrackId != trackId))
            return (null, "Track not found.");

        var students = await enrollmentService.GetStudentsByTrackAsync(trackId);
        return (students, null);
    }

    public async Task<(TrackSessionResponse? Data, string? Error)> CreateSessionAsync(int trackId, CreateTrackSessionRequest request)
    {
        if (!await OwnsTrackAsync(trackId))
            return (null, "You can only manage sessions for your assigned tracks.");

        if (string.IsNullOrWhiteSpace(request.Title))
            return (null, "Session title is required.");

        if (request.DurationMinutes <= 0)
            return (null, "Duration must be greater than zero.");

        var session = new TrackSession
        {
            TrainingTrackId = trackId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            SessionDate = request.SessionDate,
            DurationMinutes = request.DurationMinutes,
            CreatedAt = DateTime.UtcNow
        };

        context.TrackSessions.Add(session);
        await context.SaveChangesAsync();
        await auditService.LogAsync("CreateSession", "TrackSession", session.TrackSessionId,
            $"Session created for track {trackId}.");

        return (MapSession(session), null);
    }

    public async Task<(TrackSessionResponse? Data, string? Error)> UpdateSessionAsync(int sessionId, UpdateTrackSessionRequest request)
    {
        var session = await context.TrackSessions
            .Include(s => s.TrainingTrack)
            .FirstOrDefaultAsync(s => s.TrackSessionId == sessionId);

        if (session is null)
            return (null, "Session not found.");

        if (!await OwnsTrackAsync(session.TrainingTrackId))
            return (null, "You can only manage sessions for your assigned tracks.");

        if (string.IsNullOrWhiteSpace(request.Title))
            return (null, "Session title is required.");

        if (request.DurationMinutes <= 0)
            return (null, "Duration must be greater than zero.");

        session.Title = request.Title.Trim();
        session.Description = request.Description?.Trim();
        session.SessionDate = request.SessionDate;
        session.DurationMinutes = request.DurationMinutes;
        session.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await auditService.LogAsync("UpdateSession", "TrackSession", session.TrackSessionId,
            $"Session updated for track {session.TrainingTrackId}.");

        return (MapSession(session), null);
    }

    public async Task<(TrackProgressResponse? Data, string? Error)> GetTrackProgressAsync(int trackId)
    {
        if (!await OwnsTrackAsync(trackId))
            return (null, "You can only view progress for your assigned tracks.");

        var track = await context.TrainingTracks.AsNoTracking()
            .FirstOrDefaultAsync(t => t.TrainingTrackId == trackId && !t.IsDeleted);

        if (track is null)
            return (null, "Track not found.");

        var enrollments = await context.Enrollments.AsNoTracking()
            .Include(e => e.Student)
            .Where(e => e.TrainingTrackId == trackId && e.Status != EnrollmentStatus.Cancelled)
            .ToListAsync();

        var students = enrollments.Select(e => new StudentProgressItem
        {
            StudentId = e.StudentId,
            FullName = e.Student.FullName,
            Status = e.Status.ToString(),
            ProgressPercentage = e.ProgressPercentage
        }).ToList();

        return (new TrackProgressResponse
        {
            TrainingTrackId = track.TrainingTrackId,
            TrackTitle = track.Title,
            TotalStudents = students.Count,
            AverageProgress = students.Count == 0 ? 0 : students.Average(s => s.ProgressPercentage),
            Students = students
        }, null);
    }

    public async Task<IReadOnlyList<TrackSessionResponse>> GetTrackSessionsAsync(int trackId)
    {
        if (!await OwnsTrackAsync(trackId))
            return [];

        return await context.TrackSessions.AsNoTracking()
            .Where(s => s.TrainingTrackId == trackId)
            .OrderBy(s => s.SessionDate)
            .Select(s => new TrackSessionResponse
            {
                TrackSessionId = s.TrackSessionId,
                TrainingTrackId = s.TrainingTrackId,
                Title = s.Title,
                Description = s.Description,
                SessionDate = s.SessionDate,
                DurationMinutes = s.DurationMinutes
            })
            .ToListAsync();
    }

    private async Task<bool> OwnsTrackAsync(int trackId)
    {
        var instructorId = await ResolveInstructorIdAsync();
        if (instructorId is null) return false;

        return await context.TrainingTracks.AsNoTracking()
            .AnyAsync(t => t.TrainingTrackId == trackId && t.InstructorId == instructorId && !t.IsDeleted);
    }

    private async Task<int?> ResolveInstructorIdAsync()
    {
        if (currentUser.InstructorId.HasValue)
            return currentUser.InstructorId;

        if (currentUser.UserId is null)
            return null;

        return await context.ApplicationUsers.AsNoTracking()
            .Where(u => u.Id == currentUser.UserId && u.Role == AppRoles.Instructor)
            .Select(u => u.InstructorId)
            .FirstOrDefaultAsync();
    }

    private static TrackSessionResponse MapSession(TrackSession session) => new()
    {
        TrackSessionId = session.TrackSessionId,
        TrainingTrackId = session.TrainingTrackId,
        Title = session.Title,
        Description = session.Description,
        SessionDate = session.SessionDate,
        DurationMinutes = session.DurationMinutes
    };
}
