using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmInvoices : Form
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

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

        private void LoadDanhSachDieuPhoiHople(string keyword = "")
        {
            string query;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Chỉ load đơn điều phối đã hết hạn (denngay <= HÔM NAY)
                query = @"SELECT TOP 50 dp.soHD AS SoHD, dp.makh AS MaKH, dp.ngay AS NgayLapPhieu,
                                 dp.tungay AS TuNgay, dp.denngay AS DenNgay, dp.ghiChu AS GhiChu,
                                 ISNULL(hd.thanhToan, 0) AS ThanhToan,
                                 CASE WHEN hd.sohd IS NOT NULL THEN 1 ELSE 0 END AS DaLapHD
                          FROM dbo.tabDieuPhoi dp
                          LEFT JOIN dbo.tabHOADON hd
                            ON hd.sohd = dp.soHD COLLATE DATABASE_DEFAULT
                          WHERE dp.denngay <= GETDATE()
                          ORDER BY dp.ngay DESC, dp.soHD DESC";
            }
            else
            {
                // Tìm kiếm hóa đơn cũ trong tabHOADON
                query = @"SELECT TOP 50 hd.sohd AS SoHD, hd.makh AS MaKH, hd.ngayLapPhieu AS NgayLapPhieu,
                                 hd.tuNgay AS TuNgay, hd.denNgay AS DenNgay, hd.ghichu AS GhiChu,
                                 hd.thanhToan AS ThanhToan,
                                 1 AS DaLapHD
                          FROM dbo.tabHOADON hd
                          WHERE hd.sohd LIKE @kw
                          ORDER BY hd.ngayLapPhieu DESC, hd.sohd DESC";
            }
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                    da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");
                da.Fill(dt);
            }

            // Gán trực tiếp vào DataGridView, bỏ qua BindingSource vì cấu trúc cột khác nhau
            dgvHoaDon.DataSource = dt;

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

        private void LoadChiTietHoaDon(string soHD)
        {
            if (string.IsNullOrEmpty(soHD)) return;

            string query = @"SELECT sohd, ngayNhan, maBao, tenbao, sobao,
                                    soluongBan, soluongDieuPhoi, donGia,
                                    thanhTien
                             FROM dbo.tabChiTietDieuPhoi
                             WHERE sohd = @soHD
                             ORDER BY ngayNhan ASC";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@soHD", soHD);
                da.Fill(dt);
            }

            dgvChiTietHoaDon.DataSource = dt;

            if (dgvChiTietHoaDon.Columns.Count > 0)
            {
                if (dgvChiTietHoaDon.Columns["sohd"] != null) dgvChiTietHoaDon.Columns["sohd"].HeaderText = "Số HĐ";
                if (dgvChiTietHoaDon.Columns["ngayNhan"] != null) dgvChiTietHoaDon.Columns["ngayNhan"].HeaderText = "Ngày nhận";
                if (dgvChiTietHoaDon.Columns["maBao"] != null) dgvChiTietHoaDon.Columns["maBao"].HeaderText = "Mã báo";
                if (dgvChiTietHoaDon.Columns["tenbao"] != null) dgvChiTietHoaDon.Columns["tenbao"].HeaderText = "Tên báo";
                if (dgvChiTietHoaDon.Columns["sobao"] != null) dgvChiTietHoaDon.Columns["sobao"].HeaderText = "Số báo";
                if (dgvChiTietHoaDon.Columns["soluongBan"] != null) dgvChiTietHoaDon.Columns["soluongBan"].HeaderText = "SL bán";
                if (dgvChiTietHoaDon.Columns["soluongDieuPhoi"] != null) dgvChiTietHoaDon.Columns["soluongDieuPhoi"].HeaderText = "SL điều phối";
                if (dgvChiTietHoaDon.Columns["donGia"] != null) dgvChiTietHoaDon.Columns["donGia"].HeaderText = "Đơn giá";
                if (dgvChiTietHoaDon.Columns["thanhTien"] != null) dgvChiTietHoaDon.Columns["thanhTien"].HeaderText = "Thành tiền";

                if (dgvChiTietHoaDon.Columns["ngayNhan"] != null) dgvChiTietHoaDon.Columns["ngayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgvChiTietHoaDon.Columns["donGia"] != null) dgvChiTietHoaDon.Columns["donGia"].DefaultCellStyle.Format = "N0";
                if (dgvChiTietHoaDon.Columns["thanhTien"] != null) dgvChiTietHoaDon.Columns["thanhTien"].DefaultCellStyle.Format = "N0";
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
