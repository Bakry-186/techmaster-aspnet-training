# Task 04 - Product Catalog with LINQ

25 seed products and 20 LINQ query methods in `ProductQueryService`.

## Run

```bash
cd phase-01-backend-foundations/task-04-product-catalog-linq/ProductCatalogLinq
dotnet run
```

## LINQ queries implemented

1. Available products (Where)
2. Filter by category
3. Filter by price range
4. Search by name
5. Sort price ascending
6. Sort price descending
7. Group by category
8. Count per category
9. Total stock value (Sum)
10. Stock value per category
11. Top 5 expensive (Take)
12. Low stock
13. Out of stock
14. Product summary projection (Select)
15. Supplier report (GroupBy + Select)
16. Recently added (60 days)
17. Category statistics
18. Above average price
19. Combined filter chain
20. Pagination (Skip/Take)

## Notes

- Search is case-insensitive
- Price range rejects negative min and max < min
- Pagination validates page number > 0
