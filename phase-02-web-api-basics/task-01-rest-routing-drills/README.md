# Task 01 - REST & Routing Drills

15 API drills covering routing, query strings, validation, CRUD, pagination, headers, status codes, and standard error responses.

## Run

```bash
cd phase-02-web-api-basics/task-01-rest-routing-drills/RestRoutingDrills
dotnet run
```

Swagger: [https://localhost:7128/swagger](https://localhost:7128/swagger) or [http://localhost:5254/swagger](http://localhost:5254/swagger)

## Progress


| #   | Endpoint                                          | Concept                          | Status |     |
| --- | ------------------------------------------------- | -------------------------------- | ------ | --- |
| 01  | GET /api/health                                   | Basic JSON response              | Done   |     |
| 02  | GET /api/tools/echo/{name}                        | Route parameter                  | Done   |     |
| 03  | GET /api/calculator/add?a=10&b=5                  | Query string                     | Done   |     |
| 04  | GET /api/converter/celsius-to-fahrenheit?value=25 | Service + calculation            | Done   |     |
| 05  | GET /api/grades/calculate?score=85                | Validation + 400                 | Done   |     |
| 06  | POST /api/notes                                   | Request body DTO                 | Done   |     |
| 07  | GET /api/notes                                    | Collection response              | Done   |     |
| 08  | GET /api/notes/{id}                               | 404                              | Done   |     |
| 09  | PUT /api/notes/{id}                               | Full update                      | Done   |     |
| 10  | DELETE /api/notes/{id}                            | 204 delete                       | Done   |     |
| 11  | GET /api/notes/search?keyword=api                 | Search                           | Done   |     |
| 12  | GET /api/notes?pageNumber=1&pageSize=5            | Pagination                       | Done   |     |
| 13  | GET /api/request-info                             | Custom header                    | Done   |     |
| 14  | GET/POST/DELETE /api/status-codes/*               | Status codes 200/201/204/400/404 | Done   |     |
| 15  | GET /api/errors/demo                              | Standard error shape             | Done   |     |


## Standard Error Response Shape

All API error responses use the same JSON structure:

```json
{
  "success": false,
  "message": "Invalid request",
  "code": "BAD_REQUEST",
  "details": ["Name is required"]
}
```


| Field     | Type       | Description                                                   |
| --------- | ---------- | ------------------------------------------------------------- |
| `success` | `bool`     | Always `false` for errors                                     |
| `message` | `string`   | Short summary of what went wrong                              |
| `code`    | `string`   | Machine-readable error code (e.g. `BAD_REQUEST`, `NOT_FOUND`) |
| `details` | `string[]` | One or more specific error messages                           |


### Examples

**400 Bad Request** — `GET /api/errors/demo`

```json
{
  "success": false,
  "message": "Invalid request",
  "code": "BAD_REQUEST",
  "details": ["Name is required", "Score must be between 0 and 100"]
}
```

**404 Not Found** — `GET /api/errors/not-found`

```json
{
  "success": false,
  "message": "Resource not found",
  "code": "NOT_FOUND",
  "details": ["Note with id 99 was not found"]
}
```

### C# usage

```csharp
return BadRequest(new ApiErrorResponse
{
    Message = "Invalid request",
    Code = "BAD_REQUEST",
    Details = ["Name is required"]
});
```

## Evidence

- Swagger screenshots: Google Drive (to be added)
- Postman collection: (to be added)

