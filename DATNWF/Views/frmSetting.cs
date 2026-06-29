using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmSetting : Form
    {
        private class InvoiceDetailDto
        {
            public string Sohd { get; set; }
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

        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            ClearInvoiceInfo();
            dgvDetails.ReadOnly = true;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.RowHeadersVisible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string soHD = txtSearchSohd.Text.Trim();
            if (string.IsNullOrEmpty(soHD))
            {
                MessageBox.Show("Vui lòng nhập Số hóa đơn cần tra cứu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadInvoiceDetail(soHD);
        }

        private class InvoiceHeaderResponse
        {
            public string Sohd { get; set; }
            public string Makh { get; set; }
            public DateTime NgayLapPhieu { get; set; }
            public DateTime TuNgay { get; set; }
            public DateTime DenNgay { get; set; }
            public string Ghichu { get; set; }
            public bool ThanhToan { get; set; }
        }

        private void LoadInvoiceDetail(string soHD)
        {
            try
            {
                // 1. Fetch Invoices list to find the header
                var invoices = ApiClient.Instance.GetAsync<List<InvoiceHeaderResponse>>("Invoices").GetAwaiter().GetResult();
                var header = invoices.FirstOrDefault(x => x.Sohd.Equals(soHD, StringComparison.OrdinalIgnoreCase));

                if (header == null)
                {
                    MessageBox.Show($"Không tìm thấy hóa đơn nào có số '{soHD}' trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInvoiceInfo();
                    return;
                }

                // 2. Fetch Customer Info
                var customer = ApiClient.Instance.GetAsync<KhachHangDto>($"Customers/{header.Makh}").GetAwaiter().GetResult();

                // 3. Fetch Details
                var details = ApiClient.Instance.GetAsync<List<InvoiceDetailDto>>($"Invoices/{soHD}/details").GetAwaiter().GetResult();

                // 4. Update UI
                lblSohdVal.Text = header.Sohd;
                lblKHVal.Text = customer != null ? $"{customer.Ten} ({customer.MaKH})" : header.Makh;
                lblNgayLapVal.Text = header.NgayLapPhieu.ToString("dd/MM/yyyy");
                lblKyHieuVal.Text = $"Từ {header.TuNgay:dd/MM/yyyy} đến {header.DenNgay:dd/MM/yyyy}";
                lblGhiChuVal.Text = string.IsNullOrEmpty(header.Ghichu) ? "(Trống)" : header.Ghichu;
                lblThanhToanVal.Text = header.ThanhToan ? "ĐÃ THANH TOÁN" : "CHƯA THANH TOÁN";
                lblThanhToanVal.ForeColor = header.ThanhToan ? Color.ForestGreen : Color.Crimson;

                double total = details != null ? details.Sum(x => x.ThanhTien) : 0;
                lblTongTienVal.Text = $"{total:N0} đ";

                dgvDetails.DataSource = null;
                dgvDetails.DataSource = details;

                if (dgvDetails.Columns.Count > 0)
                {
                    dgvDetails.Columns["Sohd"].Visible = false;
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
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearInvoiceInfo();
            }
        }

        private void ClearInvoiceInfo()
        {
            lblSohdVal.Text = "-";
            lblKHVal.Text = "-";
            lblNgayLapVal.Text = "-";
            lblKyHieuVal.Text = "-";
            lblTongTienVal.Text = "0 đ";
            lblGhiChuVal.Text = "-";
            lblThanhToanVal.Text = "-";
            lblThanhToanVal.ForeColor = Color.Black;
            dgvDetails.DataSource = null;
        }
    }
}
