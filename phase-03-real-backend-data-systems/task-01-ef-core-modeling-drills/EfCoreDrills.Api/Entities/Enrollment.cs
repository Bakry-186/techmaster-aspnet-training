namespace EfCoreDrills.Api.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int TrainingTrackId { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    public DateTime EnrollmentDate { get; set; }
    public decimal? FinalGrade { get; set; }

    public Student Student { get; set; } = null!;
    public TrainingTrack TrainingTrack { get; set; } = null!;
    public PaymentSummary? PaymentSummary { get; set; }
}
