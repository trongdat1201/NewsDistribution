using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace DATNWF.Views
{
    public partial class frmSuaBao : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

        private string maBaoCanSua;
        private DataTable dtNgoaiLeTam;
        private bool isEditingException = false;
        private DateTime oldNgayPhatHanh;

        public frmSuaBao(string maBao)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.maBaoCanSua = maBao;

            this.Load += frmSuaBao_Load;

            chkChuNhat.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu2.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu3.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu4.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu5.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu6.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu7.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;

            dgvNgoaiLeTam.CellClick += dgvNgoaiLeTam_CellClick;
        }

        private void frmSuaBao_Load(object sender, EventArgs e)
        {
            KhoiTaoBaoTam();
            LoadDataGocTuDatabase();
            SetUIState(true);
        }

        private void KhoiTaoBaoTam()
        {
            dtNgoaiLeTam = new DataTable();
            dtNgoaiLeTam.Columns.Add("ngayPhatHanh", typeof(DateTime));
            dtNgoaiLeTam.Columns.Add("soLanTrongNam", typeof(int));

            dgvNgoaiLeTam.DataSource = dtNgoaiLeTam;

            if (dgvNgoaiLeTam.Columns["ngayPhatHanh"] != null)
            {
                dgvNgoaiLeTam.Columns["ngayPhatHanh"].HeaderText = "Ngày phát hành ngoại lệ";
                dgvNgoaiLeTam.Columns["ngayPhatHanh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvNgoaiLeTam.Columns["ngayPhatHanh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvNgoaiLeTam.Columns["ngayPhatHanh"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvNgoaiLeTam.Columns["soLanTrongNam"] != null)
            {
                dgvNgoaiLeTam.Columns["soLanTrongNam"].HeaderText = "Tần suất ngoại lệ / Năm";
                dgvNgoaiLeTam.Columns["soLanTrongNam"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvNgoaiLeTam.Columns["soLanTrongNam"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvNgoaiLeTam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataGocTuDatabase()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlBao = "SELECT * FROM tabBAO WHERE maBao = @maBao";
                using (SqlCommand cmd = new SqlCommand(sqlBao, conn))
                {
                    cmd.Parameters.AddWithValue("@maBao", maBaoCanSua);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtMaBao.Text = reader["maBao"].ToString();
                            txtTenBao.Text = reader["ten"].ToString();
                            cbDvt.SelectedItem = reader["DVT"].ToString();
                            txtDonGia.Text = reader["donGia"].ToString();

                            if (reader["sogoc"] != DBNull.Value)
                                txtSoGoc.Text = reader["sogoc"].ToString();
                            else
                                txtSoGoc.Text = "";

                            if (reader["ngayBatDau"] != DBNull.Value)
                                dtpNgayBatDau.Value = Convert.ToDateTime(reader["ngayBatDau"]);

                            chkChuNhat.Checked = reader["thu1"] != DBNull.Value && Convert.ToBoolean(reader["thu1"]);
                            chkThu2.Checked = reader["thu2"] != DBNull.Value && Convert.ToBoolean(reader["thu2"]);
                            chkThu3.Checked = reader["thu3"] != DBNull.Value && Convert.ToBoolean(reader["thu3"]);
                            chkThu4.Checked = reader["thu4"] != DBNull.Value && Convert.ToBoolean(reader["thu4"]);
                            chkThu5.Checked = reader["thu5"] != DBNull.Value && Convert.ToBoolean(reader["thu5"]);
                            chkThu6.Checked = reader["thu6"] != DBNull.Value && Convert.ToBoolean(reader["thu6"]);
                            chkThu7.Checked = reader["thu7"] != DBNull.Value && Convert.ToBoolean(reader["thu7"]);
                        }
                    }
                }

                txtMaBaoNgoaiLe.Text = txtMaBao.Text.Trim();

                dtNgoaiLeTam.Clear();
                string sqlNgoaiLe = "SELECT ngayPhatHanh, soLanTrongNam FROM tabBao_ngoaiLe WHERE maBao = @maBao ORDER BY ngayPhatHanh ASC";
                using (SqlCommand cmdNL = new SqlCommand(sqlNgoaiLe, conn))
                {
                    cmdNL.Parameters.AddWithValue("@maBao", maBaoCanSua);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmdNL))
                    {
                        da.Fill(dtNgoaiLeTam);
                    }
                }
            }
            ClearExceptionInputs();
        }

        private void SetUIState(bool isEditingMaster)
        {
            this.SuspendLayout(); 


            txtMaBao.Enabled = false;   
            txtTanSuat.Enabled = false; 

            txtTenBao.Enabled = isEditingMaster;
            cbDvt.Enabled = isEditingMaster;
            txtDonGia.Enabled = isEditingMaster;
            txtSoGoc.Enabled = isEditingMaster;
            dtpNgayBatDau.Enabled = isEditingMaster;
            chkChuNhat.Enabled = isEditingMaster;
            chkThu2.Enabled = isEditingMaster;
            chkThu3.Enabled = isEditingMaster;
            chkThu4.Enabled = isEditingMaster;
            chkThu5.Enabled = isEditingMaster;
            chkThu6.Enabled = isEditingMaster;
            chkThu7.Enabled = isEditingMaster;

            pnlAdd.BackColor = isEditingMaster ? Color.White : Color.WhiteSmoke;
            btnCreateBao.Enabled = isEditingMaster;
            btnEditBao.Enabled = !isEditingMaster;

            bool isExceptionActive = !isEditingMaster;
            txtMaBaoNgoaiLe.Enabled = false; 

            ngayPhatHanh.Enabled = isExceptionActive;
            txtSoLanPhatHanhTrongNam.Enabled = isExceptionActive;
            btnAddBaoNLe.Enabled = isExceptionActive;
            btnDeleteBaoNle.Enabled = isExceptionActive;
            dgvNgoaiLeTam.Enabled = isExceptionActive;

            tablelayoutAddBaoNLe.BackColor = isExceptionActive ? Color.White : Color.WhiteSmoke;

            imgSave.Enabled = isExceptionActive;    
            imgRefresh.Enabled = true;              
            imgCancel.Enabled = true;               

            this.ResumeLayout();
        }

        private void CheckBoxBaoChinh_CheckedChanged(object sender, EventArgs e)
        {
            int tanSuat = 0;
            if (chkChuNhat.Checked) tanSuat++;
            if (chkThu2.Checked) tanSuat++;
            if (chkThu3.Checked) tanSuat++;
            if (chkThu4.Checked) tanSuat++;
            if (chkThu5.Checked) tanSuat++;
            if (chkThu6.Checked) tanSuat++;
            if (chkThu7.Checked) tanSuat++;
            txtTanSuat.Text = tanSuat.ToString();
        }

        private void btnCreateBao_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenBao.Text) || cbDvt.SelectedItem == null)
            {
                MessageBox.Show("Tên báo và Đơn vị tính không được phép để trống!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDonGia.Text) || !double.TryParse(txtDonGia.Text, out double donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phải là số hợp lệ và không âm!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSoGoc.Text))
            {
                if (!int.TryParse(txtSoGoc.Text, out int soGocParsed) || soGocParsed < 0)
                {
                    MessageBox.Show("Số gốc phải là số nguyên hợp lệ và không âm!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoGoc.Focus();
                    return;
                }
            }

            txtMaBaoNgoaiLe.Text = txtMaBao.Text.Trim();

            SetUIState(false);
        }

        private void btnEditBao_Click(object sender, EventArgs e)
        {
            SetUIState(true);
        }

        private void ClearExceptionInputs()
        {
            ngayPhatHanh.Value = DateTime.Now;
            txtSoLanPhatHanhTrongNam.Clear();
            isEditingException = false;
            if (dgvNgoaiLeTam != null) dgvNgoaiLeTam.ClearSelection();
        }

        private void btnAddBaoNLe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoLanPhatHanhTrongNam.Text) || !int.TryParse(txtSoLanPhatHanhTrongNam.Text, out int soLan) || soLan < 0)
            {
                MessageBox.Show("Tần suất ngoài lề phải là số nguyên hợp lệ!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLanPhatHanhTrongNam.Focus();
                return;
            }

            DateTime ngayPH = ngayPhatHanh.Value.Date;

            if (isEditingException)
            {
                foreach (DataRow row in dtNgoaiLeTam.Rows)
                {
                    if (Convert.ToDateTime(row["ngayPhatHanh"]).Date == oldNgayPhatHanh)
                    {
                        if (ngayPH != oldNgayPhatHanh)
                        {
                            foreach (DataRow rCheck in dtNgoaiLeTam.Rows)
                            {
                                if (rCheck != row && Convert.ToDateTime(rCheck["ngayPhatHanh"]).Date == ngayPH)
                                {
                                    MessageBox.Show("Ngày ngoại lệ này đã tồn tại trong danh sách tạm!", "Dữ liệu trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        row["ngayPhatHanh"] = ngayPH;
                        row["soLanTrongNam"] = soLan;
                        break;
                    }
                }
            }
            else
            {
                foreach (DataRow row in dtNgoaiLeTam.Rows)
                {
                    if (Convert.ToDateTime(row["ngayPhatHanh"]).Date == ngayPH)
                    {
                        MessageBox.Show("Ngày phát hành này đã có trong danh sách tạm!", "Cảnh báo trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                DataRow newRow = dtNgoaiLeTam.NewRow();
                newRow["ngayPhatHanh"] = ngayPH;
                newRow["soLanTrongNam"] = soLan;
                dtNgoaiLeTam.Rows.Add(newRow);
            }

            ClearExceptionInputs();
        }

        private void dgvNgoaiLeTam_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNgoaiLeTam.CurrentRow != null)
            {
                DataRowView drv = (DataRowView)dgvNgoaiLeTam.CurrentRow.DataBoundItem;
                if (drv != null)
                {
                    ngayPhatHanh.Value = Convert.ToDateTime(drv.Row["ngayPhatHanh"]);
                    txtSoLanPhatHanhTrongNam.Text = drv.Row["soLanTrongNam"].ToString();

                    isEditingException = true;
                    oldNgayPhatHanh = ngayPhatHanh.Value.Date;
                }
            }
        }

        private void btnDeleteBaoNle_Click(object sender, EventArgs e)
        {
            if (dgvNgoaiLeTam.CurrentRow == null || dgvNgoaiLeTam.CurrentRow.Index < 0) return;

            DataRowView drv = (DataRowView)dgvNgoaiLeTam.CurrentRow.DataBoundItem;
            if (drv != null)
            {
                drv.Row.Delete();
                ClearExceptionInputs();
            }
        }

        private void imgRefresh_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn hủy bỏ mọi thay đổi hiện tại và khôi phục lại dữ liệu gốc ban đầu không?", "Khôi phục dữ liệu gốc", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                LoadDataGocTuDatabase();
                SetUIState(true); 
            }
        }

        private void imgSave_Click(object sender, EventArgs e)
        {
            double donGia = double.Parse(txtDonGia.Text);
            object soGocValue = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(txtSoGoc.Text) && int.TryParse(txtSoGoc.Text, out int soGocParsed))
            {
                soGocValue = soGocParsed;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlBao = @"UPDATE tabBAO SET 
                                           ten = @ten, DVT = @dvt, donGia = @donGia, ngayBatDau = @ngayBatDau,
                                           thu1 = @t1, thu2 = @t2, thu3 = @t3, thu4 = @t4, thu5 = @t5, thu6 = @t6, thu7 = @t7,
                                           soLanPHtrongTuan = @tanSuat, sogoc = @soGoc 
                                           WHERE maBao = @maBao";

                        using (SqlCommand cmdBao = new SqlCommand(sqlBao, conn, trans))
                        {
                            cmdBao.Parameters.AddWithValue("@maBao", maBaoCanSua);
                            cmdBao.Parameters.AddWithValue("@ten", txtTenBao.Text.Trim());
                            cmdBao.Parameters.AddWithValue("@dvt", cbDvt.SelectedItem.ToString());
                            cmdBao.Parameters.AddWithValue("@donGia", donGia);
                            cmdBao.Parameters.AddWithValue("@tanSuat", int.Parse(txtTanSuat.Text));
                            cmdBao.Parameters.AddWithValue("@soGoc", soGocValue);
                            cmdBao.Parameters.AddWithValue("@ngayBatDau", dtpNgayBatDau.Value);
                            cmdBao.Parameters.AddWithValue("@t1", chkChuNhat.Checked);
                            cmdBao.Parameters.AddWithValue("@t2", chkThu2.Checked);
                            cmdBao.Parameters.AddWithValue("@t3", chkThu3.Checked);
                            cmdBao.Parameters.AddWithValue("@t4", chkThu4.Checked);
                            cmdBao.Parameters.AddWithValue("@t5", chkThu5.Checked);
                            cmdBao.Parameters.AddWithValue("@t6", chkThu6.Checked);
                            cmdBao.Parameters.AddWithValue("@t7", chkThu7.Checked);
                            cmdBao.ExecuteNonQuery();
                        }

                        string sqlXoaOld = "DELETE FROM tabBao_ngoaiLe WHERE maBao = @maBao";
                        using (SqlCommand cmdXoa = new SqlCommand(sqlXoaOld, conn, trans))
                        {
                            cmdXoa.Parameters.AddWithValue("@maBao", maBaoCanSua);
                            cmdXoa.ExecuteNonQuery();
                        }

                        if (dtNgoaiLeTam != null && dtNgoaiLeTam.Rows.Count > 0)
                        {
                            string sqlInsertNgoaiLe = "INSERT INTO tabBao_ngoaiLe (maBao, ngayPhatHanh, soLanTrongNam) VALUES (@maBao, @ngay, @soLan)";

                            foreach (DataRow row in dtNgoaiLeTam.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted) continue;

                                using (SqlCommand cmdNL = new SqlCommand(sqlInsertNgoaiLe, conn, trans))
                                {
                                    cmdNL.Parameters.AddWithValue("@maBao", maBaoCanSua);
                                    cmdNL.Parameters.AddWithValue("@ngay", Convert.ToDateTime(row["ngayPhatHanh"]));
                                    cmdNL.Parameters.AddWithValue("@soLan", Convert.ToInt32(row["soLanTrongNam"]));
                                    cmdNL.ExecuteNonQuery();
                                }
                            }
                        }

                        trans.Commit();
                        MessageBox.Show("Cập nhật thông tin báo và cấu hình lịch ngoại lệ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Lỗi hệ thống khi lưu chỉnh sửa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}