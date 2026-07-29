namespace TrainingCenter.Api.DTOs;

public record CreateStudentRequest(string FullName, string Email, string? PhoneNumber);
public record UpdateStudentRequest(string FullName, string Email, string? PhoneNumber, bool IsActive);

public class StudentListItemResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}

public class StudentDetailsResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ActiveEnrollmentCount { get; set; }
}

public record CreateInstructorRequest(string FullName, string Email, string Specialization, string? Bio);
public record UpdateInstructorRequest(string FullName, string Email, string Specialization, string? Bio, bool IsActive);

public class InstructorResponse
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public bool IsActive { get; set; }
}

public record CreateTrackRequest(
    string Title, string Code, string? Description, string Level,
    int Capacity, decimal Fee, DateTime StartDate, DateTime EndDate, int InstructorId);

public record UpdateTrackRequest(
    string Title, string Code, string? Description, string Level,
    int Capacity, decimal Fee, DateTime StartDate, DateTime EndDate, string Status, int InstructorId);

public class TrackListItemResponse
{
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string InstructorName { get; set; } = string.Empty;
}

public class TrackDetailsResponse : TrackListItemResponse
{
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ActiveEnrollments { get; set; }
    public int RemainingSeats { get; set; }
}

public record CreateEnrollmentRequest(int StudentId, int TrainingTrackId);
public record UpdateEnrollmentStatusRequest(string Status);

public class EnrollmentListItemResponse
{
    public int EnrollmentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string TrackTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalRequired { get; set; }
}

public class EnrollmentDetailsResponse : EnrollmentListItemResponse
{
    public int StudentId { get; set; }
    public int TrainingTrackId { get; set; }
    public decimal ProgressPercentage { get; set; }
    public string? FinalResult { get; set; }
    public IReadOnlyList<PaymentResponse> Payments { get; set; } = [];
}

public record CreatePaymentRequest(
    int EnrollmentId, decimal Amount, string PaymentMethod,
    string ReferenceNumber, string? Notes);

public record UpdatePaymentStatusRequest(string Status);

public class PaymentResponse
{
    public int PaymentId { get; set; }
    public int EnrollmentId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class DashboardSummaryResponse
{
    public int StudentsCount { get; set; }
    public int TracksCount { get; set; }
    public int ActiveEnrollments { get; set; }
    public decimal Revenue { get; set; }
    public int UnpaidCount { get; set; }
}

public class UnpaidEnrollmentResponse
{
    public int EnrollmentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string TrackTitle { get; set; } = string.Empty;
    public decimal TotalRequired { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingAmount { get; set; }
}

public class TrackCapacityResponse
{
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int ActiveEnrollments { get; set; }
    public int RemainingSeats { get; set; }
}

public class RevenueSummaryResponse
{
    public decimal TotalRevenue { get; set; }
    public int PaidCount { get; set; }
    public int PendingCount { get; set; }
    public int FailedCount { get; set; }
}

public class RevenueByTrackResponse
{
    public int TrainingTrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public decimal TotalPaid { get; set; }
    public int EnrollmentCount { get; set; }
}

public class TrackStudentResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
}

public class InstructorWorkloadResponse
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int TrackCount { get; set; }
    public int ActiveStudents { get; set; }
}

public class TopTrackResponse
{
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ActiveEnrollmentCount { get; set; }
}
