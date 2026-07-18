namespace DATNWF.Views
{
    partial class frmInvoices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInvoices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tmrSearch = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.tabHOADONBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabHoaDon = new DATNWF.ThanhnienDataSet4();
            this.tabHOADONTableAdapter = new DATNWF.ThanhnienDataSet4TableAdapters.tabHOADONTableAdapter();
            this.pnlDesktop = new Guna.UI2.WinForms.Guna2Panel();
            this.tloDesktop = new System.Windows.Forms.TableLayoutPanel();
            this.tloHoaDon = new System.Windows.Forms.TableLayoutPanel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.grbHoaDon = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvHoaDonTamThoi = new Guna.UI2.WinForms.Guna2DataGridView();
            this.grbChiTietHoaDon = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvChiDonTamThoiTamThoi = new Guna.UI2.WinForms.Guna2DataGridView();
            this.tabCHITIETHOADONBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabChiTietHD = new DATNWF.ThanhnienDataSet8();
            this.sohdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makhDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngayLapPhieuDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tuNgayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.denNgayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ghichuDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thanhToanDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sohdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngayNhanDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tenBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soLuongThucDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soLuongDuDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donGiaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thanhTienDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dieuPhoiDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabCHITIETHOADONTableAdapter = new DATNWF.ThanhnienDataSet8TableAdapters.tabCHITIETHOADONTableAdapter();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabHOADONBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabHoaDon)).BeginInit();
            this.pnlDesktop.SuspendLayout();
            this.tloDesktop.SuspendLayout();
            this.tloHoaDon.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.grbHoaDon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDonTamThoi)).BeginInit();
            this.grbChiTietHoaDon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiDonTamThoiTamThoi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCHITIETHOADONBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabChiTietHD)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrSearch
            // 
            this.tmrSearch.Interval = 300;
            this.tmrSearch.Tick += new System.EventHandler(this.tmrSearch_Tick);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.btnAddNew);
            this.pnlHeader.Controls.Add(this.txtSearch);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1366, 100);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.Animated = true;
            this.btnAddNew.BorderRadius = 17;
            this.btnAddNew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddNew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddNew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddNew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddNew.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.ForeColor = System.Drawing.Color.Black;
            this.btnAddNew.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnAddNew.HoverState.ForeColor = System.Drawing.Color.Black;
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.ImageSize = new System.Drawing.Size(30, 30);
            this.btnAddNew.Location = new System.Drawing.Point(1147, 12);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(206, 72);
            this.btnAddNew.TabIndex = 12;
            this.btnAddNew.Text = "Lập hóa đơn";
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
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
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // tabHOADONBindingSource
            // 
            this.tabHOADONBindingSource.DataMember = "tabHOADON";
            this.tabHOADONBindingSource.DataSource = this.tabHoaDon;
            // 
            // tabHoaDon
            // 
            this.tabHoaDon.DataSetName = "ThanhnienDataSet4";
            this.tabHoaDon.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tabHOADONTableAdapter
            // 
            this.tabHOADONTableAdapter.ClearBeforeFill = true;
            // 
            // pnlDesktop
            // 
            this.pnlDesktop.Controls.Add(this.tloDesktop);
            this.pnlDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDesktop.Location = new System.Drawing.Point(0, 100);
            this.pnlDesktop.Name = "pnlDesktop";
            this.pnlDesktop.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.pnlDesktop.Size = new System.Drawing.Size(1366, 668);
            this.pnlDesktop.TabIndex = 2;
            // 
            // tloDesktop
            // 
            this.tloDesktop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tloDesktop.ColumnCount = 1;
            this.tloDesktop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tloDesktop.Controls.Add(this.tloHoaDon, 0, 0);
            this.tloDesktop.Controls.Add(this.grbChiTietHoaDon, 0, 1);
            this.tloDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tloDesktop.Location = new System.Drawing.Point(10, 10);
            this.tloDesktop.Name = "tloDesktop";
            this.tloDesktop.RowCount = 2;
            this.tloDesktop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.45679F));
            this.tloDesktop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.54321F));
            this.tloDesktop.Size = new System.Drawing.Size(1356, 648);
            this.tloDesktop.TabIndex = 1;
            // 
            // tloHoaDon
            // 
            this.tloHoaDon.ColumnCount = 1;
            this.tloHoaDon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tloHoaDon.Controls.Add(this.guna2Panel1, 0, 0);
            this.tloHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tloHoaDon.Location = new System.Drawing.Point(3, 3);
            this.tloHoaDon.Name = "tloHoaDon";
            this.tloHoaDon.RowCount = 1;
            this.tloHoaDon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tloHoaDon.Size = new System.Drawing.Size(1350, 308);
            this.tloHoaDon.TabIndex = 5;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.grbHoaDon);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(3, 3);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1344, 302);
            this.guna2Panel1.TabIndex = 2;
            // 
            // grbHoaDon
            // 
            this.grbHoaDon.Controls.Add(this.dgvHoaDonTamThoi);
            this.grbHoaDon.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.grbHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbHoaDon.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbHoaDon.ForeColor = System.Drawing.Color.Black;
            this.grbHoaDon.Location = new System.Drawing.Point(0, 0);
            this.grbHoaDon.Name = "grbHoaDon";
            this.grbHoaDon.Size = new System.Drawing.Size(1344, 302);
            this.grbHoaDon.TabIndex = 1;
            this.grbHoaDon.Text = "Thông tin hóa đơn tạm thời";
            // 
            // dgvHoaDonTamThoi
            // 
            this.dgvHoaDonTamThoi.AllowUserToAddRows = false;
            this.dgvHoaDonTamThoi.AllowUserToDeleteRows = false;
            this.dgvHoaDonTamThoi.AllowUserToResizeColumns = false;
            this.dgvHoaDonTamThoi.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvHoaDonTamThoi.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHoaDonTamThoi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHoaDonTamThoi.ColumnHeadersHeight = 71;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHoaDonTamThoi.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvHoaDonTamThoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHoaDonTamThoi.GridColor = System.Drawing.Color.White;
            this.dgvHoaDonTamThoi.Location = new System.Drawing.Point(0, 40);
            this.dgvHoaDonTamThoi.Name = "dgvHoaDonTamThoi";
            this.dgvHoaDonTamThoi.ReadOnly = true;
            this.dgvHoaDonTamThoi.RowHeadersVisible = false;
            this.dgvHoaDonTamThoi.RowHeadersWidth = 51;
            this.dgvHoaDonTamThoi.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvHoaDonTamThoi.RowTemplate.Height = 24;
            this.dgvHoaDonTamThoi.Size = new System.Drawing.Size(1344, 262);
            this.dgvHoaDonTamThoi.TabIndex = 7;
            this.dgvHoaDonTamThoi.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvHoaDonTamThoi.ThemeStyle.GridColor = System.Drawing.Color.White;
            this.dgvHoaDonTamThoi.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvHoaDonTamThoi.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvHoaDonTamThoi.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvHoaDonTamThoi.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHoaDonTamThoi.ThemeStyle.HeaderStyle.Height = 71;
            this.dgvHoaDonTamThoi.ThemeStyle.ReadOnly = true;
            this.dgvHoaDonTamThoi.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvHoaDonTamThoi.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvHoaDonTamThoi.ThemeStyle.RowsStyle.Height = 24;
            // 
            // grbChiTietHoaDon
            // 
            this.grbChiTietHoaDon.Controls.Add(this.dgvChiDonTamThoiTamThoi);
            this.grbChiTietHoaDon.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.grbChiTietHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbChiTietHoaDon.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbChiTietHoaDon.ForeColor = System.Drawing.Color.Black;
            this.grbChiTietHoaDon.Location = new System.Drawing.Point(3, 317);
            this.grbChiTietHoaDon.Name = "grbChiTietHoaDon";
            this.grbChiTietHoaDon.Size = new System.Drawing.Size(1350, 328);
            this.grbChiTietHoaDon.TabIndex = 6;
            this.grbChiTietHoaDon.Text = "Chi tiết hóa đơn tạm thời";
            // 
            // dgvChiDonTamThoiTamThoi
            // 
            this.dgvChiDonTamThoiTamThoi.AllowUserToAddRows = false;
            this.dgvChiDonTamThoiTamThoi.AllowUserToDeleteRows = false;
            this.dgvChiDonTamThoiTamThoi.AllowUserToResizeColumns = false;
            this.dgvChiDonTamThoiTamThoi.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvChiDonTamThoiTamThoi.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChiDonTamThoiTamThoi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvChiDonTamThoiTamThoi.ColumnHeadersHeight = 71;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiDonTamThoiTamThoi.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvChiDonTamThoiTamThoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiDonTamThoiTamThoi.GridColor = System.Drawing.Color.White;
            this.dgvChiDonTamThoiTamThoi.Location = new System.Drawing.Point(0, 40);
            this.dgvChiDonTamThoiTamThoi.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dgvChiDonTamThoiTamThoi.Name = "dgvChiDonTamThoiTamThoi";
            this.dgvChiDonTamThoiTamThoi.ReadOnly = true;
            this.dgvChiDonTamThoiTamThoi.RowHeadersVisible = false;
            this.dgvChiDonTamThoiTamThoi.RowHeadersWidth = 51;
            this.dgvChiDonTamThoiTamThoi.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvChiDonTamThoiTamThoi.RowTemplate.Height = 24;
            this.dgvChiDonTamThoiTamThoi.Size = new System.Drawing.Size(1350, 288);
            this.dgvChiDonTamThoiTamThoi.TabIndex = 5;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.GridColor = System.Drawing.Color.White;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.HeaderStyle.Height = 71;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.ReadOnly = true;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvChiDonTamThoiTamThoi.ThemeStyle.RowsStyle.Height = 24;
            // 
            // tabCHITIETHOADONBindingSource
            // 
            this.tabCHITIETHOADONBindingSource.DataMember = "tabCHITIETHOADON";
            this.tabCHITIETHOADONBindingSource.DataSource = this.tabChiTietHD;
            // 
            // tabChiTietHD
            // 
            this.tabChiTietHD.DataSetName = "ThanhnienDataSet8";
            this.tabChiTietHD.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sohdDataGridViewTextBoxColumn
            // 
            this.sohdDataGridViewTextBoxColumn.DataPropertyName = "SoHD";
            this.sohdDataGridViewTextBoxColumn.HeaderText = "Số phiếu";
            this.sohdDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sohdDataGridViewTextBoxColumn.Name = "sohdDataGridViewTextBoxColumn";
            this.sohdDataGridViewTextBoxColumn.ReadOnly = true;
            this.sohdDataGridViewTextBoxColumn.Width = 125;
            // 
            // makhDataGridViewTextBoxColumn
            // 
            this.makhDataGridViewTextBoxColumn.DataPropertyName = "MaKH";
            this.makhDataGridViewTextBoxColumn.HeaderText = "Mã KH";
            this.makhDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.makhDataGridViewTextBoxColumn.Name = "makhDataGridViewTextBoxColumn";
            this.makhDataGridViewTextBoxColumn.ReadOnly = true;
            this.makhDataGridViewTextBoxColumn.Width = 125;
            // 
            // ngayLapPhieuDataGridViewTextBoxColumn
            // 
            this.ngayLapPhieuDataGridViewTextBoxColumn.DataPropertyName = "ngay";
            this.ngayLapPhieuDataGridViewTextBoxColumn.HeaderText = "Ngày lập";
            this.ngayLapPhieuDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ngayLapPhieuDataGridViewTextBoxColumn.Name = "ngayLapPhieuDataGridViewTextBoxColumn";
            this.ngayLapPhieuDataGridViewTextBoxColumn.ReadOnly = true;
            this.ngayLapPhieuDataGridViewTextBoxColumn.Width = 125;
            // 
            // tuNgayDataGridViewTextBoxColumn
            // 
            this.tuNgayDataGridViewTextBoxColumn.DataPropertyName = "TuNgay";
            this.tuNgayDataGridViewTextBoxColumn.HeaderText = "Từ ngày";
            this.tuNgayDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tuNgayDataGridViewTextBoxColumn.Name = "tuNgayDataGridViewTextBoxColumn";
            this.tuNgayDataGridViewTextBoxColumn.ReadOnly = true;
            this.tuNgayDataGridViewTextBoxColumn.Width = 125;
            // 
            // denNgayDataGridViewTextBoxColumn
            // 
            this.denNgayDataGridViewTextBoxColumn.DataPropertyName = "DenNgay";
            this.denNgayDataGridViewTextBoxColumn.HeaderText = "Đến ngày";
            this.denNgayDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.denNgayDataGridViewTextBoxColumn.Name = "denNgayDataGridViewTextBoxColumn";
            this.denNgayDataGridViewTextBoxColumn.ReadOnly = true;
            this.denNgayDataGridViewTextBoxColumn.Width = 125;
            // 
            // ghichuDataGridViewTextBoxColumn
            // 
            this.ghichuDataGridViewTextBoxColumn.DataPropertyName = "GhiChu";
            this.ghichuDataGridViewTextBoxColumn.HeaderText = "Ghi chú";
            this.ghichuDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ghichuDataGridViewTextBoxColumn.Name = "ghichuDataGridViewTextBoxColumn";
            this.ghichuDataGridViewTextBoxColumn.ReadOnly = true;
            this.ghichuDataGridViewTextBoxColumn.Width = 125;
            // 
            // thanhToanDataGridViewCheckBoxColumn
            // 
            this.thanhToanDataGridViewCheckBoxColumn.DataPropertyName = "ThanhToan";
            this.thanhToanDataGridViewCheckBoxColumn.HeaderText = "TT";
            this.thanhToanDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.thanhToanDataGridViewCheckBoxColumn.Name = "thanhToanDataGridViewCheckBoxColumn";
            this.thanhToanDataGridViewCheckBoxColumn.ReadOnly = true;
            this.thanhToanDataGridViewCheckBoxColumn.Width = 125;
            // 
            // sohdDataGridViewTextBoxColumn1
            // 
            this.sohdDataGridViewTextBoxColumn1.DataPropertyName = "sohd";
            this.sohdDataGridViewTextBoxColumn1.HeaderText = "Số hóa đơn";
            this.sohdDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.sohdDataGridViewTextBoxColumn1.Name = "sohdDataGridViewTextBoxColumn1";
            this.sohdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.sohdDataGridViewTextBoxColumn1.Width = 125;
            // 
            // ngayNhanDataGridViewTextBoxColumn
            // 
            this.ngayNhanDataGridViewTextBoxColumn.DataPropertyName = "ngayNhan";
            this.ngayNhanDataGridViewTextBoxColumn.HeaderText = "Ngày nhận";
            this.ngayNhanDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ngayNhanDataGridViewTextBoxColumn.Name = "ngayNhanDataGridViewTextBoxColumn";
            this.ngayNhanDataGridViewTextBoxColumn.ReadOnly = true;
            this.ngayNhanDataGridViewTextBoxColumn.Width = 125;
            // 
            // maBaoDataGridViewTextBoxColumn
            // 
            this.maBaoDataGridViewTextBoxColumn.DataPropertyName = "maBao";
            this.maBaoDataGridViewTextBoxColumn.HeaderText = "Mã báo";
            this.maBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.maBaoDataGridViewTextBoxColumn.Name = "maBaoDataGridViewTextBoxColumn";
            this.maBaoDataGridViewTextBoxColumn.ReadOnly = true;
            this.maBaoDataGridViewTextBoxColumn.Width = 125;
            // 
            // tenBaoDataGridViewTextBoxColumn
            // 
            this.tenBaoDataGridViewTextBoxColumn.DataPropertyName = "tenBao";
            this.tenBaoDataGridViewTextBoxColumn.HeaderText = "Tên báo";
            this.tenBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tenBaoDataGridViewTextBoxColumn.Name = "tenBaoDataGridViewTextBoxColumn";
            this.tenBaoDataGridViewTextBoxColumn.ReadOnly = true;
            this.tenBaoDataGridViewTextBoxColumn.Width = 125;
            // 
            // soBaoDataGridViewTextBoxColumn
            // 
            this.soBaoDataGridViewTextBoxColumn.DataPropertyName = "soBao";
            this.soBaoDataGridViewTextBoxColumn.HeaderText = "Số báo";
            this.soBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soBaoDataGridViewTextBoxColumn.Name = "soBaoDataGridViewTextBoxColumn";
            this.soBaoDataGridViewTextBoxColumn.ReadOnly = true;
            this.soBaoDataGridViewTextBoxColumn.Width = 125;
            // 
            // soLuongThucDataGridViewTextBoxColumn
            // 
            this.soLuongThucDataGridViewTextBoxColumn.DataPropertyName = "soLuongThuc";
            this.soLuongThucDataGridViewTextBoxColumn.HeaderText = "Số lượng thực";
            this.soLuongThucDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soLuongThucDataGridViewTextBoxColumn.Name = "soLuongThucDataGridViewTextBoxColumn";
            this.soLuongThucDataGridViewTextBoxColumn.ReadOnly = true;
            this.soLuongThucDataGridViewTextBoxColumn.Width = 125;
            // 
            // soLuongDuDataGridViewTextBoxColumn
            // 
            this.soLuongDuDataGridViewTextBoxColumn.DataPropertyName = "soLuongDu";
            this.soLuongDuDataGridViewTextBoxColumn.HeaderText = "Phát sinh";
            this.soLuongDuDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soLuongDuDataGridViewTextBoxColumn.Name = "soLuongDuDataGridViewTextBoxColumn";
            this.soLuongDuDataGridViewTextBoxColumn.ReadOnly = true;
            this.soLuongDuDataGridViewTextBoxColumn.Width = 125;
            // 
            // donGiaDataGridViewTextBoxColumn
            // 
            this.donGiaDataGridViewTextBoxColumn.DataPropertyName = "donGia";
            this.donGiaDataGridViewTextBoxColumn.HeaderText = "Đơn giá";
            this.donGiaDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.donGiaDataGridViewTextBoxColumn.Name = "donGiaDataGridViewTextBoxColumn";
            this.donGiaDataGridViewTextBoxColumn.ReadOnly = true;
            this.donGiaDataGridViewTextBoxColumn.Width = 125;
            // 
            // thanhTienDataGridViewTextBoxColumn
            // 
            this.thanhTienDataGridViewTextBoxColumn.DataPropertyName = "thanhTien";
            this.thanhTienDataGridViewTextBoxColumn.HeaderText = "Thành tiền";
            this.thanhTienDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.thanhTienDataGridViewTextBoxColumn.Name = "thanhTienDataGridViewTextBoxColumn";
            this.thanhTienDataGridViewTextBoxColumn.ReadOnly = true;
            this.thanhTienDataGridViewTextBoxColumn.Width = 125;
            // 
            // dieuPhoiDataGridViewTextBoxColumn
            // 
            this.dieuPhoiDataGridViewTextBoxColumn.DataPropertyName = "dieuPhoi";
            this.dieuPhoiDataGridViewTextBoxColumn.HeaderText = "Điều phối";
            this.dieuPhoiDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.dieuPhoiDataGridViewTextBoxColumn.Name = "dieuPhoiDataGridViewTextBoxColumn";
            this.dieuPhoiDataGridViewTextBoxColumn.ReadOnly = true;
            this.dieuPhoiDataGridViewTextBoxColumn.Width = 125;
            // 
            // tabCHITIETHOADONTableAdapter
            // 
            this.tabCHITIETHOADONTableAdapter.ClearBeforeFill = true;
            // 
            // frmInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.pnlDesktop);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInvoices";
            this.Text = "frmInvoices";
            this.Activated += new System.EventHandler(this.frmInvoices_Activated);
            this.Load += new System.EventHandler(this.frmInvoices_Load);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabHOADONBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabHoaDon)).EndInit();
            this.pnlDesktop.ResumeLayout(false);
            this.tloDesktop.ResumeLayout(false);
            this.tloHoaDon.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.grbHoaDon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDonTamThoi)).EndInit();
            this.grbChiTietHoaDon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiDonTamThoiTamThoi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCHITIETHOADONBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabChiTietHD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private ThanhnienDataSet4 tabHoaDon;
        private System.Windows.Forms.BindingSource tabHOADONBindingSource;
        private ThanhnienDataSet4TableAdapters.tabHOADONTableAdapter tabHOADONTableAdapter;
        private Guna.UI2.WinForms.Guna2Button btnAddNew;
        private Guna.UI2.WinForms.Guna2Panel pnlDesktop;
        private System.Windows.Forms.TableLayoutPanel tloDesktop;
        private System.Windows.Forms.DataGridViewTextBoxColumn sohdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn makhDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayLapPhieuDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tuNgayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn denNgayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ghichuDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn thanhToanDataGridViewCheckBoxColumn;
        private ThanhnienDataSet8 tabChiTietHD;
        private System.Windows.Forms.BindingSource tabCHITIETHOADONBindingSource;
        private ThanhnienDataSet8TableAdapters.tabCHITIETHOADONTableAdapter tabCHITIETHOADONTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn sohdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayNhanDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tenBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soLuongThucDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soLuongDuDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn donGiaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn thanhTienDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dieuPhoiDataGridViewTextBoxColumn;
        private System.Windows.Forms.Timer tmrSearch;
        private System.Windows.Forms.TableLayoutPanel tloHoaDon;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2GroupBox grbHoaDon;
        private Guna.UI2.WinForms.Guna2DataGridView dgvHoaDonTamThoi;
        private Guna.UI2.WinForms.Guna2GroupBox grbChiTietHoaDon;
        private Guna.UI2.WinForms.Guna2DataGridView dgvChiDonTamThoiTamThoi;
    }
}