using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Data;
using TrainingCenter.Api.Middleware;
using TrainingCenter.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<InstructorService>();
builder.Services.AddScoped<TrackService>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        await DbSeeder.SeedAsync(scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }
}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }
