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

        public frmTaoDieuPhoi()
        {
            InitializeComponent();
        }

        private void FormDieuPhoi_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            ResetForm(); // Gọi hàm khởi tạo trạng thái chuẩn ngay khi mở form
        }

        // ======================= QUẢN LÝ TRẠNG THÁI CONTROL TẬP TRUNG (FIX LỖI LOCK) =======================
        private void SetUIState(bool isCreatingMaster)
        {
            this.SuspendLayout();

            // 1. Nhóm thiết lập thông tin (Bên trái)
            picTimKH.Enabled = isCreatingMaster;
            dtpNgayLapPhieu.Enabled = isCreatingMaster;
            dtpTuNgay.Enabled = isCreatingMaster;
            dtpDenNgay.Enabled = isCreatingMaster;
            txtGhiChu.Enabled = isCreatingMaster;
            btnCreate.Enabled = isCreatingMaster;

            // 2. KHÓA TUYỆT ĐỐI Ô MÃ HÓA ĐƠN VÀ MÃ KHÁCH HÀNG (Luôn luôn false từ lúc mở đến lúc đóng)
            txtSoHD.Enabled = false;
            txtSoHD.ReadOnly = true;
            txtSoHD.TabStop = false;

            txtMaKH.Enabled = false;
            txtMaKH.ReadOnly = true;
            txtMaKH.TabStop = false;

            // 3. Nhóm xử lý chi tiết (Bên phải)
            dgvChiTiet.Enabled = !isCreatingMaster;
            btnSave.Enabled = !isCreatingMaster;

            // 4. Các nút điều khiển chung của hệ thống
            btnRefresh.Enabled = true;
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

            // Xóa sạch dữ liệu cũ trên lưới
            dgvChiTiet.DataSource = null;
            dgvChiTiet.Columns.Clear();

            // Đưa toàn bộ UI về trạng thái thiết lập ban đầu
            SetUIState(true);
        }

        // ======================= LOGIC 1: TÌM KHÁCH HÀNG & SINH MÃ TỰ ĐỘNG =======================
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

                    string namHienTai = dtpTuNgay.Value.ToString("yy");
                    string dinhDangTimKiem = loaiMaPrefix + namHienTai + "_";

                    string queryMax = "SELECT MAX(soHD) FROM tabDieuPhoi WHERE soHD LIKE @prefix";
                    using (SqlCommand cmdMax = new SqlCommand(queryMax, conn))
                    {
                        cmdMax.Parameters.AddWithValue("@prefix", dinhDangTimKiem + "%");
                        object maxVal = cmdMax.ExecuteScalar();

                        int soThuTuTiepTheo = 1;

                        if (maxVal != DBNull.Value && maxVal != null)
                        {
                            string maxSoHD = maxVal.ToString();
                            if (maxSoHD.Length >= 4)
                            {
                                string chuoiSoCuoi = maxSoHD.Substring(maxSoHD.Length - 4);
                                if (int.TryParse(chuoiSoCuoi, out int soHienTai))
                                {
                                    soThuTuTiepTheo = soHienTai + 1;
                                }
                            }
                        }
                        txtSoHD.Text = dinhDangTimKiem + soThuTuTiepTheo.ToString("D4");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tự động sinh số hóa đơn: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================= LOGIC 2: KHỞI TẠO N DÒNG CHI TIẾT =======================
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maKhachHangDuocChon) || string.IsNullOrWhiteSpace(txtSoHD.Text))
            {
                MessageBox.Show("Vui lòng chọn Khách hàng để khởi tạo Số hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Lỗi: 'Từ ngày' không được lớn hơn 'Đến ngày'!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Khóa toàn bộ phần Master, kích hoạt phần chi tiết điều phối
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

        private void LoadBaoPhatHanhTheoGiaiDoan(DateTime tuNgay, DateTime denNgay)
        {
            // Reset cấu trúc cột cũ để tránh xung đột layout
            dgvChiTiet.DataSource = null;
            dgvChiTiet.Columns.Clear();

            DataTable dtChiTiet = new DataTable();
            dtChiTiet.Columns.Add("ngayNhan", typeof(DateTime));
            dtChiTiet.Columns.Add("maBao", typeof(string));
            dtChiTiet.Columns.Add("tenBao", typeof(string));
            dtChiTiet.Columns.Add("soBao", typeof(int));
            dtChiTiet.Columns.Add("donGia", typeof(decimal));
            dtChiTiet.Columns.Add("soluongDieuPhoi", typeof(int));
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

                        DateTime ngayBatDau = bao["ngayBatDau"] != DBNull.Value ? Convert.ToDateTime(bao["ngayBatDau"]) : date;
                        int soGoc = bao["sogoc"] != DBNull.Value ? Convert.ToInt32(bao["sogoc"]) : 1;

                        int soBaoTinhToan = TinhSoBaoNghiepVu(ngayBatDau, date, soGoc, bao);

                        dtChiTiet.Rows.Add(date, maBao, tenBao, soBaoTinhToan, donGia, 0, 0);
                    }
                }
            }

            dgvChiTiet.DataSource = dtChiTiet;

            // Định danh tiêu đề cột Tiếng Việt
            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["ngayNhan"].HeaderText = "Ngày nhận";
                dgvChiTiet.Columns["maBao"].HeaderText = "Mã báo";
                dgvChiTiet.Columns["tenBao"].HeaderText = "Tên ấn phẩm";
                dgvChiTiet.Columns["soBao"].HeaderText = "Số báo";
                dgvChiTiet.Columns["donGia"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["soluongDieuPhoi"].HeaderText = "SL Điều phối";
                dgvChiTiet.Columns["thanhTien"].HeaderText = "Thành tiền";

                dgvChiTiet.Columns["ngayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvChiTiet.Columns["donGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["thanhTien"].DefaultCellStyle.Format = "N0";
            }

            // =================================================================================
            // ĐOẠN LỆNH ÉP REDRAW ĐỂ HIỂN THỊ TÊN CỘT NGAY LẬP TỨC (SỬA TRIỆT ĐỂ LỖI ẨN HEADER)
            // =================================================================================
            dgvChiTiet.ColumnHeadersVisible = false;
            dgvChiTiet.ColumnHeadersVisible = true;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTiet.ColumnHeadersHeight = 40;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvChiTiet.Refresh();
        }

        // ======================= LOGIC 3: THUẬT TOÁN TÍNH SỐ BÁO =======================
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

        // ======================= LOGIC 4: CẬP NHẬT CHI TIẾT SỐ LƯỢNG =======================
        private void dgvChiTiet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];

                string maBao = row.Cells["maBao"].Value.ToString();
                string tenBao = row.Cells["tenBao"].Value.ToString();
                int soBao = Convert.ToInt32(row.Cells["soBao"].Value);
                decimal donGia = Convert.ToDecimal(row.Cells["donGia"].Value);
                int currentSl = Convert.ToInt32(row.Cells["soluongDieuPhoi"].Value);

                using (frmEditDelivery frmEdit = new frmEditDelivery(maBao, tenBao, soBao, donGia, currentSl))
                {
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        int newSl = frmEdit.SoLuongDieuPhoi;
                        row.Cells["soluongDieuPhoi"].Value = newSl;
                        row.Cells["thanhTien"].Value = newSl * donGia;
                    }
                }
            }
        }

        // ======================= LOGIC 5: LƯU TRANSACTION DỮ LIỆU XUỐNG DB =======================
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnCreate.Enabled) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
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
                                cmdDetail.Parameters.AddWithValue("@soluongBan", slDieuPhoi);
                                cmdDetail.Parameters.AddWithValue("@thanhTien", row.Cells["thanhTien"].Value);

                                cmdDetail.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Lưu phiếu điều phối thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetForm();
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
            ResetForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}