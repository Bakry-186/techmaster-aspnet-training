using System.ComponentModel.DataAnnotations;
using StudentManagementApi.Validation;

namespace StudentManagementApi.Models.DTOs;

public class UpdateStudentDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Phone { get; set; } = string.Empty;
    [Required]
    public string TrackName { get; set; } = string.Empty;
}
