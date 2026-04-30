using DATNWF.Views;
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
        string connectionString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

        public frmPublications()
        {
            InitializeComponent();
            
        }
        private void LoadData()
        {
            this.tabBAOTableAdapter.Fill(this.thanhnienDataSet.tabBAO);
        }
        private void frmPublications_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadBaoHomNay();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");

            if (tabBAOBindingSource != null)
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    tabBAOBindingSource.Filter = string.Empty;
                }
                else
                {
                    tabBAOBindingSource.Filter = string.Format("maBao LIKE '%{0}%' OR ten LIKE '%{0}%'", keyword);
                }

                dboTabBao.ClearSelection();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmThemBao frm = new frmThemBao();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dboTabBao.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng báo để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string maBao = dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value.ToString();

            frmSuaBao frm = new frmSuaBao(maBao);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dboTabBao.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng báo để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string maBao = dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value.ToString();
            string tenBao = dboTabBao.SelectedRows[0].Cells["tenDataGridViewTextBoxColumn"].Value.ToString();

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa báo '{tenBao}' ra khỏi hệ thống?",
                                              "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString)) 
                {
                    string sql = "DELETE FROM tabBAO WHERE maBao = @maBao";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maBao", maBao);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547)
                        {
                            MessageBox.Show("Không thể xóa báo này vì đang có dữ liệu liên quan trong kho hoặc hóa đơn!",
                                            "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void LoadBaoHomNay()
        {
            DayOfWeek today = DateTime.Now.DayOfWeek;
            string cotThuTrongSQL = "";

            switch (today)
            {
                case DayOfWeek.Sunday: cotThuTrongSQL = "thu1"; break;
                case DayOfWeek.Monday: cotThuTrongSQL = "thu2"; break;
                case DayOfWeek.Tuesday: cotThuTrongSQL = "thu3"; break;
                case DayOfWeek.Wednesday: cotThuTrongSQL = "thu4"; break;
                case DayOfWeek.Thursday: cotThuTrongSQL = "thu5"; break;
                case DayOfWeek.Friday: cotThuTrongSQL = "thu6"; break;
                case DayOfWeek.Saturday: cotThuTrongSQL = "thu7"; break;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = $"SELECT maBao, ten FROM tabBAO WHERE {cotThuTrongSQL} = 1";

                SqlCommand cmd = new SqlCommand(sql, conn);
                DataTable dtHomNay = new DataTable();

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtHomNay);

                    lstBaoHomNay.DataSource = dtHomNay;
                    lstBaoHomNay.DisplayMember = "ten"; 
                    lstBaoHomNay.ValueMember = "maBao";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách báo hôm nay: " + ex.Message);
                }
            }
        }
    }
}
