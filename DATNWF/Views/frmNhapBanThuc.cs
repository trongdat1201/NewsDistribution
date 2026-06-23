using System;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmNhapBanThuc : Form
    {
        public int SoLuongBanThuc { get; private set; }

        public frmNhapBanThuc(string maBao, string tenBao, string soBao,
            decimal donGia, int slDieuPhoi, int currentBanThuc)
        {
            InitializeComponent();

            txtMaBao.Text = maBao;
            txtTenBao.Text = tenBao;
            txtSoBao.Text = soBao;
            txtDonGia.Text = donGia.ToString("N0");
            txtSoLuong.Text = slDieuPhoi.ToString();
            txtBanThuc.Text = currentBanThuc > 0 ? currentBanThuc.ToString() : "";

            txtMaBao.ReadOnly = true;
            txtTenBao.ReadOnly = true;
            txtSoBao.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtSoLuong.ReadOnly = true;

            txtMaBao.Enabled = false;
            txtTenBao.Enabled = false;
            txtSoBao.Enabled = false;
            txtDonGia.Enabled = false;
            txtSoLuong.Enabled = false;

            txtMaBao.TabStop = false;
            txtTenBao.TabStop = false;
            txtSoBao.TabStop = false;
            txtDonGia.TabStop = false;
            txtSoLuong.TabStop = false;

            btnSave.Click += BtnSave_Click;
            btnDefault.Click += BtnDefault_Click;

            this.ActiveControl = txtBanThuc;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string input = txtBanThuc.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                SoLuongBanThuc = 0;
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            if (!int.TryParse(input, out int parsed) || parsed < 0)
            {
                MessageBox.Show("Số lượng bán thực phải là số nguyên không âm hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBanThuc.Focus();
                txtBanThuc.SelectAll();
                return;
            }

            int slDieuPhoi = int.TryParse(txtSoLuong.Text, out int sldp) ? sldp : 0;
            if (parsed > slDieuPhoi)
            {
                MessageBox.Show("Số lượng bán thực không được lớn hơn số lượng điều phối!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBanThuc.Focus();
                txtBanThuc.SelectAll();
                return;
            }

            SoLuongBanThuc = parsed;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnDefault_Click(object sender, EventArgs e)
        {
            txtBanThuc.Text = txtSoLuong.Text;
            txtBanThuc.Focus();
            txtBanThuc.SelectAll();
        }

        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
