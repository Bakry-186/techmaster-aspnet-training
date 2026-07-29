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

        var enrollments = await context.Enrollments
            .Include(e => e.Payments)
            .Include(e => e.TrainingTrack)
            .Where(e => e.Status != EnrollmentStatus.Cancelled)
            .ToListAsync();
        var unpaid = enrollments.Count(e => EnrollmentHelper.GetRemaining(e) > 0);

        return new DashboardSummaryResponse
        {
            StudentsCount = students,
            TracksCount = tracks,
            ActiveEnrollments = activeEnrollments,
            Revenue = revenue,
            UnpaidCount = unpaid
        };
    }

    public async Task<IReadOnlyList<UnpaidEnrollmentResponse>> GetUnpaidEnrollmentsAsync()
    {
        var enrollments = await context.Enrollments.AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
            .Where(e => e.Status != EnrollmentStatus.Cancelled)
            .ToListAsync();

        return enrollments
            .Select(e =>
            {
                var paid = EnrollmentHelper.GetTotalPaid(e);
                var required = e.TrainingTrack.Fee;
                return new { e, paid, required, remaining = required - paid };
            })
            .Where(x => x.remaining > 0)
            .Select(x => new UnpaidEnrollmentResponse
            {
                EnrollmentId = x.e.EnrollmentId,
                StudentName = x.e.Student.FullName,
                TrackTitle = x.e.TrainingTrack.Title,
                TotalRequired = x.required,
                TotalPaid = x.paid,
                RemainingAmount = x.remaining
            }).ToList();
    }

    public async Task<IReadOnlyList<TrackCapacityResponse>> GetTrackCapacityAsync()
    {
        var tracks = await context.TrainingTracks.AsNoTracking()
            .Include(t => t.Enrollments)
            .Where(t => !t.IsDeleted)
            .ToListAsync();

        return tracks.Select(t =>
        {
            var active = EnrollmentHelper.CountActiveEnrollments(t);
            return new TrackCapacityResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Capacity = t.Capacity,
                ActiveEnrollments = active,
                RemainingSeats = Math.Max(0, t.Capacity - active)
            };
        }).ToList();
    }

    public async Task<RevenueSummaryResponse> GetRevenueSummaryAsync()
    {
        var payments = await context.Payments.AsNoTracking().ToListAsync();
        return new RevenueSummaryResponse
        {
            TotalRevenue = payments.Where(p => p.PaymentStatus == PaymentStatus.Paid).Sum(p => p.Amount),
            PaidCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Paid),
            PendingCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Pending),
            FailedCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Failed)
        };
    }

    public async Task<IReadOnlyList<RevenueByTrackResponse>> GetRevenueByTrackAsync()
    {
        return await context.Enrollments.AsNoTracking()
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
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
    }

    public async Task<IReadOnlyList<TrackCapacityResponse>> GetTracksWithAvailableSeatsAsync()
    {
        var all = await GetTrackCapacityAsync();
        return all.Where(t => t.RemainingSeats > 0).ToList();
    }

    public async Task<IReadOnlyList<TopTrackResponse>> GetTopTracksAsync(int top = 5)
    {
        var tracks = await context.TrainingTracks.AsNoTracking()
            .Include(t => t.Enrollments)
            .Where(t => !t.IsDeleted)
            .ToListAsync();

        return tracks
            .Select(t => new TopTrackResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                ActiveEnrollmentCount = EnrollmentHelper.CountActiveEnrollments(t)
            })
            .OrderByDescending(t => t.ActiveEnrollmentCount)
            .Take(top)
            .ToList();
    }

    public async Task<IReadOnlyList<InstructorWorkloadResponse>> GetInstructorWorkloadAsync()
    {
        return await context.Instructors.AsNoTracking()
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
    }

    public async Task<IReadOnlyList<UnpaidEnrollmentResponse>> GetStudentsWithoutPaymentsAsync()
    {
        var enrollments = await context.Enrollments.AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.TrainingTrack)
            .Include(e => e.Payments)
            .Where(e => e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active)
            .ToListAsync();

        return enrollments
            .Where(e => !e.Payments.Any(p => p.PaymentStatus == PaymentStatus.Paid))
            .Select(e => new UnpaidEnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentName = e.Student.FullName,
                TrackTitle = e.TrainingTrack.Title,
                TotalRequired = e.TrainingTrack.Fee,
                TotalPaid = 0,
                RemainingAmount = e.TrainingTrack.Fee
            }).ToList();
    }
}
