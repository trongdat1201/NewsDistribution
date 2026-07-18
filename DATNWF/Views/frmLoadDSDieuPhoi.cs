using System;
using System.Data;
using System.Windows.Forms;
using DATNWF.Models;

namespace DATNWF.Views
{
    public partial class frmLoadDSDieuPhoi : Form
    {
        public frmLoadDSDieuPhoi()
        {
            InitializeComponent();

            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            Visible = false;
        }

        private void frmLoadDSDieuPhoi_Load(object sender, EventArgs e)
        {
            LoadChiTietDieuPhoi();
        }

        public void RefreshData()
        {
            LoadChiTietDieuPhoi();
        }

        private void LoadChiTietDieuPhoi()
        {
            const string query = @"SELECT sohd, ngayNhan, maBao, tenbao, sobao, donGia,
                                          soluongDieuPhoi, soluongBan, thanhTien
                                   FROM dbo.tabChiTietDieuPhoi
                                   WHERE ngayNhan >= DATEADD(MONTH, -1, GETDATE())
                                   ORDER BY ngayNhan DESC, sohd DESC";

            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable dt = DbHelper.Instance.FillDataTable(query);

                this.thanhnienDataSet11.tabChiTietDieuPhoi.Clear();
                this.thanhnienDataSet11.tabChiTietDieuPhoi.Merge(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách điều phối: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}