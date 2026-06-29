using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmTaoDieuPhoi : Form
    {
        private string maKhachHangDuocChon = "";
        private readonly string _soHDEdit = null;
        private readonly bool _isEditMode = false;

        public frmTaoDieuPhoi() { InitializeComponent(); }

        public frmTaoDieuPhoi(string soHD)
        {
            InitializeComponent();
            _soHDEdit = soHD;
            _isEditMode = true;
        }

        private void FormDieuPhoi_Load(object sender, EventArgs e)
        {
            SetupDataGridView();

            if (_isEditMode)
            {
                txtSoHD.Text = _soHDEdit;
                txtSoHD.Enabled = false;
                LoadChiTietDieuPhoiForEdit(_soHDEdit);
                SetUIStateForEdit();
            }
            else
            {
                ResetForm();
            }
        }

        private void SetUIStateForEdit()
        {
            picTimKH.Enabled = false;
            dtpNgayLapPhieu.Enabled = false;
            dtpTuNgay.Enabled = false;
            dtpDenNgay.Enabled = false;
            txtGhiChu.Enabled = false;
            btnCreate.Enabled = false;

            txtMaKH.Enabled = false;
            txtSoHD.Enabled = false;
            btnSave.Enabled = true;
            btnRefresh.Enabled = false;
            btnClose.Enabled = true;
            dgvChiTiet.Enabled = true;
        }

        private void SetUIState(bool isCreatingMaster)
        {
            this.SuspendLayout();

            picTimKH.Enabled = isCreatingMaster;
            dtpNgayLapPhieu.Enabled = isCreatingMaster;
            dtpTuNgay.Enabled = isCreatingMaster;
            dtpDenNgay.Enabled = isCreatingMaster;
            txtGhiChu.Enabled = isCreatingMaster;
            btnCreate.Enabled = isCreatingMaster;

            txtSoHD.Enabled = false;
            txtSoHD.ReadOnly = true;
            txtSoHD.TabStop = false;

            txtMaKH.Enabled = false;
            txtMaKH.ReadOnly = true;
            txtMaKH.TabStop = false;

            dgvChiTiet.Enabled = !isCreatingMaster;
            btnSave.Enabled = !isCreatingMaster;

            btnRefresh.Enabled = false;
            btnClose.Enabled = true;

            this.ResumeLayout();
        }

        private void ResetForm()
        {
            txtSoHD.Clear();
            txtMaKH.Clear();
            txtGhiChu.Clear();
            maKhachHangDuocChon = "";

            dtpNgayLapPhieu.Value = DateTime.Now;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            dgvChiTiet.DataSource = null;
            dgvChiTiet.Columns.Clear();

            SetUIState(true);
        }

        private void picTimKH_Click(object sender, EventArgs e)
        {
            using (frmTimKhachHang frmSearch = new frmTimKhachHang())
            {
                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    txtMaKH.Text = frmSearch.TenKH_Selected;
                    maKhachHangDuocChon = frmSearch.MaKH_Selected;
                    SinhMaHoaDonDieuPhoiTuDong(maKhachHangDuocChon);
                }
            }
        }

        private void SinhMaHoaDonDieuPhoiTuDong(string maKH)
        {
            if (string.IsNullOrEmpty(maKH)) return;

            string loaiMaPrefix = "PT";
            try
            {
                var khData = ApiClient.Instance.GetAsync<KhachHangDto>($"Customers/{maKH}").GetAwaiter().GetResult();
                if (khData != null)
                {
                    bool isLe = khData.P_PH;
                    bool isDaiLy = khData.P_KT;

                    if (isDaiLy) loaiMaPrefix = "PD";
                    else if (isLe) loaiMaPrefix = "PT";
                }

                string namHienTai = DateTime.Now.ToString("yy");
                string dinhDangTimKiem = loaiMaPrefix + namHienTai + "_";

                var allDeliveries = ApiClient.Instance.GetAsync<List<DieuPhoiDto>>("Deliveries").GetAwaiter().GetResult();
                int soThuTuTiepTheo = 1;

                if (allDeliveries != null)
                {
                    var matching = allDeliveries
                        .Where(x => x.Sohd.StartsWith(dinhDangTimKiem))
                        .Select(x => {
                            string suffix = x.Sohd.Substring(dinhDangTimKiem.Length);
                            int val;
                            return int.TryParse(suffix, out val) ? val : 0;
                        })
                        .DefaultIfEmpty(0)
                        .Max();
                    soThuTuTiepTheo = matching + 1;
                }

                txtSoHD.Text = dinhDangTimKiem + soThuTuTiepTheo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tự động sinh số hóa đơn: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maKhachHangDuocChon) || string.IsNullOrWhiteSpace(txtSoHD.Text))
            {
                MessageBox.Show("Vui lòng chọn Khách hàng để khởi tạo Số hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;
            DateTime ngayHienTai = DateTime.Now.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Lỗi: 'Từ ngày' không được lớn hơn 'Đến ngày'!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (denNgay < ngayHienTai)
            {
                MessageBox.Show("Lỗi: 'Đến ngày' không được nhỏ hơn ngày hiện tại!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tuNgay < ngayHienTai)
            {
                MessageBox.Show("Lỗi: 'Từ ngày' không được nhỏ hơn ngày hiện tại!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetUIState(false);

            LoadBaoPhatHanhTheoGiaiDoan(tuNgay, denNgay);
        }

        private void SetupDataGridView()
        {
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.AutoGenerateColumns = true;

            dgvChiTiet.CellDoubleClick -= dgvChiTiet_CellDoubleClick;
            dgvChiTiet.CellDoubleClick += dgvChiTiet_CellDoubleClick;
        }

        private class DetailDeliveryResponse
        {
            public string Sohd { get; set; }
            public DateTime NgayNhan { get; set; }
            public string MaBao { get; set; }
            public string Tenbao { get; set; }
            public int Sobao { get; set; }
            public double DonGia { get; set; }
            public int SoluongDieuPhoi { get; set; }
            public int SoluongBan { get; set; }
            public double ThanhTien { get; set; }
        }

        private void LoadChiTietDieuPhoiForEdit(string soHD)
        {
            try
            {
                var deliveriesList = ApiClient.Instance.GetAsync<List<DieuPhoiDto>>("Deliveries").GetAwaiter().GetResult();
                var dp = deliveriesList.FirstOrDefault(x => x.Sohd == soHD);

                if (dp != null)
                {
                    maKhachHangDuocChon = dp.Makh ?? "";
                    dtpNgayLapPhieu.Value = dp.NgayLapPhieu ?? DateTime.Now;
                    dtpTuNgay.Value = dp.TuNgay ?? DateTime.Now;
                    dtpDenNgay.Value = dp.DenNgay ?? DateTime.Now;
                    txtGhiChu.Text = dp.GhiChu ?? "";

                    LoadTenKhachHang(maKhachHangDuocChon);
                }

                var details = ApiClient.Instance.GetAsync<List<DetailDeliveryResponse>>($"Deliveries/{soHD}/details").GetAwaiter().GetResult();
                
                DataTable dt = new DataTable();
                dt.Columns.Add("ngayNhan", typeof(DateTime));
                dt.Columns.Add("maBao", typeof(string));
                dt.Columns.Add("tenBao", typeof(string));
                dt.Columns.Add("soBao", typeof(int));
                dt.Columns.Add("donGia", typeof(decimal));
                dt.Columns.Add("soluongDieuPhoi", typeof(int));
                dt.Columns.Add("soluongBan", typeof(int));
                dt.Columns.Add("thanhTien", typeof(decimal));

                foreach (var detail in details)
                {
                    dt.Rows.Add(detail.NgayNhan, detail.MaBao, detail.Tenbao, detail.Sobao, (decimal)detail.DonGia, detail.SoluongDieuPhoi, detail.SoluongBan, (decimal)detail.ThanhTien);
                }

                dgvChiTiet.DataSource = dt;

                if (dgvChiTiet.Columns.Count > 0)
                {
                    dgvChiTiet.Columns["ngayNhan"].HeaderText = "Ngày nhận";
                    dgvChiTiet.Columns["maBao"].HeaderText = "Mã báo";
                    dgvChiTiet.Columns["tenBao"].HeaderText = "Tên ấn phẩm";
                    dgvChiTiet.Columns["soBao"].HeaderText = "Số báo";
                    dgvChiTiet.Columns["donGia"].HeaderText = "Đơn giá";
                    dgvChiTiet.Columns["soluongDieuPhoi"].HeaderText = "SL Điều phối";
                    dgvChiTiet.Columns["soluongBan"].HeaderText = "SL Bán thực";
                    dgvChiTiet.Columns["thanhTien"].HeaderText = "Thành tiền";

                    dgvChiTiet.Columns["ngayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvChiTiet.Columns["donGia"].DefaultCellStyle.Format = "N0";
                    dgvChiTiet.Columns["thanhTien"].DefaultCellStyle.Format = "N0";

                    dgvChiTiet.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    dgvChiTiet.Columns["ngayNhan"].Width = 110;
                    dgvChiTiet.Columns["maBao"].Width = 90;
                    dgvChiTiet.Columns["tenBao"].Width = 160;
                    dgvChiTiet.Columns["soBao"].Width = 70;
                    dgvChiTiet.Columns["donGia"].Width = 100;
                    dgvChiTiet.Columns["soluongDieuPhoi"].Width = 120;
                    dgvChiTiet.Columns["soluongBan"].Width = 120;
                    dgvChiTiet.Columns["thanhTien"].Width = 120;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết điều phối: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTenKhachHang(string maKH)
        {
            try
            {
                var kh = ApiClient.Instance.GetAsync<KhachHangDto>($"Customers/{maKH}").GetAwaiter().GetResult();
                txtMaKH.Text = kh?.Ten ?? "";
            }
            catch
            {
                txtMaKH.Text = "";
            }
        }

        private bool GetThuValue(BaoDto bao, string cotThu)
        {
            switch (cotThu.ToLower())
            {
                case "thu1": return bao.Thu1 ?? false;
                case "thu2": return bao.Thu2 ?? false;
                case "thu3": return bao.Thu3 ?? false;
                case "thu4": return bao.Thu4 ?? false;
                case "thu5": return bao.Thu5 ?? false;
                case "thu6": return bao.Thu6 ?? false;
                case "thu7": return bao.Thu7 ?? false;
                default: return false;
            }
        }

        private void LoadBaoPhatHanhTheoGiaiDoan(DateTime tuNgay, DateTime denNgay)
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.Columns.Clear();

            DataTable dtChiTiet = new DataTable();
            dtChiTiet.Columns.Add("ngayNhan", typeof(DateTime));
            dtChiTiet.Columns.Add("maBao", typeof(string));
            dtChiTiet.Columns.Add("tenBao", typeof(string));
            dtChiTiet.Columns.Add("soBao", typeof(int));
            dtChiTiet.Columns.Add("donGia", typeof(decimal));
            dtChiTiet.Columns.Add("soluongDieuPhoi", typeof(int));
            dtChiTiet.Columns.Add("soluongBan", typeof(int));
            dtChiTiet.Columns.Add("thanhTien", typeof(decimal));

            List<BaoDto> dtBao = null;
            try
            {
                dtBao = ApiClient.Instance.GetAsync<List<BaoDto>>("Publications").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách ấn phẩm: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (DateTime date = tuNgay; date <= denNgay; date = date.AddDays(1))
            {
                int dayOfWeek = (int)date.DayOfWeek;
                string cotThu = dayOfWeek == 0 ? "thu7" : "thu" + dayOfWeek.ToString();

                foreach (var bao in dtBao)
                {
                    if (GetThuValue(bao, cotThu))
                    {
                        string maBao = bao.MaBao;
                        string tenBao = bao.Ten;
                        decimal donGia = (decimal)bao.DonGia;

                        DateTime ngayBatDau = bao.NgayBatDau ?? new DateTime(date.Year, 1, 1);
                        int soGoc = bao.Sogoc ?? 1;

                        int soBaoTinhToan = TinhSoBaoNghiepVu(ngayBatDau, date, soGoc, bao);

                        dtChiTiet.Rows.Add(date, maBao, tenBao, soBaoTinhToan, donGia, 0, 0, 0);
                    }
                }
            }

            dgvChiTiet.DataSource = dtChiTiet;

            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["ngayNhan"].HeaderText = "Ngày nhận";
                dgvChiTiet.Columns["maBao"].HeaderText = "Mã báo";
                dgvChiTiet.Columns["tenBao"].HeaderText = "Tên ấn phẩm";
                dgvChiTiet.Columns["soBao"].HeaderText = "Số báo";
                dgvChiTiet.Columns["donGia"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["soluongDieuPhoi"].HeaderText = "SL Điều phối";
                dgvChiTiet.Columns["soluongBan"].HeaderText = "SL Bán thực";
                dgvChiTiet.Columns["thanhTien"].HeaderText = "Thành tiền";

                dgvChiTiet.Columns["ngayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvChiTiet.Columns["donGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["thanhTien"].DefaultCellStyle.Format = "N0";

                dgvChiTiet.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dgvChiTiet.Columns["ngayNhan"].Width = 110;
                dgvChiTiet.Columns["maBao"].Width = 90;
                dgvChiTiet.Columns["tenBao"].Width = 160;
                dgvChiTiet.Columns["soBao"].Width = 70;
                dgvChiTiet.Columns["donGia"].Width = 100;
                dgvChiTiet.Columns["soluongDieuPhoi"].Width = 120;
                dgvChiTiet.Columns["soluongBan"].Width = 120;
                dgvChiTiet.Columns["thanhTien"].Width = 120;
            }

            dgvChiTiet.ColumnHeadersVisible = false;
            dgvChiTiet.ColumnHeadersVisible = true;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTiet.ColumnHeadersHeight = 40;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvChiTiet.Refresh();
        }

        private int TinhSoBaoNghiepVu(DateTime ngayBatDau, DateTime ngayDieuPhoi, int soGoc, BaoDto bao)
        {
            DateTime mocDauNam = new DateTime(ngayDieuPhoi.Year, 1, 1);
            DateTime mocTinhToan = (ngayBatDau > mocDauNam) ? ngayBatDau.Date : mocDauNam;

            if (ngayDieuPhoi.Date < mocTinhToan) return soGoc;

            int countNgayPhatHanhThucTe = 0;

            for (DateTime date = mocTinhToan; date <= ngayDieuPhoi.Date; date = date.AddDays(1))
            {
                int dayOfWeek = (int)date.DayOfWeek;
                string tenCotThu = (dayOfWeek == 0) ? "thu7" : "thu" + dayOfWeek.ToString();

                if (GetThuValue(bao, tenCotThu))
                {
                    countNgayPhatHanhThucTe++;
                }
            }

            if (countNgayPhatHanhThucTe == 0) return soGoc;
            return soGoc + countNgayPhatHanhThucTe - 1;
        }

        private void dgvChiTiet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];

            string maBao = row.Cells["maBao"].Value.ToString();
            string tenBao = row.Cells["tenBao"].Value.ToString();
            string soBao = row.Cells["soBao"].Value.ToString();
            decimal donGia = Convert.ToDecimal(row.Cells["donGia"].Value);

            if (_isEditMode)
            {
                int slDieuPhoi = Convert.ToInt32(row.Cells["soluongDieuPhoi"].Value);
                int currentBanThuc = row.Cells["soluongBan"].Value != DBNull.Value
                    ? Convert.ToInt32(row.Cells["soluongBan"].Value) : 0;

                using (frmNhapBanThuc frm = new frmNhapBanThuc(
                    maBao, tenBao, soBao, donGia, slDieuPhoi, currentBanThuc))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int newBanThuc = frm.SoLuongBanThuc;
                        row.Cells["soluongBan"].Value = newBanThuc;
                        row.Cells["thanhTien"].Value = newBanThuc * donGia;
                    }
                }
            }
            else
            {
                int currentSl = Convert.ToInt32(row.Cells["soluongDieuPhoi"].Value);
                using (frmNhapSoLuong frm = new frmNhapSoLuong(
                    maBao, tenBao, Convert.ToInt32(soBao), donGia, currentSl))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int newSl = frm.SoLuongDieuPhoi;
                        row.Cells["soluongDieuPhoi"].Value = newSl;
                        row.Cells["thanhTien"].Value = newSl * donGia;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_isEditMode && btnCreate.Enabled) return;

            bool hasAtLeastOneRow = false;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                int slDieuPhoi = Convert.ToInt32(row.Cells["soluongDieuPhoi"].Value);
                if (slDieuPhoi > 0)
                {
                    hasAtLeastOneRow = true;
                    break;
                }
            }
            if (!hasAtLeastOneRow)
            {
                MessageBox.Show("Vui lòng nhập số lượng điều phối cho ít nhất một báo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var model = new
            {
                Sohd = txtSoHD.Text.Trim(),
                Makh = maKhachHangDuocChon,
                Ngay = dtpNgayLapPhieu.Value.Date,
                Tungay = dtpTuNgay.Value.Date,
                Denngay = dtpDenNgay.Value.Date,
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text,
                Details = new List<object>()
            };

            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                int slDieuPhoi = Convert.ToInt32(row.Cells["soluongDieuPhoi"].Value);
                if (slDieuPhoi > 0)
                {
                    model.Details.Add(new
                    {
                        NgayNhan = Convert.ToDateTime(row.Cells["ngayNhan"].Value).Date,
                        MaBao = row.Cells["maBao"].Value.ToString(),
                        TenBao = row.Cells["tenBao"].Value.ToString(),
                        SoBao = row.Cells["soBao"].Value.ToString(),
                        DonGia = Convert.ToDouble(row.Cells["donGia"].Value),
                        SoluongDieuPhoi = slDieuPhoi,
                        SoluongBan = row.Cells["soluongBan"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["soluongBan"].Value) : 0,
                        ThanhTien = Convert.ToDouble(row.Cells["thanhTien"].Value)
                    });
                }
            }

            try
            {
                bool success = ApiClient.Instance.PostAsync("Deliveries", model).GetAwaiter().GetResult();
                if (success)
                {
                    MessageBox.Show("Lưu phiếu điều phối thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Lưu phiếu điều phối thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phiếu điều phối qua API: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
                LoadChiTietDieuPhoiForEdit(_soHDEdit);
            else
                ResetForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}