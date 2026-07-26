namespace EfCoreDrills.Api.Entities;

public class StudentProfile
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string NationalId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string EmergencyPhone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    public Student Student { get; set; } = null!;
}
