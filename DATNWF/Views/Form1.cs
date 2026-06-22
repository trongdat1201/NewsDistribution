using DATNWF.Views;
using DATNWF.Models;
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
            // Hiển thị form Đăng nhập đầu tiên
            frmLogin login = new frmLogin();
            if (login.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }

            // Áp dụng quyền hạn menu
            ApplyPermissions();

            // Mở màn hình mặc định tương ứng với quyền hạn
            if (UserSession.IsBC)
            {
                btnInvoices_Click(sender, e);
            }
            else
            {
                btnDashboard_Click(sender, e);
            }
        }

        private void ApplyPermissions()
        {
            if (UserSession.IsHT) // Hệ thống (Admin)
            {
                btnDashboard.Visible = true;
                btnPublications.Visible = true;
                btnCustomer.Visible = true;
                btnInvoices.Visible = true;
                btnDelivery.Visible = true;
                btnInventory.Visible = true;
                btnSetting.Visible = true;
                btnLogout.Visible = true; // Hiện nút Quyền truy cập
            }
            else if (UserSession.IsNV) // Nhân viên
            {
                btnDashboard.Visible = true;
                btnPublications.Visible = true;
                btnCustomer.Visible = true;
                btnInvoices.Visible = false;
                btnDelivery.Visible = true;
                btnInventory.Visible = false;
                btnSetting.Visible = true;
                btnLogout.Visible = false; // Ẩn nút Quyền truy cập
            }
            else if (UserSession.IsBC) // Báo cáo
            {
                btnDashboard.Visible = false;
                btnPublications.Visible = false;
                btnCustomer.Visible = false;
                btnInvoices.Visible = true;
                btnDelivery.Visible = false;
                btnInventory.Visible = true;
                btnSetting.Visible = false;
                btnLogout.Visible = false; // Ẩn nút Quyền truy cập
            }
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
            if (UserSession.IsBC) return; // BC không có quyền truy cập Dashboard
            if (frmDash == null || frmDash.IsDisposed) frmDash = new frmDashboard();
            OpenChildForm(frmDash);
        }

        private void btnPublications_Click(object sender, EventArgs e)
        {
            if (UserSession.IsBC) return; // BC không có quyền truy cập Publications
            if (frmPub == null || frmPub.IsDisposed) frmPub = new frmPublications();
            OpenChildForm(frmPub);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (UserSession.IsBC) return; // BC không có quyền truy cập Customers
            if (frmCus == null || frmCus.IsDisposed) frmCus = new frmCustomers();
            OpenChildForm(frmCus);
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            if (UserSession.IsNV) return; // NV không có quyền truy cập Invoices
            if (frmInv == null || frmInv.IsDisposed) frmInv = new frmInvoices();
            OpenChildForm(frmInv);
        }
        private void btnDelivery_Click(object sender, EventArgs e)
        {
            if (UserSession.IsBC) return; // BC không có quyền truy cập Delivery
            if (frmDeli == null || frmDeli.IsDisposed) frmDeli = new frmDelivery();
            OpenChildForm(frmDeli);
        }
        private void btnInventory_Click(object sender, EventArgs e)
        {
            if (UserSession.IsNV) return; // NV không có quyền truy cập Inventory
            if (frmInven == null || frmInven.IsDisposed) frmInven = new frmInventory();
            OpenChildForm(frmInven);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (UserSession.IsBC) return; // BC không có quyền truy cập Setting
            if (frmSet == null || frmSet.IsDisposed) frmSet = new frmSetting();
            OpenChildForm(frmSet);
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