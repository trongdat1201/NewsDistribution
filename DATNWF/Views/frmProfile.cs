using System;
using System.Windows.Forms;
using DATNWF.Models;

namespace DATNWF.Views
{
    public partial class frmProfile : Form
    {
        public frmProfile()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserSession.Clear();
            Home.NeedsRestart = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Allow Close() when logout button intentionally set DialogResult
            if (e.CloseReason == CloseReason.UserClosing && this.DialogResult != DialogResult.OK)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnFormClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Hide();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
