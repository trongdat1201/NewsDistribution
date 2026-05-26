using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmChonBao : Form
    {
        public frmChonBao()
        {
            InitializeComponent();
        }

        private void btnThemBao_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            frmThemBao frm = new frmThemBao();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK; 
            }
            this.Close();
        }

        private void btnThemNgoaiLe_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            frmThemBaoNLe frm = new frmThemBaoNLe();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK; 
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
