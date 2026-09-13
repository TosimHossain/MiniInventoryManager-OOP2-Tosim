namespace MiniInventoryManager;

public class LoginForm : Form
{
    private readonly TextBox usernameBox = new() { PlaceholderText = "Username", Location = new(105, 45), Width = 180 };
    private readonly TextBox passwordBox = new() { PlaceholderText = "Password", UseSystemPasswordChar = true, Location = new(105, 85), Width = 180 };

    public LoginForm()
    {
        Text = "Mini Inventory - Login"; Size = new(390, 210); StartPosition = FormStartPosition.CenterScreen;
        Controls.AddRange(new Control[] {
            new Label { Text = "Username:", Location = new(25, 48), AutoSize = true }, usernameBox,
            new Label { Text = "Password:", Location = new(25, 88), AutoSize = true }, passwordBox,
            new Button { Text = "Login", Location = new(105, 125), Width = 180 }
        });
        ((Button)Controls[^1]).Click += LoginButton_Click;
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        // Demo credential: admin / 1234. This is intentionally simple for a class project.
        if (usernameBox.Text == "admin" && passwordBox.Text == "1234")
        {
            Hide();
            new InventoryForm().ShowDialog();
            Close();
        }
        else MessageBox.Show("Wrong username or password. Try admin / 1234.");
    }
}
