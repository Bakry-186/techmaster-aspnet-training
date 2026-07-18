using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.Models.DTOs;

public class UpdateStudentStatus
{
    [Required]
    public bool IsActive { get; set; }
}
