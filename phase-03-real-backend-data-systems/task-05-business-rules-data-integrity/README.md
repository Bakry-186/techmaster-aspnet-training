# Task 05 - Business Rules & Data Integrity

Business rules are enforced in the **service layer** (`Services/`), not only in README.

## Student Rules

| Rule | Implementation | Invalid Test |
|------|----------------|--------------|
| Email must be unique | `StudentService.CreateAsync/UpdateAsync` | POST duplicate email → 400 |
| FullName required | `StudentService.CreateAsync` | Empty name → 400 |
| Soft delete only | `StudentService.SoftDeleteAsync` | DELETE sets IsDeleted=true |
| Deleted hidden from lists | `GetPagedAsync` filters `!IsDeleted` | GET list excludes deleted |
| Inactive/deleted cannot enroll | `EnrollmentService.CreateAsync` | Enroll deleted student → 400 |

## Track Rules

| Rule | Implementation | Invalid Test |
|------|----------------|--------------|
| Title required | `TrackService.ValidateTrackAsync` | Empty title → 400 |
| Code unique | Unique index + validation | Duplicate code → 400 |
| Capacity > 0 | Validation | capacity=0 → 400 |
| StartDate < EndDate | Validation | Invalid dates → 400 |
| Instructor required | FK check | Missing instructor → 400 |
| Cannot exceed capacity | `EnrollmentService.CreateAsync` | Full track → 400 |
| Closed track rejects enrollment | Status check | Closed track → 400 |
| Soft delete blocked if active enrollments | `TrackService.SoftDeleteAsync` | → 400 |

## Enrollment Rules

| Rule | Implementation | Invalid Test |
|------|----------------|--------------|
| No duplicate active enrollment | Check Pending/Active for same student+track | → 400 |
| Starts as Pending | Default on create | POST → status Pending |
| Valid status transitions | `UpdateStatusAsync` switch | Invalid transition → 400 |
| Completed cannot be cancelled | Transition guard | → 400 |
| Cancelled excluded from capacity | `EnrollmentHelper.CountActiveEnrollments` | Cancelled frees seat |

## Payment Rules

| Rule | Implementation | Invalid Test |
|------|----------------|--------------|
| Amount > 0 | `PaymentService.CreateAsync` | amount=0 → 400 |
| Cannot exceed remaining | Compare to track Fee - paid | Overpay → 400 |
| Paid activates enrollment | Status Pending → Active on first payment | Verify in Swagger |
| Only Paid in revenue | `ReportService` filters PaymentStatus.Paid | Failed not counted |

## Where Rules Live

```
Services/StudentService.cs      → email uniqueness, soft delete
Services/TrackService.cs        → code, capacity, dates, instructor
Services/EnrollmentService.cs  → duplicate, capacity, status transitions
Services/PaymentService.cs      → amount validation, remaining balance
Services/ReportService.cs       → paid-only revenue, cancelled exclusion
```
