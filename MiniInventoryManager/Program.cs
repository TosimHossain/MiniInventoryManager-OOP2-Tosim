using MiniInventoryManager.Data;

namespace MiniInventoryManager;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        DbHelper.InitializeDatabase();
        Application.Run(new LoginForm());
    }
}
