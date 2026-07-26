namespace EfCoreDrills.Api.DTOs;

public class CreateTrackRequest
{
    public string Title { get; set; } = string.Empty;
    public int InstructorId { get; set; }
}
