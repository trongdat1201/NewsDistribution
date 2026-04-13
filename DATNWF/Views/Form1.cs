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
            //this.WindowState = FormWindowState.Maximized;
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
        private void dayandtime_Tick(object sender, EventArgs e)
        {
            // 1. Tạo một bức ảnh trống có kích thước bằng với PictureBox
            Bitmap bmp = new Bitmap(picTime.Width, picTime.Height);

            // 2. Tạo đối tượng Graphics để "vẽ" lên bức ảnh đó
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Khử răng cưa để chữ mượt hơn
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Thiết lập màu nền (tùy chọn)
                g.Clear(Color.White);

                // 3. Chuẩn bị nội dung và định dạng chữ
                string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                Font font = new Font("Arial", 16, FontStyle.Bold);
                Brush brush = Brushes.Black; // Màu chữ

                // 4. Vẽ chuỗi ký tự vào giữa PictureBox
                SizeF textSize = g.MeasureString(text, font);
                PointF position = new PointF(
                    (picTime.Width - textSize.Width) / 2,
                    (picTime.Height - textSize.Height) / 2
                );

                g.DrawString(text, font, brush, position);
            }

            // 5. Gán bức ảnh đã vẽ vào PictureBox
            // Giải phóng ảnh cũ để tránh tràn bộ nhớ
            if (picTime.Image != null) picTime.Image.Dispose();
            picTime.Image = bmp;
        }
    }
}