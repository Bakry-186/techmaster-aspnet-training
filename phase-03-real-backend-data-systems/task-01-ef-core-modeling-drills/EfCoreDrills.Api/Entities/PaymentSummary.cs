using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreDrills.Api.Entities;

public class PaymentSummary
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public decimal TotalRequired { get; set; }
    public decimal TotalPaid { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    [NotMapped]
    public decimal RemainingAmount => TotalRequired - TotalPaid;

    public Enrollment Enrollment { get; set; } = null!;
}
