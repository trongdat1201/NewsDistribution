using DATNWF.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DATNWF
{
    public partial class Home : Form
    {
        private frmDashboard frmDash;
        private frmPublications frmPub;
        private frmCustomers frmCus;
        private frmDelivery frmDeli;
        private frmInvoices frmInv;
        private frmInventory frmInven;
        private frmSetting frmSet;

        private Form activeForm = null; 

        public Home()
        {
            InitializeComponent();
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.Load += Home_Load;
        }

        private void Home_Load(object sender, EventArgs e)
        {
            btnDashboard_Click(sender, e);
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm == childForm) return;

            if (activeForm != null)
            {
                activeForm.Hide();
            }

            activeForm = childForm;

            if (!panelDesktop.Controls.Contains(childForm))
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                panelDesktop.Controls.Add(childForm);
            }

            childForm.BringToFront();
            childForm.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (frmDash == null || frmDash.IsDisposed) frmDash = new frmDashboard();
            OpenChildForm(frmDash);
        }

        private void btnPublications_Click(object sender, EventArgs e)
        {
            if (frmPub == null || frmPub.IsDisposed) frmPub = new frmPublications();
            OpenChildForm(frmPub);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (frmCus == null || frmCus.IsDisposed) frmCus = new frmCustomers();
            OpenChildForm(frmCus);
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            if (frmInv == null || frmInv.IsDisposed) frmInv = new frmInvoices();
            OpenChildForm(frmInv);
        }
        private void btnDelivery_Click(object sender, EventArgs e)
        {
            if (frmDeli == null || frmDeli.IsDisposed) frmDeli = new frmDelivery();
            OpenChildForm(frmDeli);
        }
        private void btnInventory_Click(object sender, EventArgs e)
        {
            if (frmInven == null || frmInven.IsDisposed) frmInven = new frmInventory();
            OpenChildForm(frmInven);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (frmSet == null || frmSet.IsDisposed) frmSet = new frmSetting();
            OpenChildForm(frmSet);
        }

        // Đồng hồ giữ nguyên 100% logic của bạn
        private void dayandtime_Tick(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(picTime.Width, picTime.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                g.Clear(Color.White);

                string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                Font font = new Font("Arial", 16, FontStyle.Bold);
                Brush brush = Brushes.Black;

                SizeF textSize = g.MeasureString(text, font);
                PointF position = new PointF(
                    (picTime.Width - textSize.Width) / 2,
                    (picTime.Height - textSize.Height) / 2
                );

                g.DrawString(text, font, brush, position);
            }
            if (picTime.Image != null) picTime.Image.Dispose();
            picTime.Image = bmp;
        }
    }
}