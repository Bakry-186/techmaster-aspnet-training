# Simple Store & Orders - ERD Notes

## Customers
- CustomerId (PK)
- FullName, Email, PhoneNumber, CreatedAt

## Categories
- CategoryId (PK)
- Name, Description

## Suppliers
- SupplierId (PK)
- Name, PhoneNumber, Email

## Products
- ProductId (PK)
- Name, Price, StockQuantity, IsAvailable
- CategoryId (FK -> Categories)
- SupplierId (FK -> Suppliers)

## Orders
- OrderId (PK)
- CustomerId (FK -> Customers)
- OrderDate, Status, TotalAmount

## OrderItems
- OrderItemId (PK)
- OrderId (FK -> Orders)
- ProductId (FK -> Products)
- Quantity, UnitPrice

Relationships:
Customers 1---* Orders
Orders 1---* OrderItems
Products 1---* OrderItems
Categories 1---* Products
Suppliers 1---* Products
