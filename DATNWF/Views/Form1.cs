using DATNWF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
        }
        private void OpenChildForm(Form childForm)
        {
            if (panelDesktop.Controls.Count > 0)
            {
                panelDesktop.Controls[0].Dispose();
            }

            childForm.TopLevel = false;         
            childForm.FormBorderStyle = FormBorderStyle.None; 
            childForm.Dock = DockStyle.Fill;

            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.Show();
        }
        private void btnPublications_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmPublications());
        }
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmCustomers());
        }
        private void btnInvoices_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmInvoices());
        }
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
                Brush brush = Brushes.Black; // Màu chữ

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