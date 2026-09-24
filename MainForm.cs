using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class MainForm : Form
{
    private readonly HashSet<string> loadedDatabases = new(StringComparer.OrdinalIgnoreCase);
    private string? loadedServer;

    public MainForm()
    {
        InitializeComponent();
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        Db.LoadSavedSettings();
        serverText.Text = Db.Server;
        serverText.TextChanged += serverText_TextChanged;
        UpdateExerciseAvailability();
        ShowConnection();
    }

    private void SyncServer() => Db.Server = string.IsNullOrWhiteSpace(serverText.Text)
        ? "(localdb)\\MSSQLLocalDB" : serverText.Text.Trim();

    private void ShowConnection()
    {
        connectionView.Visible = true;
    }

    private static string DatabaseForExercise(int number) => number switch
    {
        1 or 2 or 4 => Db.CalculationDatabase,
        3 or 5 or 6 => Db.LibraryDatabase,
        7 or 8 => Db.ProjectDatabase,
        _ => throw new ArgumentOutOfRangeException(nameof(number))
    };

    private bool IsLoaded(string database) =>
        string.Equals(loadedServer, Db.Server, StringComparison.OrdinalIgnoreCase)
        && loadedDatabases.Contains(database);

    private void UpdateExerciseAvailability()
    {
        // Navigation is independent of SQL availability so users can inspect any exercise offline.
        nav1.Enabled = nav2.Enabled = nav3.Enabled = nav4.Enabled = true;
        nav5.Enabled = nav6.Enabled = nav7.Enabled = nav8.Enabled = true;
    }

    private void ResetConnectionState()
    {
        loadedDatabases.Clear();
        loadedServer = null;
        connectionStatus.Text = "Trạng thái: Chưa kiểm tra kết nối SQL Server";
        connectionStatus.ForeColor = SystemColors.ControlText;
        foreach (var status in new[] { calculationStatus, libraryStatus, projectStatus })
        {
            status.Text = "Trạng thái: Chưa nạp";
            status.ForeColor = SystemColors.ControlText;
        }
        UpdateExerciseAvailability();
    }

    private void serverText_TextChanged(object? sender, EventArgs e) => ResetConnectionState();

    private void ShowExercise(int number)
    {
        SyncServer();
        string database = DatabaseForExercise(number);
        ExerciseFormBase form = number switch
        {
            1 => new FrmBai1(),
            2 => new FrmBai2(),
            3 => new FrmBai3(),
            4 => new FrmBai4(),
            5 => new FrmBai5(),
            6 => new FrmBai6(),
            7 => new FrmBai7(),
            8 => new FrmBai8(),
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
        form.SetDatabaseAvailability(IsLoaded(database), Db.DatabaseDisplayName(database));
        using (form)
        {
            form.ShowDialog(this);
        }
    }

    private void navConnection_Click(object? sender, EventArgs e) => ShowConnection();
    private void nav1_Click(object? sender, EventArgs e) => ShowExercise(1);
    private void nav2_Click(object? sender, EventArgs e) => ShowExercise(2);
    private void nav3_Click(object? sender, EventArgs e) => ShowExercise(3);
    private void nav4_Click(object? sender, EventArgs e) => ShowExercise(4);
    private void nav5_Click(object? sender, EventArgs e) => ShowExercise(5);
    private void nav6_Click(object? sender, EventArgs e) => ShowExercise(6);
    private void nav7_Click(object? sender, EventArgs e) => ShowExercise(7);
    private void nav8_Click(object? sender, EventArgs e) => ShowExercise(8);

    private void testConnection_Click(object? sender, EventArgs e)
    {
        try
        {
            SyncServer();
            using var connection = Db.CreateConnection();
            Db.OpenConnection(connection);
            Db.SaveSettings();
            connectionStatus.Text = $"Trạng thái: Kết nối thành công tới {Db.Server}.";
            connectionStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show(this, $"Kết nối SQL Server thành công!\nPhiên bản: {connection.ServerVersion}",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ResetConnectionState();
            connectionStatus.Text = "Trạng thái: Lỗi kết nối SQL Server.";
            connectionStatus.ForeColor = Color.Firebrick;
            MessageBox.Show(this, $"Không thể kết nối SQL Server:\n{ex.Message}",
                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadScript(string file, string database, Label status)
    {
        SyncServer();
        if (SqlScriptLoader.Load(file, out string message))
        {
            loadedServer = Db.Server;
            loadedDatabases.Add(database);
            UpdateExerciseAvailability();
            connectionStatus.Text = $"Trạng thái: Đã kết nối tới {Db.Server}.";
            connectionStatus.ForeColor = Color.DarkGreen;
            status.Text = "Trạng thái: Đã kết nối & xử lý SQL";
            status.ForeColor = Color.DarkGreen;
            MessageBox.Show(this, message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            loadedDatabases.Remove(database);
            UpdateExerciseAvailability();
            status.Text = "Trạng thái: Lỗi khi nạp";
            status.ForeColor = Color.Firebrick;
            MessageBox.Show(this, message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void loadCalculation_Click(object? sender, EventArgs e) => LoadScript("tinhtoan.sql", Db.CalculationDatabase, calculationStatus);
    private void loadLibrary_Click(object? sender, EventArgs e) => LoadScript("01_ThuVien.sql", Db.LibraryDatabase, libraryStatus);
    private void loadProject_Click(object? sender, EventArgs e) => LoadScript("02_DeAn.sql", Db.ProjectDatabase, projectStatus);
}
