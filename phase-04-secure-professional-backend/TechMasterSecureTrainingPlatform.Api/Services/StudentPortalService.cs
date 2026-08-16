using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class StudentPortalService(
    AppDbContext context,
    ICurrentUserService currentUser,
    EnrollmentService enrollmentService,
    AuditService auditService)
{
    public async Task<StudentDetailsResponse?> GetMyProfileAsync()
    {
        var studentId = await ResolveStudentIdAsync();
        if (studentId is null) return null;

        return await context.Students.AsNoTracking()
            .Where(s => s.StudentId == studentId && !s.IsDeleted)
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
                    e.Status == EnrollmentStatus.Active || e.Status == EnrollmentStatus.Pending)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<(StudentDetailsResponse? Data, string? Error)> UpdateProfileAsync(UpdateStudentProfileRequest request)
    {
        var studentId = await ResolveStudentIdAsync();
        if (studentId is null)
            return (null, "Student profile not linked to this account.");

        var student = await context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted);
        if (student is null)
            return (null, "Student profile not found.");

        if (string.IsNullOrWhiteSpace(request.FullName))
            return (null, "Full name is required.");

        student.FullName = request.FullName.Trim();
        student.PhoneNumber = request.PhoneNumber?.Trim();
        student.UpdatedAt = DateTime.UtcNow;

        var user = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == currentUser.UserId);
        if (user is not null)
            user.FullName = student.FullName;

        await context.SaveChangesAsync();
        await auditService.LogAsync("UpdateProfile", "Student", student.StudentId, "Student updated own profile.");

        return (await GetMyProfileAsync(), null);
    }

    public async Task<IReadOnlyList<EnrollmentListItemResponse>> GetMyEnrollmentsAsync()
    {
        var studentId = await ResolveStudentIdAsync();
        if (studentId is null) return [];

        var (data, _) = await enrollmentService.GetAllAsync(null, null, studentId, null);
        return data ?? [];
    }

    public async Task<IReadOnlyList<PaymentResponse>> GetMyPaymentsAsync()
    {
        var studentId = await ResolveStudentIdAsync();
        if (studentId is null) return [];

        return await context.Payments.AsNoTracking()
            .Include(p => p.Enrollment)
            .Where(p => p.Enrollment.StudentId == studentId)
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentResponse
            {
                PaymentId = p.PaymentId,
                EnrollmentId = p.EnrollmentId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod.ToString(),
                PaymentDate = p.PaymentDate,
                PaymentStatus = p.PaymentStatus.ToString(),
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TrackListItemResponse>> GetAvailableTracksAsync()
    {
        return await context.TrainingTracks.AsNoTracking()
            .Include(t => t.Instructor)
            .Where(t => !t.IsDeleted && t.Status == TrackStatus.Open)
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

    public async Task<(EnrollmentDetailsResponse? Data, string? Error)> RequestEnrollmentAsync(CreateEnrollmentRequestDto request)
    {
        var studentId = await ResolveStudentIdAsync();
        if (studentId is null)
            return (null, "Student profile not linked to this account.");

        var (data, error) = await enrollmentService.CreateAsync(new CreateEnrollmentRequest(studentId.Value, request.TrainingTrackId));
        if (data is not null)
            await auditService.LogAsync("EnrollmentRequest", "Enrollment", data.EnrollmentId,
                $"Student requested enrollment in track {request.TrainingTrackId}.");

        return (data, error);
    }

    private async Task<int?> ResolveStudentIdAsync()
    {
        if (currentUser.StudentId.HasValue)
            return currentUser.StudentId;

        if (currentUser.UserId is null)
            return null;

        return await context.ApplicationUsers.AsNoTracking()
            .Where(u => u.Id == currentUser.UserId && u.Role == AppRoles.Student)
            .Select(u => u.StudentId)
            .FirstOrDefaultAsync();
    }
}
