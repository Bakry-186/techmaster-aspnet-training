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
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<TrackSession> TrackSessions => Set<TrackSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.StudentId);
            entity.HasIndex(s => s.Email)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(i => i.InstructorId);
            entity.HasIndex(i => i.Email).IsUnique();
        });

        modelBuilder.Entity<TrainingTrack>(entity =>
        {
            entity.HasKey(t => t.TrainingTrackId);
            entity.HasIndex(t => t.Code)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
            entity.Property(t => t.Fee).HasPrecision(18, 2);
            entity.HasOne(t => t.Instructor)
                .WithMany(i => i.Tracks)
                .HasForeignKey(t => t.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId);
            entity.HasIndex(e => new { e.StudentId, e.TrainingTrackId })
                .IsUnique()
                .HasFilter("[Status] IN (0, 1)");
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

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Role);
            entity.HasOne(u => u.Student)
                .WithMany()
                .HasForeignKey(u => u.StudentId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(u => u.Instructor)
                .WithMany()
                .HasForeignKey(u => u.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(a => a.ActivityLogId);
            entity.HasIndex(a => a.CreatedAt);
            entity.HasIndex(a => new { a.EntityType, a.EntityId });
        });

        modelBuilder.Entity<TrackSession>(entity =>
        {
            entity.HasKey(s => s.TrackSessionId);
            entity.HasOne(s => s.TrainingTrack)
                .WithMany(t => t.Sessions)
                .HasForeignKey(s => s.TrainingTrackId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
