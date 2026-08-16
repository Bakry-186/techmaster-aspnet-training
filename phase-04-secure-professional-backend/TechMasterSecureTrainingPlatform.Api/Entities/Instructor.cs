namespace TrainingCenter.Api.Entities;

public class Instructor
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public int? UserId { get; set; }

    public ICollection<TrainingTrack> Tracks { get; set; } = [];
}
