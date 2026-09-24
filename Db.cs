using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace CuoiKyWinForms;

internal static class Db
{
    // LocalDB dùng Windows Authentication nên không cần nhập port, user hoặc mật khẩu.
    public static string Server { get; set; } = "(localdb)\\MSSQLLocalDB";

    private static string SettingsDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CuoiKyWinForms");

    private static string SettingsFile => Path.Combine(SettingsDirectory, "sqlserver-connection.txt");

    // Tên 3 CSDL SQL Server tương ứng 3 file SQL đang dùng.
    public const string CalculationDatabase = "cuoiky_tinhtoan";
    public const string LibraryDatabase = "cuoiky_thuvien";
    public const string ProjectDatabase = "cuoiky_dean";

    public static string DatabaseDisplayName(string database) => database switch
    {
        CalculationDatabase => "CSDL Tính toán (tinhtoan.sql)",
        LibraryDatabase => "CSDL Thư viện (01_ThuVien.sql)",
        ProjectDatabase => "CSDL Đề án (02_DeAn.sql)",
        _ => database
    };

    public static SqlConnection CreateConnection(string? database = null)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = Server,
            InitialCatalog = string.IsNullOrWhiteSpace(database) ? "master" : database,
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            // LocalDB may need several seconds to start after being idle.
            ConnectTimeout = IsLocalDbServer ? 10 : 5
        };
        return new SqlConnection(builder.ConnectionString);
    }

    private static bool IsLocalDbServer =>
        Server.TrimStart().StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Mở kết nối và tự khởi động/chờ LocalDB nếu người dùng đang dùng
    /// instance mặc định (localdb)\MSSQLLocalDB.
    /// </summary>
    public static void OpenConnection(SqlConnection connection)
    {
        var isLocalDb = IsLocalDbServer;
        var startupDetails = isLocalDb ? StartLocalDb() : string.Empty;
        var attempts = isLocalDb ? 2 : 1;
        Exception? lastError = null;

        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                connection.Open();
                return;
            }
            catch (Exception ex)
            {
                lastError = ex;
                try { connection.Close(); } catch { }

                if (attempt < attempts) Thread.Sleep(700);
            }
        }

        if (lastError == null) throw new InvalidOperationException("Không thể mở kết nối SQL Server.");
        if (!isLocalDb) throw lastError;

        var message = $"Không thể kết nối LocalDB instance '{Server}'. Hãy kiểm tra SQL Server LocalDB và thử lại.";
        if (!string.IsNullOrWhiteSpace(startupDetails)) message += $"\r\n\r\nChi tiết khởi động LocalDB: {startupDetails}";
        message += $"\r\n\r\nChi tiết SQL Server: {lastError.Message}";
        throw new InvalidOperationException(message, lastError);
    }

    private static string StartLocalDb()
    {
        string instanceName = Server[(Server.LastIndexOf('\\') + 1)..].Trim();
        string? executable = FindSqlLocalDbExecutable();
        if (executable == null)
            return "Không tìm thấy SqlLocalDB.exe; ứng dụng vẫn thử kết nối tới instance đang chạy.";

        try
        {
            var info = RunSqlLocalDb(executable, $"info \"{instanceName}\"", 5000);
            if (info.ExitCode == 0 && info.Output.Contains("State: Running", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            var start = RunSqlLocalDb(executable, $"start \"{instanceName}\"", 15000);
            if (start.ExitCode == 0) return string.Empty;

            var afterStart = RunSqlLocalDb(executable, $"info \"{instanceName}\"", 5000);
            if (afterStart.ExitCode == 0 && afterStart.Output.Contains("State: Running", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            return JoinDiagnostics(start.Output, start.Error, afterStart.Output, afterStart.Error);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static string? FindSqlLocalDbExecutable()
    {
        var onPath = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(directory => Path.Combine(directory.Trim('"'), "SqlLocalDB.exe"))
            .FirstOrDefault(File.Exists);
        if (onPath != null) return onPath;

        var roots = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        }.Where(path => !string.IsNullOrWhiteSpace(path)).Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var root in roots)
        foreach (var version in new[] { "170", "160", "150", "140", "130" })
        {
            var candidate = Path.Combine(root, "Microsoft SQL Server", version, "Tools", "Binn", "SqlLocalDB.exe");
            if (File.Exists(candidate)) return candidate;
        }
        return null;
    }

    private static (int ExitCode, string Output, string Error) RunSqlLocalDb(string executable, string arguments, int timeoutMs)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executable,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
        if (!process.Start()) return (-1, string.Empty, "Không khởi chạy được SqlLocalDB.exe.");

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        if (!process.WaitForExit(timeoutMs))
        {
            try { process.Kill(entireProcessTree: true); } catch { }
            return (-1, outputTask.GetAwaiter().GetResult(), $"Lệnh LocalDB quá thời gian chờ ({timeoutMs} ms).");
        }

        Task.WaitAll(outputTask, errorTask);
        return (process.ExitCode, outputTask.Result.Trim(), errorTask.Result.Trim());
    }

    private static string JoinDiagnostics(params string[] values) =>
        string.Join(" | ", values.Where(value => !string.IsNullOrWhiteSpace(value)));

    public static void LoadSavedSettings()
    {
        if (!File.Exists(SettingsFile)) return;

        foreach (var line in File.ReadAllLines(SettingsFile))
        {
            var separator = line.IndexOf('=');
            if (separator <= 0) continue;

            var key = line[..separator];
            var value = line[(separator + 1)..];

            if (key == "Server" && !string.IsNullOrWhiteSpace(value)) Server = value;
        }
    }

    public static void SaveSettings()
    {
        Directory.CreateDirectory(SettingsDirectory);
        File.WriteAllText(SettingsFile, $"Server={Server}{Environment.NewLine}");
    }
}
