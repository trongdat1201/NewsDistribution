namespace DATNWF.Views
{
    partial class frmCustomers
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomers));
            this.pnlHeaderPublications = new Guna.UI2.WinForms.Guna2Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.dboTabKhachHang = new Guna.UI2.WinForms.Guna2DataGridView();
            this.mAKHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tENDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dIACHIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dIENTHOAIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHIETKHAUDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pPHDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pKTDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.uUTIENDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabKHACHHANGBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.thanhnienDataSet3 = new DATNWF.ThanhnienDataSet3();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.guna2CustomGradientPanel2 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.tabKHACHHANGTableAdapter = new DATNWF.ThanhnienDataSet3TableAdapters.tabKHACHHANGTableAdapter();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvKhachHangGanDay = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnDelete = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnEdit = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2Button();
            this.pnlHeaderPublications.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dboTabKhachHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabKHACHHANGBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet3)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.guna2CustomGradientPanel1.SuspendLayout();
            this.guna2CustomGradientPanel2.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHangGanDay)).BeginInit();
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
            this.pnlHeaderPublications.TabIndex = 1;
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
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.15959F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.84041F));
            this.tableLayoutPanel1.Controls.Add(this.guna2Panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.guna2Panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1366, 668);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.dboTabKhachHang);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(3, 3);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Padding = new System.Windows.Forms.Padding(10);
            this.guna2Panel1.Size = new System.Drawing.Size(1048, 662);
            this.guna2Panel1.TabIndex = 0;
            // 
            // dboTabKhachHang
            // 
            this.dboTabKhachHang.AllowUserToAddRows = false;
            this.dboTabKhachHang.AllowUserToDeleteRows = false;
            this.dboTabKhachHang.AllowUserToResizeColumns = false;
            this.dboTabKhachHang.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dboTabKhachHang.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dboTabKhachHang.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTabKhachHang.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dboTabKhachHang.ColumnHeadersHeight = 71;
            this.dboTabKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dboTabKhachHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mAKHDataGridViewTextBoxColumn,
            this.tENDataGridViewTextBoxColumn,
            this.dIACHIDataGridViewTextBoxColumn,
            this.dIENTHOAIDataGridViewTextBoxColumn,
            this.cHIETKHAUDataGridViewTextBoxColumn,
            this.pPHDataGridViewCheckBoxColumn,
            this.pKTDataGridViewCheckBoxColumn,
            this.uUTIENDataGridViewTextBoxColumn});
            this.dboTabKhachHang.DataSource = this.tabKHACHHANGBindingSource;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dboTabKhachHang.DefaultCellStyle = dataGridViewCellStyle4;
            this.dboTabKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dboTabKhachHang.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabKhachHang.Location = new System.Drawing.Point(10, 10);
            this.dboTabKhachHang.Name = "dboTabKhachHang";
            this.dboTabKhachHang.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTabKhachHang.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dboTabKhachHang.RowHeadersVisible = false;
            this.dboTabKhachHang.RowHeadersWidth = 51;
            this.dboTabKhachHang.RowTemplate.Height = 24;
            this.dboTabKhachHang.Size = new System.Drawing.Size(1028, 642);
            this.dboTabKhachHang.TabIndex = 1;
            this.dboTabKhachHang.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dboTabKhachHang.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dboTabKhachHang.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dboTabKhachHang.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dboTabKhachHang.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dboTabKhachHang.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dboTabKhachHang.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabKhachHang.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dboTabKhachHang.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dboTabKhachHang.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dboTabKhachHang.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dboTabKhachHang.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dboTabKhachHang.ThemeStyle.HeaderStyle.Height = 71;
            this.dboTabKhachHang.ThemeStyle.ReadOnly = true;
            this.dboTabKhachHang.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dboTabKhachHang.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dboTabKhachHang.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dboTabKhachHang.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dboTabKhachHang.ThemeStyle.RowsStyle.Height = 24;
            this.dboTabKhachHang.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabKhachHang.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // mAKHDataGridViewTextBoxColumn
            // 
            this.mAKHDataGridViewTextBoxColumn.DataPropertyName = "MAKH";
            this.mAKHDataGridViewTextBoxColumn.HeaderText = "Mã khách hàng";
            this.mAKHDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.mAKHDataGridViewTextBoxColumn.Name = "mAKHDataGridViewTextBoxColumn";
            this.mAKHDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tENDataGridViewTextBoxColumn
            // 
            this.tENDataGridViewTextBoxColumn.DataPropertyName = "TEN";
            this.tENDataGridViewTextBoxColumn.HeaderText = "Tên khách hàng";
            this.tENDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tENDataGridViewTextBoxColumn.Name = "tENDataGridViewTextBoxColumn";
            this.tENDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dIACHIDataGridViewTextBoxColumn
            // 
            this.dIACHIDataGridViewTextBoxColumn.DataPropertyName = "DIACHI";
            this.dIACHIDataGridViewTextBoxColumn.HeaderText = "Địa chỉ";
            this.dIACHIDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.dIACHIDataGridViewTextBoxColumn.Name = "dIACHIDataGridViewTextBoxColumn";
            this.dIACHIDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dIENTHOAIDataGridViewTextBoxColumn
            // 
            this.dIENTHOAIDataGridViewTextBoxColumn.DataPropertyName = "DIENTHOAI";
            this.dIENTHOAIDataGridViewTextBoxColumn.HeaderText = "Điện thoại";
            this.dIENTHOAIDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.dIENTHOAIDataGridViewTextBoxColumn.Name = "dIENTHOAIDataGridViewTextBoxColumn";
            this.dIENTHOAIDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cHIETKHAUDataGridViewTextBoxColumn
            // 
            this.cHIETKHAUDataGridViewTextBoxColumn.DataPropertyName = "CHIETKHAU";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cHIETKHAUDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.cHIETKHAUDataGridViewTextBoxColumn.HeaderText = "Chiết khấu";
            this.cHIETKHAUDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.cHIETKHAUDataGridViewTextBoxColumn.Name = "cHIETKHAUDataGridViewTextBoxColumn";
            this.cHIETKHAUDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pPHDataGridViewCheckBoxColumn
            // 
            this.pPHDataGridViewCheckBoxColumn.DataPropertyName = "P_PH";
            this.pPHDataGridViewCheckBoxColumn.HeaderText = "P_PH";
            this.pPHDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.pPHDataGridViewCheckBoxColumn.Name = "pPHDataGridViewCheckBoxColumn";
            this.pPHDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // pKTDataGridViewCheckBoxColumn
            // 
            this.pKTDataGridViewCheckBoxColumn.DataPropertyName = "P_KT";
            this.pKTDataGridViewCheckBoxColumn.HeaderText = "P_KT";
            this.pKTDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.pKTDataGridViewCheckBoxColumn.Name = "pKTDataGridViewCheckBoxColumn";
            this.pKTDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // uUTIENDataGridViewTextBoxColumn
            // 
            this.uUTIENDataGridViewTextBoxColumn.DataPropertyName = "UUTIEN";
            this.uUTIENDataGridViewTextBoxColumn.HeaderText = "Ưu tiên";
            this.uUTIENDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.uUTIENDataGridViewTextBoxColumn.Name = "uUTIENDataGridViewTextBoxColumn";
            this.uUTIENDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tabKHACHHANGBindingSource
            // 
            this.tabKHACHHANGBindingSource.DataMember = "tabKHACHHANG";
            this.tabKHACHHANGBindingSource.DataSource = this.thanhnienDataSet3;
            // 
            // thanhnienDataSet3
            // 
            this.thanhnienDataSet3.DataSetName = "ThanhnienDataSet3";
            this.thanhnienDataSet3.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Controls.Add(this.tableLayoutPanel2);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel2.Location = new System.Drawing.Point(1057, 3);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.guna2Panel2.Size = new System.Drawing.Size(306, 662);
            this.guna2Panel2.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.guna2CustomGradientPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.guna2CustomGradientPanel2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 10);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(296, 642);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // guna2CustomGradientPanel1
            // 
            this.guna2CustomGradientPanel1.Controls.Add(this.guna2GroupBox1);
            this.guna2CustomGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CustomGradientPanel1.Location = new System.Drawing.Point(3, 3);
            this.guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            this.guna2CustomGradientPanel1.Size = new System.Drawing.Size(290, 481);
            this.guna2CustomGradientPanel1.TabIndex = 2;
            // 
            // guna2CustomGradientPanel2
            // 
            this.guna2CustomGradientPanel2.Controls.Add(this.btnDelete);
            this.guna2CustomGradientPanel2.Controls.Add(this.btnEdit);
            this.guna2CustomGradientPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CustomGradientPanel2.Location = new System.Drawing.Point(3, 490);
            this.guna2CustomGradientPanel2.Name = "guna2CustomGradientPanel2";
            this.guna2CustomGradientPanel2.Size = new System.Drawing.Size(290, 149);
            this.guna2CustomGradientPanel2.TabIndex = 1;
            // 
            // tabKHACHHANGTableAdapter
            // 
            this.tabKHACHHANGTableAdapter.ClearBeforeFill = true;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.Controls.Add(this.dgvKhachHangGanDay);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.White;
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox1.Location = new System.Drawing.Point(0, 0);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(290, 481);
            this.guna2GroupBox1.TabIndex = 0;
            this.guna2GroupBox1.Text = "Khách hàng gần đây";
            this.guna2GroupBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgvKhachHangGanDay
            // 
            this.dgvKhachHangGanDay.AllowUserToAddRows = false;
            this.dgvKhachHangGanDay.AllowUserToDeleteRows = false;
            this.dgvKhachHangGanDay.AllowUserToResizeColumns = false;
            this.dgvKhachHangGanDay.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKhachHangGanDay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvKhachHangGanDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKhachHangGanDay.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvKhachHangGanDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKhachHangGanDay.GridColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.Location = new System.Drawing.Point(0, 40);
            this.dgvKhachHangGanDay.Name = "dgvKhachHangGanDay";
            this.dgvKhachHangGanDay.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKhachHangGanDay.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvKhachHangGanDay.RowHeadersVisible = false;
            this.dgvKhachHangGanDay.RowHeadersWidth = 51;
            this.dgvKhachHangGanDay.RowTemplate.Height = 24;
            this.dgvKhachHangGanDay.Size = new System.Drawing.Size(290, 441);
            this.dgvKhachHangGanDay.TabIndex = 0;
            this.dgvKhachHangGanDay.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvKhachHangGanDay.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvKhachHangGanDay.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvKhachHangGanDay.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvKhachHangGanDay.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.ThemeStyle.GridColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvKhachHangGanDay.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvKhachHangGanDay.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKhachHangGanDay.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKhachHangGanDay.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvKhachHangGanDay.ThemeStyle.ReadOnly = true;
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.Height = 24;
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvKhachHangGanDay.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnDelete
            // 
            this.btnDelete.Animated = true;
            this.btnDelete.BorderRadius = 20;
            this.btnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelete.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDelete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnDelete.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnDelete.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnDelete.Location = new System.Drawing.Point(55, 82);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(180, 45);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Animated = true;
            this.btnEdit.BorderRadius = 20;
            this.btnEdit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEdit.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEdit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEdit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnEdit.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEdit.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnEdit.Location = new System.Drawing.Point(55, 22);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(180, 45);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.btnAddNew.Location = new System.Drawing.Point(1144, 12);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(206, 72);
            this.btnAddNew.TabIndex = 7;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // frmCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pnlHeaderPublications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCustomers";
            this.Text = "frmCustomers";
            this.Load += new System.EventHandler(this.frmCustomers_Load);
            this.Click += new System.EventHandler(this.frmCustomers_Load);
            this.pnlHeaderPublications.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dboTabKhachHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabKHACHHANGBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet3)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.guna2CustomGradientPanel1.ResumeLayout(false);
            this.guna2CustomGradientPanel2.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHangGanDay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel pnlHeaderPublications;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dboTabKhachHang;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel2;
        private ThanhnienDataSet3 thanhnienDataSet3;
        private System.Windows.Forms.BindingSource tabKHACHHANGBindingSource;
        private ThanhnienDataSet3TableAdapters.tabKHACHHANGTableAdapter tabKHACHHANGTableAdapter;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mAKHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tENDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dIACHIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dIENTHOAIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHIETKHAUDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn pPHDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn pKTDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uUTIENDataGridViewTextBoxColumn;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvKhachHangGanDay;
        private Guna.UI2.WinForms.Guna2GradientButton btnDelete;
        private Guna.UI2.WinForms.Guna2GradientButton btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnAddNew;
    }
}