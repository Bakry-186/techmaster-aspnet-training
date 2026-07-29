using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController(ReportService service) : ControllerBase
{
    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> DashboardSummary() =>
        Ok(ApiResponse<DashboardSummaryResponse>.Ok(await service.GetDashboardAsync()));

    [HttpGet("unpaid-enrollments")]
    public async Task<IActionResult> UnpaidEnrollments() =>
        Ok(ApiResponse<IReadOnlyList<UnpaidEnrollmentResponse>>.Ok(await service.GetUnpaidEnrollmentsAsync()));

    [HttpGet("track-capacity")]
    public async Task<IActionResult> TrackCapacity() =>
        Ok(ApiResponse<IReadOnlyList<TrackCapacityResponse>>.Ok(await service.GetTrackCapacityAsync()));

    [HttpGet("revenue-summary")]
    public async Task<IActionResult> RevenueSummary() =>
        Ok(ApiResponse<RevenueSummaryResponse>.Ok(await service.GetRevenueSummaryAsync()));

    [HttpGet("revenue-by-track")]
    public async Task<IActionResult> RevenueByTrack() =>
        Ok(ApiResponse<IReadOnlyList<RevenueByTrackResponse>>.Ok(await service.GetRevenueByTrackAsync()));

    [HttpGet("tracks-with-available-seats")]
    public async Task<IActionResult> TracksWithAvailableSeats() =>
        Ok(ApiResponse<IReadOnlyList<TrackCapacityResponse>>.Ok(await service.GetTracksWithAvailableSeatsAsync()));

    [HttpGet("top-tracks")]
    public async Task<IActionResult> TopTracks() =>
        Ok(ApiResponse<IReadOnlyList<TopTrackResponse>>.Ok(await service.GetTopTracksAsync()));

    [HttpGet("instructor-workload")]
    public async Task<IActionResult> InstructorWorkload() =>
        Ok(ApiResponse<IReadOnlyList<InstructorWorkloadResponse>>.Ok(await service.GetInstructorWorkloadAsync()));

    [HttpGet("students-without-payments")]
    public async Task<IActionResult> StudentsWithoutPayments() =>
        Ok(ApiResponse<IReadOnlyList<UnpaidEnrollmentResponse>>.Ok(await service.GetStudentsWithoutPaymentsAsync()));
}
