namespace DATNWF.Views
{
    partial class frmDelivery
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDelivery));
            this.pnlHeaderPublications = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.dboTabKhachHang = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.guna2CustomGradientPanel2 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.thanhnienDataSet1 = new DATNWF.ThanhnienDataSet1();
            this.tabDieuPhoiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabDieuPhoiTableAdapter = new DATNWF.ThanhnienDataSet1TableAdapters.tabDieuPhoiTableAdapter();
            this.soHDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makhDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tungayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.denngayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ghiChuDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddNew = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlHeaderPublications.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dboTabKhachHang)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.guna2CustomGradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabDieuPhoiBindingSource)).BeginInit();
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.64422F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.35578F));
            this.tableLayoutPanel1.Controls.Add(this.guna2Panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.guna2Panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1366, 668);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.dboTabKhachHang);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(3, 3);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Padding = new System.Windows.Forms.Padding(10);
            this.guna2Panel1.Size = new System.Drawing.Size(959, 662);
            this.guna2Panel1.TabIndex = 0;
            // 
            // dboTabKhachHang
            // 
            this.dboTabKhachHang.AllowUserToAddRows = false;
            this.dboTabKhachHang.AllowUserToDeleteRows = false;
            this.dboTabKhachHang.AllowUserToResizeColumns = false;
            this.dboTabKhachHang.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            this.dboTabKhachHang.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dboTabKhachHang.AutoGenerateColumns = false;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTabKhachHang.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dboTabKhachHang.ColumnHeadersHeight = 71;
            this.dboTabKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dboTabKhachHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.soHDDataGridViewTextBoxColumn,
            this.makhDataGridViewTextBoxColumn,
            this.ngayDataGridViewTextBoxColumn,
            this.tungayDataGridViewTextBoxColumn,
            this.denngayDataGridViewTextBoxColumn,
            this.ghiChuDataGridViewTextBoxColumn});
            this.dboTabKhachHang.DataSource = this.tabDieuPhoiBindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dboTabKhachHang.DefaultCellStyle = dataGridViewCellStyle11;
            this.dboTabKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dboTabKhachHang.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dboTabKhachHang.Location = new System.Drawing.Point(10, 10);
            this.dboTabKhachHang.Name = "dboTabKhachHang";
            this.dboTabKhachHang.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dboTabKhachHang.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dboTabKhachHang.RowHeadersVisible = false;
            this.dboTabKhachHang.RowHeadersWidth = 51;
            this.dboTabKhachHang.RowTemplate.Height = 24;
            this.dboTabKhachHang.Size = new System.Drawing.Size(939, 642);
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
            // guna2Panel2
            // 
            this.guna2Panel2.Controls.Add(this.tableLayoutPanel2);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel2.Location = new System.Drawing.Point(968, 3);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.guna2Panel2.Size = new System.Drawing.Size(395, 662);
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
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(385, 642);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // guna2CustomGradientPanel1
            // 
            this.guna2CustomGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CustomGradientPanel1.Location = new System.Drawing.Point(3, 3);
            this.guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            this.guna2CustomGradientPanel1.Size = new System.Drawing.Size(379, 557);
            this.guna2CustomGradientPanel1.TabIndex = 2;
            // 
            // guna2CustomGradientPanel2
            // 
            this.guna2CustomGradientPanel2.Controls.Add(this.guna2Button1);
            this.guna2CustomGradientPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CustomGradientPanel2.Location = new System.Drawing.Point(3, 566);
            this.guna2CustomGradientPanel2.Name = "guna2CustomGradientPanel2";
            this.guna2CustomGradientPanel2.Size = new System.Drawing.Size(379, 73);
            this.guna2CustomGradientPanel2.TabIndex = 1;
            // 
            // guna2Button1
            // 
            this.guna2Button1.BorderRadius = 20;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Silver;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(104, 14);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(180, 45);
            this.guna2Button1.TabIndex = 0;
            this.guna2Button1.Text = "Detail";
            // 
            // thanhnienDataSet1
            // 
            this.thanhnienDataSet1.DataSetName = "ThanhnienDataSet1";
            this.thanhnienDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tabDieuPhoiBindingSource
            // 
            this.tabDieuPhoiBindingSource.DataMember = "tabDieuPhoi";
            this.tabDieuPhoiBindingSource.DataSource = this.thanhnienDataSet1;
            // 
            // tabDieuPhoiTableAdapter
            // 
            this.tabDieuPhoiTableAdapter.ClearBeforeFill = true;
            // 
            // soHDDataGridViewTextBoxColumn
            // 
            this.soHDDataGridViewTextBoxColumn.DataPropertyName = "soHD";
            this.soHDDataGridViewTextBoxColumn.HeaderText = "Số hóa đơn";
            this.soHDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soHDDataGridViewTextBoxColumn.Name = "soHDDataGridViewTextBoxColumn";
            this.soHDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // makhDataGridViewTextBoxColumn
            // 
            this.makhDataGridViewTextBoxColumn.DataPropertyName = "makh";
            this.makhDataGridViewTextBoxColumn.HeaderText = "Mã khách hàng";
            this.makhDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.makhDataGridViewTextBoxColumn.Name = "makhDataGridViewTextBoxColumn";
            this.makhDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ngayDataGridViewTextBoxColumn
            // 
            this.ngayDataGridViewTextBoxColumn.DataPropertyName = "ngay";
            this.ngayDataGridViewTextBoxColumn.HeaderText = "Ngày lập phiếu";
            this.ngayDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ngayDataGridViewTextBoxColumn.Name = "ngayDataGridViewTextBoxColumn";
            this.ngayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tungayDataGridViewTextBoxColumn
            // 
            this.tungayDataGridViewTextBoxColumn.DataPropertyName = "tungay";
            this.tungayDataGridViewTextBoxColumn.HeaderText = "Từ ngày";
            this.tungayDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tungayDataGridViewTextBoxColumn.Name = "tungayDataGridViewTextBoxColumn";
            this.tungayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // denngayDataGridViewTextBoxColumn
            // 
            this.denngayDataGridViewTextBoxColumn.DataPropertyName = "denngay";
            this.denngayDataGridViewTextBoxColumn.HeaderText = "Đến ngày";
            this.denngayDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.denngayDataGridViewTextBoxColumn.Name = "denngayDataGridViewTextBoxColumn";
            this.denngayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ghiChuDataGridViewTextBoxColumn
            // 
            this.ghiChuDataGridViewTextBoxColumn.DataPropertyName = "ghiChu";
            this.ghiChuDataGridViewTextBoxColumn.HeaderText = "Ghi chú";
            this.ghiChuDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ghiChuDataGridViewTextBoxColumn.Name = "ghiChuDataGridViewTextBoxColumn";
            this.ghiChuDataGridViewTextBoxColumn.ReadOnly = true;
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
            // 
            // frmDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pnlHeaderPublications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDelivery";
            this.Text = "frmDelivery";
            this.Load += new System.EventHandler(this.frmDelivery_Load);
            this.pnlHeaderPublications.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dboTabKhachHang)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.guna2CustomGradientPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabDieuPhoiBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeaderPublications;
        private Guna.UI2.WinForms.Guna2Button btnAddNew;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dboTabKhachHang;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private ThanhnienDataSet1 thanhnienDataSet1;
        private System.Windows.Forms.BindingSource tabDieuPhoiBindingSource;
        private ThanhnienDataSet1TableAdapters.tabDieuPhoiTableAdapter tabDieuPhoiTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn soHDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn makhDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tungayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn denngayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ghiChuDataGridViewTextBoxColumn;
    }
}