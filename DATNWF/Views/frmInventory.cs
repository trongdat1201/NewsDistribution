using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            try
            {
                var list = ApiClient.Instance.GetAsync<List<TonDto>>("Inventories").GetAwaiter().GetResult();
                tabTonBindingSource.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin tồn kho: " + ex.Message, "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
