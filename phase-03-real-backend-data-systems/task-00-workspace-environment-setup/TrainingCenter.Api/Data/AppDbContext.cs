using Microsoft.EntityFrameworkCore;

namespace TrainingCenter.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
