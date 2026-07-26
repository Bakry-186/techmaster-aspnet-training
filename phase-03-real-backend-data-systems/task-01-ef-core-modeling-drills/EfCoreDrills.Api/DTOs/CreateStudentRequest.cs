namespace EfCoreDrills.Api.DTOs;

public class CreateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
