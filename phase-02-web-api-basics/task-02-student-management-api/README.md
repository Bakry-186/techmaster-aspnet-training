# Task 02 - Student Management API

Full in-memory CRUD API for student records with search, filtering, pagination, and stats.

## Run

```bash
cd phase-02-web-api-basics/task-02-student-management-api/StudentManagementApi
dotnet run
```

Swagger: https://localhost:7128/swagger or http://localhost:5254/swagger

## Endpoints

| Method | Route | Description | Status |
|--------|-------|-------------|--------|
| GET | /api/students | List with search, track filter, isActive filter, pagination | Done |
| GET | /api/students/{id} | Get by id | Done |
| POST | /api/students | Create student | Done |
| PUT | /api/students/{id} | Full update | Done |
| PATCH | /api/students/{id}/status | Activate/deactivate | Done |
| DELETE | /api/students/{id} | Delete student | Done |
| GET | /api/students/by-track/{trackName} | Filter by track | Done |
| GET | /api/students/stats | Totals and counts by track | Done |

## Query parameters (GET /api/students)

| Param | Example | Purpose |
|-------|---------|---------|
| search | `abdel` | Search name or email |
| trackName | `ASP.NET Backend` | Filter by track |
| isActive | `true` | Filter active/inactive |
| pageNumber | `1` | Page number |
| pageSize | `10` | Items per page |

## Error response shape

```json
{
  "success": false,
  "message": "Resource not found",
  "code": "NOT_FOUND",
  "details": ["Student with id 99 was not found."]
}
```

## Project structure

```
StudentManagementApi/
  Controllers/StudentsController.cs
  Models/Student.cs
  Models/DTOs/
  Interfaces/IStudentService.cs
  Services/StudentService.cs
  Validation/UniqueEmailAttribute.cs
  Program.cs
```
