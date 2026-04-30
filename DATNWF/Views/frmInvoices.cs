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
            // TODO: This line of code loads data into the 'thanhnienDataSet4.tabHOADON' table. You can move, or remove it, as needed.
            this.tabHOADONTableAdapter.Fill(this.thanhnienDataSet4.tabHOADON);

        }
    }
}
