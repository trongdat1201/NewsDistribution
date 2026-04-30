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
    public partial class frmThemKhachHang : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";
        public frmThemKhachHang()
        {
            InitializeComponent();
        }
        private void imgSave_Click(object sender, EventArgs e)
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

            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True"))
            {
                string sql = @"INSERT INTO tabKHACHHANG (MAKH, TEN, DIACHI, DIENTHOAI, CHIETKHAU, P_PH, P_KT, UUTIEN) 
                       VALUES (@makh, @ten, @diachi, @dienthoai, @chietkhau, @pph, @pkt, @uutien)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@makh", txtMaKH.Text.Trim());
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
                    MessageBox.Show("Thêm Khách hàng thành công!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) MessageBox.Show("Lỗi: Mã khách hàng đã tồn tại!");
                    else MessageBox.Show("Lỗi DB: " + ex.Message);
                }
            }
        }
        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
