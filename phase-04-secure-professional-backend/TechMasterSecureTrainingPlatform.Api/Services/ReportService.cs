using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class ReportService(AppDbContext context)
{
    public async Task<DashboardSummaryResponse> GetDashboardAsync()
    {
        var students = await context.Students.CountAsync(s => !s.IsDeleted);
        var tracks = await context.TrainingTracks.CountAsync(t => !t.IsDeleted);
        var activeEnrollments = await context.Enrollments.CountAsync(e =>
            e.Status == EnrollmentStatus.Active || e.Status == EnrollmentStatus.Pending);
        var revenue = await context.Payments
            .Where(p => p.PaymentStatus == PaymentStatus.Paid)
            .SumAsync(p => p.Amount);

        var unpaid = await context.Enrollments.AsNoTracking()
            .Where(e => e.Status != EnrollmentStatus.Cancelled)
            .Select(e => new
            {
                Required = e.TrainingTrack.Fee,
                Paid = e.Payments.Where(p => p.PaymentStatus == PaymentStatus.Paid).Sum(p => p.Amount)
            })
            .CountAsync(x => x.Required - x.Paid > 0);

        return new DashboardSummaryResponse
        {
            StudentsCount = students,
            TracksCount = tracks,
            ActiveEnrollments = activeEnrollments,
            Revenue = revenue,
            UnpaidCount = unpaid
        };
    }

    public async Task<IReadOnlyList<UnpaidEnrollmentResponse>> GetUnpaidEnrollmentsAsync() =>
        await context.Enrollments.AsNoTracking()
            .Where(e => e.Status != EnrollmentStatus.Cancelled)
            .Select(e => new
            {
                e.EnrollmentId,
                StudentName = e.Student.FullName,
                TrackTitle = e.TrainingTrack.Title,
                TotalRequired = e.TrainingTrack.Fee,
                TotalPaid = e.Payments
                    .Where(p => p.PaymentStatus == PaymentStatus.Paid)
                    .Sum(p => p.Amount)
            })
            .Where(x => x.TotalRequired - x.TotalPaid > 0)
            .Select(x => new UnpaidEnrollmentResponse
            {
                EnrollmentId = x.EnrollmentId,
                StudentName = x.StudentName,
                TrackTitle = x.TrackTitle,
                TotalRequired = x.TotalRequired,
                TotalPaid = x.TotalPaid,
                RemainingAmount = x.TotalRequired - x.TotalPaid
            })
            .ToListAsync();

    public async Task<IReadOnlyList<TrackCapacityResponse>> GetTrackCapacityAsync() =>
        await context.TrainingTracks.AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Select(t => new TrackCapacityResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Capacity = t.Capacity,
                ActiveEnrollments = t.Enrollments.Count(e =>
                    e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active),
                RemainingSeats = t.Capacity - t.Enrollments.Count(e =>
                    e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active)
            })
            .ToListAsync();

    public async Task<RevenueSummaryResponse> GetRevenueSummaryAsync()
    {
        var grouped = await context.Payments.AsNoTracking()
            .GroupBy(_ => 1)
            .Select(g => new RevenueSummaryResponse
            {
                TotalRevenue = g.Where(p => p.PaymentStatus == PaymentStatus.Paid).Sum(p => p.Amount),
                PaidCount = g.Count(p => p.PaymentStatus == PaymentStatus.Paid),
                PendingCount = g.Count(p => p.PaymentStatus == PaymentStatus.Pending),
                FailedCount = g.Count(p => p.PaymentStatus == PaymentStatus.Failed)
            })
            .FirstOrDefaultAsync();

        return grouped ?? new RevenueSummaryResponse();
    }

    public async Task<IReadOnlyList<RevenueByTrackResponse>> GetRevenueByTrackAsync() =>
        await context.Enrollments.AsNoTracking()
            .GroupBy(e => new { e.TrainingTrackId, e.TrainingTrack.Title })
            .Select(g => new RevenueByTrackResponse
            {
                TrainingTrackId = g.Key.TrainingTrackId,
                TrackTitle = g.Key.Title,
                TotalPaid = g.SelectMany(e => e.Payments)
                    .Where(p => p.PaymentStatus == PaymentStatus.Paid)
                    .Sum(p => p.Amount),
                EnrollmentCount = g.Count()
            }).ToListAsync();

    public async Task<IReadOnlyList<TrackCapacityResponse>> GetTracksWithAvailableSeatsAsync()
    {
        var all = await GetTrackCapacityAsync();
        return all.Where(t => t.RemainingSeats > 0).ToList();
    }

    public async Task<IReadOnlyList<TopTrackResponse>> GetTopTracksAsync(int top = 5) =>
        await context.TrainingTracks.AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Select(t => new TopTrackResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                ActiveEnrollmentCount = t.Enrollments.Count(e =>
                    e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active)
            })
            .OrderByDescending(t => t.ActiveEnrollmentCount)
            .Take(top)
            .ToListAsync();

    public async Task<IReadOnlyList<InstructorWorkloadResponse>> GetInstructorWorkloadAsync() =>
        await context.Instructors.AsNoTracking()
            .Select(i => new InstructorWorkloadResponse
            {
                InstructorId = i.InstructorId,
                FullName = i.FullName,
                TrackCount = i.Tracks.Count(t => !t.IsDeleted),
                ActiveStudents = i.Tracks
                    .Where(t => !t.IsDeleted)
                    .SelectMany(t => t.Enrollments)
                    .Count(e => e.Status == EnrollmentStatus.Active || e.Status == EnrollmentStatus.Pending)
            }).ToListAsync();

    public async Task<IReadOnlyList<UnpaidEnrollmentResponse>> GetStudentsWithoutPaymentsAsync() =>
        await context.Enrollments.AsNoTracking()
            .Where(e => e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active)
            .Where(e => !e.Payments.Any(p => p.PaymentStatus == PaymentStatus.Paid))
            .Select(e => new UnpaidEnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentName = e.Student.FullName,
                TrackTitle = e.TrainingTrack.Title,
                TotalRequired = e.TrainingTrack.Fee,
                TotalPaid = 0,
                RemainingAmount = e.TrainingTrack.Fee
            }).ToListAsync();
}
