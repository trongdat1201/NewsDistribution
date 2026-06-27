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
using DATNWF.Models;

namespace DATNWF.Views
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            // Đăng ký sự kiện
            this.guna2GradientButton1.Click += Guna2GradientButton1_Click;
            this.Load += FrmLogin_Load;
            this.KeyDown += FrmLogin_KeyDown;
            this.KeyPreview = true; // Cho phép nhận sự kiện bàn phím
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            // Thiết lập nút Enter mặc định
            this.AcceptButton = guna2GradientButton1;
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            // Nhấn ESC để thoát
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void Guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox2.Text.Trim();
            string password = guna2TextBox3.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Chỉ truy vấn bằng Username để lấy mật khẩu đã băm từ DB
                string sql = "SELECT matKhau, HT, NV, BC FROM tabLogin WHERE tenDangNhap = @username";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader["matKhau"].ToString();
                            bool isPasswordCorrect = false;

                            // Kiểm tra xem mật khẩu lưu trong DB có phải là hash BCrypt không (bắt đầu bằng $2a$, $2b$, hoặc $2y$)
                            if (storedPassword.StartsWith("$2a$") || storedPassword.StartsWith("$2b$") || storedPassword.StartsWith("$2y$"))
                            {
                                try
                                {
                                    isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, storedPassword);
                                }
                                catch
                                {
                                    isPasswordCorrect = false;
                                }
                            }
                            else
                            {
                                // Cơ chế dự phòng (fallback) cho mật khẩu text thuần cũ chưa băm trong database
                                isPasswordCorrect = (password == storedPassword);
                            }

                            if (isPasswordCorrect)
                            {
                                // Đăng nhập thành công, lưu thông tin vào UserSession
                                UserSession.Username = username;
                                UserSession.IsHT = Convert.ToBoolean(reader["HT"]);
                                UserSession.IsNV = Convert.ToBoolean(reader["NV"]);
                                UserSession.IsBC = Convert.ToBoolean(reader["BC"]);

                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Tên đăng nhập hoặc Mật khẩu không chính xác!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc Mật khẩu không chính xác!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
