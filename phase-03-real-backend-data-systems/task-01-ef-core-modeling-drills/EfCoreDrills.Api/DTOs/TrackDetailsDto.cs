namespace EfCoreDrills.Api.DTOs;

public class TrackDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public int EnrolledStudentCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
