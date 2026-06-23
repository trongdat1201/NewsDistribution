using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmTaoDieuPhoi : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;
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
            string queryKH = "SELECT P_PH, P_KT FROM tabKHACHHANG WHERE maKH = @maKH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmdKH = new SqlCommand(queryKH, conn))
                    {
                        cmdKH.Parameters.AddWithValue("@maKH", maKH);
                        using (SqlDataReader reader = cmdKH.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool isLe = reader["P_PH"] != DBNull.Value && Convert.ToBoolean(reader["P_PH"]);
                                bool isDaiLy = reader["P_KT"] != DBNull.Value && Convert.ToBoolean(reader["P_KT"]);

                                if (isDaiLy) loaiMaPrefix = "PD";
                                else if (isLe) loaiMaPrefix = "PT";
                            }
                        }
                    }

                    string namHienTai = DateTime.Now.ToString("yy");
                    string dinhDangTimKiem = loaiMaPrefix + namHienTai + "_";

                    string queryMax = "SELECT MAX(CAST(SUBSTRING(soHD, @lenPrefix + 1, 999) AS INT)) FROM tabDieuPhoi WHERE soHD LIKE @prefix";
                    using (SqlCommand cmdMax = new SqlCommand(queryMax, conn))
                    {
                        cmdMax.Parameters.AddWithValue("@lenPrefix", dinhDangTimKiem.Length);
                        cmdMax.Parameters.AddWithValue("@prefix", dinhDangTimKiem + "%");
                        object maxVal = cmdMax.ExecuteScalar();

                        int soThuTuTiepTheo = 1;

                        if (maxVal != DBNull.Value && maxVal != null)
                        {
                            soThuTuTiepTheo = Convert.ToInt32(maxVal) + 1;
                        }

                        txtSoHD.Text = dinhDangTimKiem + soThuTuTiepTheo;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tự động sinh số hóa đơn: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void LoadChiTietDieuPhoiForEdit(string soHD)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryMaster = @"SELECT makh, ngay, tungay, denngay, ghiChu
                                      FROM tabDieuPhoi WHERE soHD = @soHD";
                using (SqlCommand cmd = new SqlCommand(queryMaster, conn))
                {
                    cmd.Parameters.AddWithValue("@soHD", soHD);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            maKhachHangDuocChon = reader["makh"]?.ToString() ?? "";
                            dtpNgayLapPhieu.Value = reader["ngay"] != DBNull.Value
                                ? Convert.ToDateTime(reader["ngay"]) : DateTime.Now;
                            dtpTuNgay.Value = reader["tungay"] != DBNull.Value
                                ? Convert.ToDateTime(reader["tungay"]) : DateTime.Now;
                            dtpDenNgay.Value = reader["denngay"] != DBNull.Value
                                ? Convert.ToDateTime(reader["denngay"]) : DateTime.Now;
                            txtGhiChu.Text = reader["ghiChu"] != DBNull.Value
                                ? reader["ghiChu"].ToString() : "";

                            LoadTenKhachHang(maKhachHangDuocChon);
                        }
                    }
                }

                string queryDetail = @"SELECT ngayNhan, maBao, tenBao, soBao, donGia,
                                              soluongDieuPhoi, soluongBan, thanhTien
                                       FROM tabChiTietDieuPhoi
                                       WHERE sohd = @soHD
                                       ORDER BY ngayNhan ASC";
                using (SqlDataAdapter da = new SqlDataAdapter(queryDetail, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@soHD", soHD);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
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
            }
        }

        private void LoadTenKhachHang(string maKH)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TEN FROM tabKHACHHANG WHERE MAKH = @maKH";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maKH", maKH);
                    object result = cmd.ExecuteScalar();
                    txtMaKH.Text = result?.ToString() ?? "";
                }
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

            DataTable dtBao = new DataTable();
            string queryBao = "SELECT maBao, ten, donGia, ngayBatDau, thu1, thu2, thu3, thu4, thu5, thu6, thu7, sogoc FROM tabBAO";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(queryBao, conn))
                {
                    da.Fill(dtBao);
                }
            }

            for (DateTime date = tuNgay; date <= denNgay; date = date.AddDays(1))
            {
                int dayOfWeek = (int)date.DayOfWeek;
                string cotThu = dayOfWeek == 0 ? "thu7" : "thu" + dayOfWeek.ToString();

                foreach (DataRow bao in dtBao.Rows)
                {
                    if (bao[cotThu] != DBNull.Value && Convert.ToBoolean(bao[cotThu]) == true)
                    {
                        string maBao = bao["maBao"].ToString();
                        string tenBao = bao["ten"].ToString();
                        decimal donGia = Convert.ToDecimal(bao["donGia"]);

                        DateTime ngayBatDau = bao["ngayBatDau"] != DBNull.Value && !string.IsNullOrEmpty(bao["ngayBatDau"].ToString())
                            ? Convert.ToDateTime(bao["ngayBatDau"])
                            : new DateTime(date.Year, 1, 1);
                        int soGoc = bao["sogoc"] != DBNull.Value ? Convert.ToInt32(bao["sogoc"]) : 1;

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

        private int TinhSoBaoNghiepVu(DateTime ngayBatDau, DateTime ngayDieuPhoi, int soGoc, DataRow thongTinBao)
        {
            DateTime mocDauNam = new DateTime(ngayDieuPhoi.Year, 1, 1);
            DateTime mocTinhToan = (ngayBatDau > mocDauNam) ? ngayBatDau.Date : mocDauNam;

            if (ngayDieuPhoi.Date < mocTinhToan) return soGoc;

            int countNgayPhatHanhThucTe = 0;

            for (DateTime date = mocTinhToan; date <= ngayDieuPhoi.Date; date = date.AddDays(1))
            {
                int dayOfWeek = (int)date.DayOfWeek;
                string tenCotThu = (dayOfWeek == 0) ? "thu7" : "thu" + dayOfWeek.ToString();

                if (thongTinBao[tenCotThu] != DBNull.Value && Convert.ToBoolean(thongTinBao[tenCotThu]) == true)
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    if (!_isEditMode)
                    {
                        string queryMaster = @"INSERT INTO tabDieuPhoi (soHD, makh, ngay, tungay, denngay, ghiChu) 
                                             VALUES (@soHD, @makh, @ngay, @tungay, @denngay, @ghiChu)";
                        using (SqlCommand cmdMaster = new SqlCommand(queryMaster, conn, transaction))
                        {
                            cmdMaster.Parameters.AddWithValue("@soHD", txtSoHD.Text.Trim());
                            cmdMaster.Parameters.AddWithValue("@makh", maKhachHangDuocChon);
                            cmdMaster.Parameters.AddWithValue("@ngay", dtpNgayLapPhieu.Value.Date);
                            cmdMaster.Parameters.AddWithValue("@tungay", dtpTuNgay.Value.Date);
                            cmdMaster.Parameters.AddWithValue("@denngay", dtpDenNgay.Value.Date);
                            cmdMaster.Parameters.AddWithValue("@ghiChu", string.IsNullOrWhiteSpace(txtGhiChu.Text) ? (object)DBNull.Value : txtGhiChu.Text);
                            cmdMaster.ExecuteNonQuery();
                        }
                    }

                    string queryDeleteDetail = @"DELETE FROM tabChiTietDieuPhoi
                                               WHERE sohd = @sohd
                                                 AND ngayNhan = @ngayNhan
                                                 AND maBao = @maBao";
                    using (SqlCommand cmdDelete = new SqlCommand(queryDeleteDetail, conn, transaction))
                    {
                        cmdDelete.Parameters.Add("@sohd", SqlDbType.VarChar);
                        cmdDelete.Parameters.Add("@ngayNhan", SqlDbType.DateTime);
                        cmdDelete.Parameters.Add("@maBao", SqlDbType.VarChar);

                        foreach (DataGridViewRow row in dgvChiTiet.Rows)
                        {
                            cmdDelete.Parameters["@sohd"].Value = txtSoHD.Text.Trim();
                            cmdDelete.Parameters["@ngayNhan"].Value = row.Cells["ngayNhan"].Value;
                            cmdDelete.Parameters["@maBao"].Value = row.Cells["maBao"].Value;
                            cmdDelete.ExecuteNonQuery();
                        }
                    }

                    string queryDetail = @"INSERT INTO tabChiTietDieuPhoi (sohd, ngayNhan, maBao, tenbao, sobao, donGia, soluongDieuPhoi, soluongBan, thanhTien) 
                                         VALUES (@sohd, @ngayNhan, @maBao, @tenbao, @sobao, @donGia, @soluongDieuPhoi, @soluongBan, @thanhTien)";

                    foreach (DataGridViewRow row in dgvChiTiet.Rows)
                    {
                        int slDieuPhoi = Convert.ToInt32(row.Cells["soluongDieuPhoi"].Value);

                        if (slDieuPhoi > 0)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(queryDetail, conn, transaction))
                            {
                                cmdDetail.Parameters.AddWithValue("@sohd", txtSoHD.Text.Trim());
                                cmdDetail.Parameters.AddWithValue("@ngayNhan", row.Cells["ngayNhan"].Value);
                                cmdDetail.Parameters.AddWithValue("@maBao", row.Cells["maBao"].Value);
                                cmdDetail.Parameters.AddWithValue("@tenbao", row.Cells["tenBao"].Value);
                                cmdDetail.Parameters.AddWithValue("@sobao", row.Cells["soBao"].Value.ToString());
                                cmdDetail.Parameters.AddWithValue("@donGia", row.Cells["donGia"].Value);

                                cmdDetail.Parameters.AddWithValue("@soluongDieuPhoi", slDieuPhoi);
                                cmdDetail.Parameters.AddWithValue("@soluongBan", row.Cells["soluongBan"].Value ?? 0);
                                cmdDetail.Parameters.AddWithValue("@thanhTien", row.Cells["thanhTien"].Value);

                                cmdDetail.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Lưu phiếu điều phối thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetForm();
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Đã xảy ra lỗi, toàn bộ dữ liệu đã được hoàn tác. Chi tiết: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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