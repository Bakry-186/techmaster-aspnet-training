namespace EfCoreDrills.Api.DTOs;

public class EnrollmentDetailsDto
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string TrackTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public decimal? FinalGrade { get; set; }
    public PaymentSummaryDto? PaymentSummary { get; set; }
}

public class PaymentSummaryDto
{
    public decimal TotalRequired { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}
