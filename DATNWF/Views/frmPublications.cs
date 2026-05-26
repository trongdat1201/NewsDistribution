using DATNWF.Views;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace DATNWF
{
    public partial class frmPublications : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

        public frmPublications()
        {
            InitializeComponent();
            this.dboTabBao.SelectionChanged += new System.EventHandler(this.dboTabBao_SelectionChanged);
        }

        private void LoadData()
        {
            this.tabBAOTableAdapter.Fill(this.thanhnienDataSet.tabBAO);
            this.tabBao_ngoaiLeTableAdapter.Fill(this.thanhnienDataSet8.tabBao_ngoaiLe);
        }

        private void frmPublications_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadBaoHomNay();
        }

        private void dboTabBao_SelectionChanged(object sender, EventArgs e)
        {
            if (dboTabBao.SelectedRows.Count > 0)
            {
                string maBao = dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value.ToString();

                if (tabBaongoaiLeBindingSource != null)
                {
                    tabBaongoaiLeBindingSource.Filter = $"maBao = '{maBao.Replace("'", "''")}'";
                }
            }
            else
            {
                if (tabBaongoaiLeBindingSource != null)
                {
                    tabBaongoaiLeBindingSource.Filter = "1 = 0";
                }
            }
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
            frmThemBao frmChon = new frmThemBao();
            if (frmChon.ShowDialog() == DialogResult.OK)
            {
                LoadData();
                LoadBaoHomNay();
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
                LoadBaoHomNay();
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

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa báo '{tenBao}' ra khỏi hệ thống?\nThao tác này sẽ xóa cả lịch phát hành ngoại lệ của báo này.",
                                              "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string sqlNgoaiLe = "DELETE FROM tabBao_ngoaiLe WHERE maBao = @maBao";
                            SqlCommand cmdNgoaiLe = new SqlCommand(sqlNgoaiLe, conn, trans);
                            cmdNgoaiLe.Parameters.AddWithValue("@maBao", maBao);
                            cmdNgoaiLe.ExecuteNonQuery();

                            string sqlBao = "DELETE FROM tabBAO WHERE maBao = @maBao";
                            SqlCommand cmdBao = new SqlCommand(sqlBao, conn, trans);
                            cmdBao.Parameters.AddWithValue("@maBao", maBao);
                            cmdBao.ExecuteNonQuery();

                            trans.Commit(); 
                            MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            LoadBaoHomNay();
                        }
                        catch (SqlException ex)
                        {
                            trans.Rollback(); 
                            if (ex.Number == 547)
                            {
                                MessageBox.Show("Không thể xóa báo này vì đang có dữ liệu liên quan trong kho hoặc hóa đơn điều phối!",
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

                    dgvBaoHomNay.DataSource = dtHomNay;

                    if (dgvBaoHomNay.Columns.Count > 0)
                    {
                        dgvBaoHomNay.Columns["maBao"].Visible = false;
                        dgvBaoHomNay.Columns["ten"].HeaderText = "Tên báo";
                        dgvBaoHomNay.Columns["ten"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    dgvBaoHomNay.ColumnHeadersVisible = false;
                    dgvBaoHomNay.RowHeadersVisible = false;
                    dgvBaoHomNay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvBaoHomNay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvBaoHomNay.AllowUserToAddRows = false;
                    dgvBaoHomNay.ReadOnly = true;
                    dgvBaoHomNay.BackgroundColor = Color.White;
                    dgvBaoHomNay.BorderStyle = BorderStyle.None;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách báo hôm nay: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}