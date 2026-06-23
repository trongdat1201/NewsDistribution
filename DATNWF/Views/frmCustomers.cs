using DATNWF.Models;   // [THAY ĐỔI] Thêm thư mục chứa class DTO
using Newtonsoft.Json; // [THAY ĐỔI] Thêm thư viện dịch chuỗi JSON
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http; // [THAY ĐỔI] Thêm thư viện gọi API
using System.Threading.Tasks;
using System.Windows.Forms;
<<<<<<< HEAD
using Guna.Charts.WinForms;
=======
using DATNWF.Models.DTO;
>>>>>>> 77aeb72f4bcc074768ac1db263dc5a33b2c68ed5

namespace DATNWF.Views
{
    public partial class frmCustomers : Form
    {
        // [THAY ĐỔI] Xóa sạch chuỗi kết nối SQL và thay bằng biến HTTP Client / URL
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7088/api/Customers";

        // [THAY ĐỔI] Lưu lại danh sách gốc để phục vụ chức năng Tìm kiếm (Search)
        private List<KhachHangDto> _danhSachGoc = new List<KhachHangDto>();

        public frmCustomers()
        {
            InitializeComponent();
        }

        // [THAY ĐỔI] Form Load phải là 'async'
        private async void frmCustomers_Load(object sender, EventArgs e)
        {
<<<<<<< HEAD
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
=======
            // Xóa bỏ lệnh TableAdapter.Fill cũ
            await LoadDataAsync();
            await LoadKhachHangOrderGanDayAsync();
        }

        // [THAY ĐỔI] Đổi thành hàm Async, gọi API Get
        private async Task LoadDataAsync()
>>>>>>> 77aeb72f4bcc074768ac1db263dc5a33b2c68ed5
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_apiBaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    _danhSachGoc = JsonConvert.DeserializeObject<List<KhachHangDto>>(jsonResult);

                    dboTabKhachHang.DataSource = _danhSachGoc;
                    dboTabKhachHang.ClearSelection();
                }
                else
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu từ API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến Server API: " + ex.Message);
            }
        }

        // [THAY ĐỔI] Hàm tìm kiếm dùng LINQ thay vì DataTable.RowFilter
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Trả về danh sách gốc nếu không gõ gì
                dboTabKhachHang.DataSource = _danhSachGoc;
            }
            else
            {
                // Lọc dữ liệu trực tiếp trên List bằng LINQ
                var filteredList = _danhSachGoc.Where(kh =>
                    (kh.MaKH != null && kh.MaKH.ToLower().Contains(keyword)) ||
                    (kh.Ten != null && kh.Ten.ToLower().Contains(keyword)) ||
                    (kh.DienThoai != null && kh.DienThoai.Contains(keyword))
                ).ToList();

                dboTabKhachHang.DataSource = filteredList;
            }
            dboTabKhachHang.ClearSelection();
        }

        private async void btnAddNew_Click(object sender, EventArgs e)
        {
            frmThemKhachHang frm = new frmThemKhachHang();
            if (frm.ShowDialog() == DialogResult.OK)
            {
<<<<<<< HEAD
                LoadData();
                LoadChartPhanLoai();
                LoadChartDoanhThu();
=======
                await LoadDataAsync(); // Cập nhật lại grid
>>>>>>> 77aeb72f4bcc074768ac1db263dc5a33b2c68ed5
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            // Chú ý: Tên cột phải khớp với thuộc tính của Class KhachHangDto
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
<<<<<<< HEAD
                LoadData();
                LoadChartPhanLoai();
                LoadChartDoanhThu();
=======
                await LoadDataAsync();
>>>>>>> 77aeb72f4bcc074768ac1db263dc5a33b2c68ed5
            }
        }

        // [THAY ĐỔI] Gọi API Delete thay vì chạy lệnh SQL
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dboTabKhachHang.SelectedRows.Count == 0 || dboTabKhachHang.SelectedRows[0].Cells["MaKH"].Value == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!"); return;
            }

            string makh = dboTabKhachHang.SelectedRows[0].Cells["MaKH"].Value.ToString();
            string ten = dboTabKhachHang.SelectedRows[0].Cells["Ten"].Value.ToString();

            if (MessageBox.Show($"Bạn muốn xóa khách hàng '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    // Gọi API với method DELETE
                    HttpResponseMessage response = await _client.DeleteAsync($"{_apiBaseUrl}/{makh}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Xóa thành công!");
<<<<<<< HEAD
                        LoadData();
                        LoadChartPhanLoai();
                        LoadChartDoanhThu();
                        LoadKhachHangOrderGanDay();
=======
                        await LoadDataAsync();
>>>>>>> 77aeb72f4bcc074768ac1db263dc5a33b2c68ed5
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        // Giả sử API trả về lỗi 409 Conflict nếu vướng khóa ngoại (lỗi 547 SQL cũ)
                        MessageBox.Show("Không thể xóa! Khách hàng này đã phát sinh Hóa Đơn hoặc Điều Phối.");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi xóa từ server: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối Server: " + ex.Message);
                }
            }
        }

        // [THAY ĐỔI] Lấy dữ liệu gần đây từ Endpoint riêng của API
        private async Task LoadKhachHangOrderGanDayAsync()
        {
            try
            {
                string url = $"{_apiBaseUrl}/recent-orders";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var dtGanDay = JsonConvert.DeserializeObject<List<KhachHangGanDayDto>>(jsonResult);

                    dgvKhachHangGanDay.DataSource = dtGanDay;

                    // Tùy chỉnh hiển thị cột giống như cũ
                    if (dgvKhachHangGanDay.Columns.Count > 0)
                    {
                        dgvKhachHangGanDay.Columns["MaKH"].Visible = false;
                        dgvKhachHangGanDay.Columns["Ten"].HeaderText = "Khách hàng order gần đây";
                        dgvKhachHangGanDay.Columns["Ten"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    // Các cấu hình giao diện giữ nguyên
                    dgvKhachHangGanDay.ColumnHeadersVisible = false;
                    dgvKhachHangGanDay.RowHeadersVisible = false;
                    dgvKhachHangGanDay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvKhachHangGanDay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvKhachHangGanDay.AllowUserToAddRows = false;
                    dgvKhachHangGanDay.ReadOnly = true;
                    dgvKhachHangGanDay.BackgroundColor = Color.White;
                    dgvKhachHangGanDay.BorderStyle = BorderStyle.None;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khách hàng order gần đây: " + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}