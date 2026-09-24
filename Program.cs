namespace CuoiKyWinForms;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Contains("--check-designers", StringComparer.OrdinalIgnoreCase))
        {
            ApplicationConfiguration.Initialize();
            Form[] forms =
            [
                new FrmBai1(), new FrmBai2(), new FrmBai3(), new FrmBai4(),
                new FrmBai5(), new FrmBai6(), new FrmBai7(), new FrmBai8()
            ];
            foreach (var form in forms) form.Dispose();
            return 0;
        }

        if (args.Contains("--test-connection", StringComparer.OrdinalIgnoreCase))
        {
            var serverArgument = args.FirstOrDefault(x => x.StartsWith("--server=", StringComparison.OrdinalIgnoreCase));
            if (serverArgument != null) Db.Server = serverArgument["--server=".Length..].Trim();
            return TestConnections();
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
        return 0;
    }

    private static int TestConnections()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var databases = new[]
        {
            Db.CalculationDatabase,
            Db.LibraryDatabase,
            Db.ProjectDatabase
        };

        var failed = false;
        foreach (var database in databases)
        {
            try
            {
                using var connection = Db.CreateConnection(database);
                Db.OpenConnection(connection);
                Console.WriteLine($"OK   {Db.Server} / {database}");
            }
            catch (Exception ex)
            {
                failed = true;
                Console.WriteLine($"FAIL {Db.Server} / {database}: {ex.Message}");
            }
        }
        return failed ? 1 : 0;
    }
}
