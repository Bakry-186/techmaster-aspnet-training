namespace TrainingCenter.Api.Services;

public static class FilterValidation
{
    public static string? ValidateTrackFilters(string? level, string? status)
    {
        if (!string.IsNullOrWhiteSpace(level) && !Enum.TryParse<Entities.TrackLevel>(level, true, out _))
            return "Invalid level filter.";
        if (!string.IsNullOrWhiteSpace(status) && !Enum.TryParse<Entities.TrackStatus>(status, true, out _))
            return "Invalid status filter.";
        return null;
    }

    public static string? ValidateEnrollmentStatusFilter(string? status)
    {
        if (!string.IsNullOrWhiteSpace(status) &&
            !Enum.TryParse<Entities.EnrollmentStatus>(status, true, out _))
            return "Invalid enrollment status filter.";
        return null;
    }

    public static string? ValidatePaymentStatusFilter(string? status)
    {
        if (!string.IsNullOrWhiteSpace(status) &&
            !Enum.TryParse<Entities.PaymentStatus>(status, true, out _))
            return "Invalid payment status filter.";
        return null;
    }

    public static string? ValidatePaymentSummaryFilter(string? paymentStatus)
    {
        if (string.IsNullOrWhiteSpace(paymentStatus)) return null;
        return paymentStatus.ToLower() switch
        {
            "paid" or "unpaid" or "partial" => null,
            _ => "Invalid payment status filter."
        };
    }
}
