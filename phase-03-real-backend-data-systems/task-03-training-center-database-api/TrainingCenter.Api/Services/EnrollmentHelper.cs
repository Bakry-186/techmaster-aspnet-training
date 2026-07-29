using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public static class EnrollmentHelper
{
    public static bool IsActiveEnrollment(EnrollmentStatus status) =>
        status is EnrollmentStatus.Pending or EnrollmentStatus.Active;

    public static decimal GetTotalPaid(Enrollment enrollment) =>
        enrollment.Payments.Where(p => p.PaymentStatus == PaymentStatus.Paid).Sum(p => p.Amount);

    public static decimal GetRemaining(Enrollment enrollment) =>
        Math.Max(0, enrollment.TrainingTrack.Fee - GetTotalPaid(enrollment));

    public static int CountActiveEnrollments(TrainingTrack track) =>
        track.Enrollments.Count(e => IsActiveEnrollment(e.Status));
}
