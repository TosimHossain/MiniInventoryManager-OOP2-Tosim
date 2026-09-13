using MiniInventoryManager.Data;
using MiniInventoryManager.Models;

namespace MiniInventoryManager;

public class InventoryForm : Form
{
    private readonly TextBox nameBox = new() { Location = new(95, 20), Width = 160 };
    private readonly ComboBox categoryBox = new() { Location = new(95, 55), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown priceBox = new() { Location = new(95, 90), Width = 160, Maximum = 100000, DecimalPlaces = 2 };
    private readonly NumericUpDown stockBox = new() { Location = new(95, 125), Width = 160, Maximum = 10000 };
    private readonly TextBox searchBox = new() { Location = new(315, 20), Width = 150, PlaceholderText = "Search by name" };
    private readonly ComboBox filterCategoryBox = new() { Location = new(475, 20), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox lowStockBox = new() { Text = "Low stock (< 5)", Location = new(605, 20), AutoSize = true };
    private readonly CheckBox highToLowBox = new() { Text = "Price high to low", Location = new(735, 20), AutoSize = true };
    private readonly DataGridView grid = new() { Location = new(315, 55), Size = new(570, 300), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false };
    private int selectedId;

    public InventoryForm()
    {
        Text = "Mini Inventory Manager"; Size = new(920, 450); StartPosition = FormStartPosition.CenterScreen;
        categoryBox.Items.AddRange(["Electronics", "Stationery", "Food"]); categoryBox.SelectedIndex = 0;
        filterCategoryBox.Items.AddRange(["All", "Electronics", "Stationery", "Food"]); filterCategoryBox.SelectedIndex = 0;
        var add = Button("Add", 20, 180, AddButton_Click);
        var update = Button("Update", 100, 180, UpdateButton_Click);
        var delete = Button("Delete", 180, 180, DeleteButton_Click);
        var clear = Button("Clear", 100, 220, (_, _) => ClearInputs());
        var report = Button("Low-stock report", 55, 270, (_, _) => ShowLowStockReport());
        Controls.AddRange(new Control[] {
            Label("Name", 20, 23), nameBox, Label("Category", 20, 58), categoryBox, Label("Price", 20, 93), priceBox, Label("Stock", 20, 128), stockBox,
            add, update, delete, clear, report, searchBox, filterCategoryBox, lowStockBox, highToLowBox, grid
        });
        grid.SelectionChanged += Grid_SelectionChanged;
        searchBox.TextChanged += (_, _) => LoadProducts();
        filterCategoryBox.SelectedIndexChanged += (_, _) => LoadProducts();
        lowStockBox.CheckedChanged += (_, _) => LoadProducts();
        highToLowBox.CheckedChanged += (_, _) => LoadProducts();
        LoadProducts();
    }

    private static Label Label(string text, int x, int y) => new() { Text = text + ":", Location = new(x, y), AutoSize = true };
    private static Button Button(string text, int x, int y, EventHandler handler) { var button = new Button { Text = text, Location = new(x, y), Width = 75 }; button.Click += handler; return button; }

    // UI event -> input validation -> database INSERT -> refresh DataGridView.
    private void AddButton_Click(object? sender, EventArgs e)
    {
        if (!TryProduct(out var product)) return;
        DbHelper.AddProduct(product); ClearInputs(); LoadProducts();
    }
    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (selectedId == 0) { MessageBox.Show("Select a product first."); return; }
        if (!TryProduct(out var product)) return;
        product.Id = selectedId; DbHelper.UpdateProduct(product); ClearInputs(); LoadProducts();
    }
    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (selectedId == 0) { MessageBox.Show("Select a product first."); return; }
        if (MessageBox.Show("Delete selected product?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
        { DbHelper.DeleteProduct(selectedId); ClearInputs(); LoadProducts(); }
    }
    private bool TryProduct(out Product product)
    {
        product = new Product();
        if (string.IsNullOrWhiteSpace(nameBox.Text)) { MessageBox.Show("Product name is required."); return false; }
        product = new Product { Name = nameBox.Text.Trim(), Category = categoryBox.Text, Price = priceBox.Value, Stock = (int)stockBox.Value };
        return true;
    }
    private void LoadProducts() => grid.DataSource = DbHelper.GetProducts(searchBox.Text.Trim(), filterCategoryBox.Text, lowStockBox.Checked, highToLowBox.Checked);
    private void Grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (grid.CurrentRow?.DataBoundItem is not Product product) return;
        selectedId = product.Id; nameBox.Text = product.Name; categoryBox.Text = product.Category; priceBox.Value = product.Price; stockBox.Value = product.Stock;
    }
    private void ClearInputs() { selectedId = 0; nameBox.Clear(); categoryBox.SelectedIndex = 0; priceBox.Value = 0; stockBox.Value = 0; grid.ClearSelection(); }
    private void ShowLowStockReport()
    {
        var items = DbHelper.GetProducts(lowStockOnly: true);
        MessageBox.Show(items.Count == 0 ? "No low-stock products." : string.Join(Environment.NewLine, items.Select(p => $"{p.Name}: {p.Stock} left")), "Low-stock report");
    }
}
