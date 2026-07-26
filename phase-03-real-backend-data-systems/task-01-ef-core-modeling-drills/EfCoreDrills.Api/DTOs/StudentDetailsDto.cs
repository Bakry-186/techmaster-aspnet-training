namespace EfCoreDrills.Api.DTOs;

public class StudentDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public StudentProfileDto? Profile { get; set; }
    public IReadOnlyList<EnrollmentSummaryDto> Enrollments { get; set; } = [];
}

public class StudentProfileDto
{
    public string NationalId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string EmergencyPhone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

public class EnrollmentSummaryDto
{
    public int EnrollmentId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
}
