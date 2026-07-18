namespace DATNWF.Views
{
    partial class frmLoadDSDieuPhoi
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoadDSDieuPhoi));
            this.grbDSDieuPhoi = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvDSDieuPhoi = new Guna.UI2.WinForms.Guna2DataGridView();
            this.sohdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngayNhanDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maBaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tenbaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sobaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donGiaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soluongDieuPhoiDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soluongBanDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thanhTienDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabChiTietDieuPhoiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.thanhnienDataSet11 = new DATNWF.ThanhnienDataSet11();
            this.imgCancel = new Guna.UI2.WinForms.Guna2PictureBox();
            this.tabChiTietDieuPhoiTableAdapter = new DATNWF.ThanhnienDataSet11TableAdapters.tabChiTietDieuPhoiTableAdapter();
            this.grbDSDieuPhoi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDSDieuPhoi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabChiTietDieuPhoiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // grbDSDieuPhoi
            // 
            this.grbDSDieuPhoi.Controls.Add(this.dgvDSDieuPhoi);
            this.grbDSDieuPhoi.Controls.Add(this.imgCancel);
            this.grbDSDieuPhoi.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.grbDSDieuPhoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbDSDieuPhoi.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbDSDieuPhoi.ForeColor = System.Drawing.Color.Black;
            this.grbDSDieuPhoi.Location = new System.Drawing.Point(10, 10);
            this.grbDSDieuPhoi.Name = "grbDSDieuPhoi";
            this.grbDSDieuPhoi.Size = new System.Drawing.Size(1146, 687);
            this.grbDSDieuPhoi.TabIndex = 0;
            this.grbDSDieuPhoi.Text = "Danh sách các đơn điều phối ";
            this.grbDSDieuPhoi.TextOffset = new System.Drawing.Point(50, 0);
            // 
            // dgvDSDieuPhoi
            // 
            this.dgvDSDieuPhoi.AllowUserToAddRows = false;
            this.dgvDSDieuPhoi.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvDSDieuPhoi.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDSDieuPhoi.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDSDieuPhoi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDSDieuPhoi.ColumnHeadersHeight = 55;
            this.dgvDSDieuPhoi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDSDieuPhoi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sohdDataGridViewTextBoxColumn,
            this.ngayNhanDataGridViewTextBoxColumn,
            this.maBaoDataGridViewTextBoxColumn,
            this.tenbaoDataGridViewTextBoxColumn,
            this.sobaoDataGridViewTextBoxColumn,
            this.donGiaDataGridViewTextBoxColumn,
            this.soluongDieuPhoiDataGridViewTextBoxColumn,
            this.soluongBanDataGridViewTextBoxColumn,
            this.thanhTienDataGridViewTextBoxColumn});
            this.dgvDSDieuPhoi.DataSource = this.tabChiTietDieuPhoiBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDSDieuPhoi.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDSDieuPhoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDSDieuPhoi.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDSDieuPhoi.Location = new System.Drawing.Point(0, 40);
            this.dgvDSDieuPhoi.Name = "dgvDSDieuPhoi";
            this.dgvDSDieuPhoi.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDSDieuPhoi.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDSDieuPhoi.RowHeadersVisible = false;
            this.dgvDSDieuPhoi.RowHeadersWidth = 51;
            this.dgvDSDieuPhoi.RowTemplate.Height = 24;
            this.dgvDSDieuPhoi.Size = new System.Drawing.Size(1146, 647);
            this.dgvDSDieuPhoi.TabIndex = 48;
            this.dgvDSDieuPhoi.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDSDieuPhoi.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvDSDieuPhoi.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDSDieuPhoi.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvDSDieuPhoi.ThemeStyle.HeaderStyle.Height = 55;
            this.dgvDSDieuPhoi.ThemeStyle.ReadOnly = true;
            this.dgvDSDieuPhoi.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDSDieuPhoi.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvDSDieuPhoi.ThemeStyle.RowsStyle.Height = 24;
            // 
            // sohdDataGridViewTextBoxColumn
            // 
            this.sohdDataGridViewTextBoxColumn.DataPropertyName = "sohd";
            this.sohdDataGridViewTextBoxColumn.HeaderText = "Số hóa đơn";
            this.sohdDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sohdDataGridViewTextBoxColumn.Name = "sohdDataGridViewTextBoxColumn";
            this.sohdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ngayNhanDataGridViewTextBoxColumn
            // 
            this.ngayNhanDataGridViewTextBoxColumn.DataPropertyName = "ngayNhan";
            this.ngayNhanDataGridViewTextBoxColumn.HeaderText = "Ngày Nhận";
            this.ngayNhanDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.ngayNhanDataGridViewTextBoxColumn.Name = "ngayNhanDataGridViewTextBoxColumn";
            this.ngayNhanDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maBaoDataGridViewTextBoxColumn
            // 
            this.maBaoDataGridViewTextBoxColumn.DataPropertyName = "maBao";
            this.maBaoDataGridViewTextBoxColumn.HeaderText = "Mã báo";
            this.maBaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.maBaoDataGridViewTextBoxColumn.Name = "maBaoDataGridViewTextBoxColumn";
            this.maBaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tenbaoDataGridViewTextBoxColumn
            // 
            this.tenbaoDataGridViewTextBoxColumn.DataPropertyName = "tenbao";
            this.tenbaoDataGridViewTextBoxColumn.HeaderText = "Tên báo";
            this.tenbaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.tenbaoDataGridViewTextBoxColumn.Name = "tenbaoDataGridViewTextBoxColumn";
            this.tenbaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sobaoDataGridViewTextBoxColumn
            // 
            this.sobaoDataGridViewTextBoxColumn.DataPropertyName = "sobao";
            this.sobaoDataGridViewTextBoxColumn.HeaderText = "Số hóa đơn";
            this.sobaoDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.sobaoDataGridViewTextBoxColumn.Name = "sobaoDataGridViewTextBoxColumn";
            this.sobaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // donGiaDataGridViewTextBoxColumn
            // 
            this.donGiaDataGridViewTextBoxColumn.DataPropertyName = "donGia";
            this.donGiaDataGridViewTextBoxColumn.HeaderText = "Đơn giá";
            this.donGiaDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.donGiaDataGridViewTextBoxColumn.Name = "donGiaDataGridViewTextBoxColumn";
            this.donGiaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // soluongDieuPhoiDataGridViewTextBoxColumn
            // 
            this.soluongDieuPhoiDataGridViewTextBoxColumn.DataPropertyName = "soluongDieuPhoi";
            this.soluongDieuPhoiDataGridViewTextBoxColumn.HeaderText = "SL điều phối";
            this.soluongDieuPhoiDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soluongDieuPhoiDataGridViewTextBoxColumn.Name = "soluongDieuPhoiDataGridViewTextBoxColumn";
            this.soluongDieuPhoiDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // soluongBanDataGridViewTextBoxColumn
            // 
            this.soluongBanDataGridViewTextBoxColumn.DataPropertyName = "soluongBan";
            this.soluongBanDataGridViewTextBoxColumn.HeaderText = "SL bán";
            this.soluongBanDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.soluongBanDataGridViewTextBoxColumn.Name = "soluongBanDataGridViewTextBoxColumn";
            this.soluongBanDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // thanhTienDataGridViewTextBoxColumn
            // 
            this.thanhTienDataGridViewTextBoxColumn.DataPropertyName = "thanhTien";
            this.thanhTienDataGridViewTextBoxColumn.HeaderText = "Thành tiền";
            this.thanhTienDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.thanhTienDataGridViewTextBoxColumn.Name = "thanhTienDataGridViewTextBoxColumn";
            this.thanhTienDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tabChiTietDieuPhoiBindingSource
            // 
            this.tabChiTietDieuPhoiBindingSource.DataMember = "tabChiTietDieuPhoi";
            this.tabChiTietDieuPhoiBindingSource.DataSource = this.thanhnienDataSet11;
            // 
            // thanhnienDataSet11
            // 
            this.thanhnienDataSet11.DataSetName = "ThanhnienDataSet11";
            this.thanhnienDataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // imgCancel
            // 
            this.imgCancel.BackColor = System.Drawing.Color.Transparent;
            this.imgCancel.Image = ((System.Drawing.Image)(resources.GetObject("imgCancel.Image")));
            this.imgCancel.ImageRotate = 0F;
            this.imgCancel.Location = new System.Drawing.Point(3, 3);
            this.imgCancel.Name = "imgCancel";
            this.imgCancel.Size = new System.Drawing.Size(38, 34);
            this.imgCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgCancel.TabIndex = 47;
            this.imgCancel.TabStop = false;
            this.imgCancel.Click += new System.EventHandler(this.imgCancel_Click);
            // 
            // tabChiTietDieuPhoiTableAdapter
            // 
            this.tabChiTietDieuPhoiTableAdapter.ClearBeforeFill = true;
            // 
            // frmLoadDSDieuPhoi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 707);
            this.Controls.Add(this.grbDSDieuPhoi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoadDSDieuPhoi";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "frmLoadDSDieuPhoi";
            this.Load += new System.EventHandler(this.frmLoadDSDieuPhoi_Load);
            this.grbDSDieuPhoi.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDSDieuPhoi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabChiTietDieuPhoiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thanhnienDataSet11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GroupBox grbDSDieuPhoi;
        private Guna.UI2.WinForms.Guna2PictureBox imgCancel;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDSDieuPhoi;
        private ThanhnienDataSet11 thanhnienDataSet11;
        private System.Windows.Forms.BindingSource tabChiTietDieuPhoiBindingSource;
        private ThanhnienDataSet11TableAdapters.tabChiTietDieuPhoiTableAdapter tabChiTietDieuPhoiTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn sohdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayNhanDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maBaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tenbaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sobaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn donGiaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soluongDieuPhoiDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soluongBanDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn thanhTienDataGridViewTextBoxColumn;
    }
}