using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.Charts.WinForms;

namespace DATNWF.Views
{
    public partial class frmCustomers : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            this.tabKHACHHANGTableAdapter.Fill(this.thanhnienDataSet3.tabKHACHHANG);
            LoadData();
            LoadKhachHangOrderGanDay();
            LoadChartPhanLoai();
            LoadChartDoanhThu();
        }

        #region Biểu đồ phân loại P_PH / P_KT

        private void LoadChartPhanLoai()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT
                        CASE
                            WHEN P_PH = 1 AND P_KT = 1 THEN N'P_PH & P_KT'
                            WHEN P_PH = 1 THEN N'P_PH'
                            WHEN P_KT = 1 THEN N'P_KT'
                            ELSE N'Không phân loại'
                        END AS Loai,
                        COUNT(*) AS SoLuong
                    FROM tabKHACHHANG
                    GROUP BY
                        CASE
                            WHEN P_PH = 1 AND P_KT = 1 THEN N'P_PH & P_KT'
                            WHEN P_PH = 1 THEN N'P_PH'
                            WHEN P_KT = 1 THEN N'P_KT'
                            ELSE N'Không phân loại'
                        END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                var ds = new GunaDoughnutDataset { Label = "Phân loại khách hàng" };
                ds.FillColors.AddRange(new[]
                {
                    Color.FromArgb(100, 88, 255),
                    Color.FromArgb(255, 192, 128),
                    Color.FromArgb(76, 175, 80),
                    Color.FromArgb(158, 158, 158)
                });

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string loai = reader["Loai"].ToString();
                        int count = reader.GetInt32(1);
                        ds.DataPoints.Add(loai, count);
                    }
                }

                chartPhanLoai.Datasets.Clear();
                chartPhanLoai.Datasets.Add(ds);
                chartPhanLoai.Legend.Position = LegendPosition.Right;
                chartPhanLoai.Legend.Display = true;
                chartPhanLoai.Refresh();
            }
        }

        #endregion

        #region Biểu đồ doanh thu top khách hàng

        private void LoadChartDoanhThu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT TOP 5
                        kh.TEN,
                        SUM(ct.thanhTien) AS TongDoanhThu
                    FROM tabKHACHHANG kh
                    JOIN tabHOADON h ON kh.MAKH = h.makh
                    JOIN tabCHITIETHOADON ct ON h.sohd = ct.sohd
                    GROUP BY kh.MAKH, kh.TEN
                    ORDER BY TongDoanhThu DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                var ds = new GunaBarDataset { Label = "Doanh thu (VNĐ)" };
                ds.FillColors.Add(Color.FromArgb(255, 192, 128));
                ds.BorderColors.Add(Color.FromArgb(255, 160, 80));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ten = reader["TEN"].ToString();
                        double dt = reader["TongDoanhThu"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TongDoanhThu"]);
                        ds.DataPoints.Add(ten, dt);
                    }
                }

                chartDoanhThu.Datasets.Clear();
                chartDoanhThu.Datasets.Add(ds);
                chartDoanhThu.XAxes.Display = true;
                chartDoanhThu.YAxes.Display = true;
                chartDoanhThu.Legend.Display = false;
                chartDoanhThu.Refresh();
            }
        }

        #endregion
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
                LoadChartPhanLoai();
                LoadChartDoanhThu();
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

            frmChiTietKhachHang frm = new frmChiTietKhachHang(maKH);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
                LoadChartPhanLoai();
                LoadChartDoanhThu();
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
                        LoadChartPhanLoai();
                        LoadChartDoanhThu();
                        LoadKhachHangOrderGanDay();
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
        private void LoadKhachHangOrderGanDay()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT TOP 10 kh.MAKH, kh.TEN 
                    FROM tabKHACHHANG kh
                    INNER JOIN tabHOADON hd ON kh.MAKH = hd.makh
                    GROUP BY kh.MAKH, kh.TEN
                    ORDER BY MAX(hd.ngayLapPhieu) DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                DataTable dtGanDay = new DataTable();

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtGanDay);

                    dgvKhachHangGanDay.DataSource = dtGanDay;

                    if (dgvKhachHangGanDay.Columns.Count > 0)
                    {
                        dgvKhachHangGanDay.Columns["MAKH"].Visible = false;

                        dgvKhachHangGanDay.Columns["TEN"].HeaderText = "Khách hàng order gần đây";
                        dgvKhachHangGanDay.Columns["TEN"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    dgvKhachHangGanDay.ColumnHeadersVisible = false;
                    dgvKhachHangGanDay.RowHeadersVisible = false;
                    dgvKhachHangGanDay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvKhachHangGanDay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvKhachHangGanDay.AllowUserToAddRows = false;
                    dgvKhachHangGanDay.ReadOnly = true;
                    dgvKhachHangGanDay.BackgroundColor = Color.White;
                    dgvKhachHangGanDay.BorderStyle = BorderStyle.None;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi tải danh sách khách hàng order gần đây: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
