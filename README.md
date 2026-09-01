# Hệ Quản Trị Cơ Sở Dữ Liệu - DBMS330284

Ứng dụng **Windows Forms kết hợp SQL Server** thực hiện bài tập lớn cuối kỳ môn Hệ Quản Trị Cơ Sở Dữ Liệu. Dự án minh họa cách xây dựng và gọi **Stored Procedure**, **Scalar Function**, **Table-Valued Function**, **Trigger**, ràng buộc toàn vẹn và các truy vấn tổng hợp trên nhiều mô hình dữ liệu.

## Nội dung dự án

Dự án triển khai đầy đủ 10 nhóm bài tập với một giao diện WinForms chính. Mỗi bài có màn hình riêng để:

- Hiển thị yêu cầu bài toán.
- Nhập tham số đầu vào.
- Xem câu lệnh gọi procedure/function/trigger.
- Thực thi truy vấn trên SQL Server.
- Hiển thị kết quả bằng `DataGridView`.
- Kiểm tra trigger trong transaction và tự động `ROLLBACK` để không làm thay đổi dữ liệu mẫu.

## Danh sách bài tập

| Bài | Nội dung | CSDL |
| --- | --- | --- |
| 1 | Stored procedure giải phương trình bậc nhất `ax + b = 0` | Thư viện |
| 2 | Function giải phương trình bậc hai `ax² + bx + c = 0` | Thư viện |
| 3 | Stored procedure tra cứu đầu sách và số cuốn chưa được mượn theo ISBN | Thư viện |
| 4 | Function tính tuổi theo năm sinh | Thư viện |
| 5 | Các stored procedure tra cứu độc giả, đầu sách, người đang mượn và quá hạn | Thư viện |
| 6 | Trigger cập nhật tình trạng mượn/trả sách và thông báo thay đổi tựa sách | Thư viện |
| 7 | Scalar Function, Inline TVF và Multistatement TVF quản lý đề án | Đề án |
| 8 | Truy vấn nhóm theo dự án, phòng ban, lương và số nhân viên | Đề án |
| 9 | Function quản lý thợ, hợp đồng, thanh toán và thời hạn sửa xe | Gara |
| 10 | Function và trigger ràng buộc lịch thi, thời gian thi, phân công coi thi | Trường học |

## Công nghệ sử dụng

- **C# / .NET 9**
- **Windows Forms**
- **Microsoft SQL Server / SQL Server LocalDB**
- **T-SQL**
- **Microsoft.Data.SqlClient 6.1.2**

## Các cơ sở dữ liệu

| Database | Script | Phạm vi |
| --- | --- | --- |
| `CuoiKy_ThuVien` | `database/01_ThuVien.sql` | Bài 1 đến bài 6 |
| `CuoiKy_DeAn` | `database/02_DeAn.sql` | Bài 7 và bài 8 |
| `CuoiKy_Gara` | `database/03_Gara.sql` | Bài 9 |
| `CuoiKy_TruongHoc` | `database/04_TruongHoc.sql` | Bài 10 |

Các script có thể chạy lại nhiều lần. Dữ liệu mẫu được chuẩn bị để kiểm tra cả trường hợp thông thường và trường hợp biên như:

- Phương trình vô nghiệm, vô số nghiệm và có nghiệm.
- Độc giả người lớn, trẻ em và sách quá hạn.
- Dự án có nhiều hơn hai nhân viên.
- Hợp đồng đã nghiệm thu nhưng chưa thanh toán đủ.
- Người thợ chưa tham gia hợp đồng.
- Giáo viên chưa được phân công coi thi.
- Phân công giáo viên coi chính môn mình chủ nhiệm bị trigger từ chối.

## Cấu trúc thư mục

```text
cuoikycsdl/
├── database/
│   ├── 01_ThuVien.sql
│   ├── 02_DeAn.sql
│   ├── 03_Gara.sql
│   └── 04_TruongHoc.sql
├── CuoiKyWinForms.csproj
├── Program.cs
├── Db.cs
├── MainForm.cs
├── QueryForm.cs
├── ExerciseMenuForm.cs
├── Models.cs
└── README.md
```

## Yêu cầu môi trường

- Windows 10 hoặc Windows 11.
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
- SQL Server LocalDB, SQL Server Express hoặc SQL Server đầy đủ.
- `sqlcmd` nếu muốn cài CSDL bằng PowerShell.

Kiểm tra môi trường:

```powershell
dotnet --version
sqllocaldb info
sqlcmd -?
```

## Cài đặt và chạy dự án

### 1. Tải mã nguồn

```powershell
git clone https://github.com/leminhhuy-ute/He-Quan-Tri-Co-So-Du-Lieu-DBMS330284.git
cd He-Quan-Tri-Co-So-Du-Lieu-DBMS330284
```

### 2. Khởi động SQL Server LocalDB

```powershell
sqllocaldb start MSSQLLocalDB
```

Instance mặc định của ứng dụng:

```text
(localdb)\MSSQLLocalDB
```

### 3. Tạo và nạp dữ liệu cho 4 CSDL

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -f 65001 -i .\database\01_ThuVien.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -f 65001 -i .\database\02_DeAn.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -f 65001 -i .\database\03_Gara.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -f 65001 -i .\database\04_TruongHoc.sql
```

Nếu sử dụng SQL Server Express hoặc instance khác, thay giá trị sau tham số `-S` bằng tên server tương ứng.

### 4. Restore và build

```powershell
dotnet restore
dotnet build
```

### 5. Chạy ứng dụng

```powershell
dotnet run --project .\CuoiKyWinForms.csproj
```

Hoặc chạy bản Release:

```powershell
dotnet build --configuration Release
.\bin\Release\net9.0-windows\CuoiKyWinForms.exe
```

## Kiểm tra kết nối không cần mở giao diện

Dự án có chế độ kiểm tra cả 4 CSDL bằng đúng thư viện `Microsoft.Data.SqlClient` mà WinForms sử dụng:

```powershell
dotnet .\bin\Release\net9.0-windows\CuoiKyWinForms.dll --test-connection
```

Kết quả mong đợi:

```text
OK   (localdb)\MSSQLLocalDB / CuoiKy_ThuVien
OK   (localdb)\MSSQLLocalDB / CuoiKy_DeAn
OK   (localdb)\MSSQLLocalDB / CuoiKy_Gara
OK   (localdb)\MSSQLLocalDB / CuoiKy_TruongHoc
```

Có thể kiểm tra một server khác bằng tham số `--server`:

```powershell
dotnet .\bin\Release\net9.0-windows\CuoiKyWinForms.dll `
  --test-connection `
  "--server=.\SQLEXPRESS"
```

## Xử lý lỗi LocalDB

Nếu xuất hiện lỗi `Local Database Runtime error` hoặc `SQL Server process failed to start`, kiểm tra trạng thái instance:

```powershell
sqllocaldb info MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

Ứng dụng có cơ chế:

1. Thử kết nối bằng server người dùng nhập.
2. Khởi động instance LocalDB nếu đang dừng.
3. Tự lấy named pipe của LocalDB và kết nối trực tiếp nếu tên instance bị lỗi phân giải.
4. Thử instance dự phòng `BTWinFormsLocalDB` nếu CSDL tồn tại ở đó.

Không nên tạo cùng một database trên hai LocalDB instance khi chúng dùng chung đường dẫn file `.mdf`.

## Thành phần chính

- `Db.cs`: tạo connection string và khai báo tên 4 CSDL.
- `MainForm.cs`: giao diện chính, kết nối CSDL và menu 10 bài tập.
- `ExerciseMenuForm.cs`: giao diện lựa chọn các câu nhỏ.
- `QueryForm.cs`: nhập tham số, xem SQL và hiển thị kết quả.
- `Program.cs`: khởi động WinForms và chế độ `--test-connection`.
- `database/*.sql`: cấu trúc bảng, dữ liệu mẫu, function, procedure và trigger.

## Kiểm tra Trigger

Các chức năng thử trigger trên giao diện sử dụng mẫu:

```sql
BEGIN TRAN;
-- INSERT / UPDATE / DELETE để kiểm tra trigger
ROLLBACK;
```

Vì vậy dữ liệu thật không bị thay đổi sau mỗi lần thử.

## Tác giả

**Lê Minh Huy**  
GitHub: [leminhhuy-ute](https://github.com/leminhhuy-ute)

---

Nếu dự án hữu ích, bạn có thể đánh dấu ⭐ repository để lưu lại.
