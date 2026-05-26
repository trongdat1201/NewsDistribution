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
            txtSoLuong.Text = currentSl.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SoLuongDieuPhoi = Convert.ToInt32(txtSoLuong.Text); 

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