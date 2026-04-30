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
    public partial class frmSuaBao : Form
    {
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";
        private string maBaoCanSua;
        public frmSuaBao(String maBao)
        {
            InitializeComponent();
            maBaoCanSua = maBao;
        }
        private void frmSuaBao_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM tabBAO WHERE maBao = @maBao";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@maBao", maBaoCanSua);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtMaBao.Text = reader["maBao"].ToString();
                    txtTenBao.Text = reader["ten"].ToString();
                    txtDVT.Text = reader["DVT"].ToString();
                    txtDonGia.Text = reader["donGia"].ToString();
                    txtTanSuat.Text = reader["soLanPHtrongTuan"].ToString();
                    txtSoGoc.Text = reader["sogoc"].ToString();

                    if (reader["ngayBatDau"] != DBNull.Value)
                        dtpNgayBatDau.Value = Convert.ToDateTime(reader["ngayBatDau"]);

                    chkChuNhat.Checked = reader["thu1"] != DBNull.Value && Convert.ToBoolean(reader["thu1"]);
                    chkThu2.Checked = reader["thu2"] != DBNull.Value && Convert.ToBoolean(reader["thu2"]);
                    chkThu3.Checked = reader["thu3"] != DBNull.Value && Convert.ToBoolean(reader["thu3"]);
                    chkThu4.Checked = reader["thu4"] != DBNull.Value && Convert.ToBoolean(reader["thu4"]);
                    chkThu5.Checked = reader["thu5"] != DBNull.Value && Convert.ToBoolean(reader["thu5"]);
                    chkThu6.Checked = reader["thu6"] != DBNull.Value && Convert.ToBoolean(reader["thu6"]);
                    chkThu7.Checked = reader["thu7"] != DBNull.Value && Convert.ToBoolean(reader["thu7"]);
                }
            }
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
                string sql = @"UPDATE tabBAO SET 
                               ten = @ten, DVT = @dvt, donGia = @donGia, ngayBatDau = @ngayBatDau, 
                               thu1 = @t1, thu2 = @t2, thu3 = @t3, thu4 = @t4, thu5 = @t5, thu6 = @t6, thu7 = @t7, 
                               soLanPHtrongTuan = @tanSuat, sogoc = @soGoc
                               WHERE maBao = @maBao";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Gán Parameters (Copy y hệt bên Add New)
                cmd.Parameters.AddWithValue("@maBao", txtMaBao.Text.Trim());
                cmd.Parameters.AddWithValue("@ten", txtTenBao.Text.Trim());
                cmd.Parameters.AddWithValue("@dvt", txtDVT.Text.Trim());
                cmd.Parameters.AddWithValue("@donGia", Convert.ToDouble(txtDonGia.Text));
                cmd.Parameters.AddWithValue("@tanSuat", Convert.ToInt32(txtTanSuat.Text));
                cmd.Parameters.AddWithValue("@soGoc", Convert.ToInt32(txtSoGoc.Text));
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
                    MessageBox.Show("Cập nhật báo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
