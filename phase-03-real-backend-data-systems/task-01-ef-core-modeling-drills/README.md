# Task 01 - EF Core Modeling Drills

Practice project for all 10 EF Core modeling drills before building the main Training Center API in Task 03.

## How To Run

```bash
cd phase-03-real-backend-data-systems/task-01-ef-core-modeling-drills/EfCoreDrills.Api
dotnet ef database update
dotnet run
```

Open the Swagger URL shown in the terminal.

Database: `TechMasterEfDrillsDb` (local SQL Server, Windows auth).

## Migrations

| Migration | Drill | What it adds |
|-----------|-------|--------------|
| `InitialStudentSchema` | 01 | `Students` table, DbContext, first migration |
| `AddStudentProfile` | 02–08 | Profiles, instructors, tracks, enrollments, payment summaries, soft delete, audit fields |

## Drill Summary

| # | Concept | Implementation |
|---|---------|----------------|
| 01 | DbContext / DbSet / Migration | `Student` entity, `AppDbContext`, `InitialStudentSchema` |
| 02 | One-to-one | `Student` ↔ `StudentProfile` with unique `StudentId` FK |
| 03 | One-to-many | `Instructor` → many `TrainingTrack`, required `InstructorId` |
| 04 | Many-to-many via join entity | `Enrollment` links `Student` + `TrainingTrack` with `Status`, `EnrollmentDate`, `FinalGrade` |
| 05 | One-to-one payment summary | `PaymentSummary` per `Enrollment`, decimal money fields, `PaymentStatus` enum |
| 06 | Seed data | `DbSeeder` runs once on startup if database is empty |
| 07 | Soft delete | `IsDeleted` / `DeletedAt` on `Student`; DELETE marks row, GET excludes deleted |
| 08 | Audit fields | `CreatedAt` / `UpdatedAt` set in service layer using UTC |
| 09 | Projection DTOs | `.Select()` to `StudentListItemDto`, `TrackDetailsDto`, `EnrollmentDetailsDto` |
| 10 | Pagination | `PaginationResult<T>` with `pageNumber`, `pageSize`, `totalCount`, `totalPages` |

## Endpoints

| Method | Route | Drill | Purpose |
|--------|-------|-------|---------|
| GET | `/api/students?pageNumber=1&pageSize=5` | 09, 10 | Paginated student list (DTO projection) |
| GET | `/api/students?includeDeleted=true` | 07 | Include soft-deleted students |
| GET | `/api/students/{id}` | 02, 04, 09 | Student details with profile and enrollments |
| POST | `/api/students` | 08 | Create student (`CreatedAt` auto-set) |
| PUT | `/api/students/{id}` | 08 | Update student (`UpdatedAt` auto-set) |
| DELETE | `/api/students/{id}` | 07 | Soft delete (returns 204) |
| GET | `/api/instructors/{id}/tracks` | 03 | Instructor tracks (one-to-many) |
| GET | `/api/tracks/{id}` | 09 | Track details DTO with instructor name |
| POST | `/api/tracks` | 03 | Create track (validates instructor exists) |
| GET | `/api/enrollments/{id}` | 04, 05, 09 | Enrollment with payment summary projection |

## Pagination Rules (Drill 10)

- `pageNumber` must be > 0 → otherwise **400**
- `pageSize` must be between 1 and 50 → otherwise **400**
- Formula: `skip = (pageNumber - 1) * pageSize`

Example response:

```json
{
  "items": [...],
  "totalCount": 5,
  "pageNumber": 1,
  "pageSize": 5,
  "totalPages": 1
}
```

## Seed Data (Drill 06)

Seeded automatically on first run (idempotent — no duplicates on restart):

| Entity | Count | Sample |
|--------|-------|--------|
| Students | 5 | Mohamed Ayman (id 1) has a profile |
| Instructors | 2 | Dr. Ahmed Nabil (id 1) |
| Tracks | 3 | ASP.NET Backend Career (id 1) |
| Enrollments | 5 | Enrollment 1 = Mohamed → ASP.NET track |
| Payment summaries | 4 | Enrollment 1 is fully paid |

## Relationship Diagram

```text
Student 1──1 StudentProfile
Student 1──* Enrollment *──1 TrainingTrack *──1 Instructor
Enrollment 1──1 PaymentSummary
```

## Key Learnings

- **DbContext** = session with the database; **DbSet** = table collection
- Use a **join entity** (`Enrollment`) when the relationship carries business data
- Use **DTOs + Select** instead of returning EF entities with heavy `Include` chains
- **Soft delete** keeps history; filter with `!IsDeleted` in queries
- **Audit fields** should be set by the server, not the client

## Evidence Checklist

- [x] Screenshot: `Students` table in SQL Server
- [x] Screenshot: Swagger paginated students response
- [x] Screenshot: Student with profile (`GET /api/students/1`)
- [x] Screenshot: Instructor tracks (`GET /api/instructors/1/tracks`)
- [x] Screenshot: Enrollment with payment summary (`GET /api/enrollments/1`)
- [x] Screenshot: Soft delete before/after (row still in DB, hidden from GET)

## Acceptance Criteria

- [x] All 10 drills implemented
- [x] Migrations created and applied
- [x] Seed data on first run without duplicates
- [x] DTO projection on list/detail endpoints
- [x] Pagination with validation
- [x] Soft delete on students
- [x] Audit fields on create/update
