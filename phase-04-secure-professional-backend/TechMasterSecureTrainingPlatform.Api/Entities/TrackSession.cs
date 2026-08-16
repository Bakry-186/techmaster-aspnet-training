namespace TrainingCenter.Api.Entities;

public class TrackSession
{
    public int TrackSessionId { get; set; }
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public TrainingTrack TrainingTrack { get; set; } = null!;
}
