using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Entities;

namespace TrainingCenter.Api.Services;

public class AuditService(AppDbContext context, ICurrentUserService currentUser)
{
    public async Task LogAsync(string action, string entityType, int? entityId, string? details = null)
    {
        var log = new ActivityLog
        {
            UserId = currentUser.UserId,
            UserEmail = currentUser.Email ?? "system",
            UserRole = currentUser.Role ?? "Anonymous",
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.UtcNow
        };

        context.ActivityLogs.Add(log);
        await context.SaveChangesAsync();
    }

    public async Task<(IReadOnlyList<ActivityLogResponse> Data, string? Error)> GetLogsAsync(
        string? action, string? entityType, DateTime? from, DateTime? to, int pageNumber = 1, int pageSize = 20)
    {
        if (pageNumber <= 0 || pageSize is < 1 or > 50)
            return ([], "Invalid pagination parameters.");

        var query = context.ActivityLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(l => l.Action == action);
        if (!string.IsNullOrWhiteSpace(entityType))
            query = query.Where(l => l.EntityType == entityType);
        if (from.HasValue)
            query = query.Where(l => l.CreatedAt >= from.Value);
        if (to.HasValue)
            query = query.Where(l => l.CreatedAt <= to.Value);

        var data = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new ActivityLogResponse
            {
                ActivityLogId = l.ActivityLogId,
                UserId = l.UserId,
                UserEmail = l.UserEmail,
                UserRole = l.UserRole,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityId = l.EntityId,
                Details = l.Details,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return (data, null);
    }
}
