using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmTimKhachHang : Form
    {
        public string MaKH_Selected { get; private set; }
        public string TenKH_Selected { get; private set; }
        private DataTable dtKhachHang;
        public frmTimKhachHang()
        {
            InitializeComponent();
        }

        private void frmTimKhachHang_Load(object sender, EventArgs e)
        {
            this.tabKHACHHANGTableAdapter.Fill(this.thanhnienDataSet2.tabKHACHHANG);
            dtKhachHang = this.thanhnienDataSet2.tabKHACHHANG;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtSearch.Text.Trim().Replace("'", "''");

            if (tabKHACHHANGBindingSource != null)
            {
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    tabKHACHHANGBindingSource.Filter = "";
                }
                else
                {
                    tabKHACHHANGBindingSource.Filter = $"TEN LIKE '%{tuKhoa}%' OR MAKH LIKE '%{tuKhoa}%' OR DIENTHOAI LIKE '%{tuKhoa}%'";
                }
            }
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MaKH_Selected = dgvKhachHang.Rows[e.RowIndex].Cells["MAKH"].Value.ToString();
                TenKH_Selected = dgvKhachHang.Rows[e.RowIndex].Cells["TEN"].Value.ToString();

                this.DialogResult = DialogResult.OK;

                this.Close();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.CurrentRow != null && dgvKhachHang.CurrentRow.Index >= 0)
            {
                int rowIndex = dgvKhachHang.CurrentRow.Index;

                MaKH_Selected = dgvKhachHang.Rows[rowIndex].Cells[0].Value.ToString();
                TenKH_Selected = dgvKhachHang.Rows[rowIndex].Cells[1].Value.ToString();
                this.DialogResult = DialogResult.OK;

                this.Close();
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
