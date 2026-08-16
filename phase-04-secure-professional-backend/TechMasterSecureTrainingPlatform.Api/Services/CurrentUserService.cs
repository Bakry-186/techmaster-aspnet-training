using System.Security.Claims;

namespace TrainingCenter.Api.Services;
public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    int? StudentId { get; }
    int? InstructorId { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public int? UserId => int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public string? Role => User?.FindFirstValue(ClaimTypes.Role);

    public int? StudentId => int.TryParse(User?.FindFirstValue("studentId"), out var id) ? id : null;

    public int? InstructorId => int.TryParse(User?.FindFirstValue("instructorId"), out var id) ? id : null;

    public bool IsInRole(string role) => Role == role;
}
