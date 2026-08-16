# Task 08 - Bad Auth Refactor Pack

## Status: Done

## Bad Code (intentionally insecure)

`Controllers/OriginalBadCode/BadAuthController.cs` at `/api/bad-auth`

Problems demonstrated:
- Plain-text password comparison (no hashing)
- Returns `PasswordHash` in login response
- No role enforcement
- Exposes all users via `GET /api/bad-auth/users`
- Accepts entity directly on register (mass assignment risk)
- Wrong HTTP status codes (`Ok` for not found)

## Refactored Code

`Controllers/AuthController.cs` + `Services/AuthService.cs`

Fixes:
- Password hashing with ASP.NET Core PasswordHasher
- JWT tokens — never expose secrets
- Role-controlled registration (no public Admin)
- Proper 400/401 status codes
- `[Authorize]` on protected endpoints
- Audit logging on auth events

## Interview Answer

Compare `/api/bad-auth/login` vs `/api/auth/login` — explain hashing, JWT, least privilege, and safe error responses.
