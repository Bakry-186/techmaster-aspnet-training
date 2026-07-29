# Task 08 - Interview & Demo Pack

## Demo Video Checklist (5–8 minutes)

1. Show repository `phase-03-real-backend-data-systems/` folder structure.
2. Explain ERD from Task 02 (Student → Enrollment → Track → Instructor → Payment).
3. Run live Swagger URL (local or deployed).
4. Demo POST `/api/students` — create a student.
5. Demo POST `/api/enrollments` — enroll in a track.
6. Demo GET `/api/reports/dashboard-summary` — report endpoint.
7. Show SQL Server tables or remote DB screenshot.
8. Explain one business rule (e.g. duplicate enrollment prevention).

Upload video to Google Drive.

## Interview Answers

### 1. What is DbContext and what does it do in your project?
DbContext is the bridge between C# entity classes and SQL Server. In my Training Center API, `AppDbContext` holds `DbSet` properties for Students, Instructors, Tracks, Enrollments, and Payments. Services use it to query and save data.

### 2. What is DbSet and how does it map to database tables?
A DbSet represents a table. `DbSet<Student>` maps to the `Students` table. EF Core translates LINQ queries on DbSet into SQL.

### 3. What is a migration and why do we use it?
A migration is a version-controlled schema change. I used `InitialTrainingCenterSchema` to create all tables from my entity models. Migrations let the team sync database structure without manual SQL scripts.

### 4. What is the difference between entity and DTO?
An entity mirrors the database table with navigation properties. A DTO is a response/request shape for the API — only the fields the client needs. I return `StudentListItemResponse` instead of the full `Student` entity.

### 5. Why should APIs not return EF entities directly?
Entities expose internal fields, create large nested JSON from `Include` chains, and can cause circular references. DTOs with `.Select()` projection give controlled, efficient responses.

### 6. What is a foreign key?
A column that references another table's primary key. Example: `Enrollment.StudentId` references `Student.StudentId`, linking an enrollment to a student.

### 7. Explain Student, TrainingTrack, and Enrollment relationship.
Student and TrainingTrack have a many-to-many relationship through Enrollment. One student can enroll in many tracks; one track can have many students. Enrollment stores extra data like status and progress.

### 8. Why is Enrollment a join entity instead of simple many-to-many?
Because enrollment carries business data: Status, ProgressPercentage, FinalResult, EnrollmentDate. EF automatic many-to-many cannot store these extra fields.

### 9. What is Include and when did you use it?
Include eagerly loads related entities in one query. I use it in services when I need related data for in-memory calculations (e.g. payment totals). For API responses I prefer `.Select()` projection instead.

### 10. What is Select projection and why is it useful?
`.Select(x => new Dto { ... })` shapes the query output directly in SQL. It avoids loading full entity graphs and returns only needed fields — faster and safer.

### 11. What is pagination and why does an API need it?
Pagination returns a page of results with metadata (totalCount, totalPages). Without it, listing thousands of students would be slow and wasteful. I use `Skip`/`Take` with validation on pageNumber and pageSize.

### 12. How did you prevent duplicate active enrollments?
In `EnrollmentService.CreateAsync`, I check if a Pending or Active enrollment already exists for the same StudentId + TrainingTrackId. If yes, return 400.

### 13. How did you protect track capacity?
Before creating enrollment, I count active enrollments (Pending + Active, excluding Cancelled) and compare to `TrainingTrack.Capacity`. If full, return 400.

### 14. How did you handle payment validation?
`PaymentService` rejects amount <= 0 and amounts exceeding remaining balance (track Fee minus sum of Paid payments).

### 15. What is soft delete and why did you use it?
Soft delete sets `IsDeleted = true` instead of removing the row. Students and tracks keep history for reports. GET lists filter out deleted records by default.

### 16. What is the difference between local and remote database?
Local runs on my machine (SQL Server with Windows auth). Remote runs on the hosting provider's server, accessed via connection string configured in the hosting panel.

### 17. How did you configure the production connection string?
In `appsettings.Production.json` I use a placeholder. The real connection string is set in the hosting provider's environment settings panel — never in GitHub.

### 18. Why should connection strings not be pushed to GitHub?
They contain server names and passwords. Public repos expose credentials to anyone. Use hosting panel or user secrets locally.

### 19. What is the hardest bug you faced in deployment?
Port conflicts when running multiple API projects locally (5083 already in use). Fixed by stopping the previous process with `taskkill` or changing the port in `launchSettings.json`.

### 20. If you had one more week, how would you improve the system?
Add JWT authentication (Phase 04), global exception handling middleware, integration tests, and track session/attendance entities as bonus entities from the requirements.
