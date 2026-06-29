using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmTaoHoaDon : Form
    {
        private string maKhachHangDuocChon = "";
        private DataGridView dgvDetails;
        private List<InvoiceDetailInputModel> _invoiceDetails = new List<InvoiceDetailInputModel>();

        public frmTaoHoaDon()
        {
            InitializeComponent();
            SetupDetailsGrid();
            this.guna2GradientButton4.Click += new System.EventHandler(this.btnSave_Click);
        }

        private void SetupDetailsGrid()
        {
            dgvDetails = new DataGridView();
            dgvDetails.Dock = DockStyle.Fill;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.BackgroundColor = Color.White;
            dgvDetails.BorderStyle = BorderStyle.None;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            pnlDetails.Controls.Add(dgvDetails);
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

            try
            {
                var invoices = ApiClient.Instance.GetAsync<List<HoaDonDto>>("Invoices").GetAwaiter().GetResult();
                bool exists = invoices.Any(x => x.Sohd.Equals(soHD, StringComparison.OrdinalIgnoreCase));

                if (exists)
                {
                    MessageBox.Show("Số Hóa Đơn này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSoHD.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối API: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            LoadInvoiceDetails(maKhachHangDuocChon, tuNgay, denNgay);
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

        private void LoadInvoiceDetails(string makh, DateTime tuNgay, DateTime denNgay)
        {
            _invoiceDetails.Clear();
            try
            {
                var deliveries = ApiClient.Instance.GetAsync<List<DieuPhoiDto>>("Deliveries").GetAwaiter().GetResult();
                var matchingDeliveries = deliveries.Where(d => d.Makh == makh && d.NgayLapPhieu >= tuNgay && d.NgayLapPhieu <= denNgay).ToList();

                foreach (var dp in matchingDeliveries)
                {
                    var details = ApiClient.Instance.GetAsync<List<DetailDeliveryResponse>>($"Deliveries/{dp.Sohd}/details").GetAwaiter().GetResult();
                    if (details != null)
                    {
                        foreach (var det in details)
                        {
                            _invoiceDetails.Add(new InvoiceDetailInputModel
                            {
                                NgayNhan = det.NgayNhan,
                                MaBao = det.MaBao,
                                TenBao = det.Tenbao,
                                SoBao = det.Sobao,
                                SoLuongThuc = det.SoluongBan,
                                SoLuongPhatSinh = 0,
                                DonGia = det.DonGia,
                                ThanhTien = det.SoluongBan * det.DonGia,
                                DieuPhoi = det.SoluongDieuPhoi
                            });
                        }
                    }
                }

                dgvDetails.DataSource = null;
                dgvDetails.DataSource = _invoiceDetails;

                if (dgvDetails.Columns.Count > 0)
                {
                    dgvDetails.Columns["NgayNhan"].HeaderText = "Ngày nhận";
                    dgvDetails.Columns["MaBao"].HeaderText = "Mã báo";
                    dgvDetails.Columns["TenBao"].HeaderText = "Tên báo";
                    dgvDetails.Columns["SoBao"].HeaderText = "Số báo";
                    dgvDetails.Columns["SoLuongThuc"].HeaderText = "SL thực nhận";
                    dgvDetails.Columns["SoLuongPhatSinh"].HeaderText = "SL phát sinh";
                    dgvDetails.Columns["DonGia"].HeaderText = "Đơn giá";
                    dgvDetails.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    dgvDetails.Columns["DieuPhoi"].HeaderText = "SL điều phối";

                    dgvDetails.Columns["NgayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvDetails.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                    dgvDetails.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn từ lịch sử điều phối: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maKhachHangDuocChon)) return;

            var model = new
            {
                Sohd = txtSoHD.Text.Trim(),
                Makh = maKhachHangDuocChon,
                NgayLapPhieu = dtpNgayLapPhieu.Value.Date,
                TuNgay = dtpTuNgay.Value.Date,
                DenNgay = dtpDenNgay.Value.Date,
                Ghichu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text,
                ThanhToan = chkThanhToan.Checked,
                Details = _invoiceDetails
            };

            try
            {
                bool success = ApiClient.Instance.PostAsync("Invoices", model).GetAwaiter().GetResult();
                if (success)
                {
                    MessageBox.Show("Lưu hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu hóa đơn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn qua API: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

    public class InvoiceDetailInputModel
    {
        public DateTime NgayNhan { get; set; }
        public string MaBao { get; set; }
        public string TenBao { get; set; }
        public int SoBao { get; set; }
        public int SoLuongThuc { get; set; }
        public int SoLuongPhatSinh { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get; set; }
        public int DieuPhoi { get; set; }
    }
}
