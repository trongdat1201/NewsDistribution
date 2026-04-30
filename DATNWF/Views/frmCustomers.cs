using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmCustomers : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            this.tabKHACHHANGTableAdapter.Fill(this.thanhnienDataSet3.tabKHACHHANG);
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT MAKH, TEN, DIACHI, DIENTHOAI, CHIETKHAU, P_PH, P_KT, UUTIEN FROM tabKHACHHANG";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dboTabKhachHang.DataSource = dt;
                dboTabKhachHang.ClearSelection();
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dboTabKhachHang.DataSource != null && dboTabKhachHang.DataSource is DataTable)
            {
                DataTable dt = (DataTable)dboTabKhachHang.DataSource;

                string keyword = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    dt.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    string query = string.Format("MAKH LIKE '%{0}%' OR TEN LIKE '%{0}%' OR DIENTHOAI LIKE '%{0}%'", keyword);

                    try
                    {
                        dt.DefaultView.RowFilter = query;
                    }
                    catch (Exception)
                    {

                    }
                }
                dboTabKhachHang.ClearSelection();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmThemKhachHang frm = new frmThemKhachHang();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dboTabKhachHang.SelectedRows.Count == 0 ||
                dboTabKhachHang.SelectedRows[0].Cells["mAKHDataGridViewTextBoxColumn"].Value == null ||
                string.IsNullOrWhiteSpace(dboTabKhachHang.SelectedRows[0].Cells["mAKHDataGridViewTextBoxColumn"].Value.ToString()))
            {
                MessageBox.Show("Vui lòng click chọn một khách hàng trên bảng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maKH = dboTabKhachHang.SelectedRows[0].Cells["mAKHDataGridViewTextBoxColumn"].Value.ToString();

            frmSuaKhachHang frm = new frmSuaKhachHang(maKH);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dboTabKhachHang.SelectedRows.Count == 0 || dboTabKhachHang.SelectedRows[0].Cells["mAKHDataGridViewTextBoxColumn"].Value == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!"); return;
            }

            string makh = dboTabKhachHang.SelectedRows[0].Cells["mAKHDataGridViewTextBoxColumn"].Value.ToString();
            string ten = dboTabKhachHang.SelectedRows[0].Cells["tENDataGridViewTextBoxColumn"].Value.ToString();

            if (MessageBox.Show($"Bạn muốn xóa khách hàng '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "DELETE FROM tabKHACHHANG WHERE MAKH = @makh";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@makh", makh);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        LoadData();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547)
                            MessageBox.Show("Không thể xóa! Khách hàng này đã phát sinh Hóa Đơn hoặc Điều Phối.");
                        else
                            MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }
    }
}
