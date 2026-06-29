using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;

namespace DATNWF.Views
{
    public partial class frmAccess : Form
    {
        private class UserDto
        {
            public string TenDangNhap { get; set; }
            public bool Ht { get; set; }
            public bool Nv { get; set; }
            public bool Bc { get; set; }
        }

        private List<UserDto> _usersList = new List<UserDto>();
        private bool _isEditMode = false;

        public frmAccess()
        {
            InitializeComponent();
        }

        private void frmAccess_Load(object sender, EventArgs e)
        {
            LoadUsers();
            ResetForm();
        }

        private void LoadUsers()
        {
            try
            {
                _usersList = ApiClient.Instance.GetAsync<List<UserDto>>("Users").GetAwaiter().GetResult();
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = _usersList;

                if (dgvUsers.Columns.Count > 0)
                {
                    dgvUsers.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                    dgvUsers.Columns["Ht"].HeaderText = "Hệ thống (HT)";
                    dgvUsers.Columns["Nv"].HeaderText = "Nhân viên (NV)";
                    dgvUsers.Columns["Bc"].HeaderText = "Báo cáo (BC)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách người dùng: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtUsername.Clear();
            txtUsername.Enabled = true;
            txtPassword.Clear();
            chkHT.Checked = false;
            chkNV.Checked = false;
            chkBC.Checked = false;
            _isEditMode = false;
            lblStatus.Text = "Chế độ: Thêm mới người dùng";
            lblStatus.ForeColor = Color.ForestGreen;
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                var row = dgvUsers.SelectedRows[0].DataBoundItem as UserDto;
                if (row != null)
                {
                    txtUsername.Text = row.TenDangNhap;
                    txtUsername.Enabled = false; // Không cho sửa username
                    txtPassword.Clear(); // Password bỏ trống nếu không đổi
                    chkHT.Checked = row.Ht;
                    chkNV.Checked = row.Nv;
                    chkBC.Checked = row.Bc;
                    _isEditMode = true;
                    lblStatus.Text = "Chế độ: Chỉnh sửa người dùng";
                    lblStatus.ForeColor = Color.RoyalBlue;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (!_isEditMode && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Mật khẩu cho người dùng mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            var model = new
            {
                TenDangNhap = username,
                MatKhau = string.IsNullOrEmpty(password) ? null : password,
                Ht = chkHT.Checked,
                Nv = chkNV.Checked,
                Bc = chkBC.Checked
            };

            try
            {
                bool ok;
                if (_isEditMode)
                {
                    ok = ApiClient.Instance.PutAsync($"Users/{username}", model).GetAwaiter().GetResult();
                }
                else
                {
                    // POST returns created object
                    var res = ApiClient.Instance.PostAsync<object, object>("Users", model);
                    ok = (res != null);
                }

                if (ok)
                {
                    MessageBox.Show("Lưu thông tin người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Lưu thông tin người dùng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu người dùng: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;
            var row = dgvUsers.SelectedRows[0].DataBoundItem as UserDto;
            if (row == null) return;

            if (row.TenDangNhap.Equals(UserSession.Username, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không thể tự xóa tài khoản đang đăng nhập của chính mình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản '{row.TenDangNhap}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool ok = ApiClient.Instance.DeleteAsync($"Users/{row.TenDangNhap}").GetAwaiter().GetResult();
                    if (ok)
                    {
                        MessageBox.Show("Đã xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa tài khoản: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            dgvUsers.ClearSelection();
        }
    }
}
