using Microsoft.Data.Sqlite;
using MiniInventoryManager.Models;

namespace MiniInventoryManager.Data;

public static class DbHelper
{
    // The SQLite database file is created beside the application executable.
    private static readonly string ConnectionString = "Data Source=inventory.db";

    public static void InitializeDatabase()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT NOT NULL,
                Price REAL NOT NULL,
                Stock INTEGER NOT NULL
            );
            INSERT INTO Products (Name, Category, Price, Stock)
            SELECT 'Notebook', 'Stationery', 80, 15
            WHERE NOT EXISTS (SELECT 1 FROM Products);
            INSERT INTO Products (Name, Category, Price, Stock)
            SELECT 'USB Mouse', 'Electronics', 550, 3
            WHERE (SELECT COUNT(*) FROM Products) = 1;
            """;
        command.ExecuteNonQuery();
    }

    public static List<Product> GetProducts(string keyword = "", string category = "All", bool lowStockOnly = false, bool highToLow = false)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        // Parameters protect the search text from SQL injection.
        command.CommandText = "SELECT Id, Name, Category, Price, Stock FROM Products WHERE Name LIKE $keyword" +
                              (category == "All" ? "" : " AND Category = $category") +
                              (lowStockOnly ? " AND Stock < 5" : "") +
                              " ORDER BY Price " + (highToLow ? "DESC" : "ASC");
        command.Parameters.AddWithValue("$keyword", $"%{keyword}%");
        if (category != "All") command.Parameters.AddWithValue("$category", category);

        using var reader = command.ExecuteReader();
        var products = new List<Product>();
        while (reader.Read())
            products.Add(new Product { Id = reader.GetInt32(0), Name = reader.GetString(1), Category = reader.GetString(2), Price = reader.GetDecimal(3), Stock = reader.GetInt32(4) });
        return products;
    }

    public static void AddProduct(Product product)
    {
        Execute("INSERT INTO Products (Name, Category, Price, Stock) VALUES ($name, $category, $price, $stock)", product);
    }

    public static void UpdateProduct(Product product)
    {
        Execute("UPDATE Products SET Name=$name, Category=$category, Price=$price, Stock=$stock WHERE Id=$id", product);
    }

    public static void DeleteProduct(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Products WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    private static void Execute(string sql, Product product)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.AddWithValue("$name", product.Name);
        command.Parameters.AddWithValue("$category", product.Category);
        command.Parameters.AddWithValue("$price", product.Price);
        command.Parameters.AddWithValue("$stock", product.Stock);
        if (sql.StartsWith("UPDATE")) command.Parameters.AddWithValue("$id", product.Id);
        command.ExecuteNonQuery();
    }
}
