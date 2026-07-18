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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeaderPublications = new Guna.UI2.WinForms.Guna2Panel();
            this.btnEdit = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlDesktop = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grbThongKeTonKhoHomNay = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvChiTietHomNay = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dboTon = new Guna.UI2.WinForms.Guna2DataGridView();
            this.ngayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tenBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slPhatHanhDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.banthucDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dieuPhoiDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabTonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.thanhnienDataSet5 = new DATNWF.ThanhnienDataSet5();
            this.tabTonTableAdapter = new DATNWF.ThanhnienDataSet5TableAdapters.tabTonTableAdapter();
            this.pnlHeaderPublications.SuspendLayout();
            this.pnlDesktop.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grbThongKeTonKhoHomNay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHomNay)).BeginInit();
            this.guna2GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dboTon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabTonBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet5)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeaderPublications
            // 
            this.pnlHeaderPublications.BackColor = System.Drawing.Color.White;
            this.pnlHeaderPublications.Controls.Add(this.btnEdit);
            this.pnlHeaderPublications.Controls.Add(this.guna2Button1);
            this.pnlHeaderPublications.Controls.Add(this.txtSearch);
            this.pnlHeaderPublications.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaderPublications.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaderPublications.Name = "pnlHeaderPublications";
            this.pnlHeaderPublications.Size = new System.Drawing.Size(1366, 100);
            this.pnlHeaderPublications.TabIndex = 2;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Animated = true;
            this.btnEdit.BorderRadius = 17;
            this.btnEdit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEdit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEdit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnEdit.HoverState.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageSize = new System.Drawing.Size(30, 30);
            this.btnEdit.Location = new System.Drawing.Point(938, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(206, 72);
            this.btnEdit.TabIndex = 12;
            this.btnEdit.Text = "Chi tiết";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button1.Animated = true;
            this.guna2Button1.BorderRadius = 17;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button1.ForeColor = System.Drawing.Color.Black;
            this.guna2Button1.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.guna2Button1.HoverState.ForeColor = System.Drawing.Color.Black;
            this.guna2Button1.Image = ((System.Drawing.Image)(resources.GetObject("guna2Button1.Image")));
            this.guna2Button1.ImageSize = new System.Drawing.Size(30, 30);
            this.guna2Button1.Location = new System.Drawing.Point(1150, 12);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(206, 72);
            this.guna2Button1.TabIndex = 11;
            this.guna2Button1.Text = "Thống kê";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
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
            // pnlDesktop
            // 
            this.pnlDesktop.Controls.Add(this.tableLayoutPanel1);
            this.pnlDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDesktop.Location = new System.Drawing.Point(0, 100);
            this.pnlDesktop.Name = "pnlDesktop";
            this.pnlDesktop.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.pnlDesktop.Size = new System.Drawing.Size(1366, 668);
            this.pnlDesktop.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.grbThongKeTonKhoHomNay, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.guna2GroupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.41358F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65.58642F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1356, 648);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grbThongKeTonKhoHomNay
            // 
            this.grbThongKeTonKhoHomNay.Controls.Add(this.dgvChiTietHomNay);
            this.grbThongKeTonKhoHomNay.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.grbThongKeTonKhoHomNay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbThongKeTonKhoHomNay.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbThongKeTonKhoHomNay.ForeColor = System.Drawing.Color.Black;
            this.grbThongKeTonKhoHomNay.Location = new System.Drawing.Point(3, 3);
            this.grbThongKeTonKhoHomNay.Name = "grbThongKeTonKhoHomNay";
            this.grbThongKeTonKhoHomNay.Size = new System.Drawing.Size(1350, 217);
            this.grbThongKeTonKhoHomNay.TabIndex = 2;
            this.grbThongKeTonKhoHomNay.Text = "Thống kê tồn kho hôm nay ";
            // 
            // dgvChiTietHomNay
            // 
            this.dgvChiTietHomNay.AllowUserToAddRows = false;
            this.dgvChiTietHomNay.AllowUserToDeleteRows = false;
            this.dgvChiTietHomNay.AllowUserToResizeColumns = false;
            this.dgvChiTietHomNay.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChiTietHomNay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvChiTietHomNay.ColumnHeadersHeight = 55;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiTietHomNay.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvChiTietHomNay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTietHomNay.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvChiTietHomNay.Location = new System.Drawing.Point(0, 40);
            this.dgvChiTietHomNay.Name = "dgvChiTietHomNay";
            this.dgvChiTietHomNay.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChiTietHomNay.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvChiTietHomNay.RowHeadersVisible = false;
            this.dgvChiTietHomNay.RowHeadersWidth = 51;
            this.dgvChiTietHomNay.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvChiTietHomNay.RowTemplate.Height = 24;
            this.dgvChiTietHomNay.Size = new System.Drawing.Size(1350, 177);
            this.dgvChiTietHomNay.TabIndex = 0;
            this.dgvChiTietHomNay.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiTietHomNay.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvChiTietHomNay.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvChiTietHomNay.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvChiTietHomNay.ThemeStyle.HeaderStyle.Height = 55;
            this.dgvChiTietHomNay.ThemeStyle.ReadOnly = true;
            this.dgvChiTietHomNay.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvChiTietHomNay.ThemeStyle.RowsStyle.Height = 24;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.Controls.Add(this.dboTon);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox1.Location = new System.Drawing.Point(3, 233);
            this.guna2GroupBox1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(1350, 412);
            this.guna2GroupBox1.TabIndex = 1;
            this.guna2GroupBox1.Text = "Thông kê tồn kho ";
            // 
            // dboTon
            // 
            this.dboTon.AllowUserToAddRows = false;
            this.dboTon.AllowUserToDeleteRows = false;
            this.dboTon.AllowUserToResizeColumns = false;
            this.dboTon.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dboTon.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dboTon.AutoGenerateColumns = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dboTon.ColumnHeadersHeight = 71;
            this.dboTon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ngayDataGridViewTextBoxColumn,
            this.maBaoDataGridViewTextBoxColumn,
            this.tenBaoDataGridViewTextBoxColumn,
            this.soBaoDataGridViewTextBoxColumn,
            this.slPhatHanhDataGridViewTextBoxColumn,
            this.banthucDataGridViewTextBoxColumn,
            this.dieuPhoiDataGridViewTextBoxColumn,
            this.tonDataGridViewTextBoxColumn});
            this.dboTon.DataSource = this.tabTonBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dboTon.DefaultCellStyle = dataGridViewCellStyle6;
            this.dboTon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dboTon.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTon.Location = new System.Drawing.Point(0, 40);
            this.dboTon.Name = "dboTon";
            this.dboTon.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTon.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dboTon.RowHeadersVisible = false;
            this.dboTon.RowHeadersWidth = 51;
            this.dboTon.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dboTon.RowTemplate.Height = 24;
            this.dboTon.Size = new System.Drawing.Size(1350, 372);
            this.dboTon.TabIndex = 3;
            this.dboTon.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dboTon.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dboTon.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dboTon.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dboTon.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dboTon.ThemeStyle.HeaderStyle.Height = 71;
            this.dboTon.ThemeStyle.ReadOnly = true;
            this.dboTon.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dboTon.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dboTon.ThemeStyle.RowsStyle.Height = 24;
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
            // tabTonBindingSource
            // 
            this.tabTonBindingSource.DataMember = "tabTon";
            this.tabTonBindingSource.DataSource = this.thanhnienDataSet5;
            // 
            // thanhnienDataSet5
            // 
            this.thanhnienDataSet5.DataSetName = "ThanhnienDataSet5";
            this.thanhnienDataSet5.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tabTonTableAdapter
            // 
            this.tabTonTableAdapter.ClearBeforeFill = true;
            // 
            // frmInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.pnlDesktop);
            this.Controls.Add(this.pnlHeaderPublications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInventory";
            this.Text = "frmInventory";
            this.Load += new System.EventHandler(this.frmInventory_Load);
            this.pnlHeaderPublications.ResumeLayout(false);
            this.pnlDesktop.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grbThongKeTonKhoHomNay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHomNay)).EndInit();
            this.guna2GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dboTon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabTonBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeaderPublications;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Panel pnlDesktop;
        private ThanhnienDataSet5 thanhnienDataSet5;
        private System.Windows.Forms.BindingSource tabTonBindingSource;
        private ThanhnienDataSet5TableAdapters.tabTonTableAdapter tabTonTableAdapter;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2GroupBox grbThongKeTonKhoHomNay;
        private Guna.UI2.WinForms.Guna2DataGridView dgvChiTietHomNay;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2DataGridView dboTon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tenBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slPhatHanhDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn banthucDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dieuPhoiDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tonDataGridViewTextBoxColumn;
    }
}