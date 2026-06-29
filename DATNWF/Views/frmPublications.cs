using DATNWF.Views;
using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF
{
    public partial class frmPublications : Form
    {
        private List<BaoDto> _listBao = new List<BaoDto>();
        private List<BaoNgoaiLeDto> _listNgoaiLe = new List<BaoNgoaiLeDto>();

        public frmPublications()
        {
            InitializeComponent();
            this.dboTabBao.SelectionChanged += new System.EventHandler(this.dboTabBao_SelectionChanged);
        }

        private void LoadData()
        {
            try
            {
                _listBao = ApiClient.Instance.GetAsync<List<BaoDto>>("Publications").GetAwaiter().GetResult();
                _listNgoaiLe = ApiClient.Instance.GetAsync<List<BaoNgoaiLeDto>>("Publications/NgoaiLe").GetAwaiter().GetResult();

                tabBAOBindingSource.DataSource = _listBao;
                dboTabBao.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối API tải danh sách báo: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmPublications_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadBaoHomNay();
            LoadTopBaoDoanhThu();
        }

        private void dboTabBao_SelectionChanged(object sender, EventArgs e)
        {
            if (dboTabBao.SelectedRows.Count > 0 && dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value != null)
            {
                string maBao = dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value.ToString();
                var filtered = _listNgoaiLe.Where(n => n.MaBao == maBao).ToList();
                tabBaongoaiLeBindingSource.DataSource = filtered;
            }
            else
            {
                tabBaongoaiLeBindingSource.DataSource = null;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                tabBAOBindingSource.DataSource = _listBao;
            }
            else
            {
                var filtered = _listBao.Where(b => b.MaBao.ToLower().Contains(keyword) || b.Ten.ToLower().Contains(keyword)).ToList();
                tabBAOBindingSource.DataSource = filtered;
            }
            dboTabBao.ClearSelection();
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
            if (dboTabBao.SelectedRows.Count == 0 || dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value == null)
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
            if (dboTabBao.SelectedRows.Count == 0 || dboTabBao.SelectedRows[0].Cells["maBaoDataGridViewTextBoxColumn"].Value == null)
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
                try
                {
                    bool ok = ApiClient.Instance.DeleteAsync($"Publications/{maBao}").GetAwaiter().GetResult();
                    if (ok)
                    {
                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        LoadBaoHomNay();
                        LoadTopBaoDoanhThu();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa báo này!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private class BaoHomNayResponse
        {
            public string MaBao { get; set; }
            public string Ten { get; set; }
        }

        private void LoadBaoHomNay()
        {
            try
            {
                var result = ApiClient.Instance.GetAsync<List<BaoHomNayResponse>>("Publications/BaoHomNay").GetAwaiter().GetResult();
                dgvBaoHomNay.DataSource = result;

                if (dgvBaoHomNay.Columns.Count > 0)
                {
                    if (dgvBaoHomNay.Columns["MaBao"] != null) dgvBaoHomNay.Columns["MaBao"].Visible = false;
                    if (dgvBaoHomNay.Columns["Ten"] != null)
                    {
                        dgvBaoHomNay.Columns["Ten"].HeaderText = "Tên báo";
                        dgvBaoHomNay.Columns["Ten"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
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
                MessageBox.Show("Lỗi tải danh sách báo hôm nay: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class TopRevenueResponse
        {
            public string TenBao { get; set; }
            public double TongDoanhThu { get; set; }
        }

        private void LoadTopBaoDoanhThu()
        {
            try
            {
                var topData = ApiClient.Instance.GetAsync<List<TopRevenueResponse>>("Publications/top-revenue").GetAwaiter().GetResult();
                
                var ds = new GunaDoughnutDataset { Label = "Doanh thu" };
                ds.FillColors.AddRange(new[]
                {
                    Color.FromArgb(100, 88, 255),
                    Color.FromArgb(255, 192, 128),
                    Color.FromArgb(76, 175, 80),
                    Color.FromArgb(244, 67, 54),
                    Color.FromArgb(255, 152, 0),
                    Color.FromArgb(0, 188, 212),
                    Color.FromArgb(156, 39, 176),
                    Color.FromArgb(255, 235, 59),
                    Color.FromArgb(103, 58, 183),
                    Color.FromArgb(0, 150, 136)
                });

                if (topData != null)
                {
                    foreach (var item in topData)
                    {
                        ds.DataPoints.Add(item.TenBao, item.TongDoanhThu);
                    }
                }

                barChart.Datasets.Clear();
                barChart.Datasets.Add(ds);
                barChart.Legend.Position = LegendPosition.Right;
                barChart.Legend.Display = true;
                barChart.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải doanh thu báo từ API: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}