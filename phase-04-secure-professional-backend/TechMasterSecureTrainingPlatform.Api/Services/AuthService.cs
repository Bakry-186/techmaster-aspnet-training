using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;
using TrainingCenter.Api.Helpers;

namespace TrainingCenter.Api.Services;

public class AuthService(AppDbContext context, JwtTokenService jwtTokenService, AuditService auditService)
{
    private static readonly HashSet<string> AllowedRegisterRoles =
        [AppRoles.Student, AppRoles.Instructor];

    public async Task<(RegisterResponse? Data, string? Error)> RegisterAsync(RegisterRequest request)
    {
        var validationError = ValidateRegisterRequest(request);
        if (validationError is not null)
            return (null, validationError);

        var normalizedEmail = NormalizeEmail(request.Email);
        var emailExists = await context.ApplicationUsers
            .AnyAsync(u => u.Email == normalizedEmail);

        if (emailExists)
            return (null, "A user with this email already exists.");

        var now = DateTime.UtcNow;
        int? studentId = null;
        int? instructorId = null;

        if (request.Role == AppRoles.Student)
        {
            var studentEmailTaken = await context.Students
                .AnyAsync(s => s.Email == normalizedEmail && !s.IsDeleted);
            if (studentEmailTaken)
                return (null, "A student profile with this email already exists.");

            var student = new Student
            {
                FullName = request.FullName.Trim(),
                Email = normalizedEmail,
                CreatedAt = now,
                IsActive = true
            };
            context.Students.Add(student);
            await context.SaveChangesAsync();
            studentId = student.StudentId;
        }
        else if (request.Role == AppRoles.Instructor)
        {
            var instructorEmailTaken = await context.Instructors
                .AnyAsync(i => i.Email == normalizedEmail);
            if (instructorEmailTaken)
                return (null, "An instructor profile with this email already exists.");

            var instructor = new Instructor
            {
                FullName = request.FullName.Trim(),
                Email = normalizedEmail,
                Specialization = "General",
                CreatedAt = now,
                IsActive = true
            };
            context.Instructors.Add(instructor);
            await context.SaveChangesAsync();
            instructorId = instructor.InstructorId;
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            Role = request.Role,
            IsActive = true,
            CreatedAt = now,
            StudentId = studentId,
            InstructorId = instructorId
        };

        user.PasswordHash = PasswordHelper.HashPassword(user, request.Password);
        context.ApplicationUsers.Add(user);
        await context.SaveChangesAsync();

        if (studentId.HasValue)
        {
            var student = await context.Students.FindAsync(studentId.Value);
            if (student is not null)
                student.UserId = user.Id;
        }

        if (instructorId.HasValue)
        {
            var instructor = await context.Instructors.FindAsync(instructorId.Value);
            if (instructor is not null)
                instructor.UserId = user.Id;
        }

        await context.SaveChangesAsync();

        await auditService.LogAsync("Register", "ApplicationUser", user.Id,
            $"New {user.Role} registered: {user.Email}");

        return (new RegisterResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        }, null);
    }

    public async Task<(AuthResponse? Data, string? Error, int StatusCode)> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return (null, "Email and password are required.", StatusCodes.Status400BadRequest);

        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await context.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

        if (user is null || !PasswordHelper.VerifyPassword(user, request.Password))
            return (null, "Invalid email or password.", StatusCodes.Status401Unauthorized);

        if (!user.IsActive)
            return (null, "This account is inactive.", StatusCodes.Status401Unauthorized);

        user.LastLoginAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await auditService.LogAsync("Login", "ApplicationUser", user.Id, $"User logged in: {user.Email}");

        return (jwtTokenService.CreateAuthResponse(user), null, StatusCodes.Status200OK);
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync(int userId)
    {
        var user = await context.ApplicationUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user is null
            ? null
            : new CurrentUserResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                LinkedStudentId = user.StudentId,
                LinkedInstructorId = user.InstructorId
            };
    }

    public async Task<(bool Success, string? Error)> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CurrentPassword)
            || string.IsNullOrWhiteSpace(request.NewPassword)
            || string.IsNullOrWhiteSpace(request.ConfirmNewPassword))
            return (false, "All password fields are required.");

        if (request.NewPassword != request.ConfirmNewPassword)
            return (false, "New password and confirmation do not match.");

        var passwordError = PasswordHelper.ValidatePassword(request.NewPassword);
        if (passwordError is not null)
            return (false, passwordError);

        var user = await context.ApplicationUsers.FindAsync(userId);
        if (user is null)
            return (false, "User not found.");

        if (!PasswordHelper.VerifyPassword(user, request.CurrentPassword))
            return (false, "Current password is incorrect.");

        user.PasswordHash = PasswordHelper.HashPassword(user, request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await auditService.LogAsync("ChangePassword", "ApplicationUser", user.Id, "Password changed.");

        return (true, null);
    }

    private static string? ValidateRegisterRequest(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return "Full name is required.";

        if (string.IsNullOrWhiteSpace(request.Email))
            return "Email is required.";

        if (!IsValidEmail(request.Email))
            return "Email format is invalid.";

        var passwordError = PasswordHelper.ValidatePassword(request.Password);
        if (passwordError is not null)
            return passwordError;

        if (request.Password != request.ConfirmPassword)
            return "Password and confirmation do not match.";

        if (string.IsNullOrWhiteSpace(request.Role))
            return "Role is required.";

        if (request.Role == AppRoles.Admin)
            return "Admin accounts cannot be created through public registration.";

        if (!AllowedRegisterRoles.Contains(request.Role))
            return "Invalid role. Allowed roles: Student, Instructor.";

        return null;
    }

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();

    private static bool IsValidEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
