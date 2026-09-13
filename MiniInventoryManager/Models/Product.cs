namespace MiniInventoryManager.Models;

// Encapsulation: properties are controlled through this class rather than public fields.
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

// Inheritance: SaleProduct gets all Product properties and adds a discount feature.
public class SaleProduct : Product
{
    public decimal DiscountPercent { get; set; }
    public decimal DiscountedPrice() => Price * (1 - DiscountPercent / 100);
}
