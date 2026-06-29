using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.Charts.WinForms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmChiTietKhachHang : Form
    {
        private string maKHCanSua;
        private List<CustomerHistoryDto> _listHistory = new List<CustomerHistoryDto>();

        // giá trị mặc định được load lúc mở form
        private string _defaultTen;
        private string _defaultDiaChi;
        private string _defaultDienThoai;
        private string _defaultChietKhau;
        private bool _defaultP_PH;
        private bool _defaultP_KT;
        private string _defaultUuTien;

        public frmChiTietKhachHang(string maKH)
        {
            InitializeComponent();
            maKHCanSua = maKH;
        }

        #region Chart

        private class ChartDataPoint
        {
            public string Label { get; set; }
            public double DoanhThu { get; set; }
            public int SoDonHang { get; set; }
        }

        private List<ChartDataPoint> LoadChartData()
        {
            try
            {
                return ApiClient.Instance.GetAsync<List<ChartDataPoint>>($"Customers/{maKHCanSua}/growth-chart").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải biểu đồ tăng trưởng: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<ChartDataPoint>();
            }
        }

        private void RenderChart(List<ChartDataPoint> data)
        {
            if (data == null || data.Count == 0)
            {
                chartthongketangtruong.Datasets.Clear();
                return;
            }

            var seriesDoanhThu = new GunaLineDataset { Label = "Doanh thu (VNĐ)" };
            var seriesSoDon = new GunaLineDataset
            {
                Label = "Số đơn hàng",
                BorderColor = Color.Red,
                FillColor = Color.FromArgb(50, Color.Red)
            };

            foreach (var p in data)
            {
                seriesDoanhThu.DataPoints.Add(p.Label, p.DoanhThu);
                seriesSoDon.DataPoints.Add(p.Label, p.SoDonHang);
            }

            chartthongketangtruong.Datasets.Clear();
            chartthongketangtruong.Datasets.Add(seriesDoanhThu);
            chartthongketangtruong.Datasets.Add(seriesSoDon);

            chartthongketangtruong.Refresh();
        }

        #endregion

        #region Lịch sử giao dịch

        private class CustomerHistoryDto
        {
            public string SoHD { get; set; }
            public DateTime NgayLapPhieu { get; set; }
            public string TenBao { get; set; }
            public int SoLuong { get; set; }
            public double DonGia { get; set; }
            public double ThanhTien { get; set; }
        }

        private List<CustomerHistoryDto> FetchLichSuGiaoDich()
        {
            try
            {
                return ApiClient.Instance.GetAsync<List<CustomerHistoryDto>>($"Customers/{maKHCanSua}/history").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử giao dịch: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<CustomerHistoryDto>();
            }
        }

        private void BindLichSu(List<CustomerHistoryDto> list)
        {
            dgvLichsugiaodich.DataSource = null;
            dgvLichsugiaodich.DataSource = list;
            dgvLichsugiaodich.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            if (dgvLichsugiaodich.Columns.Count > 0)
            {
                if (dgvLichsugiaodich.Columns["SoHD"] != null)
                {
                    dgvLichsugiaodich.Columns["SoHD"].HeaderText = "Số hóa đơn";
                    dgvLichsugiaodich.Columns["SoHD"].Width = 120;
                }

                if (dgvLichsugiaodich.Columns["NgayLapPhieu"] != null)
                {
                    dgvLichsugiaodich.Columns["NgayLapPhieu"].HeaderText = "Ngày lập";
                    dgvLichsugiaodich.Columns["NgayLapPhieu"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvLichsugiaodich.Columns["NgayLapPhieu"].Width = 110;
                }

                if (dgvLichsugiaodich.Columns["TenBao"] != null)
                {
                    dgvLichsugiaodich.Columns["TenBao"].HeaderText = "Tên báo";
                    dgvLichsugiaodich.Columns["TenBao"].Width = 180;
                }

                if (dgvLichsugiaodich.Columns["SoLuong"] != null)
                {
                    dgvLichsugiaodich.Columns["SoLuong"].HeaderText = "Số lượng";
                    dgvLichsugiaodich.Columns["SoLuong"].Width = 80;
                    dgvLichsugiaodich.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvLichsugiaodich.Columns["DonGia"] != null)
                {
                    dgvLichsugiaodich.Columns["DonGia"].HeaderText = "Đơn giá";
                    dgvLichsugiaodich.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                    dgvLichsugiaodich.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvLichsugiaodich.Columns["DonGia"].Width = 100;
                }

                if (dgvLichsugiaodich.Columns["ThanhTien"] != null)
                {
                    dgvLichsugiaodich.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    dgvLichsugiaodich.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                    dgvLichsugiaodich.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvLichsugiaodich.Columns["ThanhTien"].Width = 120;
                }

                dgvLichsugiaodich.ReadOnly = true;
                dgvLichsugiaodich.AllowUserToResizeRows = false;
                dgvLichsugiaodich.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_listHistory == null) return;

            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                BindLichSu(_listHistory);
                return;
            }

            var filtered = _listHistory.Where(h =>
                (h.SoHD != null && h.SoHD.ToLower().Contains(keyword)) ||
                (h.TenBao != null && h.TenBao.ToLower().Contains(keyword))
            ).ToList();

            BindLichSu(filtered);
        }

        #endregion

        #region Form Load

        private void frmSuaKhachHang_Load(object sender, EventArgs e)
        {
            txtMaKH.Enabled = false;

            KhachHangDto khData = null;
            try
            {
                khData = ApiClient.Instance.GetAsync<KhachHangDto>($"Customers/{maKHCanSua}").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin chi tiết khách hàng: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (khData != null)
            {
                txtMaKH.Text = khData.MaKH;
                txtTenKH.Text = khData.Ten;
                txtDiaChi.Text = khData.DiaChi ?? "";
                txtDienThoai.Text = khData.DienThoai ?? "";
                txtChietKhau.Text = khData.ChietKhau.ToString();
                chkP_PH.Checked = khData.P_PH;
                chkP_KT.Checked = khData.P_KT;
                if (khData.Uutien != null)
                    cboUuTien.SelectedItem = khData.Uutien;

                // lưu giá trị mặc định để btnDefault restore sau này
                _defaultTen = khData.Ten;
                _defaultDiaChi = khData.DiaChi ?? "";
                _defaultDienThoai = khData.DienThoai ?? "";
                _defaultChietKhau = khData.ChietKhau.ToString();
                _defaultP_PH = khData.P_PH;
                _defaultP_KT = khData.P_KT;
                _defaultUuTien = khData.Uutien;
            }

            var chartData = LoadChartData();
            RenderChart(chartData);

            _listHistory = FetchLichSuGiaoDich();
            BindLichSu(_listHistory);
        }

        #endregion

        #region Save / Cancel

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Mã khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKH.Focus(); return;
            }
            if (txtMaKH.Text.Length > 30)
            {
                MessageBox.Show("Mã khách hàng không vượt quá 30 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus(); return;
            }

            if (!short.TryParse(txtChietKhau.Text, out short chietKhau) || chietKhau < 0 || chietKhau > 100)
            {
                MessageBox.Show("Chiết khấu phải là số từ 0 đến 100!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChietKhau.Focus(); return;
            }

            var kh = new KhachHangDto
            {
                MaKH = maKHCanSua,
                Ten = txtTenKH.Text.Trim(),
                DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim(),
                DienThoai = string.IsNullOrWhiteSpace(txtDienThoai.Text) ? null : txtDienThoai.Text.Trim(),
                ChietKhau = chietKhau,
                P_PH = chkP_PH.Checked,
                P_KT = chkP_KT.Checked,
                Uutien = cboUuTien.SelectedItem == null ? null : cboUuTien.SelectedItem.ToString()
            };

            try
            {
                bool success = ApiClient.Instance.PutAsync($"Customers/{maKHCanSua}", kh).GetAwaiter().GetResult();
                if (success)
                {
                    MessageBox.Show("Cập nhật Khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi: Không thể lưu thông tin cập nhật khách hàng.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtTenKH.Text = _defaultTen;
            txtDiaChi.Text = _defaultDiaChi;
            txtDienThoai.Text = _defaultDienThoai;
            txtChietKhau.Text = _defaultChietKhau;
            chkP_PH.Checked = _defaultP_PH;
            chkP_KT.Checked = _defaultP_KT;
            cboUuTien.SelectedItem = _defaultUuTien;
        }

        #endregion
    }
}
