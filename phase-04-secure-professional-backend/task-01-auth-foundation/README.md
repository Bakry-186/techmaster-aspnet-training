# Task 01 - Authentication Foundation

## Status: Done

| Method | Route | Auth |
|--------|-------|------|
| POST | `/api/auth/register` | Public |
| POST | `/api/auth/login` | Public |
| GET | `/api/auth/me` | Bearer JWT |
| POST | `/api/auth/change-password` | Bearer JWT |

## Deliverables

- [x] `ApplicationUser` entity + migration
- [x] Password hashing via `PasswordHelper` (ASP.NET Core PasswordHasher)
- [x] JWT with user id, email, role, linked profile claims
- [x] Seed users: admin, instructor, student, inactive
- [x] Swagger Bearer auth support
- [x] Postman collection

## Files

`Entities/ApplicationUser.cs`, `Services/AuthService.cs`, `Services/JwtTokenService.cs`, `Controllers/AuthController.cs`, `Helpers/PasswordHelper.cs`, `DTOs/AuthDtos.cs`
