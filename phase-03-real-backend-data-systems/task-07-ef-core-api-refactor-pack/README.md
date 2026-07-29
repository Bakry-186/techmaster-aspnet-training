# Task 07 - EF Core API Refactor Pack

Refactor bad EF Core enrollment code into production-ready patterns.

## Files

| Version | Location |
|---------|----------|
| **Original (bad)** | `task-03/.../Controllers/OriginalBadCode/BadEnrollmentsController.cs` |
| **Refactored (good)** | `task-03/.../Controllers/EnrollmentsController.cs` + `Services/EnrollmentService.cs` |

Route for bad code (testing only): `/api/bad-enrollments`

## 10 Problems Found

1. Returns full EF entities with navigation graphs — huge JSON payloads, circular reference risk.
2. No pagination or filtering on list endpoint.
3. Accepts `Enrollment` entity directly from request body — exposes internal model.
4. No input validation on create.
5. Sets status to `Active` immediately — skips Pending workflow.
6. Allows duplicate active enrollments for same student + track.
7. Ignores track capacity when enrolling.
8. Uses synchronous EF methods (`ToList`, `SaveChanges`) — blocks threads.
9. Wrong HTTP status codes — returns 200 for not-found and delete success.
10. Hard delete removes enrollment history permanently.

Additional payment issues:
11. No validation for zero/negative payment amounts.
12. No check that payment exceeds remaining balance.
13. Payment logic duplicated in controller instead of service layer.

## 10 Improvements Made

1. **DTO projection** — `EnrollmentListItemResponse`, `EnrollmentDetailsResponse` via `.Select()`.
2. **Service layer** — business logic moved to `EnrollmentService`.
3. **Request DTOs** — `CreateEnrollmentRequest`, `UpdateEnrollmentStatusRequest`.
4. **Async throughout** — `ToListAsync`, `SaveChangesAsync`, `FirstOrDefaultAsync`.
5. **Correct status codes** — 201 Created, 404 NotFound, 400 BadRequest, 204 NoContent.
6. **Duplicate enrollment check** — rejects second Pending/Active for same student+track.
7. **Capacity validation** — counts active enrollments vs track capacity.
8. **Soft delete pattern** — used on students/tracks; enrollments use status Cancelled.
9. **Status transitions** — valid workflow: Pending → Active → Completed/Cancelled.
10. **Standard response wrapper** — `ApiResponse<T>` with success/message/data.

## Refactored Endpoints

| Method | Route | Behavior |
|--------|-------|----------|
| GET | `/api/enrollments` | DTO list with filters |
| GET | `/api/enrollments/{id}` | DTO details with payments |
| POST | `/api/enrollments` | Validated create, starts Pending |
| PUT | `/api/enrollments/{id}/status` | Validated status transition |

Payments refactored in `PaymentService` + `PaymentsController`.
