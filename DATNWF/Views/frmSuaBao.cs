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
    public partial class frmSuaBao : Form
    {
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
            try
            {
                var bao = ApiClient.Instance.GetAsync<BaoDto>($"Publications/{maBaoCanSua}").GetAwaiter().GetResult();
                if (bao != null)
                {
                    txtMaBao.Text = bao.MaBao;
                    txtTenBao.Text = bao.Ten;
                    cbDvt.Text = bao.Dvt;
                    txtDonGia.Text = bao.DonGia.ToString();
                    txtSoGoc.Text = bao.Sogoc?.ToString() ?? "";
                    if (bao.NgayBatDau.HasValue)
                        dtpNgayBatDau.Value = bao.NgayBatDau.Value;

                    chkChuNhat.Checked = bao.Thu1 ?? false;
                    chkThu2.Checked = bao.Thu2 ?? false;
                    chkThu3.Checked = bao.Thu3 ?? false;
                    chkThu4.Checked = bao.Thu4 ?? false;
                    chkThu5.Checked = bao.Thu5 ?? false;
                    chkThu6.Checked = bao.Thu6 ?? false;
                    chkThu7.Checked = bao.Thu7 ?? false;
                }

                txtMaBaoNgoaiLe.Text = txtMaBao.Text.Trim();

                dtNgoaiLeTam.Clear();
                var ngoaiLeAll = ApiClient.Instance.GetAsync<List<BaoNgoaiLeDto>>("Publications/NgoaiLe").GetAwaiter().GetResult();
                var filtered = ngoaiLeAll.Where(n => n.MaBao == maBaoCanSua).OrderBy(n => n.NgayPhatHanh).ToList();

                foreach (var nl in filtered)
                {
                    DataRow row = dtNgoaiLeTam.NewRow();
                    row["ngayPhatHanh"] = nl.NgayPhatHanh;
                    row["soLanTrongNam"] = nl.SoLanTrongNam ?? 1;
                    dtNgoaiLeTam.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin báo: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


            Color colorMaster = isEditingMaster ? Color.White : Color.WhiteSmoke;
            txtMaBao.BackColor = Color.WhiteSmoke; 
            txtTanSuat.BackColor = Color.WhiteSmoke; 
            txtTenBao.BackColor = colorMaster;
            cbDvt.BackColor = colorMaster;
            txtDonGia.BackColor = colorMaster;
            txtSoGoc.BackColor = colorMaster;

            bool isExceptionActive = !isEditingMaster;
            txtMaBaoNgoaiLe.Enabled = false;
            ngayPhatHanh.Enabled = isExceptionActive;

            txtSoLanPhatHanhTrongNam.Enabled = false;
            txtSoLanPhatHanhTrongNam.BackColor = Color.WhiteSmoke;
            txtSoLanPhatHanhTrongNam.Text = "1";

            btnAddBaoNLe.Enabled = isExceptionActive;
            btnDeleteBaoNle.Enabled = isExceptionActive;
            dgvNgoaiLeTam.Enabled = isExceptionActive;

            imgSave.Enabled = true;
            imgRestore.Enabled = true;
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
            if (string.IsNullOrWhiteSpace(txtTenBao.Text) || string.IsNullOrWhiteSpace(cbDvt.Text))
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

            if (string.IsNullOrWhiteSpace(txtSoGoc.Text))
            {
                txtSoGoc.Text = "1";
            }
            if (!int.TryParse(txtSoGoc.Text, out int soGocParsed) || soGocParsed < 0)
            {
                MessageBox.Show("Số gốc phải là số nguyên hợp lệ và không âm!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    txtSoLanPhatHanhTrongNam.Text = "1";

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

        private void imgRestore_Click(object sender, EventArgs e)
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
            if (txtTenBao.Enabled)
            {
                return;
            }

            double donGia = double.Parse(txtDonGia.Text);
            int soGocParsed = int.Parse(txtSoGoc.Text);

            var model = new {
                MaBao = maBaoCanSua,
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
                NgoaiLeList = new System.Collections.Generic.List<object>()
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