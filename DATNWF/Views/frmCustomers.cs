using DATNWF.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.Charts.WinForms;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmCustomers : Form
    {
        private List<KhachHangDto> _danhSachGoc = new List<KhachHangDto>();

        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadChartPhanLoai();
            LoadChartDoanhThu();
        }

        #region Biểu đồ phân loại P_PH / P_KT

        private class ClassificationResponse
        {
            public string Loai { get; set; }
            public int SoLuong { get; set; }
        }

        private void LoadChartPhanLoai()
        {
            try
            {
                var list = ApiClient.Instance.GetAsync<List<ClassificationResponse>>("Customers/classification-chart").GetAwaiter().GetResult();

                var ds = new GunaDoughnutDataset { Label = "Phân loại khách hàng" };
                ds.FillColors.AddRange(new[]
                {
                    Color.FromArgb(100, 88, 255),
                    Color.FromArgb(255, 192, 128),
                    Color.FromArgb(76, 175, 80),
                    Color.FromArgb(158, 158, 158)
                });

                if (list != null)
                {
                    foreach (var item in list)
                    {
                        ds.DataPoints.Add(item.Loai, item.SoLuong);
                    }
                }

                chartPhanLoai.Datasets.Clear();
                chartPhanLoai.Datasets.Add(ds);
                chartPhanLoai.Legend.Position = LegendPosition.Right;
                chartPhanLoai.Legend.Display = true;
                chartPhanLoai.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phân loại khách hàng: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Biểu đồ doanh thu top khách hàng

        private class TopCustomerResponse
        {
            public string Ten { get; set; }
            public double TongDoanhThu { get; set; }
        }

        private void LoadChartDoanhThu()
        {
            try
            {
                var list = ApiClient.Instance.GetAsync<List<TopCustomerResponse>>("Customers/top-revenue-chart").GetAwaiter().GetResult();

                var ds = new GunaBarDataset { Label = "Doanh thu (VNĐ)" };
                ds.FillColors.Add(Color.FromArgb(255, 192, 128));
                ds.BorderColors.Add(Color.FromArgb(255, 160, 80));

                if (list != null)
                {
                    foreach (var item in list)
                    {
                        ds.DataPoints.Add(item.Ten, item.TongDoanhThu);
                    }
                }

                chartDoanhThu.Datasets.Clear();
                chartDoanhThu.Datasets.Add(ds);
                chartDoanhThu.XAxes.Display = true;
                chartDoanhThu.YAxes.Display = true;
                chartDoanhThu.Legend.Display = false;
                chartDoanhThu.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải doanh thu khách hàng: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void LoadData()
        {
            LoadDataAsync();
            LoadKhachHangOrderGanDayAsync();
        }

        private void LoadDataAsync()
        {
            try
            {
                _danhSachGoc = ApiClient.Instance.GetAsync<List<KhachHangDto>>("Customers").GetAwaiter().GetResult();
                dboTabKhachHang.DataSource = _danhSachGoc;
                dboTabKhachHang.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến Server API: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dboTabKhachHang.DataSource = _danhSachGoc;
            }
            else
            {
                var filteredList = _danhSachGoc.Where(kh =>
                    (kh.MaKH != null && kh.MaKH.ToLower().Contains(keyword)) ||
                    (kh.Ten != null && kh.Ten.ToLower().Contains(keyword)) ||
                    (kh.DienThoai != null && kh.DienThoai.Contains(keyword))
                ).ToList();

                dboTabKhachHang.DataSource = filteredList;
            }
            dboTabKhachHang.ClearSelection();
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
                dboTabKhachHang.SelectedRows[0].Cells["MaKH"].Value == null)
            {
                MessageBox.Show("Vui lòng click chọn một khách hàng trên bảng để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maKH = dboTabKhachHang.SelectedRows[0].Cells["MaKH"].Value.ToString();

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
            if (dboTabKhachHang.SelectedRows.Count == 0 || dboTabKhachHang.SelectedRows[0].Cells["MaKH"].Value == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!"); return;
            }

            string makh = dboTabKhachHang.SelectedRows[0].Cells["MaKH"].Value.ToString();
            string ten = dboTabKhachHang.SelectedRows[0].Cells["Ten"].Value.ToString();

            if (MessageBox.Show($"Bạn muốn xóa khách hàng '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool ok = ApiClient.Instance.DeleteAsync($"Customers/{makh}").GetAwaiter().GetResult();
                    if (ok)
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadData();
                        LoadChartPhanLoai();
                        LoadChartDoanhThu();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadKhachHangOrderGanDayAsync()
        {
            try
            {
                var dtGanDay = ApiClient.Instance.GetAsync<List<KhachHangGanDayDto>>("Customers/recent-orders").GetAwaiter().GetResult();

                dgvKhachHangGanDay.DataSource = dtGanDay;

                if (dgvKhachHangGanDay.Columns.Count > 0)
                {
                    dgvKhachHangGanDay.Columns["MaKH"].Visible = false;
                    dgvKhachHangGanDay.Columns["Ten"].HeaderText = "Khách hàng order gần đây";
                    dgvKhachHangGanDay.Columns["Ten"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khách hàng order gần đây: " + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}