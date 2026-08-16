namespace TrainingCenter.Api.DTOs;

public record UpdateStudentProfileRequest(string FullName, string? PhoneNumber);

public record CreateEnrollmentRequestDto(int TrainingTrackId);

public record CreateTrackSessionRequest(
    string Title,
    string? Description,
    DateTime SessionDate,
    int DurationMinutes);

public record UpdateTrackSessionRequest(
    string Title,
    string? Description,
    DateTime SessionDate,
    int DurationMinutes);

public class TrackSessionResponse
{
    public int TrackSessionId { get; set; }
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public int DurationMinutes { get; set; }
}

public class TrackProgressResponse
{
    public int TrainingTrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public decimal AverageProgress { get; set; }
    public IReadOnlyList<StudentProgressItem> Students { get; set; } = [];
}

public class StudentProgressItem
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ProgressPercentage { get; set; }
}

public class ActivityLogResponse
{
    public int ActivityLogId { get; set; }
    public int? UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? Details { get; set; }
    public DateTime CreatedAt { get; set; }
}
