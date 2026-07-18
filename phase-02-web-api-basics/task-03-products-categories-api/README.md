# Task 03 - Products & Categories API

In-memory Products and Categories API with filters, stock management, and reports.

## Run

```bash
cd phase-02-web-api-basics/task-03-products-categories-api/ProductsCategoriesApi
dotnet run
```

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | /api/categories | List categories |
| POST | /api/categories | Create category |
| GET | /api/products | List with filters + pagination |
| GET | /api/products/{id} | Get product |
| POST | /api/products | Create product |
| PUT | /api/products/{id} | Update product |
| PATCH | /api/products/{id}/stock | Update stock |
| DELETE | /api/products/{id} | Delete product |
| GET | /api/products/low-stock | Low stock products |
| GET | /api/products/reports/stock-value | Stock value report |

## Seed data

- 4 categories: Electronics, Furniture, Stationery, Accessories
- 16 products across all categories

