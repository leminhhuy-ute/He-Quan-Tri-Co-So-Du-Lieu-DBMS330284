namespace CuoiKyWinForms;

public partial class FrmBai4
{
    private void InitializeComponent()
    {
        Name = "FrmBai4";
        Text = "Bài 4 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 4 — Tính tuổi theo ngày sinh";
        descriptionBox.Text = "Nhập ngày sinh dd/MM/yyyy. Hàm SQL kiểm tra ngày và tính số năm, tổng số ngày, tổng số tháng.\r\nSQL Server: SELECT SoNam, TongSoNgay, TongSoThang, Loi FROM dbo.fn_TinhTuoiChiTiet(@NgaySinhText);";
        paramLabel1.Text = "Ngày sinh:";
        paramText1.Clear();
        paramLabel2.Visible = false;
        paramText2.Visible = false;
        paramLabel3.Visible = false;
        paramText3.Visible = false;
        subButtons.Visible = false;
    }
}
