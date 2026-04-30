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
    public partial class frmThemBao : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";
        public frmThemBao()
        {
            InitializeComponent();
        }
        private void imgSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaBao.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã báo!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaBao.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenBao.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên báo!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBao.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDVT.Text))
            {
                MessageBox.Show("Vui lòng nhập Đơn vị tính!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDVT.Focus();
                return;
            }

            if (txtMaBao.Text.Trim().Length > 30)
            {
                MessageBox.Show("Mã báo không được vượt quá 30 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaBao.Focus();
                return;
            }
            if (txtTenBao.Text.Trim().Length > 50)
            {
                MessageBox.Show("Tên báo không được vượt quá 50 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBao.Focus();
                return;
            }
            if (txtDVT.Text.Trim().Length > 50)
            {
                MessageBox.Show("Đơn vị tính không được vượt quá 50 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDVT.Focus();
                return;
            }
            double donGia = 0;
            if (!string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                if (!double.TryParse(txtDonGia.Text, out donGia) || donGia < 0)
                {
                    MessageBox.Show("Đơn giá phải là một số hợp lệ và không được âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                    return;
                }
            }

            int tanSuat = 0;
            if (!string.IsNullOrWhiteSpace(txtTanSuat.Text))
            {
                if (!int.TryParse(txtTanSuat.Text, out tanSuat) || tanSuat < 0)
                {
                    MessageBox.Show("Tần suất phải là số nguyên hợp lệ và không được âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTanSuat.Focus();
                    return;
                }
            }

            int soGoc = 0;
            if (!string.IsNullOrWhiteSpace(txtSoGoc.Text))
            {
                if (!int.TryParse(txtSoGoc.Text, out soGoc) || soGoc < 0)
                {
                    MessageBox.Show("Số gốc phải là số nguyên hợp lệ và không được âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoGoc.Focus();
                    return;
                }
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO tabBAO 
                       (maBao, ten, DVT, donGia, ngayBatDau, thu1, thu2, thu3, thu4, thu5, thu6, thu7, soLanPHtrongTuan, sogoc) 
                       VALUES 
                       (@maBao, @ten, @dvt, @donGia, @ngayBatDau, @t1, @t2, @t3, @t4, @t5, @t6, @t7, @tanSuat, @soGoc)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@maBao", txtMaBao.Text.Trim());
                cmd.Parameters.AddWithValue("@ten", txtTenBao.Text.Trim());
                cmd.Parameters.AddWithValue("@dvt", txtDVT.Text.Trim());
                cmd.Parameters.AddWithValue("@donGia", donGia);
                cmd.Parameters.AddWithValue("@tanSuat", tanSuat);
                cmd.Parameters.AddWithValue("@soGoc", soGoc);
                cmd.Parameters.AddWithValue("@ngayBatDau", dtpNgayBatDau.Value);

                cmd.Parameters.AddWithValue("@t1", chkChuNhat.Checked);
                cmd.Parameters.AddWithValue("@t2", chkThu2.Checked);
                cmd.Parameters.AddWithValue("@t3", chkThu3.Checked);
                cmd.Parameters.AddWithValue("@t4", chkThu4.Checked);
                cmd.Parameters.AddWithValue("@t5", chkThu5.Checked);
                cmd.Parameters.AddWithValue("@t6", chkThu6.Checked);
                cmd.Parameters.AddWithValue("@t7", chkThu7.Checked);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm báo mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("Lỗi: Mã báo này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMaBao.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
