# Bài tập lớn Hệ Quản Trị Cơ Sở Dữ Liệu

Ứng dụng WinForms C# kết nối SQL Server để thực hiện và hiển thị kết quả các bài tập 1–8. Mỗi bài có form riêng (`FrmBai1` đến `FrmBai8`); các phép tính và truy vấn được xử lý trong SQL Server, WinForms nhận kết quả rồi trình bày.

## Nội dung

| Bài | Nội dung | Cơ sở dữ liệu |
| --- | --- | --- |
| 1 | Phương trình bậc nhất | `cuoiky_tinhtoan` |
| 2 | Phương trình bậc hai | `cuoiky_tinhtoan` |
| 3 | Tra cứu thông tin đầu sách | `cuoiky_thuvien` |
| 4 | Tính tuổi từ ngày sinh | `cuoiky_tinhtoan` |
| 5 | Tra cứu thông tin thư viện | `cuoiky_thuvien` |
| 6 | Trigger thư viện: xóa/thêm lượt mượn, cập nhật cuốn sách, thông báo tựa sách | `cuoiky_thuvien` |
| 7 | Hàm tính toán dữ liệu đề án | `cuoiky_dean` |
| 8 | Thống kê tổng hợp đề án | `cuoiky_dean` |

## Mã nguồn SQL

- `database/tinhtoan.sql`: các hàm và thủ tục cho Bài 1, 2, 4.
- `database/01_ThuVien.sql`: các thủ tục và trigger cho Bài 3, 5, 6.
- `database/02_DeAn.sql`: các hàm cho Bài 7, 8.

Các script tạo/cập nhật đối tượng phục vụ bài tập trong những CSDL tương ứng. Cần có sẵn CSDL và các bảng nền theo đề trước khi chạy script; script bài tập không thay thế bộ cài schema/dữ liệu nền.

## Yêu cầu và chạy chương trình

- Windows với .NET 9 SDK.
- SQL Server hoặc SQL Server LocalDB; máy chạy ứng dụng cần có các CSDL tương ứng.
- Kết nối mặc định của ứng dụng: `(localdb)\MSSQLLocalDB`.

Mở PowerShell tại thư mục dự án rồi chạy:

```powershell
dotnet restore
dotnet build --configuration Release
dotnet run --project .\CuoiKyWinForms.csproj
```

Trong giao diện, kiểm tra kết nối và nạp đúng CSDL cần dùng trước khi mở bài. Khi kiểm thử trigger Bài 6, thao tác được đặt trong transaction và rollback để dữ liệu gốc được giữ nguyên.

## Cấu trúc chính

```text
CuoiKyWinForms.csproj
MainForm.cs
FrmBai1.cs ... FrmBai8.cs
Db.cs
ExerciseFormBase.cs
SqlScriptLoader.cs
database/
  tinhtoan.sql
  01_ThuVien.sql
  02_DeAn.sql
```
