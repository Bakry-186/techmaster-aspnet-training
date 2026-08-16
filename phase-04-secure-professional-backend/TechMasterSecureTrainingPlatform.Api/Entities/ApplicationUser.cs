namespace TrainingCenter.Api.Entities;

public class ApplicationUser
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int? StudentId { get; set; }
    public int? InstructorId { get; set; }

    public Student? Student { get; set; }
    public Instructor? Instructor { get; set; }
}
