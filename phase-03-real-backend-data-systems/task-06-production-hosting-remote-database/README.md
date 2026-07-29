# Task 06 - Production Hosting & Remote Database

Deploy the Training Center API online with a remote SQL Server database.

## Prerequisites

- [x] API works locally (`dotnet run` + Swagger)
- [x] Migrations applied locally
- [x] Hosting account (MonsterASP.NET or similar)
- [x] Remote SQL Server database created

## Step 1 — Local Proof (Done)

```bash
cd task-03-training-center-database-api/TrainingCenter.Api
dotnet ef database update
dotnet run
```

Evidence: local Swagger screenshot, SQL tables screenshot, Migrations folder screenshot.

## Step 2 — Create Hosting App

1. Create ASP.NET Core 8 site on hosting provider.
2. Enable HTTPS.
3. Note the live URL (add to this README when deployed):

```
Live Swagger URL: http://trainingcenter.runasp.net/swagger
```

## Step 3 — Create Remote SQL Database

1. Create SQL Server database on hosting panel.
2. Copy connection string to hosting **environment settings only**.
3. **Never commit passwords to GitHub.**

`appsettings.Production.json` uses placeholder:

```json
"DefaultConnection": "CONFIGURE_ON_HOSTING_PANEL"
```

## Step 4 — Apply Migrations to Remote DB

From your machine (with firewall access) or hosting deploy tool:

```bash
dotnet ef database update --connection "YOUR_REMOTE_CONNECTION_STRING"
```

Verify tables exist in remote database.

## Step 5 — Publish API

```bash
dotnet publish -c Release -o ./publish
```

Upload publish output to hosting provider. Set production connection string in hosting panel.

Test:
- GET `/api/reports/dashboard-summary`
- POST `/api/students`

## Step 6 — Safety Check

- [x] No passwords in GitHub repo
- [x] No passwords in README or screenshots
- [x] `appsettings.Production.json` has placeholder only
- [x] Production connection string configured on hosting panel only

