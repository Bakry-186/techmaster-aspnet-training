# Task 04 - Book Store API

Mini capstone API with Books, Authors, and Categories.

## Run

```bash
cd phase-02-web-api-basics/task-04-book-store-api/BookStoreApi
dotnet run
```

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | /api/authors | List authors |
| POST | /api/authors | Create author |
| DELETE | /api/authors/{id} | Delete author (blocked if books exist) |
| GET | /api/categories | List categories |
| POST | /api/categories | Create category |
| GET | /api/books | List with search + pagination |
| GET | /api/books/{id} | Get book |
| POST | /api/books | Create book |
| PUT | /api/books/{id} | Update book |
| DELETE | /api/books/{id} | Delete book |
| GET | /api/books/reports/summary | Inventory summary report |

## Business rules

- ISBN must be unique
- Price must be positive
- Author and category must exist
- Cannot delete author with related books
