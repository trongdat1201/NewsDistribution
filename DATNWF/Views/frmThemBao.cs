using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmThemBao : Form
    {
        private DataTable dtNgoaiLeTam;
        private bool isEditingException = false;
        private DateTime oldNgayPhatHanh;

        public frmThemBao()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.Load += frmThemBao_Load;

            // Đăng ký sự kiện đếm số lần phát hành tự động
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

            btnCreateBao.Enabled = isCreatingMaster;

            Color colorMaster = isCreatingMaster ? Color.White : Color.WhiteSmoke;
            txtMaBao.BackColor = colorMaster;
            txtTenBao.BackColor = colorMaster;
            cbDvt.BackColor = colorMaster;
            txtDonGia.BackColor = colorMaster;
            txtSoGoc.BackColor = colorMaster;
            txtTanSuat.BackColor = colorMaster;


            bool isExceptionActive = !isCreatingMaster;

            txtMaBaoNgoaiLe.Enabled = false;
            ngayPhatHanh.Enabled = isExceptionActive;

            txtSoLanPhatHanhTrongNam.Enabled = false;
            txtSoLanPhatHanhTrongNam.BackColor = Color.WhiteSmoke;
            txtSoLanPhatHanhTrongNam.Text = "1";

            btnAddBaoNLe.Enabled = isExceptionActive;

            if (this.Controls.ContainsKey("btnDeleteBaoNle"))
            {
                Control[] btns = this.Controls.Find("btnDeleteBaoNle", true);
                if (btns.Length > 0) btns[0].Enabled = isExceptionActive;
            }

            dgvNgoaiLeTam.Enabled = isExceptionActive;

            imgSave.Enabled = true;
            imgRefresh.Enabled = true;
            imgCancel.Enabled = true;

            this.ResumeLayout();
        }

        private void ResetToInitState()
        {
            txtMaBao.Clear();
            txtTenBao.Clear();
            cbDvt.SelectedIndex = -1;
            cbDvt.Text = "";
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
            if (string.IsNullOrWhiteSpace(txtMaBao.Text) || string.IsNullOrWhiteSpace(txtTenBao.Text) || string.IsNullOrWhiteSpace(cbDvt.Text))
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

            try
            {
                var listBao = ApiClient.Instance.GetAsync<List<BaoDto>>("Publications").GetAwaiter().GetResult();
                bool exists = listBao.Any(b => b.MaBao.Equals(txtMaBao.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    MessageBox.Show("Mã báo này đã tồn tại trong hệ thống!\nVui lòng nhập một Mã báo khác.", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaBao.Focus();
                    txtMaBao.SelectAll();
                    return; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối API để kiểm tra Mã báo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSoGoc.Text))
            {
                txtSoGoc.Text = "1";
            }
            if (!int.TryParse(txtSoGoc.Text, out int soGocParsed) || soGocParsed < 0)
            {
                MessageBox.Show("Số gốc phải là số nguyên hợp lệ và không âm!", "Ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoGoc.Focus();
                return;
            }

            txtMaBaoNgoaiLe.Text = txtMaBao.Text.Trim();

            SetUIState(false);
        }

        private void ClearExceptionInputs()
        {
            ngayPhatHanh.Value = DateTime.Now;
            txtSoLanPhatHanhTrongNam.Text = "1"; 
            txtSoLanPhatHanhTrongNam.Enabled = false; 
            isEditingException = false;
            if (dgvNgoaiLeTam != null) dgvNgoaiLeTam.ClearSelection();
        }

        private void btnAddBaoNLe_Click(object sender, EventArgs e)
        {
            int soLan = 1;
            DateTime ngayPH = ngayPhatHanh.Value.Date;

            if (isEditingException)
            {
                foreach (DataRow row in dtNgoaiLeTam.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (Convert.ToDateTime(row["ngayPhatHanh"]).Date == oldNgayPhatHanh)
                    {
                        if (ngayPH != oldNgayPhatHanh)
                        {
                            foreach (DataRow rCheck in dtNgoaiLeTam.Rows)
                            {
                                if (rCheck.RowState == DataRowState.Deleted) continue;
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
                    if (row.RowState == DataRowState.Deleted) continue;
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
                    txtSoLanPhatHanhTrongNam.Text = "1"; 

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
            if (txtMaBao.Enabled)
            {
                MessageBox.Show("Vui lòng hoàn thiện thông tin đầu báo và nhấn nút [Tạo Báo] trước khi Hoàn thành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maBao = txtMaBao.Text.Trim();
            double donGia = double.Parse(txtDonGia.Text);
            int soGocParsed = int.Parse(txtSoGoc.Text);

            var model = new {
                MaBao = maBao,
                Ten = txtTenBao.Text.Trim(),
                Dvt = cbDvt.Text.Trim(),
                DonGia = donGia,
                SoLanPhtrongTuan = int.Parse(txtTanSuat.Text),
                Sogoc = soGocParsed,
                NgayBatDau = dtpNgayBatDau.Value.Date,
                Thu1 = chkChuNhat.Checked,
                Thu2 = chkThu2.Checked,
                Thu3 = chkThu3.Checked,
                Thu4 = chkThu4.Checked,
                Thu5 = chkThu5.Checked,
                Thu6 = chkThu6.Checked,
                Thu7 = chkThu7.Checked,
                NgoaiLeList = new List<object>()
            };

            if (dtNgoaiLeTam != null && dtNgoaiLeTam.Rows.Count > 0)
            {
                foreach (DataRow row in dtNgoaiLeTam.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    model.NgoaiLeList.Add(new {
                        NgayPhatHanh = Convert.ToDateTime(row["ngayPhatHanh"]).Date,
                        SoLanTrongNam = 1
                    });
                }
            }

            try
            {
                bool success = ApiClient.Instance.PostAsync("Publications", model).GetAwaiter().GetResult();
                if (success)
                {
                    MessageBox.Show("Lưu thành công thông tin báo và cấu hình ngoại lệ.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu qua API: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}