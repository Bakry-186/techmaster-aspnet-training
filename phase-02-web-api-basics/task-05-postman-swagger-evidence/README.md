# Task 05 - Postman & Swagger Evidence

Postman collection and evidence checklist for Phase 02 APIs.

## Import Postman collection

1. Open Postman
2. Import `TechMaster-Phase02.postman_collection.json`
3. Create environment variable:
   - `baseUrl` = `http://localhost:5254` (or your running API port)

## Collection folders

| Folder | API project | Sample requests |
|--------|-------------|-----------------|
| Students | Task 02 | GET all, POST create, GET by id, 404 test |
| Products | Task 03 | GET all, POST create, PATCH stock, low-stock report |
| Book Store | Task 04 | GET books, POST book, summary report |
| Error Cases | Task 01/02 | 400 validation, 404 not found |


## Notes

Run one API project at a time (`dotnet run`) and update `baseUrl` to match its port from the terminal output.
