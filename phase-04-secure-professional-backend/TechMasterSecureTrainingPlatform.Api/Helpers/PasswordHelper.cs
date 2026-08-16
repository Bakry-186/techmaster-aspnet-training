using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Helpers;

public static partial class PasswordHelper
{
    private static readonly PasswordHasher<ApplicationUser> Hasher = new();

    public static string HashPassword(ApplicationUser user, string password) =>
        Hasher.HashPassword(user, password);

    public static bool VerifyPassword(ApplicationUser user, string password) =>
        Hasher.VerifyHashedPassword(user, user.PasswordHash, password)
            is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;

    public static string? ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "Password is required.";

        if (password.Length < 8)
            return "Password must be at least 8 characters.";

        if (!UppercaseRegex().IsMatch(password))
            return "Password must contain at least one uppercase letter.";

        if (!LowercaseRegex().IsMatch(password))
            return "Password must contain at least one lowercase letter.";

        if (!DigitRegex().IsMatch(password))
            return "Password must contain at least one digit.";

        return null;
    }

    [GeneratedRegex("[A-Z]")]
    private static partial Regex UppercaseRegex();

    [GeneratedRegex("[a-z]")]
    private static partial Regex LowercaseRegex();

    [GeneratedRegex("[0-9]")]
    private static partial Regex DigitRegex();
}
