using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmDelivery : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;
        private string _soHDHienTai = string.Empty;

        public frmDelivery()
        {
            InitializeComponent();
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        private void frmDelivery_Load(object sender, EventArgs e)
        {
            dgvDieuPhoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDieuPhoi.ReadOnly = true;
            dgvChiTietDieuPhoi.ReadOnly = true;

            txtMaKH.ReadOnly = true;
            txtTenKH.ReadOnly = true;
            txtSDT.ReadOnly = true;
            txtCK.ReadOnly = true;

            ClearKhachHangInfo();
            lblTongSoTien.Text = "0";

            LoadDanhSachDieuPhoi();
        }

        private void LoadDanhSachDieuPhoi(string keyword = "")
        {
            string query;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                query = @"SELECT TOP 50 soHD, makh, ngay, tungay, denngay, ghiChu
                          FROM dbo.tabDieuPhoi
                          ORDER BY ngay DESC, soHD DESC";
            }
            else
            {
                query = @"SELECT soHD, makh, ngay, tungay, denngay, ghiChu
                          FROM dbo.tabDieuPhoi
                          WHERE soHD LIKE @kw
                          ORDER BY ngay DESC, soHD DESC";
            }

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                    da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                da.Fill(dt);
            }

            tabDieuPhoiBindingSource.DataSource = null;
            tabDieuPhoiBindingSource.DataMember = "";
            tabDieuPhoiBindingSource.DataSource = dt;
            dgvDieuPhoi.DataSource = tabDieuPhoiBindingSource;
        }

        private void LoadChiTietDieuPhoi(string soHD)
        {
            if (string.IsNullOrEmpty(soHD)) return;

            string query = @"SELECT sohd, ngayNhan, maBao, tenbao, sobao, donGia,
                                    soluongDieuPhoi, soluongBan, thanhTien
                             FROM dbo.tabChiTietDieuPhoi
                             WHERE sohd = @soHD
                             ORDER BY ngayNhan ASC";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@soHD", soHD);
                da.Fill(dt);
            }

            tabChiTietDieuPhoiBindingSource.DataSource = null;
            tabChiTietDieuPhoiBindingSource.DataMember = "";
            tabChiTietDieuPhoiBindingSource.DataSource = dt;
            dgvChiTietDieuPhoi.DataSource = tabChiTietDieuPhoiBindingSource;

            decimal tongTien = 0m;
            foreach (DataRow row in dt.Rows)
            {
                if (row["thanhTien"] != DBNull.Value)
                    tongTien += Convert.ToDecimal(row["thanhTien"]);
            }
            lblTongSoTien.Text = string.Format("{0:N0} đ", tongTien);
        }

        private void LoadThongTinKhachHang(string maKH)
        {
            if (string.IsNullOrEmpty(maKH))
            {
                ClearKhachHangInfo();
                return;
            }

            string query = @"SELECT MAKH, TEN, DIENTHOAI, CHIETKHAU
                             FROM dbo.tabKHACHHANG
                             WHERE MAKH = @maKH";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maKH", maKH);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtMaKH.Text = reader["MAKH"] != DBNull.Value ? reader["MAKH"].ToString() : "";
                        txtTenKH.Text = reader["TEN"] != DBNull.Value ? reader["TEN"].ToString() : "";
                        txtSDT.Text = reader["DIENTHOAI"] != DBNull.Value ? reader["DIENTHOAI"].ToString() : "";
                        txtCK.Text = reader["CHIETKHAU"] != DBNull.Value ? reader["CHIETKHAU"].ToString() : "";
                    }
                    else
                    {
                        ClearKhachHangInfo();
                    }
                }
            }
        }

        private void ClearKhachHangInfo()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtCK.Clear();
        }

        private void dgvDieuPhoi_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDieuPhoi.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDieuPhoi.SelectedRows[0];
                string soHD = row.Cells["soHDDataGridViewTextBoxColumn"].Value?.ToString() ?? string.Empty;
                string maKH = row.Cells["makhDataGridViewTextBoxColumn"].Value?.ToString() ?? string.Empty;

                _soHDHienTai = soHD;
                LoadThongTinKhachHang(maKH);
                LoadChiTietDieuPhoi(soHD);
            }
            else
            {
                _soHDHienTai = string.Empty;
                ClearKhachHangInfo();
                dgvChiTietDieuPhoi.DataSource = null;
                lblTongSoTien.Text = "0";
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            tmrSearch.Enabled = false;
            tmrSearch.Enabled = true;
        }

        private void tmrSearch_Tick(object sender, EventArgs e)
        {
            tmrSearch.Enabled = false;
            string keyword = txtSearch.Text.Trim();
            LoadDanhSachDieuPhoi(keyword);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmTaoDieuPhoi frm = new frmTaoDieuPhoi();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachDieuPhoi();
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (dgvDieuPhoi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách phiếu điều phối.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string soHD = dgvDieuPhoi.SelectedRows[0].Cells["soHDDataGridViewTextBoxColumn"].Value?.ToString();
            if (string.IsNullOrEmpty(soHD)) return;

            frmTaoDieuPhoi frm = new frmTaoDieuPhoi(soHD);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachDieuPhoi();
                if (!string.IsNullOrEmpty(_soHDHienTai))
                    LoadChiTietDieuPhoi(_soHDHienTai);
            }
        }
    }
}
