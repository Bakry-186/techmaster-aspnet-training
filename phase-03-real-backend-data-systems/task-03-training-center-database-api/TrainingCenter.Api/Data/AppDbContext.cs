using TrainingCenter.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace TrainingCenter.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<TrainingTrack> TrainingTracks => Set<TrainingTrack>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.StudentId);
            entity.HasIndex(s => s.Email).IsUnique();
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(i => i.InstructorId);
            entity.HasIndex(i => i.Email).IsUnique();
        });

        modelBuilder.Entity<TrainingTrack>(entity =>
        {
            entity.HasKey(t => t.TrainingTrackId);
            entity.HasIndex(t => t.Code).IsUnique();
            entity.Property(t => t.Fee).HasPrecision(18, 2);
            entity.HasOne(t => t.Instructor)
                .WithMany(i => i.Tracks)
                .HasForeignKey(t => t.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId);
            entity.Property(e => e.ProgressPercentage).HasPrecision(5, 2);
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.TrainingTrack)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(e => e.TrainingTrackId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);
            entity.Property(p => p.Amount).HasPrecision(18, 2);
            entity.HasOne(p => p.Enrollment)
                .WithMany(e => e.Payments)
                .HasForeignKey(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
