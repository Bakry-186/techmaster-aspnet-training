# Phase 03 - Real Backend Data Systems

## Student Information

- Name: Abdelrahman Abdelhamid Mostafa
- Track: ASP.NET Backend Career Training
- Phase: Phase 03 - Real Backend Data Systems

## Tasks

| Task | Description | Status |
|------|-------------|--------|
| 00 | Workspace & environment setup | Done |
| 01 | EF Core modeling drills | Done |
| 02 | Requirements to ERD | Done |
| 03 | Training Center Database API | Done |
| 04 | Querying, filtering & reporting | Done |
| 05 | Business rules & data integrity | Done |
| 06 | Production hosting & remote database | Ready (deploy + add live URL) |
| 07 | EF Core API refactor pack | Done |
| 08 | Interview & demo pack | Done |

## Main API — How To Run

```bash
cd phase-03-real-backend-data-systems/task-03-training-center-database-api/TrainingCenter.Api
dotnet ef database update
dotnet run
```

Open Swagger from the terminal URL.

Database: `TechMasterTrainingCenterDb` (local SQL Server, Windows auth).

## Task 01 — EF Core Drills

```bash
cd phase-03-real-backend-data-systems/task-01-ef-core-modeling-drills/EfCoreDrills.Api
dotnet run
```

## Submission Checklist

- [x] Phase 03 folder in same GitHub repo
- [x] ERD documented (Task 02)
- [x] EF Core migrations created and applied locally
- [x] Training Center API with CRUD, DTOs, services
- [x] 20 query/report specs implemented
- [x] Business rules enforced in services
- [x] Bad + refactored EF code (Task 07)
- [x] Postman collection exported
- [x] Interview answers (Task 08)
- [ ] Live Swagger URL (Task 06 — add after deployment)
- [ ] Screenshots + demo video in Google Drive

## Remaining Actions

1. **Deploy** to MonsterASP.NET (or similar) — follow Task 06 README
2. **Add live Swagger URL** to Task 06 README
3. **Capture evidence** — SQL tables, Swagger, Postman → Google Drive
4. **Record demo video** — follow Task 08 checklist
