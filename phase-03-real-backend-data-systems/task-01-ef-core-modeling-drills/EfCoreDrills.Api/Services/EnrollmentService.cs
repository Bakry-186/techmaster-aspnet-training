using EfCoreDrills.Api.Data;
using EfCoreDrills.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDrills.Api.Services;

public class EnrollmentService(AppDbContext context)
{
    public async Task<EnrollmentDetailsDto?> GetByIdAsync(int id)
    {
        return await context.Enrollments
            .AsNoTracking()
            .Where(enrollment => enrollment.Id == id)
            .Select(enrollment => new EnrollmentDetailsDto
            {
                Id = enrollment.Id,
                StudentName = enrollment.Student.FullName,
                TrackTitle = enrollment.TrainingTrack.Title,
                Status = enrollment.Status.ToString(),
                EnrollmentDate = enrollment.EnrollmentDate,
                FinalGrade = enrollment.FinalGrade,
                PaymentSummary = enrollment.PaymentSummary == null
                    ? null
                    : new PaymentSummaryDto
                    {
                        TotalRequired = enrollment.PaymentSummary.TotalRequired,
                        TotalPaid = enrollment.PaymentSummary.TotalPaid,
                        RemainingAmount = enrollment.PaymentSummary.TotalRequired - enrollment.PaymentSummary.TotalPaid,
                        PaymentStatus = enrollment.PaymentSummary.PaymentStatus.ToString()
                    }
            })
            .FirstOrDefaultAsync();
    }
}
