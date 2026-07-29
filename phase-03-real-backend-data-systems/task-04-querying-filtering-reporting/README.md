# Task 04 - Querying, Filtering, Pagination & Reports

All 20 query specifications implemented in the Task 03 API.

## Implemented Query Specs

| # | Route | EF Concept | Status |
|---|-------|------------|--------|
| 01 | `GET /api/students?search=mohamed` | Where + Contains + projection | Done |
| 02 | `GET /api/students?isActive=true` | Where on IsActive/IsDeleted | Done |
| 03 | `GET /api/students?pageNumber=1&pageSize=10` | CountAsync + Skip + Take | Done |
| 04 | `GET /api/tracks?keyword=backend` | Where + Contains | Done |
| 05 | `GET /api/tracks?level=Beginner` | Where on Level enum | Done |
| 06 | `GET /api/tracks?instructorId=1` | Where on InstructorId | Done |
| 07 | `GET /api/reports/tracks-with-available-seats` | Count active enrollments | Done |
| 08 | `GET /api/enrollments` | Select projection DTO | Done |
| 09 | `GET /api/enrollments?status=Pending` | Where on Status | Done |
| 10 | `GET /api/students/{id}/enrollments` | Where + Select | Done |
| 11 | `GET /api/tracks/{id}/students` | Where + Select Student DTO | Done |
| 12 | `GET /api/reports/unpaid-enrollments` | Payment balance filter | Done |
| 13 | `GET /api/payments?from=...&to=...` | Date range Where | Done |
| 14 | `GET /api/reports/revenue-summary` | Sum + Count | Done |
| 15 | `GET /api/reports/revenue-by-track` | GroupBy track | Done |
| 16 | `GET /api/reports/top-tracks` | OrderByDescending + Take(5) | Done |
| 17 | `GET /api/reports/instructor-workload` | Group by instructor | Done |
| 18 | `GET /api/reports/students-without-payments` | Any/All on payments | Done |
| 19 | `GET /api/enrollments?trackId=1&status=Active&paymentStatus=Paid` | Conditional filters | Done |
| 20 | `GET /api/reports/dashboard-summary` | Multiple aggregates | Done |

## Five Important Queries Explained

### 1. Paginated student search (Query 01 + 03)
Combines `Contains` on name/email/phone with `Skip`/`Take`. Validates pageNumber > 0 and pageSize 1–50. Returns metadata so clients can build pagination UI without loading all rows.

### 2. Unpaid enrollments report (Query 12)
Loads enrollments with payments, compares `TotalPaid` (sum of Paid payments) against `TrainingTrack.Fee`. Cancelled enrollments are excluded. Answers: "Who still owes money?"

### 3. Track capacity report (Query 07)
Counts only Pending + Active enrollments (cancelled seats are freed). Returns `capacity`, `activeEnrollments`, `remainingSeats` per track.

### 4. Revenue by track (Query 15)
Groups paid payments by track through enrollment FK. Uses decimal for money. Only `PaymentStatus.Paid` counts.

### 5. Advanced enrollment filter (Query 19)
Builds `IQueryable` conditionally — each query param is applied only when present. Prevents over-filtering when params are omitted.
