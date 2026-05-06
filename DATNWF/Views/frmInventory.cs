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
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'thanhnienDataSet5.tabTon' table. You can move, or remove it, as needed.
            this.tabTonTableAdapter.Fill(this.thanhnienDataSet5.tabTon);

        }
    }
}
