using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DATNWF.Views
{
    public partial class frmTaoDieuPhoi : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

        private string maKhachHangDuocChon = "";

        public frmTaoDieuPhoi()
        {
            InitializeComponent();
        }

        private void FormDieuPhoi_Load(object sender, EventArgs e)
        {
            ResetForm();
            txtMaKH.ReadOnly = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string soHD = txtSoHD.Text.Trim();

            if (string.IsNullOrWhiteSpace(soHD))
            {
                MessageBox.Show("Vui lòng nhập Số hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoHD.Focus();
                return;
            }

            string query = "SELECT COUNT(*) FROM tabDieuPhoi WHERE soHD = @soHD";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@soHD", soHD);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Số Hóa Đơn (Điều phối) này đã tồn tại trong hệ thống!\nVui lòng nhập số khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtSoHD.Focus();
                            txtSoHD.SelectAll();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dtpTuNgay.Focus();
                return;
            }

            if (ngayLapPhieu > tuNgay)
            {
                MessageBox.Show("Cảnh báo: 'Ngày lập phiếu' đang lớn hơn 'Từ ngày'!\nVui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayLapPhieu.Focus();
                return;
            }

            panelLeft.Enabled = false;
            panelRight.Enabled = true;
            LoadBaoPhatHanhTheoGiaiDoan(tuNgay, denNgay);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            panelLeft.Enabled = true;

            txtSoHD.Clear();
            txtGhiChu.Clear();

            txtMaKH.Clear();
            maKhachHangDuocChon = "";

            dtpNgayLapPhieu.Value = DateTime.Now;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;

            txtSoHD.Focus();
            if (dgvChiTiet.DataSource != null)
            {
                dgvChiTiet.DataSource = null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void LoadBaoPhatHanhTheoGiaiDoan(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dtChiTiet = new DataTable();
            dtChiTiet.Columns.Add("ngayNhan", typeof(DateTime));
            dtChiTiet.Columns.Add("maBao", typeof(string));
            dtChiTiet.Columns.Add("tenBao", typeof(string));
            dtChiTiet.Columns.Add("soBao", typeof(int));
            dtChiTiet.Columns.Add("donGia", typeof(decimal));
            dtChiTiet.Columns.Add("soluongDieuPhoi", typeof(int));
            dtChiTiet.Columns.Add("thanhTien", typeof(decimal));

            DataTable dtBao = new DataTable();
            string queryBao = "SELECT maBao, ten, donGia, ngayBatDau, thu1, thu2, thu3, thu4, thu5, thu6, thu7, sogoc, soLanPHtrongTuan FROM tabBAO";
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
                        int soLanPH = bao["soLanPHtrongTuan"] != DBNull.Value ? Convert.ToInt32(bao["soLanPHtrongTuan"]) : 1;

                        int soBaoTinhToan = TinhSoBao(ngayBatDau, date, soGoc, soLanPH);

                        dtChiTiet.Rows.Add(date, maBao, tenBao, soBaoTinhToan, donGia, 0, 0);
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
                dgvChiTiet.Columns["thanhTien"].HeaderText = "Thành tiền";

                dgvChiTiet.Columns["ngayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvChiTiet.Columns["donGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["thanhTien"].DefaultCellStyle.Format = "N0";

                dgvChiTiet.Columns["soBao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvChiTiet.Columns["soluongDieuPhoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvChiTiet.Columns["donGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["thanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                dgvChiTiet.ColumnHeadersHeight = 40;
                dgvChiTiet.Refresh();
            }
        }

        private int TinhSoBao(DateTime ngayBatDau, DateTime ngayHienTai, int soGoc, int soLanPHTuan)
        {
            if (ngayHienTai <= ngayBatDau) return soGoc;
            TimeSpan diff = ngayHienTai - ngayBatDau;
            int totalWeeks = (int)(diff.TotalDays / 7);
            return soGoc + (totalWeeks * soLanPHTuan);
        }

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

                        row.DefaultCellStyle.BackColor = Color.FromArgb(0, 60, 70);
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (panelLeft.Enabled) return;

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
                    panelRight.Enabled = false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Đã xảy ra lỗi, toàn bộ dữ liệu đã được hoàn tác. Chi tiết: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}