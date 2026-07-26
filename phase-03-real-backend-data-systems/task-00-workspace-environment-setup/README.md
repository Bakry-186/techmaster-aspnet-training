# Task 00 - Workspace & Environment Setup

Prepare the Phase 03 workspace before building the database system.

## Phase 03 Local Setup

1. Install SQL Server or use a remote SQL Server database.
2. Update `ConnectionStrings:DefaultConnection` locally in `appsettings.Development.json`.
3. Install EF Core tools (if missing):
   ```bash
   dotnet tool install --global dotnet-ef
   ```
4. Run the API:
   ```bash
   cd phase-03-real-backend-data-systems/task-00-workspace-environment-setup/TrainingCenter.Api
   dotnet run
   ```
5. Open Swagger from the terminal URL.

> **Note:** Production connection string is configured on the hosting provider panel and is not stored in this repository.

## EF Core Packages Installed

- `Microsoft.EntityFrameworkCore.SqlServer` (8.0.13)
- `Microsoft.EntityFrameworkCore.Tools` (8.0.13)
- `Microsoft.EntityFrameworkCore.Design` (8.0.13)

## What Is Configured

- Controller-based Web API
- Swagger / OpenAPI in Development
- `AppDbContext` registered with SQL Server
- Sample connection string (Windows auth, no password)
- `Common/ApiResponse.cs` and `Common/PaginationResult.cs` placeholders

## Project Structure

```
TrainingCenter.Api/
  Controllers/
  Data/
    AppDbContext.cs
  Entities/
  DTOs/
  Services/
  Common/
    ApiResponse.cs
    PaginationResult.cs
  Program.cs
  appsettings.json
  appsettings.Development.json
```

## Acceptance Criteria

- [x] Project runs locally
- [x] EF Core packages installed
- [x] DbContext registered
- [x] No secrets committed (local Windows auth only)
- [x] README explains local setup commands
