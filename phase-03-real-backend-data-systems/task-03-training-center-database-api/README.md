# Task 03 - Training Center Database API

Main Phase 03 system: database-driven API for TechMaster Academy.

## How To Run

```bash
cd phase-03-real-backend-data-systems/task-03-training-center-database-api/TrainingCenter.Api
dotnet ef database update
dotnet run
```

Database: `TechMasterTrainingCenterDb` (local SQL Server, Windows auth).

## Stack

- ASP.NET Core 8 Web API
- EF Core 8 + SQL Server
- Controllers + Services + DTOs
- Swagger / OpenAPI

## Endpoints

### Students
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/students` | Paginated list with search, isActive, includeDeleted |
| GET | `/api/students/{id}` | Details with enrollment summary |
| POST | `/api/students` | Create (unique email) |
| PUT | `/api/students/{id}` | Update |
| DELETE | `/api/students/{id}` | Soft delete |
| GET | `/api/students/{id}/enrollments` | Enrollment history |

### Instructors
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/instructors` | List all |
| GET | `/api/instructors/{id}` | Details |
| POST | `/api/instructors` | Create |
| PUT | `/api/instructors/{id}` | Update |
| GET | `/api/instructors/{id}/tracks` | Assigned tracks |

### Tracks
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/tracks` | Filter by keyword, level, status, instructorId |
| GET | `/api/tracks/{id}` | Details with capacity summary |
| POST | `/api/tracks` | Create |
| PUT | `/api/tracks/{id}` | Update |
| DELETE | `/api/tracks/{id}` | Soft delete |
| GET | `/api/tracks/{id}/students` | Enrolled students |

### Enrollments
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/enrollments` | Filter by status, trackId, studentId, paymentStatus |
| GET | `/api/enrollments/{id}` | Details with payments |
| POST | `/api/enrollments` | Enroll student (capacity + duplicate rules) |
| PUT | `/api/enrollments/{id}/status` | Status transition |

### Payments
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/payments` | Filter by date range and status |
| POST | `/api/payments` | Create payment |
| GET | `/api/enrollments/{id}/payments` | Payment history |
| PUT | `/api/payments/{id}/status` | Update status |

### Reports
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/reports/dashboard-summary` | High-level counts |
| GET | `/api/reports/unpaid-enrollments` | Unpaid / partial |
| GET | `/api/reports/track-capacity` | Capacity per track |
| GET | `/api/reports/revenue-summary` | Revenue totals |
| GET | `/api/reports/revenue-by-track` | Revenue grouped by track |
| GET | `/api/reports/tracks-with-available-seats` | Open seats |
| GET | `/api/reports/top-tracks` | Top 5 by enrollment |
| GET | `/api/reports/instructor-workload` | Tracks + students per instructor |
| GET | `/api/reports/students-without-payments` | Active/pending with no paid payment |

## Response Shape

```json
{
  "success": true,
  "message": "Student created successfully.",
  "data": { ... }
}
```

## Project Structure

```
TrainingCenter.Api/
  Controllers/
  Data/AppDbContext.cs, DbSeeder.cs
  Entities/
  DTOs/Dtos.cs
  Services/
  Common/ApiResponse.cs, PaginationResult.cs
  Migrations/
  postman/
```

## Acceptance Criteria

- [x] All core entities with PK/FK relationships
- [x] EF Core migration applied
- [x] CRUD endpoints with DTOs and services
- [x] Business rules enforced (see Task 05)
- [x] Report endpoints implemented
- [x] Seed data for testing
- [x] No secrets in repository
