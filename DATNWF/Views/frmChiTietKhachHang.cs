using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.Charts.WinForms;

namespace DATNWF.Views
{
    public partial class frmChiTietKhachHang : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

        private string maKHCanSua;
        private DataTable _dtLichSu; // cache để search nhanh

        // giá trị mặc định được load lúc mở form
        private string _defaultTen;
        private string _defaultDiaChi;
        private string _defaultDienThoai;
        private string _defaultChietKhau;
        private bool _defaultP_PH;
        private bool _defaultP_KT;
        private string _defaultUuTien;

        public frmChiTietKhachHang(string maKH)
        {
            InitializeComponent();
            maKHCanSua = maKH;
        }

        #region Chart

        private class ChartDataPoint
        {
            public string Label { get; set; }
            public double DoanhThu { get; set; }
            public int SoDonHang { get; set; }
        }

        private List<ChartDataPoint> LoadChartData()
        {
            var points = new List<ChartDataPoint>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT
                        YEAR(h.ngayLapPhieu) AS Nam,
                        SUM(ct.thanhTien) AS TongDoanhThu,
                        COUNT(DISTINCT h.sohd) AS SoDonHang
                    FROM tabHOADON h
                    JOIN tabCHITIETHOADON ct ON h.sohd = ct.sohd
                    WHERE h.makh = @makh
                    GROUP BY YEAR(h.ngayLapPhieu)
                    ORDER BY Nam";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@makh", maKHCanSua);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        points.Add(new ChartDataPoint
                        {
                            Label = reader.GetInt32(0).ToString(),
                            DoanhThu = Convert.ToDouble(reader[1]),
                            SoDonHang = reader.GetInt32(2)
                        });
                    }
                }
            }

            return points;
        }

        private void RenderChart(List<ChartDataPoint> data)
        {
            if (data == null || data.Count == 0)
            {
                chartthongketangtruong.Datasets.Clear();
                return;
            }

            var seriesDoanhThu = new GunaLineDataset { Label = "Doanh thu (VNĐ)" };
            var seriesSoDon = new GunaLineDataset
            {
                Label = "Số đơn hàng",
                BorderColor = Color.Red,
                FillColor = Color.FromArgb(50, Color.Red)
            };

            foreach (var p in data)
            {
                seriesDoanhThu.DataPoints.Add(p.Label, p.DoanhThu);
                seriesSoDon.DataPoints.Add(p.Label, p.SoDonHang);
            }

            chartthongketangtruong.Datasets.Clear();
            chartthongketangtruong.Datasets.Add(seriesDoanhThu);
            chartthongketangtruong.Datasets.Add(seriesSoDon);

            chartthongketangtruong.Refresh();
        }

        #endregion

        #region Lịch sử giao dịch

        private DataTable FetchLichSuGiaoDich()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT
                        h.sohd           AS SoHD,
                        h.ngayLapPhieu   AS NgayLapPhieu,
                        ct.tenBao        AS TenBao,
                        ct.soLuongThuc   AS SoLuong,
                        ct.donGia        AS DonGia,
                        ct.thanhTien     AS ThanhTien
                    FROM tabHOADON h
                    JOIN tabCHITIETHOADON ct ON h.sohd = ct.sohd
                    WHERE h.makh = @makh
                    ORDER BY h.ngayLapPhieu DESC, h.sohd";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@makh", maKHCanSua);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void BindLichSu(DataTable dt)
        {
            dgvLichsugiaodich.DataSource = dt;
            dgvLichsugiaodich.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            if (dgvLichsugiaodich.Columns.Count > 0)
            {
                dgvLichsugiaodich.Columns["SoHD"].HeaderText = "Số hóa đơn";
                dgvLichsugiaodich.Columns["SoHD"].Width = 120;

                dgvLichsugiaodich.Columns["NgayLapPhieu"].HeaderText = "Ngày lập";
                dgvLichsugiaodich.Columns["NgayLapPhieu"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvLichsugiaodich.Columns["NgayLapPhieu"].Width = 110;

                dgvLichsugiaodich.Columns["TenBao"].HeaderText = "Tên báo";
                dgvLichsugiaodich.Columns["TenBao"].Width = 180;

                dgvLichsugiaodich.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvLichsugiaodich.Columns["SoLuong"].Width = 80;
                dgvLichsugiaodich.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvLichsugiaodich.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvLichsugiaodich.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvLichsugiaodich.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvLichsugiaodich.Columns["DonGia"].Width = 100;

                dgvLichsugiaodich.Columns["ThanhTien"].HeaderText = "Thành tiền";
                dgvLichsugiaodich.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvLichsugiaodich.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvLichsugiaodich.Columns["ThanhTien"].Width = 120;

                dgvLichsugiaodich.ReadOnly = true;
                dgvLichsugiaodich.AllowUserToResizeRows = false;
                dgvLichsugiaodich.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_dtLichSu == null) return;

            string keyword = txtSearch.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(keyword))
            {
                BindLichSu(_dtLichSu);
                return;
            }

            DataView dv = new DataView(_dtLichSu);
            dv.RowFilter = $"SoHD LIKE '%{keyword}%' OR TenBao LIKE '%{keyword}%'";
            dgvLichsugiaodich.DataSource = dv.ToTable();
        }

        #endregion

        #region Form Load

        private async void frmSuaKhachHang_Load(object sender, EventArgs e)
        {
            txtMaKH.Enabled = false;

            var khData = await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM tabKHACHHANG WHERE MAKH = @makh";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@makh", maKHCanSua);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new
                                {
                                    MaKH = reader["MAKH"].ToString(),
                                    Ten = reader["TEN"].ToString(),
                                    DiaChi = reader["DIACHI"] == DBNull.Value ? "" : reader["DIACHI"].ToString(),
                                    DienThoai = reader["DIENTHOAI"] == DBNull.Value ? "" : reader["DIENTHOAI"].ToString(),
                                    ChietKhau = reader["CHIETKHAU"].ToString(),
                                    P_PH = reader["P_PH"] != DBNull.Value && Convert.ToBoolean(reader["P_PH"]),
                                    P_KT = reader["P_KT"] != DBNull.Value && Convert.ToBoolean(reader["P_KT"]),
                                    UuTien = reader["UUTIEN"] == DBNull.Value ? null : reader["UUTIEN"].ToString()
                                };
                            }
                        }
                    }
                }
                return null;
            });

            if (khData != null)
            {
                txtMaKH.Text = khData.MaKH;
                txtTenKH.Text = khData.Ten;
                txtDiaChi.Text = khData.DiaChi;
                txtDienThoai.Text = khData.DienThoai;
                txtChietKhau.Text = khData.ChietKhau;
                chkP_PH.Checked = khData.P_PH;
                chkP_KT.Checked = khData.P_KT;
                if (khData.UuTien != null)
                    cboUuTien.SelectedItem = khData.UuTien;

                // lưu giá trị mặc định để btnDefault restore sau này
                _defaultTen = khData.Ten;
                _defaultDiaChi = khData.DiaChi;
                _defaultDienThoai = khData.DienThoai;
                _defaultChietKhau = khData.ChietKhau;
                _defaultP_PH = khData.P_PH;
                _defaultP_KT = khData.P_KT;
                _defaultUuTien = khData.UuTien;
            }

            var chartData = await Task.Run(() => LoadChartData());
            RenderChart(chartData);

            _dtLichSu = await Task.Run(() => FetchLichSuGiaoDich());
            BindLichSu(_dtLichSu);
        }


        #endregion

        #region Save / Cancel

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Mã khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKH.Focus(); return;
            }
            if (txtMaKH.Text.Length > 30)
            {
                MessageBox.Show("Mã khách hàng không vượt quá 30 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus(); return;
            }

            if (!short.TryParse(txtChietKhau.Text, out short chietKhau) || chietKhau < 0 || chietKhau > 100)
            {
                MessageBox.Show("Chiết khấu phải là số từ 0 đến 100!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChietKhau.Focus(); return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE tabKHACHHANG SET 
                               TEN = @ten, 
                               DIACHI = @diachi, 
                               DIENTHOAI = @dienthoai, 
                               CHIETKHAU = @chietkhau, 
                               P_PH = @pph, 
                               P_KT = @pkt, 
                               UUTIEN = @uutien
                               WHERE MAKH = @makh";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@makh", maKHCanSua); 
                cmd.Parameters.AddWithValue("@ten", txtTenKH.Text.Trim());

                cmd.Parameters.AddWithValue("@diachi", string.IsNullOrWhiteSpace(txtDiaChi.Text) ? (object)DBNull.Value : txtDiaChi.Text.Trim());
                cmd.Parameters.AddWithValue("@dienthoai", string.IsNullOrWhiteSpace(txtDienThoai.Text) ? (object)DBNull.Value : txtDienThoai.Text.Trim());

                cmd.Parameters.AddWithValue("@chietkhau", chietKhau);
                cmd.Parameters.AddWithValue("@pph", chkP_PH.Checked);
                cmd.Parameters.AddWithValue("@pkt", chkP_KT.Checked);
                cmd.Parameters.AddWithValue("@uutien", cboUuTien.SelectedItem == null ? (object)DBNull.Value : cboUuTien.SelectedItem.ToString());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật Khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtTenKH.Text = _defaultTen;
            txtDiaChi.Text = _defaultDiaChi;
            txtDienThoai.Text = _defaultDienThoai;
            txtChietKhau.Text = _defaultChietKhau;
            chkP_PH.Checked = _defaultP_PH;
            chkP_KT.Checked = _defaultP_KT;
            cboUuTien.SelectedItem = _defaultUuTien;
        }

        #endregion
    }
}
