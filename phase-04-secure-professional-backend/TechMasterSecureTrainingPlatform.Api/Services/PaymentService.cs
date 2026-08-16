using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class PaymentService(AppDbContext context)
{
    public async Task<(IReadOnlyList<PaymentResponse>? Data, string? Error)> GetAllAsync(
        DateTime? from, DateTime? to, string? status)
    {
        var filterError = FilterValidation.ValidatePaymentStatusFilter(status);
        if (filterError is not null) return (null, filterError);

        var query = context.Payments.AsNoTracking().AsQueryable();
        if (from.HasValue) query = query.Where(p => p.PaymentDate >= from.Value);
        if (to.HasValue) query = query.Where(p => p.PaymentDate <= to.Value);
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<PaymentStatus>(status, true, out var st))
            query = query.Where(p => p.PaymentStatus == st);

        var data = await query.OrderByDescending(p => p.PaymentDate)
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
            }).ToListAsync();

        return (data, null);
    }

    public async Task<IReadOnlyList<PaymentResponse>> GetByEnrollmentAsync(int enrollmentId) =>
        await context.Payments.AsNoTracking()
            .Where(p => p.EnrollmentId == enrollmentId)
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
            }).ToListAsync();

    public async Task<(PaymentResponse? Data, string? Error)> CreateAsync(CreatePaymentRequest request)
    {
        if (request.Amount <= 0) return (null, "Payment amount must be positive.");

        var enrollment = await context.Enrollments
            .Include(e => e.Payments)
            .Include(e => e.TrainingTrack)
            .FirstOrDefaultAsync(e => e.EnrollmentId == request.EnrollmentId);
        if (enrollment is null) return (null, "Enrollment not found.");
        if (enrollment.Status is EnrollmentStatus.Cancelled or EnrollmentStatus.Completed)
            return (null, "Cannot add payment to cancelled or completed enrollment.");

        var remaining = EnrollmentHelper.GetRemaining(enrollment);
        if (request.Amount > remaining)
            return (null, "Payment amount exceeds remaining balance.");

        if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var method))
            return (null, "Invalid payment method.");

        var payment = new Payment
        {
            EnrollmentId = request.EnrollmentId,
            Amount = request.Amount,
            PaymentMethod = method,
            PaymentDate = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.Pending,
            ReferenceNumber = request.ReferenceNumber,
            Notes = request.Notes
        };
        context.Payments.Add(payment);
        await context.SaveChangesAsync();

        return (await MapPaymentAsync(payment.PaymentId), null);
    }

    public async Task<(PaymentResponse? Data, string? Error)> UpdateStatusAsync(
        int id, UpdatePaymentStatusRequest request)
    {
        var payment = await context.Payments
            .Include(p => p.Enrollment)
            .ThenInclude(e => e.Payments)
            .Include(p => p.Enrollment)
            .ThenInclude(e => e.TrainingTrack)
            .FirstOrDefaultAsync(p => p.PaymentId == id);
        if (payment is null) return (null, "Payment not found.");
        if (!Enum.TryParse<PaymentStatus>(request.Status, true, out var status))
            return (null, "Invalid payment status.");

        if (!PaymentWorkflow.IsValidTransition(payment.PaymentStatus, status))
            return (null, "Invalid payment status transition.");

        payment.PaymentStatus = status;
        if (status == PaymentStatus.Paid)
            PaymentWorkflow.ApplyEnrollmentEffects(payment.Enrollment);
        await context.SaveChangesAsync();

        return (await MapPaymentAsync(payment.PaymentId), null);
    }

    private async Task<PaymentResponse> MapPaymentAsync(int paymentId) =>
        await context.Payments.AsNoTracking()
            .Where(p => p.PaymentId == paymentId)
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
            }).FirstAsync();
}
