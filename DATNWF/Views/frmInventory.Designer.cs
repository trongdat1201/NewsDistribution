namespace DATNWF.Views
{
    partial class frmInventory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInventory));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeaderPublications = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.dboTabBao = new Guna.UI2.WinForms.Guna2DataGridView();
            this.thanhnienDataSet5 = new DATNWF.ThanhnienDataSet5();
            this.tabTonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabTonTableAdapter = new DATNWF.ThanhnienDataSet5TableAdapters.tabTonTableAdapter();
            this.ngayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tenBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slPhatHanhDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.banthucDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.banLeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dieuPhoiDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlHeaderPublications.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dboTabBao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabTonBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeaderPublications
            // 
            this.pnlHeaderPublications.BackColor = System.Drawing.Color.White;
            this.pnlHeaderPublications.Controls.Add(this.btnAddNew);
            this.pnlHeaderPublications.Controls.Add(this.txtSearch);
            this.pnlHeaderPublications.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaderPublications.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaderPublications.Name = "pnlHeaderPublications";
            this.pnlHeaderPublications.Size = new System.Drawing.Size(1366, 100);
            this.pnlHeaderPublications.TabIndex = 2;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddNew.BorderRadius = 17;
            this.btnAddNew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddNew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddNew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddNew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddNew.FillColor = System.Drawing.Color.Transparent;
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.ForeColor = System.Drawing.Color.Black;
            this.btnAddNew.HoverState.FillColor = System.Drawing.Color.Black;
            this.btnAddNew.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.ImageSize = new System.Drawing.Size(30, 30);
            this.btnAddNew.Location = new System.Drawing.Point(1148, 12);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(206, 72);
            this.btnAddNew.TabIndex = 5;
            this.btnAddNew.Text = "Add New";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.Transparent;
            this.txtSearch.BorderRadius = 20;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.IconLeft = ((System.Drawing.Image)(resources.GetObject("txtSearch.IconLeft")));
            this.txtSearch.IconLeftOffset = new System.Drawing.Point(10, 0);
            this.txtSearch.Location = new System.Drawing.Point(24, 25);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtSearch.PlaceholderText = "\"Search dispatches, customers...\".";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(331, 48);
            this.txtSearch.TabIndex = 4;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.dboTabBao);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 100);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Padding = new System.Windows.Forms.Padding(10);
            this.guna2Panel1.Size = new System.Drawing.Size(1366, 668);
            this.guna2Panel1.TabIndex = 3;
            // 
            // dboTabBao
            // 
            this.dboTabBao.AllowUserToAddRows = false;
            this.dboTabBao.AllowUserToDeleteRows = false;
            this.dboTabBao.AllowUserToResizeColumns = false;
            this.dboTabBao.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dboTabBao.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dboTabBao.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTabBao.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dboTabBao.ColumnHeadersHeight = 71;
            this.dboTabBao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dboTabBao.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ngayDataGridViewTextBoxColumn,
            this.maBaoDataGridViewTextBoxColumn,
            this.tenBaoDataGridViewTextBoxColumn,
            this.soBaoDataGridViewTextBoxColumn,
            this.slPhatHanhDataGridViewTextBoxColumn,
            this.banthucDataGridViewTextBoxColumn,
            this.banLeDataGridViewTextBoxColumn,
            this.dieuPhoiDataGridViewTextBoxColumn,
            this.tonDataGridViewTextBoxColumn});
            this.dboTabBao.DataSource = this.tabTonBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dboTabBao.DefaultCellStyle = dataGridViewCellStyle3;
            this.dboTabBao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dboTabBao.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabBao.Location = new System.Drawing.Point(10, 10);
            this.dboTabBao.Name = "dboTabBao";
            this.dboTabBao.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTabBao.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dboTabBao.RowHeadersVisible = false;
            this.dboTabBao.RowHeadersWidth = 51;
            this.dboTabBao.RowTemplate.Height = 24;
            this.dboTabBao.Size = new System.Drawing.Size(1346, 648);
            this.dboTabBao.TabIndex = 2;
            this.dboTabBao.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dboTabBao.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dboTabBao.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dboTabBao.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dboTabBao.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dboTabBao.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dboTabBao.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabBao.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dboTabBao.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dboTabBao.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dboTabBao.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dboTabBao.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dboTabBao.ThemeStyle.HeaderStyle.Height = 71;
            this.dboTabBao.ThemeStyle.ReadOnly = true;
            this.dboTabBao.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dboTabBao.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dboTabBao.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dboTabBao.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dboTabBao.ThemeStyle.RowsStyle.Height = 24;
            this.dboTabBao.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabBao.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // thanhnienDataSet5
            // 
            this.thanhnienDataSet5.DataSetName = "ThanhnienDataSet5";
            this.thanhnienDataSet5.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tabTonBindingSource
            // 
            this.tabTonBindingSource.DataMember = "tabTon";
            this.tabTonBindingSource.DataSource = this.thanhnienDataSet5;
            // 
            // tabTonTableAdapter
            // 
            this.tabTonTableAdapter.ClearBeforeFill = true;
            // 
            // ngayDataGridViewTextBoxColumn
            // 
            this.ngayDataGridViewTextBoxColumn.DataPropertyName = "ngay";
            this.ngayDataGridViewTextBoxColumn.HeaderText = "Ngày";
            this.ngayDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ngayDataGridViewTextBoxColumn.Name = "ngayDataGridViewTextBoxColumn";
            this.ngayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maBaoDataGridViewTextBoxColumn
            // 
            this.maBaoDataGridViewTextBoxColumn.DataPropertyName = "maBao";
            this.maBaoDataGridViewTextBoxColumn.HeaderText = "Mã báo";
            this.maBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.maBaoDataGridViewTextBoxColumn.Name = "maBaoDataGridViewTextBoxColumn";
            this.maBaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tenBaoDataGridViewTextBoxColumn
            // 
            this.tenBaoDataGridViewTextBoxColumn.DataPropertyName = "tenBao";
            this.tenBaoDataGridViewTextBoxColumn.HeaderText = "Tên báo";
            this.tenBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tenBaoDataGridViewTextBoxColumn.Name = "tenBaoDataGridViewTextBoxColumn";
            this.tenBaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // soBaoDataGridViewTextBoxColumn
            // 
            this.soBaoDataGridViewTextBoxColumn.DataPropertyName = "soBao";
            this.soBaoDataGridViewTextBoxColumn.HeaderText = "Số báo";
            this.soBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soBaoDataGridViewTextBoxColumn.Name = "soBaoDataGridViewTextBoxColumn";
            this.soBaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // slPhatHanhDataGridViewTextBoxColumn
            // 
            this.slPhatHanhDataGridViewTextBoxColumn.DataPropertyName = "slPhatHanh";
            this.slPhatHanhDataGridViewTextBoxColumn.HeaderText = "Số lượng phát hành";
            this.slPhatHanhDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.slPhatHanhDataGridViewTextBoxColumn.Name = "slPhatHanhDataGridViewTextBoxColumn";
            this.slPhatHanhDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // banthucDataGridViewTextBoxColumn
            // 
            this.banthucDataGridViewTextBoxColumn.DataPropertyName = "banthuc";
            this.banthucDataGridViewTextBoxColumn.HeaderText = "Bán thực";
            this.banthucDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.banthucDataGridViewTextBoxColumn.Name = "banthucDataGridViewTextBoxColumn";
            this.banthucDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // banLeDataGridViewTextBoxColumn
            // 
            this.banLeDataGridViewTextBoxColumn.DataPropertyName = "banLe";
            this.banLeDataGridViewTextBoxColumn.HeaderText = "Bán lẻ";
            this.banLeDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.banLeDataGridViewTextBoxColumn.Name = "banLeDataGridViewTextBoxColumn";
            this.banLeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dieuPhoiDataGridViewTextBoxColumn
            // 
            this.dieuPhoiDataGridViewTextBoxColumn.DataPropertyName = "dieuPhoi";
            this.dieuPhoiDataGridViewTextBoxColumn.HeaderText = "Điều phối";
            this.dieuPhoiDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.dieuPhoiDataGridViewTextBoxColumn.Name = "dieuPhoiDataGridViewTextBoxColumn";
            this.dieuPhoiDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tonDataGridViewTextBoxColumn
            // 
            this.tonDataGridViewTextBoxColumn.DataPropertyName = "ton";
            this.tonDataGridViewTextBoxColumn.HeaderText = "Tồn";
            this.tonDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tonDataGridViewTextBoxColumn.Name = "tonDataGridViewTextBoxColumn";
            this.tonDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.pnlHeaderPublications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInventory";
            this.Text = "frmInventory";
            this.Load += new System.EventHandler(this.frmInventory_Load);
            this.pnlHeaderPublications.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dboTabBao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabTonBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeaderPublications;
        private Guna.UI2.WinForms.Guna2Button btnAddNew;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dboTabBao;
        private ThanhnienDataSet5 thanhnienDataSet5;
        private System.Windows.Forms.BindingSource tabTonBindingSource;
        private ThanhnienDataSet5TableAdapters.tabTonTableAdapter tabTonTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tenBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slPhatHanhDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn banthucDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn banLeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dieuPhoiDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tonDataGridViewTextBoxColumn;
    }
}