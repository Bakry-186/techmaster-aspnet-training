using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Controllers.OriginalBadCode;

/// <summary>
/// ORIGINAL BAD CODE — preserved for Task 08 refactor review.
/// Demonstrates insecure auth patterns: no hashing, no roles, no validation.
/// </summary>
[ApiController]
[Route("api/bad-auth")]
public class BadAuthController(AppDbContext db) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
            return Ok(new { message = "not found" });

        if (user.PasswordHash != password)
            return Ok(new { message = "wrong password" });

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.PasswordHash,
            user.Role
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(ApplicationUser user)
    {
        user.PasswordHash = user.PasswordHash;
        user.CreatedAt = DateTime.Now;
        db.ApplicationUsers.Add(user);
        await db.SaveChangesAsync();
        return Ok(user);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await db.ApplicationUsers.ToListAsync();
        return Ok(users);
    }
}
