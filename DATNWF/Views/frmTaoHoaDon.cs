using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmTaoHoaDon : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

        private string maKhachHangDuocChon = "";
        public frmTaoHoaDon()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string GenerateInvoiceCode()
        {
            return "HD" + DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoHD.Text))
            {
                txtSoHD.Text = GenerateInvoiceCode();
            }

            string soHD = txtSoHD.Text.Trim();

            string query = "SELECT COUNT(*) FROM tabHOADON WHERE sohd = @soHD";
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
                            MessageBox.Show("Số Hóa Đơn này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtSoHD.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                return;
            }

            panelLeft.Enabled = false;
            panelRight.Enabled = true; 
        }

        private void ResetForm()
        {
            panelLeft.Enabled = true;
            panelRight.Enabled = false; 

            txtSoHD.Clear();
            txtGhiChu.Clear();
            txtMaKH.Clear();
            maKhachHangDuocChon = "";

            chkThanhToan.Checked = false;

            dtpNgayLapPhieu.Value = DateTime.Now;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            txtSoHD.Text = GenerateInvoiceCode(); 
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetForm();
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
