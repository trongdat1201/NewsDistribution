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

        private class LoginResponse
        {
            public string Token { get; set; }
            public string Username { get; set; }
            public bool Ht { get; set; }
            public bool Nv { get; set; }
            public bool Bc { get; set; }
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

            try
            {
                var reqData = new { Username = username, Password = password };
                var response = ApiClient.Instance.PostAsync<object, LoginResponse>("Auth/login", reqData).GetAwaiter().GetResult();

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    UserSession.Username = response.Username;
                    UserSession.IsHT = response.Ht;
                    UserSession.IsNV = response.Nv;
                    UserSession.IsBC = response.Bc;
                    UserSession.JwtToken = response.Token;

                    ApiClient.Instance.SetToken(response.Token);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc Mật khẩu không chính xác!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
