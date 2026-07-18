using DATNWF.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmChonDuongDan : Form
    {
        private readonly string _soHD;

        public string SelectedFilePath { get; private set; }

        public frmChonDuongDan(string soHD)
        {
            InitializeComponent();
            _soHD = soHD;
            txtFilePath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog
            {
                Description = "Chọn thư mục lưu file Excel",
                SelectedPath = txtFilePath.Text,
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog() == DialogResult.OK)
                txtFilePath.Text = fbd.SelectedPath;
        }

        private async void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            string folder = txtFilePath.Text.Trim();
            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Vui lòng chọn đường dẫn lưu file.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                MessageBox.Show("Không thể tạo thư mục. Vui lòng kiểm tra quyền truy cập.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            }

            btnXuatExcel.Enabled = false;
            btnXuatExcel.Text = "Đang xuất…";

            try
            {
                var svc = new ExportExcelService();
                string path = await svc.ExportHoaDonAsync(_soHD, folder);

                if (!string.IsNullOrEmpty(path))
                {
                    SelectedFilePath = path;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Xuất Excel thất bại. Vui lòng thử lại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnXuatExcel.Enabled = true;
                btnXuatExcel.Text = "Xuất excel";
            }
        }

        private void ImgCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
