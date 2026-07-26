namespace EfCoreDrills.Api.Entities;

public class TrainingTrack
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int InstructorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Instructor Instructor { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
