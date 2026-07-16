using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmTimKhachHang : Form
    {
        public string MaKH_Selected { get; private set; }
        public string TenKH_Selected { get; private set; }
        private List<KhachHangDto> _allKhachHang = new List<KhachHangDto>();

        public frmTimKhachHang()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                _allKhachHang = ApiClient.Instance.GetAsync<List<KhachHangDto>>("Customers").GetAwaiter().GetResult();
                tabKHACHHANGBindingSource.DataSource = _allKhachHang;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khách hàng từ API: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmTimKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                tabKHACHHANGBindingSource.DataSource = _allKhachHang;
            }
            else
            {
                var filtered = _allKhachHang.Where(k => 
                    k.Ten.ToLower().Contains(tuKhoa) || 
                    k.MaKH.ToLower().Contains(tuKhoa) || 
                    (k.DienThoai != null && k.DienThoai.Contains(tuKhoa))
                ).ToList();
                tabKHACHHANGBindingSource.DataSource = filtered;
            }
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var kh = dgvKhachHang.Rows[e.RowIndex].DataBoundItem as KhachHangDto;
                if (kh != null)
                {
                    MaKH_Selected = kh.MaKH;
                    TenKH_Selected = kh.Ten;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow != null && dgvKhachHang.CurrentRow.Index >= 0)
            {
                var kh = dgvKhachHang.CurrentRow.DataBoundItem as KhachHangDto;
                if (kh != null)
                {
                    MaKH_Selected = kh.MaKH;
                    TenKH_Selected = kh.Ten;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng từ danh sách trước khi lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
