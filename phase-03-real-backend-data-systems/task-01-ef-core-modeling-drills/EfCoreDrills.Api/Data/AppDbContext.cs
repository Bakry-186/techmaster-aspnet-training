using EfCoreDrills.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDrills.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<TrainingTrack> TrainingTracks => Set<TrainingTrack>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<PaymentSummary> PaymentSummaries => Set<PaymentSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.HasIndex(profile => profile.StudentId).IsUnique();

            entity.HasOne(profile => profile.Student)
                .WithOne(student => student.Profile)
                .HasForeignKey<StudentProfile>(profile => profile.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrainingTrack>(entity =>
        {
            entity.HasOne(track => track.Instructor)
                .WithMany(instructor => instructor.Tracks)
                .HasForeignKey(track => track.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasOne(enrollment => enrollment.Student)
                .WithMany(student => student.Enrollments)
                .HasForeignKey(enrollment => enrollment.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(enrollment => enrollment.TrainingTrack)
                .WithMany(track => track.Enrollments)
                .HasForeignKey(enrollment => enrollment.TrainingTrackId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PaymentSummary>(entity =>
        {
            entity.HasIndex(summary => summary.EnrollmentId).IsUnique();

            entity.Property(summary => summary.TotalRequired).HasPrecision(18, 2);
            entity.Property(summary => summary.TotalPaid).HasPrecision(18, 2);

            entity.HasOne(summary => summary.Enrollment)
                .WithOne(enrollment => enrollment.PaymentSummary)
                .HasForeignKey<PaymentSummary>(summary => summary.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Enrollment>()
            .Property(enrollment => enrollment.FinalGrade)
            .HasPrecision(5, 2);
    }
}
