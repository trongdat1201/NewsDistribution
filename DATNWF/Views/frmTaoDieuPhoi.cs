using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DATNWF.Views
{
    public partial class frmTaoDieuPhoi : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

        private string maKhachHangDuocChon = "";

        public frmTaoDieuPhoi()
        {
            InitializeComponent();
        }

        private void FormDieuPhoi_Load(object sender, EventArgs e)
        {
            ResetForm();
            txtMaKH.ReadOnly = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string soHD = txtSoHD.Text.Trim();

            if (string.IsNullOrWhiteSpace(soHD))
            {
                MessageBox.Show("Vui lòng nhập Số hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoHD.Focus();
                return;
            }

            string query = "SELECT COUNT(*) FROM tabDieuPhoi WHERE soHD = @soHD";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@soHD", soHD);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Số Hóa Đơn (Điều phối) này đã tồn tại trong hệ thống!\nVui lòng nhập số khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtSoHD.Focus();
                            txtSoHD.SelectAll(); 
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            if (string.IsNullOrEmpty(maKhachHangDuocChon))
            {
                MessageBox.Show("Vui lòng chọn Khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime ngayLapPhieu = dtpNgayLapPhieu.Value.Date;
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Lỗi: 'Từ ngày' không được lớn hơn 'Đến ngày'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpTuNgay.Focus();
                return;
            }

            if (ngayLapPhieu > tuNgay)
            {
                MessageBox.Show("Cảnh báo: 'Ngày lập phiếu' đang lớn hơn 'Từ ngày'!\nVui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayLapPhieu.Focus();
                return;
            }

            panelLeft.Enabled = false;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            panelLeft.Enabled = true;

            txtSoHD.Clear();
            txtGhiChu.Clear();

            txtMaKH.Clear();
            maKhachHangDuocChon = "";

            dtpNgayLapPhieu.Value = DateTime.Now;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            txtSoHD.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picTimKH_Click(object sender, EventArgs e)
        {
            using (frmTimKhachHang frmSearch = new frmTimKhachHang())
            {
                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    txtMaKH.Text = frmSearch.TenKH_Selected;

                    maKhachHangDuocChon = frmSearch.MaKH_Selected;
                }
            }
        }
    }
}