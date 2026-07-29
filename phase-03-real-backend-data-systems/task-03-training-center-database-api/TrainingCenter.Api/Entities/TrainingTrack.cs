namespace TrainingCenter.Api.Entities;

public class TrainingTrack
{
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TrackLevel Level { get; set; }
    public int Capacity { get; set; }
    public decimal Fee { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TrackStatus Status { get; set; } = TrackStatus.Open;
    public int InstructorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Instructor Instructor { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
