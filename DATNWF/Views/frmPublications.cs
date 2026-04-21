using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DATNWF
{
    public partial class frmPublications : Form
    {
        public frmPublications()
        {
            InitializeComponent();
            this.Load += FrmPublications_Load;
            colmaBao.DataPropertyName = "maBao";
            colTen.DataPropertyName = "ten";
            colDVT.DataPropertyName = "DVT";
            coldonGia.DataPropertyName = "donGia";
            colngayBatDau.DataPropertyName = "ngayBatDau";
            coltanSuat.DataPropertyName = "soLanPHtrongTuan";
            colsoGoc.DataPropertyName = "sogoc";
        }
        private void LoadData()
        {
            string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

            // Chỉ lấy đúng các trường bạn cần từ bảng tabBAO
            string query = "SELECT maBao, ten, DVT, donGia, ngayBatDau, soLanPHtrongTuan, sogoc FROM tabBAO";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // QUAN TRỌNG: Dòng này lệnh cho lưới CHỈ DÙNG các cột bạn đã thiết kế thủ công
                    dgvBao.AutoGenerateColumns = true;

                    // Đổ dữ liệu vào là xong
                    dgvBao.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
            }
        }
        private void FrmPublications_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
