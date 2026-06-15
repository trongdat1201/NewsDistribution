using System;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmEditDelivery : Form
    {
        public int SoLuongDieuPhoi { get; private set; }

        public frmEditDelivery(string maBao, string tenBao, int soBao, decimal donGia, int currentSl)
        {
            InitializeComponent();

            txtMaBao.Text = maBao;
            txtTenBao.Text = tenBao;
            txtSoBao.Text = soBao.ToString();
            txtDonGia.Text = donGia.ToString("N0");
            txtSoLuong.Text = currentSl > 0 ? currentSl.ToString() : "";

            txtMaBao.ReadOnly = true;
            txtTenBao.ReadOnly = true;
            txtSoBao.ReadOnly = true;
            txtDonGia.ReadOnly = true;

            txtMaBao.Enabled = false;
            txtTenBao.Enabled = false;
            txtSoBao.Enabled = false;
            txtDonGia.Enabled = false;

            txtMaBao.TabStop = false;
            txtTenBao.TabStop = false;
            txtSoBao.TabStop = false;
            txtDonGia.TabStop = false;

            this.ActiveControl = txtSoLuong;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sl = txtSoLuong.Text.Trim();
            if (string.IsNullOrEmpty(sl) || sl == "0")
            {
                SoLuongDieuPhoi = 0;
            }
            else
            {
                if (!int.TryParse(sl, out int parsedSl) || parsedSl < 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    txtSoLuong.SelectAll();
                    return;
                }
                SoLuongDieuPhoi = parsedSl;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}