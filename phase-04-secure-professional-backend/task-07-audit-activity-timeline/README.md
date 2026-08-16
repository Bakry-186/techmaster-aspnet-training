# Task 07 - Audit Activity Timeline

## Status: Done

## Entity

`ActivityLog` — UserId, UserEmail, UserRole, Action, EntityType, EntityId, Details, CreatedAt

## Endpoint

`GET /api/admin/activity-logs` — Admin only, supports filters: action, entityType, from, to, pagination

## Logged Operations

- [x] Login / Register / ChangePassword
- [x] Track create / update
- [x] Enrollment create / status change / approve
- [x] Payment create / status change
- [x] Student profile update (portal)
- [x] Instructor session create / update

## Files

`Entities/ActivityLog.cs`, `Services/AuditService.cs`, `Controllers/AdminPortalController.cs`
