using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmSuaKhachHang : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";
        private string maKHCanSua;
        public frmSuaKhachHang(string maKH)
        {
            InitializeComponent();
            maKHCanSua = maKH;
        }
        private void frmSuaKhachHang_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM tabKHACHHANG WHERE MAKH = @makh";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@makh", maKHCanSua);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtMaKH.Text = reader["MAKH"].ToString();
                    txtTenKH.Text = reader["TEN"].ToString();
                    txtDiaChi.Text = reader["DIACHI"].ToString();
                    txtDienThoai.Text = reader["DIENTHOAI"].ToString();
                    txtChietKhau.Text = reader["CHIETKHAU"].ToString();

                    // Load Checkbox kiểu bit (P_PH, P_KT)
                    chkP_PH.Checked = reader["P_PH"] != DBNull.Value && Convert.ToBoolean(reader["P_PH"]);
                    chkP_KT.Checked = reader["P_KT"] != DBNull.Value && Convert.ToBoolean(reader["P_KT"]);

                    // Load ComboBox (Mức ưu tiên)
                    if (reader["UUTIEN"] != DBNull.Value)
                    {
                        cboUuTien.SelectedItem = reader["UUTIEN"].ToString();
                    }
                }
            }
        }

        // Sự kiện bấm nút Lưu (Save)
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
    }
}

