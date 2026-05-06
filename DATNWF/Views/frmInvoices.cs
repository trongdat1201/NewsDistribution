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
    public partial class frmInvoices : Form
    {
        public frmInvoices()
        {
            InitializeComponent();
        }

        private void frmInvoices_Load(object sender, EventArgs e)
        {
            this.tabHOADONTableAdapter.Fill(this.thanhnienDataSet4.tabHOADON);

        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
    }
}
