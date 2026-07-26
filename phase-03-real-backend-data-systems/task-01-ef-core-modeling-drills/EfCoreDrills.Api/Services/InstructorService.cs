using EfCoreDrills.Api.Data;
using EfCoreDrills.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDrills.Api.Services;

public class InstructorService(AppDbContext context)
{
    public async Task<IReadOnlyList<InstructorTrackDto>> GetTracksAsync(int instructorId)
    {
        return await context.TrainingTracks
            .AsNoTracking()
            .Where(track => track.InstructorId == instructorId && !track.IsDeleted)
            .Select(track => new InstructorTrackDto
            {
                Id = track.Id,
                Title = track.Title
            })
            .ToListAsync();
    }
}
