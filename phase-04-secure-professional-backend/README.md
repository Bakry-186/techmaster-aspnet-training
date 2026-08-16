# Phase 04 — Secure Professional Backend

Upgrade the Phase 03 Training Center API into **TechMaster Secure Training Platform API** with JWT auth, role-based access, audit trail, and production-ready architecture.

## Project

`TechMasterSecureTrainingPlatform.Api/` — secure backend built on Phase 03 baseline (with all mentor review fixes).

## Sprint Backlog

| Task | Focus | Status |
|------|-------|--------|
| 00 | Sprint setup | Done |
| 01 | Auth foundation (JWT, register, login, me) | Done |
| 02 | Role stories & access control | Done |
| 03 | Secure platform upgrade (portals) | Done |
| 04 | Professional architecture refactor | Done |
| 05 | Validation, errors, logging | Done |
| 06 | Production redeployment | Docs ready — deploy when hosting available |
| 07 | Audit activity timeline | Done |
| 08 | Bad auth refactor pack | Done |
| 09 | Demo & LinkedIn showcase | Template ready |

## Core Rule

**No anonymous CRUD.** All admin endpoints require JWT + Admin role. Students and instructors use dedicated portal routes.

## Quick Start

```bash
cd phase-04-secure-professional-backend/TechMasterSecureTrainingPlatform.Api
dotnet ef database update
dotnet run
```

Swagger: `http://localhost:5120/swagger`

## Seed Users (local dev only)

Password: see `appsettings.Development.json` → `Seed:DefaultPassword`

| Email | Role |
|-------|------|
| admin@techmaster.test | Admin |
| instructor@techmaster.test | Instructor (linked to Dr. Ahmed Nabil) |
| student@techmaster.test | Student (linked to Mohamed Ayman) |
| inactive@techmaster.test | Inactive (login rejected) |

## Key Endpoints

### Auth (public + authenticated)
- `POST /api/auth/register` — Student/Instructor only
- `POST /api/auth/login`
- `GET /api/auth/me`
- `POST /api/auth/change-password`

### Student Portal (`Student` role)
- `GET /api/student/me`
- `PUT /api/student/profile`
- `GET /api/student/my-enrollments`
- `GET /api/student/my-payments`
- `GET /api/student/available-tracks`
- `POST /api/student/enrollment-requests`

### Instructor Portal (`Instructor` role)
- `GET /api/instructor/my-tracks`
- `GET /api/instructor/tracks/{id}/students`
- `GET /api/instructor/tracks/{id}/sessions`
- `POST /api/instructor/tracks/{id}/sessions`
- `PUT /api/instructor/sessions/{id}`
- `GET /api/instructor/tracks/{id}/progress`

### Admin (`Admin` role)
- All Phase 03 CRUD: `/api/students`, `/api/instructors`, `/api/tracks`, `/api/enrollments`, `/api/payments`, `/api/reports`
- `PUT /api/admin/enrollments/{id}/approve`
- `GET /api/admin/activity-logs`

## Postman

- [TechMaster-Phase04-Secure-Platform.postman_collection.json](./postman/TechMaster-Phase04-Secure-Platform.postman_collection.json)

## Architecture Highlights

- **Auth:** JWT Bearer + ASP.NET Core PasswordHasher
- **Roles:** Admin / Instructor / Student via `[Authorize(Roles = ...)]`
- **Services:** Controllers → Services → DbContext (Phase 04 extensions in `Extensions/ServiceCollectionExtensions.cs`)
- **Audit:** `ActivityLog` entity + `AuditService` for login, enrollments, payments, tracks
- **Errors:** `ApiResponse<T>` with `Errors[]` + `GlobalExceptionMiddleware`
- **Logging:** `RequestLoggingMiddleware` with request id
- **Bad code:** `Controllers/OriginalBadCode/BadAuthController.cs` (intentionally insecure — compare with refactored auth)

## Evidence Checklist

- [ ] Swagger screenshots with Authorize button
- [ ] Postman: login + role-based 401/403 tests
- [ ] Postman: student portal + instructor portal flows
- [ ] Activity logs screenshot (admin)
- [ ] Live deployment URL (Task 06)
- [ ] Demo video (Task 09)
- [ ] LinkedIn post screenshot (Task 09)

## Database

Same database as Phase 03: `TechMasterTrainingCenterDb`

New migration: `phase04_secure_platform_upgrade` (ApplicationUsers, ActivityLogs, TrackSessions, UserId links)
