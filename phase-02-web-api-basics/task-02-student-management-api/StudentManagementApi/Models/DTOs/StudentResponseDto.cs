namespace StudentManagementApi.Models.DTOs;

public class StudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public string? GithubUrl { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; } = string.Empty;
}
