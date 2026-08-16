# Task 02 - Role Stories & Access Control

## Status: Done

## Access Matrix

| Endpoint Group | Admin | Instructor | Student |
|----------------|-------|------------|---------|
| Students CRUD | Full | — | — |
| Instructors CRUD | Full | — | — |
| Tracks CRUD | Full | — | — |
| Enrollments | Full | — | Portal only |
| Payments | Full | — | Own via portal |
| Reports | Full | — | — |
| Activity Logs | Full | — | — |

## Authorization Tests Verified

- [x] No token → **401** on `/api/students`
- [x] Student token on admin endpoint → **403**
- [x] Admin token → full access
- [x] Instructor token → instructor portal only
- [x] Student token → student portal only

## Implementation

- `[Authorize(Roles = AppRoles.Admin)]` on all Phase 03 admin controllers
- `ICurrentUserService` / `CurrentUserService` for claim-based identity
- Dedicated portal controllers with role-specific routes
