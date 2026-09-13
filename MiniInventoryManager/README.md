# Mini Inventory Manager (OOP2)

A small Windows Forms + SQLite project for demonstrating OOP, CRUD, searching, filtering, sorting, validation, low-stock reporting, and parameterised SQL.

## Features
- Login: `admin` / `1234`
- Add, view, update and delete products
- Search by product name
- Category filter, low-stock filter, and ascending/descending price order
- Low-stock report
- SQLite database is created automatically as `inventory.db`

## OOP concepts to explain in viva
- **Encapsulation:** `Models/Product.cs` keeps product data as properties in a class.
- **Inheritance:** `SaleProduct : Product` inherits fields from `Product` and adds discount behavior.
- **Abstraction:** `Data/DbHelper.cs` hides database connection and SQL details from the forms.
- **Event-driven UI:** `AddButton_Click` is called when the Add button is clicked.

## Database table
`Products(Id, Name, Category, Price, Stock)`

`GetProducts` uses a parameterised `LIKE $keyword` search. It changes `ORDER BY Price ASC` to `DESC` when the “Price high to low” checkbox is selected.

## Run
1. Open `MiniInventoryManager.csproj` in Visual Studio 2022 or later.
2. Restore packages if Visual Studio asks.
3. Press **F5**.

Alternatively, in the project folder run: `dotnet run`.

Do not claim features you cannot explain. Practice this sequence: click button → event method → `DbHelper` SQL → `LoadProducts()` binds the result to the grid.
