using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmInvoices : Form
    {
        private string _soHDHienTai = string.Empty;
        private string _mAKHHienTai = string.Empty;

        public frmInvoices()
        {
            InitializeComponent();
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        private void frmInvoices_Load(object sender, EventArgs e)
        {
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.ReadOnly = true;
            dgvChiTietHoaDon.ReadOnly = true;

            LoadDanhSachDieuPhoiHople();
        }

        private class InvoiceItemResponse
        {
            public string Sohd { get; set; }
            public string Makh { get; set; }
            public DateTime? NgayLapPhieu { get; set; }
            public DateTime? TuNgay { get; set; }
            public DateTime? DenNgay { get; set; }
            public string Ghichu { get; set; }
            public bool ThanhToan { get; set; }
        }

        private void LoadDanhSachDieuPhoiHople(string keyword = "")
        {
            try
            {
                var deliveries = ApiClient.Instance.GetAsync<List<DieuPhoiDto>>("Deliveries").GetAwaiter().GetResult();
                var invoices = ApiClient.Instance.GetAsync<List<InvoiceItemResponse>>("Invoices").GetAwaiter().GetResult();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    // Chỉ load đơn điều phối đã hết hạn (denngay <= HÔM NAY)
                    var today = DateTime.Now.Date;
                    var query = from dp in deliveries
                                where dp.DenNgay <= today
                                join hd in invoices on dp.Sohd equals hd.Sohd into joined
                                from hd in joined.DefaultIfEmpty()
                                select new
                                {
                                    sohd = dp.Sohd,
                                    makh = dp.Makh,
                                    ngayLapPhieu = dp.NgayLapPhieu,
                                    tuNgay = dp.TuNgay,
                                    denNgay = dp.DenNgay,
                                    ghichu = dp.GhiChu,
                                    ThanhToan = hd != null ? hd.ThanhToan : false,
                                    DaLapHD = hd != null ? 1 : 0
                                };

                    dgvHoaDon.DataSource = query.OrderByDescending(x => x.ngayLapPhieu).ThenByDescending(x => x.sohd).ToList();
                }
                else
                {
                    // Tìm kiếm hóa đơn cũ trong invoices
                    string kw = keyword.Trim().ToLower();
                    var matching = invoices.Where(hd => hd.Sohd.ToLower().Contains(kw))
                                           .Select(hd => new
                                           {
                                               sohd = hd.Sohd,
                                               makh = hd.Makh,
                                               ngayLapPhieu = hd.NgayLapPhieu,
                                               tuNgay = hd.TuNgay,
                                               denNgay = hd.DenNgay,
                                               ghichu = hd.Ghichu,
                                               ThanhToan = hd.ThanhToan,
                                               DaLapHD = 1
                                           });

                    dgvHoaDon.DataSource = matching.OrderByDescending(x => x.ngayLapPhieu).ThenByDescending(x => x.sohd).ToList();
                }

                // Đặt lại tên cột theo alias query
                if (dgvHoaDon.DataSource != null && dgvHoaDon.Columns.Count > 0)
                {
                    if (dgvHoaDon.Columns["sohd"] != null) dgvHoaDon.Columns["sohd"].HeaderText = "Số phiếu";
                    if (dgvHoaDon.Columns["makh"] != null) dgvHoaDon.Columns["makh"].HeaderText = "Mã KH";
                    if (dgvHoaDon.Columns["ngayLapPhieu"] != null) dgvHoaDon.Columns["ngayLapPhieu"].HeaderText = "Ngày lập";
                    if (dgvHoaDon.Columns["tuNgay"] != null) dgvHoaDon.Columns["tuNgay"].HeaderText = "Từ ngày";
                    if (dgvHoaDon.Columns["denNgay"] != null) dgvHoaDon.Columns["denNgay"].HeaderText = "Đến ngày";
                    if (dgvHoaDon.Columns["ghichu"] != null) dgvHoaDon.Columns["ghichu"].HeaderText = "Ghi chú";

                    // Format cột ngày
                    if (dgvHoaDon.Columns["ngayLapPhieu"] != null) dgvHoaDon.Columns["ngayLapPhieu"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    if (dgvHoaDon.Columns["tuNgay"] != null) dgvHoaDon.Columns["tuNgay"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    if (dgvHoaDon.Columns["denNgay"] != null) dgvHoaDon.Columns["denNgay"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    // Đặt lại event click row
                    dgvHoaDon.SelectionChanged -= dgvHoaDon_SelectionChanged;
                    dgvHoaDon.SelectionChanged += dgvHoaDon_SelectionChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hóa đơn/điều phối: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class DetailDeliveryResponse
        {
            public string Sohd { get; set; }
            public DateTime NgayNhan { get; set; }
            public string MaBao { get; set; }
            public string Tenbao { get; set; }
            public int Sobao { get; set; }
            public int SoluongBan { get; set; }
            public int SoluongDieuPhoi { get; set; }
            public double DonGia { get; set; }
            public double ThanhTien { get; set; }
        }

        private void LoadChiTietHoaDon(string soHD)
        {
            if (string.IsNullOrEmpty(soHD)) return;

            try
            {
                var dt = ApiClient.Instance.GetAsync<List<DetailDeliveryResponse>>($"Deliveries/{soHD}/details").GetAwaiter().GetResult();
                dgvChiTietHoaDon.DataSource = dt;

                if (dgvChiTietHoaDon.Columns.Count > 0)
                {
                    if (dgvChiTietHoaDon.Columns["Sohd"] != null) dgvChiTietHoaDon.Columns["Sohd"].HeaderText = "Số HĐ";
                    if (dgvChiTietHoaDon.Columns["NgayNhan"] != null) dgvChiTietHoaDon.Columns["NgayNhan"].HeaderText = "Ngày nhận";
                    if (dgvChiTietHoaDon.Columns["MaBao"] != null) dgvChiTietHoaDon.Columns["MaBao"].HeaderText = "Mã báo";
                    if (dgvChiTietHoaDon.Columns["Tenbao"] != null) dgvChiTietHoaDon.Columns["Tenbao"].HeaderText = "Tên báo";
                    if (dgvChiTietHoaDon.Columns["Sobao"] != null) dgvChiTietHoaDon.Columns["Sobao"].HeaderText = "Số báo";
                    if (dgvChiTietHoaDon.Columns["SoluongBan"] != null) dgvChiTietHoaDon.Columns["SoluongBan"].HeaderText = "SL bán";
                    if (dgvChiTietHoaDon.Columns["SoluongDieuPhoi"] != null) dgvChiTietHoaDon.Columns["SoluongDieuPhoi"].HeaderText = "SL điều phối";
                    if (dgvChiTietHoaDon.Columns["DonGia"] != null) dgvChiTietHoaDon.Columns["DonGia"].HeaderText = "Đơn giá";
                    if (dgvChiTietHoaDon.Columns["ThanhTien"] != null) dgvChiTietHoaDon.Columns["ThanhTien"].HeaderText = "Thành tiền";

                    if (dgvChiTietHoaDon.Columns["NgayNhan"] != null) dgvChiTietHoaDon.Columns["NgayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    if (dgvChiTietHoaDon.Columns["DonGia"] != null) dgvChiTietHoaDon.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                    if (dgvChiTietHoaDon.Columns["ThanhTien"] != null) dgvChiTietHoaDon.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvHoaDon.SelectedRows[0];
                string soHD = row.Cells["sohd"].Value?.ToString() ?? string.Empty;
                string maKH = row.Cells["makh"].Value?.ToString() ?? string.Empty;

                _soHDHienTai = soHD;
                _mAKHHienTai = maKH;

                LoadChiTietHoaDon(soHD);
            }
            else
            {
                _soHDHienTai = string.Empty;
                _mAKHHienTai = string.Empty;
                dgvChiTietHoaDon.DataSource = null;
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
            LoadDanhSachDieuPhoiHople(keyword);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmTaoHoaDon frm = new frmTaoHoaDon();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachDieuPhoiHople();
            }
        }
    }
}
