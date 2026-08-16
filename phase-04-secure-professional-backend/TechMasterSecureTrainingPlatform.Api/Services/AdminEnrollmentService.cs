using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class AdminEnrollmentService(AppDbContext context, AuditService auditService)
{
    public async Task<(EnrollmentDetailsResponse? Data, string? Error)> ApproveEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
            .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

        if (enrollment is null)
            return (null, "Enrollment not found.");

        if (enrollment.Status != EnrollmentStatus.Pending)
            return (null, "Only pending enrollments can be approved.");

        enrollment.Status = EnrollmentStatus.Active;
        enrollment.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await auditService.LogAsync("ApproveEnrollment", "Enrollment", enrollment.EnrollmentId,
            $"Enrollment approved for student {enrollment.StudentId} in track {enrollment.TrainingTrackId}.");

        return (MapDetails(enrollment), null);
    }

    private static EnrollmentDetailsResponse MapDetails(Enrollment e)
    {
        var totalPaid = EnrollmentHelper.GetTotalPaid(e);
        return new EnrollmentDetailsResponse
        {
            EnrollmentId = e.EnrollmentId,
            StudentId = e.StudentId,
            TrainingTrackId = e.TrainingTrackId,
            StudentName = e.Student.FullName,
            TrackTitle = e.TrainingTrack.Title,
            Status = e.Status.ToString(),
            EnrollmentDate = e.EnrollmentDate,
            TotalPaid = totalPaid,
            TotalRequired = e.TrainingTrack.Fee,
            ProgressPercentage = e.ProgressPercentage,
            FinalResult = e.FinalResult,
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
}
