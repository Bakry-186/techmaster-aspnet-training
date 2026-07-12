# Task 06 - SQL & ERD Starter

## Selected Scenario

**Simple Store & Orders System**

## Main Entities

- Customers
- Categories
- Suppliers
- Products
- Orders
- OrderItems

## Relationships

- Customer **1 → N** Orders
- Order **1 → N** OrderItems
- Product **1 → N** OrderItems
- Category **1 → N** Products
- Supplier **1 → N** Products

## Why I designed it this way

I split products from orders using an OrderItems junction table so one order can contain many products without duplicating product data. Categories and suppliers are separate lookup tables because many products share the same category and supplier. This keeps the database normalized and makes reporting (sales by category, low stock, best sellers) easier with JOIN queries.

## Files

- `erd-notes.md` - table fields and keys
- `queries.sql` - required SQL queries

See `erd-diagram.txt` for a simple text ERD you can recreate in dbdiagram.io.
