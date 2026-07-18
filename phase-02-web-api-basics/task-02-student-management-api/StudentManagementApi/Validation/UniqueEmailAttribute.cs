using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueEmailAttribute : ValidationAttribute
{
    public UniqueEmailAttribute()
    {
        ErrorMessage = "Email must be unique.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Uniqueness is enforced in StudentService during create/update.
        return ValidationResult.Success;
    }
}
