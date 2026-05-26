using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace DATNWF.Views
{
    public partial class frmThemBao : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"].ConnectionString;

        private DataTable dtNgoaiLeTam;
        private bool isEditingException = false;
        private DateTime oldNgayPhatHanh;

        public frmThemBao()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.Load += frmThemBao_Load;

            chkChuNhat.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu2.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu3.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu4.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu5.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu6.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;
            chkThu7.CheckedChanged += CheckBoxBaoChinh_CheckedChanged;

            dgvNgoaiLeTam.CellClick += dgvNgoaiLeTam_CellClick;
        }

        private void frmThemBao_Load(object sender, EventArgs e)
        {
            KhoiTaoBaoTam();
            ResetToInitState();
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
            }
            if (dgvNgoaiLeTam.Columns["soLanTrongNam"] != null)
            {
                dgvNgoaiLeTam.Columns["soLanTrongNam"].HeaderText = "Tần suất ngoại lệ / Năm";
            }

            dgvNgoaiLeTam.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNgoaiLeTam.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvNgoaiLeTam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SetUIState(bool isCreatingMaster)
        {
            this.SuspendLayout(); 

            txtMaBao.Enabled = isCreatingMaster && string.IsNullOrEmpty(txtMaBao.Text);

            txtTenBao.Enabled = isCreatingMaster;
            cbDvt.Enabled = isCreatingMaster;
            txtDonGia.Enabled = isCreatingMaster;
            txtSoGoc.Enabled = isCreatingMaster;
            dtpNgayBatDau.Enabled = isCreatingMaster;
            chkChuNhat.Enabled = isCreatingMaster;
            chkThu2.Enabled = isCreatingMaster;
            chkThu3.Enabled = isCreatingMaster;
            chkThu4.Enabled = isCreatingMaster;
            chkThu5.Enabled = isCreatingMaster;
            chkThu6.Enabled = isCreatingMaster;
            chkThu7.Enabled = isCreatingMaster;

            pnlAdd.BackColor = isCreatingMaster ? Color.White : Color.WhiteSmoke;
            btnCreateBao.Enabled = isCreatingMaster;
            btnEditBao.Enabled = !isCreatingMaster;

            bool isExceptionActive = !isCreatingMaster;

            txtMaBaoNgoaiLe.Enabled = false;

            ngayPhatHanh.Enabled = isExceptionActive;
            txtSoLanPhatHanhTrongNam.Enabled = isExceptionActive;
            btnAddBaoNLe.Enabled = isExceptionActive;
            btnDeleteBaoNle.Enabled = isExceptionActive;
            dgvNgoaiLeTam.Enabled = isExceptionActive;

            tablelayoutAddBaoNLe.BackColor = isExceptionActive ? Color.White : Color.WhiteSmoke;

            imgSave.Enabled = isExceptionActive;    
            imgRefresh.Enabled = isExceptionActive;
            imgCancel.Enabled = true;              

            this.ResumeLayout(); 
        }

        private void ResetToInitState()
        {
            txtMaBao.Clear();
            txtTenBao.Clear();
            cbDvt.SelectedIndex = -1;
            txtDonGia.Clear();
            txtSoGoc.Clear();
            txtTanSuat.Text = "0";
            txtTanSuat.Enabled = false;
            dtpNgayBatDau.Value = DateTime.Now;

            chkChuNhat.Checked = false;
            chkThu2.Checked = false;
            chkThu3.Checked = false;
            chkThu4.Checked = false;
            chkThu5.Checked = false;
            chkThu6.Checked = false;
            chkThu7.Checked = false;

            ClearExceptionInputs();

            txtMaBaoNgoaiLe.Clear();

            dtNgoaiLeTam.Clear(); 

            SetUIState(true); 
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
            if (string.IsNullOrWhiteSpace(txtMaBao.Text) || string.IsNullOrWhiteSpace(txtTenBao.Text) || cbDvt.SelectedItem == null)
            {
                MessageBox.Show("Mã báo, Tên báo và Đơn vị tính không được phép để trống!", "Ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaBao.Text.Trim().Length > 30 || txtTenBao.Text.Trim().Length > 50)
            {
                MessageBox.Show("Mã báo (tối đa 30) hoặc Tên báo (tối đa 50) vượt quá độ dài!", "Ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDonGia.Text) || !double.TryParse(txtDonGia.Text, out double donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phải là số hợp lệ và không âm!", "Ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSoGoc.Text))
            {
                if (!int.TryParse(txtSoGoc.Text, out int soGocParsed) || soGocParsed < 0)
                {
                    MessageBox.Show("Số gốc phải là số nguyên hợp lệ và không âm!", "Ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Tần suất ngoài lề phải là số nguyên hợp lệ!", "Ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                    MessageBox.Show("Ngày ngoại lệ này đã tồn tại trong danh sách!", "Trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("Ngày phát hành này đã được thêm vào danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnXoaNgoaiLe_Click(object sender, EventArgs e)
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
            ResetToInitState();
        }

        private void imgSave_Click(object sender, EventArgs e)
        {
            string maBao = txtMaBao.Text.Trim();

            if (string.IsNullOrEmpty(maBao) || string.IsNullOrEmpty(txtTenBao.Text))
            {
                MessageBox.Show("Vui lòng hoàn thiện thông tin đầu báo trước khi lưu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                        string sqlBao = @"
                            IF EXISTS (SELECT 1 FROM tabBAO WHERE maBao = @maBao)
                            BEGIN
                                UPDATE tabBAO SET ten = @ten, DVT = @dvt, donGia = @donGia, ngayBatDau = @ngayBatDau,
                                               thu1 = @t1, thu2 = @t2, thu3 = @t3, thu4 = @t4, thu5 = @t5, thu6 = @t6, thu7 = @t7,
                                               soLanPHtrongTuan = @tanSuat, sogoc = @soGoc WHERE maBao = @maBao
                            END
                            ELSE
                            BEGIN
                                INSERT INTO tabBAO (maBao, ten, DVT, donGia, ngayBatDau, thu1, thu2, thu3, thu4, thu5, thu6, thu7, soLanPHtrongTuan, sogoc)
                                VALUES (@maBao, @ten, @dvt, @donGia, @ngayBatDau, @t1, @t2, @t3, @t4, @t5, @t6, @t7, @tanSuat, @soGoc)
                            END";

                        using (SqlCommand cmdBao = new SqlCommand(sqlBao, conn, trans))
                        {
                            cmdBao.Parameters.AddWithValue("@maBao", maBao);
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

                        if (dtNgoaiLeTam != null && dtNgoaiLeTam.Rows.Count > 0)
                        {
                            string sqlNgoaiLe = @"
                                IF EXISTS (SELECT 1 FROM tabBao_ngoaiLe WHERE maBao = @maBao AND ngayPhatHanh = @ngay)
                                BEGIN
                                    UPDATE tabBao_ngoaiLe SET soLanTrongNam = @soLan WHERE maBao = @maBao AND ngayPhatHanh = @ngay
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO tabBao_ngoaiLe (maBao, ngayPhatHanh, soLanTrongNam) VALUES (@maBao, @ngay, @soLan)
                                END";

                            foreach (DataRow row in dtNgoaiLeTam.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted) continue;

                                using (SqlCommand cmdNgoaiLe = new SqlCommand(sqlNgoaiLe, conn, trans))
                                {
                                    cmdNgoaiLe.Parameters.AddWithValue("@maBao", maBao);
                                    cmdNgoaiLe.Parameters.AddWithValue("@ngay", Convert.ToDateTime(row["ngayPhatHanh"]));
                                    cmdNgoaiLe.Parameters.AddWithValue("@soLan", Convert.ToInt32(row["soLanTrongNam"]));
                                    cmdNgoaiLe.ExecuteNonQuery();
                                }
                            }
                        }

                        trans.Commit();
                        MessageBox.Show("Lưu thành công thông tin báo và cấu hình ngoại lệ.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
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