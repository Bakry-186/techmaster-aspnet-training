using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public static class PaymentWorkflow
{
    public static bool IsValidTransition(PaymentStatus from, PaymentStatus to) =>
        from == to || (from, to) switch
        {
            (PaymentStatus.Pending, PaymentStatus.Paid) => true,
            (PaymentStatus.Pending, PaymentStatus.Failed) => true,
            (PaymentStatus.Paid, PaymentStatus.Refunded) => true,
            (PaymentStatus.Paid, PaymentStatus.Failed) => true,
            _ => false
        };

    public static void ApplyEnrollmentEffects(Enrollment enrollment)
    {
        var totalPaid = EnrollmentHelper.GetTotalPaid(enrollment);
        if (totalPaid > 0 && enrollment.Status == EnrollmentStatus.Pending)
        {
            enrollment.Status = EnrollmentStatus.Active;
            enrollment.UpdatedAt = DateTime.UtcNow;
        }
    }
}
