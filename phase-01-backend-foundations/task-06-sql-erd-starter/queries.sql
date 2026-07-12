-- Task 06 - Simple Store & Orders System
-- Sample queries for the ERD scenario

-- 1. Select all products
SELECT ProductId, Name, Price, StockQuantity, IsAvailable
FROM Products;

-- 2. Select available products
SELECT ProductId, Name, Price, StockQuantity
FROM Products
WHERE IsAvailable = 1 AND StockQuantity > 0;

-- 3. Select products by category
SELECT p.ProductId, p.Name, p.Price, c.Name AS CategoryName
FROM Products p
INNER JOIN Categories c ON p.CategoryId = c.CategoryId
WHERE c.Name = 'Electronics';

-- 4. Select products with low stock
SELECT ProductId, Name, StockQuantity
FROM Products
WHERE StockQuantity <= 5;

-- 5. Select orders for one customer
SELECT o.OrderId, o.OrderDate, o.Status, o.TotalAmount
FROM Orders o
WHERE o.CustomerId = 1;

-- 6. Select order details using JOIN
SELECT o.OrderId, c.FullName, p.Name AS ProductName, oi.Quantity, oi.UnitPrice
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.CustomerId
INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
INNER JOIN Products p ON oi.ProductId = p.ProductId
WHERE o.OrderId = 1001;

-- 7. Calculate total sales
SELECT SUM(TotalAmount) AS TotalSales
FROM Orders
WHERE Status = 'Completed';

-- 8. Count products per category
SELECT c.Name, COUNT(p.ProductId) AS ProductCount
FROM Categories c
LEFT JOIN Products p ON c.CategoryId = p.CategoryId
GROUP BY c.Name;

-- 9. Select best-selling products
SELECT TOP 5 p.Name, SUM(oi.Quantity) AS TotalSold
FROM OrderItems oi
INNER JOIN Products p ON oi.ProductId = p.ProductId
GROUP BY p.Name
ORDER BY TotalSold DESC;

-- 10. Select suppliers with their products
SELECT s.Name AS SupplierName, p.Name AS ProductName, p.Price
FROM Suppliers s
INNER JOIN Products p ON s.SupplierId = p.SupplierId
ORDER BY s.Name, p.Name;
