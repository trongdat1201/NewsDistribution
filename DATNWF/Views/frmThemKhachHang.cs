using System;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmThemKhachHang : Form
    {
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

            var kh = new KhachHangDto
            {
                MaKH = txtMaKH.Text.Trim(),
                Ten = txtTenKH.Text.Trim(),
                DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim(),
                DienThoai = string.IsNullOrWhiteSpace(txtDienThoai.Text) ? null : txtDienThoai.Text.Trim(),
                ChietKhau = chietKhau,
                P_PH = chkP_PH.Checked,
                P_KT = chkP_KT.Checked,
                Uutien = cboUuTien.SelectedItem == null ? null : cboUuTien.SelectedItem.ToString()
            };

            try
            {
                bool success = ApiClient.Instance.PostAsync("Customers", kh).GetAwaiter().GetResult();
                if (success)
                {
                    MessageBox.Show("Thêm Khách hàng thành công!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi: Mã khách hàng đã tồn tại hoặc dữ liệu không hợp lệ!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi API: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
