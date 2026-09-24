using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CuoiKyWinForms;

internal static class SqlScriptLoader
{
    public static bool Load(string fileName, out string message)
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "database", fileName),
            Path.Combine(Directory.GetCurrentDirectory(), "database", fileName),
            Path.Combine(AppContext.BaseDirectory, fileName),
            Path.Combine(Directory.GetCurrentDirectory(), fileName)
        };
        var path = candidates.FirstOrDefault(File.Exists);
        if (path == null)
        {
            message = $"Không tìm thấy file {fileName}.";
            return false;
        }

        try
        {
            using var connection = Db.CreateConnection();
            Db.OpenConnection(connection);
            int procedures = 0, functions = 0, triggers = 0, views = 0;
            foreach (var batch in Regex.Split(File.ReadAllText(path), @"(?im)^\s*GO\s*(?:--.*)?$"))
            {
                if (string.IsNullOrWhiteSpace(batch)) continue;
                var match = Regex.Match(batch, @"(?im)^\s*CREATE\s+(OR\s+ALTER\s+)?(PROCEDURE|FUNCTION|TRIGGER|VIEW)\s+(dbo\.\w+)\b");
                if (match.Success)
                {
                    using var check = new SqlCommand("SELECT OBJECT_ID(@Name)", connection);
                    check.Parameters.AddWithValue("@Name", match.Groups[3].Value);
                    if (check.ExecuteScalar() != DBNull.Value && !match.Groups[1].Success)
                    {
                        switch (match.Groups[2].Value.ToUpperInvariant())
                        {
                            case "PROCEDURE": procedures++; break;
                            case "FUNCTION": functions++; break;
                            case "TRIGGER": triggers++; break;
                            case "VIEW": views++; break;
                        }
                        continue;
                    }
                }

                using var command = new SqlCommand(batch, connection);
                command.ExecuteNonQuery();
            }
            message = $"Đã chạy {fileName}. Giữ nguyên {procedures} thủ tục, {functions} hàm, {triggers} trigger và {views} view đã tồn tại.";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Lỗi khi nạp {fileName}: {ex.Message}";
            return false;
        }
    }

}
