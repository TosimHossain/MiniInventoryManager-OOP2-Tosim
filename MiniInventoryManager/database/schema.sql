CREATE TABLE Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Category TEXT NOT NULL,
    Price REAL NOT NULL,
    Stock INTEGER NOT NULL
);

-- Low-stock products for the report
SELECT Name, Stock FROM Products WHERE Stock < 5 ORDER BY Stock ASC;

-- Category-wise inventory value (GROUP BY example for viva)
SELECT Category, COUNT(*) AS ProductCount, SUM(Price * Stock) AS InventoryValue
FROM Products
GROUP BY Category;
