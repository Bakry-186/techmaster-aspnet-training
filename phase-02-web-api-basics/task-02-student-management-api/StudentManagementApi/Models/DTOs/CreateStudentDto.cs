using System.ComponentModel.DataAnnotations;
using StudentManagementApi.Validation;

namespace StudentManagementApi.Models.DTOs;

public class CreateStudentDto
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
    public string? GithubUrl { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; } = string.Empty;
}
