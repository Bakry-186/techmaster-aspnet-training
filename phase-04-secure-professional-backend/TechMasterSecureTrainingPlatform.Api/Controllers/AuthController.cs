using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var (data, error) = await authService.RegisterAsync(request);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<RegisterResponse>.Ok(data!, "User registered successfully."));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var (data, error, statusCode) = await authService.LoginAsync(request);
        return error is not null
            ? StatusCode(statusCode, ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<AuthResponse>.Ok(data!, "Login successful."));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse<object>.Fail("Authentication required."));

        var user = await authService.GetCurrentUserAsync(userId);
        return user is null
            ? NotFound(ApiResponse<object>.Fail("User not found."))
            : Ok(ApiResponse<CurrentUserResponse>.Ok(user));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse<object>.Fail("Authentication required."));

        var (success, error) = await authService.ChangePasswordAsync(userId, request);
        return success
            ? Ok(ApiResponse<object>.Ok(new { }, "Password changed successfully."))
            : BadRequest(ApiResponse<object>.Fail(error!));
    }
}
