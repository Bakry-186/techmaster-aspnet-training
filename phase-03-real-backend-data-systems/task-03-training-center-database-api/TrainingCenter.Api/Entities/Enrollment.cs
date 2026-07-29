namespace TrainingCenter.Api.Entities;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int TrainingTrackId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    public decimal ProgressPercentage { get; set; }
    public string? FinalResult { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Student Student { get; set; } = null!;
    public TrainingTrack TrainingTrack { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = [];
}
