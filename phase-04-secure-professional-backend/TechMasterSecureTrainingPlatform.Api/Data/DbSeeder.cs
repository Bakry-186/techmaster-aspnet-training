using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.Entities;
using TrainingCenter.Api.Helpers;

namespace TrainingCenter.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IConfiguration configuration)
    {
        await SeedTrainingDataAsync(context);
        await SeedAuthUsersAsync(context, configuration);
    }

    private static async Task SeedTrainingDataAsync(AppDbContext context)
    {
        if (await context.Students.AnyAsync()) return;

        var seed = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);

        var instructor1 = new Instructor
        {
            FullName = "Dr. Ahmed Nabil",
            Email = "ahmed.nabil@techmaster.com",
            Specialization = "ASP.NET Backend",
            Bio = "Senior backend mentor",
            CreatedAt = seed,
            IsActive = true
        };
        var instructor2 = new Instructor
        {
            FullName = "Eng. Laila Samir",
            Email = "laila.samir@techmaster.com",
            Specialization = "Database Systems",
            CreatedAt = seed,
            IsActive = true
        };

        var track1 = new TrainingTrack
        {
            Title = "ASP.NET Backend Career",
            Code = "BE-101",
            Description = "Full backend track",
            Level = TrackLevel.Intermediate,
            Capacity = 30,
            Fee = 5000m,
            StartDate = seed.AddDays(7),
            EndDate = seed.AddMonths(3),
            Status = TrackStatus.Open,
            Instructor = instructor1,
            CreatedAt = seed
        };
        var track2 = new TrainingTrack
        {
            Title = "EF Core Deep Dive",
            Code = "EF-201",
            Level = TrackLevel.Advanced,
            Capacity = 20,
            Fee = 3500m,
            StartDate = seed.AddDays(14),
            EndDate = seed.AddMonths(2),
            Status = TrackStatus.Open,
            Instructor = instructor1,
            CreatedAt = seed
        };
        var track3 = new TrainingTrack
        {
            Title = "SQL Server for Developers",
            Code = "SQL-301",
            Level = TrackLevel.Beginner,
            Capacity = 25,
            Fee = 3000m,
            StartDate = seed.AddDays(10),
            EndDate = seed.AddMonths(2),
            Status = TrackStatus.Open,
            Instructor = instructor2,
            CreatedAt = seed
        };

        var student1 = new Student { FullName = "Mohamed Ayman", Email = "mohamed@example.com", PhoneNumber = "01011111111", CreatedAt = seed, IsActive = true };
        var student2 = new Student { FullName = "Sara Hassan", Email = "sara@example.com", CreatedAt = seed, IsActive = true };
        var student3 = new Student { FullName = "Omar Ali", Email = "omar@example.com", CreatedAt = seed, IsActive = true };
        var student4 = new Student { FullName = "Nour Ibrahim", Email = "nour@example.com", CreatedAt = seed, IsActive = true };
        var student5 = new Student { FullName = "Youssef Khaled", Email = "youssef@example.com", CreatedAt = seed, IsActive = true };

        context.Students.AddRange(student1, student2, student3, student4, student5);
        context.Instructors.AddRange(instructor1, instructor2);
        context.TrainingTracks.AddRange(track1, track2, track3);
        await context.SaveChangesAsync();

        var enrollment1 = new Enrollment
        {
            StudentId = student1.StudentId,
            TrainingTrackId = track1.TrainingTrackId,
            EnrollmentDate = seed.AddDays(2),
            Status = EnrollmentStatus.Active,
            ProgressPercentage = 25,
            CreatedAt = seed
        };
        var enrollment2 = new Enrollment
        {
            StudentId = student2.StudentId,
            TrainingTrackId = track1.TrainingTrackId,
            EnrollmentDate = seed.AddDays(3),
            Status = EnrollmentStatus.Pending,
            CreatedAt = seed
        };
        var enrollment3 = new Enrollment
        {
            StudentId = student3.StudentId,
            TrainingTrackId = track2.TrainingTrackId,
            EnrollmentDate = seed.AddDays(4),
            Status = EnrollmentStatus.Active,
            ProgressPercentage = 10,
            CreatedAt = seed
        };

        context.Enrollments.AddRange(enrollment1, enrollment2, enrollment3);
        await context.SaveChangesAsync();

        context.Payments.AddRange(
            new Payment
            {
                EnrollmentId = enrollment1.EnrollmentId,
                Amount = 5000m,
                PaymentMethod = PaymentMethod.Online,
                PaymentDate = seed.AddDays(2),
                PaymentStatus = PaymentStatus.Paid,
                ReferenceNumber = "PAY-001"
            },
            new Payment
            {
                EnrollmentId = enrollment3.EnrollmentId,
                Amount = 1500m,
                PaymentMethod = PaymentMethod.Card,
                PaymentDate = seed.AddDays(5),
                PaymentStatus = PaymentStatus.Paid,
                ReferenceNumber = "PAY-002"
            });
        await context.SaveChangesAsync();
    }

    private static async Task SeedAuthUsersAsync(AppDbContext context, IConfiguration configuration)
    {
        if (await context.ApplicationUsers.AnyAsync()) return;

        var password = configuration["Seed:DefaultPassword"] ?? "TechMaster@123";
        var seed = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc);

        var instructor = await context.Instructors.FirstAsync(i => i.Email == "ahmed.nabil@techmaster.com");
        var student = await context.Students.FirstAsync(s => s.Email == "mohamed@example.com");

        var inactiveStudent = new Student
        {
            FullName = "Inactive Test Student",
            Email = "inactive@techmaster.test",
            CreatedAt = seed,
            IsActive = true
        };
        context.Students.Add(inactiveStudent);
        await context.SaveChangesAsync();

        var users = new List<(ApplicationUser User, string Password)>
        {
            (new ApplicationUser
            {
                FullName = "TechMaster Admin",
                Email = "admin@techmaster.test",
                Role = AppRoles.Admin,
                IsActive = true,
                CreatedAt = seed
            }, password),
            (new ApplicationUser
            {
                FullName = "TechMaster Instructor",
                Email = "instructor@techmaster.test",
                Role = AppRoles.Instructor,
                IsActive = true,
                CreatedAt = seed,
                InstructorId = instructor.InstructorId
            }, password),
            (new ApplicationUser
            {
                FullName = "TechMaster Student",
                Email = "student@techmaster.test",
                Role = AppRoles.Student,
                IsActive = true,
                CreatedAt = seed,
                StudentId = student.StudentId
            }, password),
            (new ApplicationUser
            {
                FullName = "Inactive Test Student",
                Email = "inactive@techmaster.test",
                Role = AppRoles.Student,
                IsActive = false,
                CreatedAt = seed,
                StudentId = inactiveStudent.StudentId
            }, password)
        };

        foreach (var (user, userPassword) in users)
        {
            user.PasswordHash = PasswordHelper.HashPassword(user, userPassword);
            context.ApplicationUsers.Add(user);
        }

        await context.SaveChangesAsync();

        instructor.UserId = users[1].User.Id;
        student.UserId = users[2].User.Id;
        inactiveStudent.UserId = users[3].User.Id;
        await context.SaveChangesAsync();
    }
}
