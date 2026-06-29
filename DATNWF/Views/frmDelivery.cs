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
    public partial class frmDelivery : Form
    {
        private string _soHDHienTai = string.Empty;
        private List<DieuPhoiDto> _listDieuPhoi = new List<DieuPhoiDto>();

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
            try
            {
                _listDieuPhoi = ApiClient.Instance.GetAsync<List<DieuPhoiDto>>("Deliveries").GetAwaiter().GetResult();
                
                var displayList = _listDieuPhoi;
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string kw = keyword.ToLower();
                    displayList = _listDieuPhoi.Where(x => x.Sohd.ToLower().Contains(kw) || (x.Makh != null && x.Makh.ToLower().Contains(kw))).ToList();
                }

                tabDieuPhoiBindingSource.DataSource = null;
                tabDieuPhoiBindingSource.DataSource = displayList;
                dgvDieuPhoi.DataSource = tabDieuPhoiBindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách điều phối: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class DetailDeliveryDto
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

        private void LoadChiTietDieuPhoi(string soHD)
        {
            if (string.IsNullOrEmpty(soHD)) return;

            try
            {
                var dt = ApiClient.Instance.GetAsync<List<DetailDeliveryDto>>($"Deliveries/{soHD}/details").GetAwaiter().GetResult();

                tabChiTietDieuPhoiBindingSource.DataSource = null;
                tabChiTietDieuPhoiBindingSource.DataSource = dt;
                dgvChiTietDieuPhoi.DataSource = tabChiTietDieuPhoiBindingSource;

                decimal tongTien = 0m;
                if (dt != null)
                {
                    foreach (var row in dt)
                    {
                        tongTien += (decimal)row.ThanhTien;
                    }
                }
                lblTongSoTien.Text = string.Format("{0:N0} đ", tongTien);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết điều phối: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongTinKhachHang(string maKH)
        {
            if (string.IsNullOrEmpty(maKH))
            {
                ClearKhachHangInfo();
                return;
            }

            try
            {
                var khData = ApiClient.Instance.GetAsync<KhachHangDto>($"Customers/{maKH}").GetAwaiter().GetResult();
                if (khData != null)
                {
                    txtMaKH.Text = khData.MaKH;
                    txtTenKH.Text = khData.Ten;
                    txtSDT.Text = khData.DienThoai ?? "";
                    txtCK.Text = khData.ChietKhau.ToString();
                }
                else
                {
                    ClearKhachHangInfo();
                }
            }
            catch
            {
                ClearKhachHangInfo();
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
                var row = dgvDieuPhoi.SelectedRows[0];
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
