# Task 06 - API Standards Refactor Pack

Compare bad API code vs refactored standards-compliant code.

## Projects

| Project | Purpose |
|---------|---------|
| `OriginalBadCode/` | Intentionally bad controller (for learning) |
| `RefactoredApi/` | Refactored version with proper standards |

## Run

```bash
cd phase-02-web-api-basics/task-06-api-standards-refactor-pack/OriginalBadCode/OriginalBadCode
dotnet run

cd ../../RefactoredApi/RefactoredApi
dotnet run
```

## Problems in OriginalBadCode

1. Non-REST routes (`/products/getall`, `/products/add`)
2. Uses `[HttpPost]` for update and delete
3. Returns `200 OK` for errors and not-found cases
4. Public fields instead of properties
5. Static list inside controller (no service layer)
6. String query params instead of DTOs/body
7. No validation, no pagination safety
8. Inherits `Controller` instead of `ControllerBase` + `[ApiController]`

## Improvements in RefactoredApi

1. REST routes: `GET/POST/PUT/DELETE /api/products`
2. Correct status codes: 201, 204, 400, 404
3. DTOs for create/update
4. Service layer with interface + DI
5. Standard error response shape
6. Properties on model, validation in service

## Before vs After

| Area | Before | After |
|------|--------|-------|
| Routing | `/products/getbyid?id=1` | `GET /api/products/1` |
| Delete | `POST /products/delete` | `DELETE /api/products/1` → 204 |
| Not found | 200 + message | 404 + error JSON |
| Structure | All in controller | Controller → Service → Model |
