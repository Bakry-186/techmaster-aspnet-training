using EfCoreDrills.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDrills.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Students.AnyAsync())
        {
            return;
        }

        var seedTime = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);

        var instructor1 = new Instructor
        {
            FullName = "Dr. Ahmed Nabil",
            Email = "ahmed.nabil@techmaster.com",
            CreatedAt = seedTime
        };

        var instructor2 = new Instructor
        {
            FullName = "Eng. Laila Samir",
            Email = "laila.samir@techmaster.com",
            CreatedAt = seedTime
        };

        var track1 = new TrainingTrack
        {
            Title = "ASP.NET Backend Career",
            Instructor = instructor1,
            CreatedAt = seedTime
        };

        var track2 = new TrainingTrack
        {
            Title = "EF Core Deep Dive",
            Instructor = instructor1,
            CreatedAt = seedTime
        };

        var track3 = new TrainingTrack
        {
            Title = "SQL Server for Developers",
            Instructor = instructor2,
            CreatedAt = seedTime
        };

        var student1 = new Student
        {
            FullName = "Mohamed Ayman",
            Email = "mohamed@example.com",
            CreatedAt = seedTime,
            Profile = new StudentProfile
            {
                NationalId = "29901011234567",
                Address = "Cairo, Egypt",
                EmergencyPhone = "01012345678",
                DateOfBirth = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        var student2 = new Student
        {
            FullName = "Sara Hassan",
            Email = "sara@example.com",
            CreatedAt = seedTime
        };

        var student3 = new Student
        {
            FullName = "Omar Ali",
            Email = "omar@example.com",
            CreatedAt = seedTime
        };

        var student4 = new Student
        {
            FullName = "Nour Ibrahim",
            Email = "nour@example.com",
            CreatedAt = seedTime
        };

        var student5 = new Student
        {
            FullName = "Youssef Khaled",
            Email = "youssef@example.com",
            CreatedAt = seedTime
        };

        context.Enrollments.AddRange(
            new Enrollment
            {
                Student = student1,
                TrainingTrack = track1,
                Status = EnrollmentStatus.Active,
                EnrollmentDate = seedTime.AddDays(1),
                PaymentSummary = new PaymentSummary
                {
                    TotalRequired = 5000m,
                    TotalPaid = 5000m,
                    PaymentStatus = PaymentStatus.Paid
                }
            },
            new Enrollment
            {
                Student = student2,
                TrainingTrack = track1,
                Status = EnrollmentStatus.Pending,
                EnrollmentDate = seedTime.AddDays(2),
                PaymentSummary = new PaymentSummary
                {
                    TotalRequired = 5000m,
                    TotalPaid = 2000m,
                    PaymentStatus = PaymentStatus.PartiallyPaid
                }
            },
            new Enrollment
            {
                Student = student3,
                TrainingTrack = track2,
                Status = EnrollmentStatus.Active,
                EnrollmentDate = seedTime.AddDays(3),
                PaymentSummary = new PaymentSummary
                {
                    TotalRequired = 3500m,
                    TotalPaid = 0m,
                    PaymentStatus = PaymentStatus.Pending
                }
            },
            new Enrollment
            {
                Student = student4,
                TrainingTrack = track3,
                Status = EnrollmentStatus.Completed,
                EnrollmentDate = seedTime.AddDays(4),
                FinalGrade = 92.5m,
                PaymentSummary = new PaymentSummary
                {
                    TotalRequired = 3000m,
                    TotalPaid = 3000m,
                    PaymentStatus = PaymentStatus.Paid
                }
            },
            new Enrollment
            {
                Student = student5,
                TrainingTrack = track2,
                Status = EnrollmentStatus.Pending,
                EnrollmentDate = seedTime.AddDays(5)
            });

        context.Students.AddRange(student1, student2, student3, student4, student5);
        context.Instructors.AddRange(instructor1, instructor2);
        context.TrainingTracks.AddRange(track1, track2, track3);

        await context.SaveChangesAsync();
    }
}
