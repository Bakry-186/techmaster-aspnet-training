using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class EnrollmentService(AppDbContext context)
{
    public async Task<IReadOnlyList<EnrollmentListItemResponse>> GetAllAsync(
        string? status, int? trackId, int? studentId, string? paymentStatus)
    {
        var query = context.Enrollments.AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<EnrollmentStatus>(status, true, out var st))
            query = query.Where(e => e.Status == st);
        if (trackId.HasValue) query = query.Where(e => e.TrainingTrackId == trackId.Value);
        if (studentId.HasValue) query = query.Where(e => e.StudentId == studentId.Value);

        var list = await query.OrderByDescending(e => e.EnrollmentDate).ToListAsync();

        return list.Select(e =>
        {
            var totalPaid = EnrollmentHelper.GetTotalPaid(e);
            var totalRequired = e.TrainingTrack.Fee;
            var item = new EnrollmentListItemResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentName = e.Student.FullName,
                TrackTitle = e.TrainingTrack.Title,
                Status = e.Status.ToString(),
                EnrollmentDate = e.EnrollmentDate,
                TotalPaid = totalPaid,
                TotalRequired = totalRequired
            };
            return item;
        }).Where(e =>
        {
            if (string.IsNullOrWhiteSpace(paymentStatus)) return true;
            var remaining = e.TotalRequired - e.TotalPaid;
            return paymentStatus.ToLower() switch
            {
                "paid" => remaining <= 0,
                "unpaid" => remaining > 0,
                "partial" => e.TotalPaid > 0 && remaining > 0,
                _ => true
            };
        }).ToList();
    }

    public async Task<EnrollmentDetailsResponse?> GetByIdAsync(int id)
    {
        var e = await context.Enrollments.AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.TrainingTrack)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.EnrollmentId == id);
        if (e is null) return null;

        return MapDetails(e);
    }

    public async Task<(EnrollmentDetailsResponse? Data, string? Error)> CreateAsync(CreateEnrollmentRequest request)
    {
        var student = await context.Students.FirstOrDefaultAsync(s =>
            s.StudentId == request.StudentId && !s.IsDeleted && s.IsActive);
        if (student is null) return (null, "Student not found or inactive.");

        var track = await context.TrainingTracks
            .Include(t => t.Enrollments)
            .FirstOrDefaultAsync(t => t.TrainingTrackId == request.TrainingTrackId && !t.IsDeleted);
        if (track is null) return (null, "Track not found.");
        if (track.Status == TrackStatus.Closed) return (null, "Closed track cannot accept new enrollments.");

        if (await context.Enrollments.AnyAsync(e =>
                e.StudentId == request.StudentId &&
                e.TrainingTrackId == request.TrainingTrackId &&
                EnrollmentHelper.IsActiveEnrollment(e.Status)))
            return (null, "Duplicate active enrollment is not allowed.");

        var activeCount = EnrollmentHelper.CountActiveEnrollments(track);
        if (activeCount >= track.Capacity)
            return (null, "Track capacity exceeded.");

        var enrollment = new Enrollment
        {
            StudentId = request.StudentId,
            TrainingTrackId = request.TrainingTrackId,
            EnrollmentDate = DateTime.UtcNow,
            Status = EnrollmentStatus.Pending,
            ProgressPercentage = 0,
            CreatedAt = DateTime.UtcNow
        };
        context.Enrollments.Add(enrollment);
        await context.SaveChangesAsync();
        return (await GetByIdAsync(enrollment.EnrollmentId), null);
    }

    public async Task<(EnrollmentDetailsResponse? Data, string? Error)> UpdateStatusAsync(
        int id, UpdateEnrollmentStatusRequest request)
    {
        var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == id);
        if (enrollment is null) return (null, "Enrollment not found.");
        if (!Enum.TryParse<EnrollmentStatus>(request.Status, true, out var newStatus))
            return (null, "Invalid status.");

        if (enrollment.Status == EnrollmentStatus.Completed && newStatus == EnrollmentStatus.Cancelled)
            return (null, "Completed enrollment cannot be cancelled directly.");

        var valid = (enrollment.Status, newStatus) switch
        {
            (EnrollmentStatus.Pending, EnrollmentStatus.Active) => true,
            (EnrollmentStatus.Pending, EnrollmentStatus.Cancelled) => true,
            (EnrollmentStatus.Active, EnrollmentStatus.Completed) => true,
            (EnrollmentStatus.Active, EnrollmentStatus.Cancelled) => true,
            _ when enrollment.Status == newStatus => true,
            _ => false
        };
        if (!valid) return (null, "Invalid status transition.");

        enrollment.Status = newStatus;
        enrollment.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return (await GetByIdAsync(id), null);
    }

    public async Task<IReadOnlyList<EnrollmentListItemResponse>> GetByStudentAsync(int studentId)
    {
        if (!await context.Students.AnyAsync(s => s.StudentId == studentId && !s.IsDeleted))
            return [];

        return await GetAllAsync(null, null, studentId, null);
    }

    public async Task<IReadOnlyList<TrackStudentResponse>> GetStudentsByTrackAsync(int trackId)
    {
        if (!await context.TrainingTracks.AnyAsync(t => t.TrainingTrackId == trackId && !t.IsDeleted))
            return [];

        return await context.Enrollments.AsNoTracking()
            .Where(e => e.TrainingTrackId == trackId)
            .Select(e => new TrackStudentResponse
            {
                StudentId = e.StudentId,
                FullName = e.Student.FullName,
                Email = e.Student.Email,
                Status = e.Status.ToString(),
                EnrollmentDate = e.EnrollmentDate
            }).ToListAsync();
    }

    private static EnrollmentDetailsResponse MapDetails(Enrollment e) => new()
    {
        EnrollmentId = e.EnrollmentId,
        StudentId = e.StudentId,
        TrainingTrackId = e.TrainingTrackId,
        StudentName = e.Student.FullName,
        TrackTitle = e.TrainingTrack.Title,
        Status = e.Status.ToString(),
        EnrollmentDate = e.EnrollmentDate,
        ProgressPercentage = e.ProgressPercentage,
        FinalResult = e.FinalResult,
        TotalPaid = EnrollmentHelper.GetTotalPaid(e),
        TotalRequired = e.TrainingTrack.Fee,
        Payments = e.Payments.Select(p => new PaymentResponse
        {
            PaymentId = p.PaymentId,
            EnrollmentId = p.EnrollmentId,
            Amount = p.Amount,
            PaymentMethod = p.PaymentMethod.ToString(),
            PaymentDate = p.PaymentDate,
            PaymentStatus = p.PaymentStatus.ToString(),
            ReferenceNumber = p.ReferenceNumber,
            Notes = p.Notes
        }).ToList()
    };
}
