namespace StudentManagementApi.Models.DTOs;

public class StudentStatsDto
{
    public int TotalStudents { get; set; }
    public int TotalActiveStudents { get; set; }
    public int TotalInactiveStudents { get; set; }
    public Dictionary<string, int> StudentsByTrack { get; set; } = new();
}
