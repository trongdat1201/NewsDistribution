using DATNWF.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmDetailInvoices : Form
    {
        private readonly DbHelper _db = DbHelper.Instance;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(3);
        private static DateTime _cacheStamp = DateTime.MinValue;
        private static readonly object _cacheLock = new object();

        private static DataTable _cacheHoaDon;
        private static string     _cacheKeyword = null;

        private bool _isExporting;

        public frmDetailInvoices()
        {
            InitializeComponent();
            txtMaKH.ReadOnly = true;
            txtTenKH.ReadOnly = true;
            txtSDT.ReadOnly   = true;
            txtCK.ReadOnly    = true;

            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.KeyDown     += TxtSearch_KeyDown;

            btnXuatExcel.Click    += BtnXuatExcel_Click;
            btnInHoaDon.Click     += BtnInHoaDon_Click;
        }

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isExporting) return;

            string soHD = dgvHoaDon.SelectedRows[0].Cells["sohd"]?.Value?.ToString();
            if (string.IsNullOrEmpty(soHD)) return;

            _isExporting = true;
            btnXuatExcel.Enabled = false;
            btnXuatExcel.Text    = "Đang xuất…";

            try
            {
                using var frm = new frmChonDuongDan(soHD);
                if (frm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(frm.SelectedFilePath))
                {
                    MessageBox.Show($"Xuất Excel thành công!\n\n{frm.SelectedFilePath}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isExporting = false;
                btnXuatExcel.Enabled = true;
                btnXuatExcel.Text    = "Xuất excel";
            }
        }

        private void BtnInHoaDon_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để in.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string soHD = dgvHoaDon.SelectedRows[0].Cells["sohd"]?.Value?.ToString();
            if (string.IsNullOrEmpty(soHD)) return;

            DataGridViewRow row = dgvHoaDon.SelectedRows[0];
            var hdData = new System.Collections.Generic.Dictionary<string, object>
            {
                ["tenKhachHang"] = row.Cells["tenKhachHang"]?.Value ?? DBNull.Value,
                ["sdtKhachHang"] = row.Cells["sdtKhachHang"]?.Value ?? DBNull.Value,
                ["tuNgay"]       = row.Cells["tuNgay"]?.Value       ?? DBNull.Value,
                ["denNgay"]      = row.Cells["denNgay"]?.Value      ?? DBNull.Value,
                ["chietKhau"]    = row.Cells["chietKhau"]?.Value    ?? DBNull.Value,
            };

            DataTable dtChiTiet = (dgvChiTietHoaDon.DataSource as DataTable)
                ?? new DataTable();

            try
            {
                var printSvc = new PrintInvoiceService();
                printSvc.Print(soHD, hdData, dtChiTiet);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in hóa đơn:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmDetailInvoices_Load(object sender, EventArgs e)
        {
            dgvHoaDon.Columns.Clear();
            dgvHoaDon.AutoGenerateColumns = true;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.ReadOnly      = true;
            dgvChiTietHoaDon.Columns.Clear();
            dgvChiTietHoaDon.AutoGenerateColumns = true;
            dgvChiTietHoaDon.ReadOnly = true;

            dgvHoaDon.SelectionChanged -= dgvHoaDon_SelectionChanged;
            dgvHoaDon.SelectionChanged += dgvHoaDon_SelectionChanged;

            this.BeginInvoke(new Action(() => LoadDanhSachHoaDonAsync()));
        }


        private void LoadDanhSachHoaDonAsync(string keyword = "")
        {
            bool cacheValid;
            DataTable cached;
            lock (_cacheLock)
            {
                cached = _cacheHoaDon;
                cacheValid = cached != null && keyword == _cacheKeyword
                    && (DateTime.Now - _cacheStamp) < CacheDuration;
            }

            if (cacheValid)
            {
                BindHoaDon(cached);
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    DataTable dt = string.IsNullOrWhiteSpace(keyword)
                        ? _db.FillDataTable(Queries.HoaDonList)
                        : _db.FillDataTable(
                            Queries.HoaDonList + @"
                          WHERE hd.sohd LIKE @kw OR kh.TEN LIKE @kw OR hd.makh LIKE @kw",
                            new SqlParameter("@kw", $"%{keyword.Trim()}%"));

                    lock (_cacheLock)
                    {
                        _cacheHoaDon  = dt;
                        _cacheKeyword  = keyword;
                        _cacheStamp    = DateTime.Now;
                    }

                    if (this.IsDisposed) return;
                    this.BeginInvoke(new Action(() => BindHoaDon(dt)));
                }
                catch (Exception ex)
                {
                    if (this.IsDisposed) return;
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show($"Lỗi tải danh sách hóa đơn:\n{ex.Message}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private void BindHoaDon(DataTable dt)
        {
            dgvHoaDon.DataSource = dt;
            dgvHoaDon.ClearSelection();

            if (dgvHoaDon.Rows.Count > 0)
                dgvHoaDon.Rows[0].Selected = true;

            ApplyGridHeader(dgvHoaDon, new[]
            {
                ("sohd",          "Số HĐ"),
                ("makh",          "Mã KH"),
                ("ngayLapPhieu",  "Ngày lập"),
                ("tuNgay",        "Từ ngày"),
                ("denNgay",       "Đến ngày"),
                ("ghichu",        "Ghi chú"),
                ("thanhToan",     "Thanh toán")
            });
            ApplyDateFormat(dgvHoaDon, new[] { "ngayLapPhieu", "tuNgay", "denNgay" });

            // Ẩn 3 cột KH vì đã hiển thị ở groupbox bên cạnh
            foreach (var col in new[] { "tenKhachHang", "sdtKhachHang", "chietKhau" })
                if (dgvHoaDon.Columns[col] != null)
                    dgvHoaDon.Columns[col].Visible = false;
        }


        private void LoadChiTietHoaDonAsync(string soHD)
        {
            if (string.IsNullOrEmpty(soHD))
            {
                dgvChiTietHoaDon.DataSource = null;
                dgvChiTietHoaDon.Columns.Clear();
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    const string query = @"
                    SELECT sohd, ngayNhan, maBao, tenBao, soBao,
                           soLuongThuc, soLuongDu, donGia, thanhTien, dieuPhoi
                    FROM dbo.tabCHITIETHOADON
                    WHERE sohd = @soHD
                    ORDER BY ngayNhan ASC";

                    DataTable dt = _db.FillDataTable(query, new SqlParameter("@soHD", soHD));

                    if (this.IsDisposed) return;
                    this.BeginInvoke(new Action(() => BindChiTietHoaDon(dt)));
                }
                catch (Exception ex)
                {
                    if (this.IsDisposed) return;
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show($"Lỗi tải chi tiết hóa đơn:\n{ex.Message}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private void BindChiTietHoaDon(DataTable dt)
        {
            dgvChiTietHoaDon.DataSource = dt;

            if (dgvChiTietHoaDon.Columns.Count > 0)
            {
                ApplyGridHeader(dgvChiTietHoaDon, new[]
                {
                    ("sohd",             "Số HĐ"),
                    ("ngayNhan",         "Ngày nhận"),
                    ("maBao",            "Mã báo"),
                    ("tenBao",           "Tên báo"),
                    ("soBao",            "Số báo"),
                    ("soLuongThuc",      "SL thực"),
                    ("soLuongDu",        "Phát sinh"),
                    ("donGia",           "Đơn giá"),
                    ("thanhTien",        "Thành tiền"),
                    ("dieuPhoi",         "Điều phối")
                });

                foreach (DataGridViewColumn col in dgvChiTietHoaDon.Columns)
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                ApplyDateFormat(dgvChiTietHoaDon, new[] { "ngayNhan" });
                ApplyNumberFormat(dgvChiTietHoaDon, new[] { "donGia", "thanhTien", "soLuongDu" });
            }
        }
        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count == 0)
            {
                txtMaKH.Text = txtTenKH.Text = txtSDT.Text = txtCK.Text = "";
                dgvChiTietHoaDon.DataSource = null;
                return;
            }

            var row = dgvHoaDon.SelectedRows[0];

            txtMaKH.Text   = row.Cells["makh"]?.Value?.ToString()       ?? "";
            txtTenKH.Text  = row.Cells["tenKhachHang"]?.Value?.ToString() ?? "";
            txtSDT.Text    = row.Cells["sdtKhachHang"]?.Value?.ToString() ?? "";
            var ckVal = row.Cells["chietKhau"]?.Value;
            txtCK.Text     = ckVal != null && ckVal != DBNull.Value
                ? Convert.ToDecimal(ckVal).ToString("N0")
                : "";

            string soHD = row.Cells["sohd"]?.Value?.ToString() ?? string.Empty;
            LoadChiTietHoaDonAsync(soHD);
        }


        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            tmrSearch?.Stop();
            tmrSearch?.Start();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtSearch.Clear();
                e.Handled = true;
            }
        }

        private void tmrSearch_Tick(object sender, EventArgs e)
        {
            tmrSearch?.Stop();
            lock (_cacheLock) { _cacheHoaDon = null; }
            LoadDanhSachHoaDonAsync(txtSearch.Text.Trim());
        }


        private static class Queries
        {
            public const string HoaDonList = @"
                SELECT TOP 50
                    hd.sohd,
                    hd.makh,
                    hd.ngayLapPhieu,
                    hd.tuNgay,
                    hd.denNgay,
                    hd.ghichu,
                    hd.thanhToan,
                    kh.TEN      AS tenKhachHang,
                    kh.DIENTHOAI AS sdtKhachHang,
                    kh.CHIETKHAU AS chietKhau
                FROM dbo.tabHOADON hd
                INNER JOIN dbo.tabKHACHHANG kh ON kh.MAKH = hd.makh
                ORDER BY hd.ngayLapPhieu DESC, hd.sohd DESC";
        }

        // ── Helpers ────────────────────────────────────────────────────

        private static void ApplyGridHeader(DataGridView dgv, (string col, string label)[] columns)
        {
            foreach (var (col, label) in columns)
                if (dgv.Columns[col] != null)
                    dgv.Columns[col].HeaderText = label;
        }

        private static void ApplyDateFormat(DataGridView dgv, string[] columns)
        {
            foreach (string col in columns)
                if (dgv.Columns[col] != null)
                    dgv.Columns[col].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private static void ApplyNumberFormat(DataGridView dgv, string[] columns)
        {
            foreach (string col in columns)
                if (dgv.Columns[col] != null)
                    dgv.Columns[col].DefaultCellStyle.Format = "N0";
        }
    }
}
