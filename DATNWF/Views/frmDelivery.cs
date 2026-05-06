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
    public partial class frmDelivery : Form
    {
        public frmDelivery()
        {
            InitializeComponent();
        }

        private void frmDelivery_Load(object sender, EventArgs e)
        {
            this.tabDieuPhoiTableAdapter.Fill(this.thanhnienDataSet1.tabDieuPhoi);

        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmTaoDieuPhoi frm = new frmTaoDieuPhoi();
            if (frm.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
