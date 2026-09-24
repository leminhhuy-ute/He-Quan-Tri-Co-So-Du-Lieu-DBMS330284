namespace CuoiKyWinForms;

public partial class FrmBai3
{
    private void InitializeComponent()
    {
        Name = "FrmBai3";
        Text = "Bài 3 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 3 — Thông tin đầu sách theo ISBN";
        descriptionBox.Text = "Liệt kê thông tin đầu sách, tựa sách và số lượng cuốn chưa mượn.\r\nSQL Server: EXEC dbo.sp_ThongtinDausach @ISBN;";
        paramLabel1.Text = "Mã ISBN:";
        paramText1.Clear();
        paramLabel2.Visible = false;
        paramText2.Visible = false;
        paramLabel3.Visible = false;
        paramText3.Visible = false;
        subButtons.Visible = false;
    }
}
