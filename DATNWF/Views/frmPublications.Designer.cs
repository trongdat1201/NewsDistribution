namespace DATNWF
{
    partial class frmPublications
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPublications));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlHeaderPublications = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvBao = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colmaBao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coldonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colngayBatDau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coltanSuat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colsoGoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlHeaderPublications.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBao)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSearch
            // 
            this.lblSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblSearch.BorderRadius = 20;
            this.lblSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblSearch.DefaultText = "";
            this.lblSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.lblSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lblSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.lblSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.lblSearch.FillColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.lblSearch.IconLeft = ((System.Drawing.Image)(resources.GetObject("lblSearch.IconLeft")));
            this.lblSearch.IconLeftOffset = new System.Drawing.Point(10, 0);
            this.lblSearch.Location = new System.Drawing.Point(24, 25);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.lblSearch.PlaceholderText = "\"Search dispatches, customers...\".";
            this.lblSearch.SelectedText = "";
            this.lblSearch.Size = new System.Drawing.Size(331, 48);
            this.lblSearch.TabIndex = 4;
            // 
            // pnlHeaderPublications
            // 
            this.pnlHeaderPublications.BackColor = System.Drawing.Color.White;
            this.pnlHeaderPublications.Controls.Add(this.guna2Button1);
            this.pnlHeaderPublications.Controls.Add(this.lblSearch);
            this.pnlHeaderPublications.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaderPublications.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaderPublications.Name = "pnlHeaderPublications";
            this.pnlHeaderPublications.Size = new System.Drawing.Size(1271, 100);
            this.pnlHeaderPublications.TabIndex = 0;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.guna2Button1.BorderRadius = 17;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button1.ForeColor = System.Drawing.Color.Black;
            this.guna2Button1.HoverState.FillColor = System.Drawing.Color.Black;
            this.guna2Button1.HoverState.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Image = ((System.Drawing.Image)(resources.GetObject("guna2Button1.Image")));
            this.guna2Button1.ImageSize = new System.Drawing.Size(30, 30);
            this.guna2Button1.Location = new System.Drawing.Point(1045, 12);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(206, 72);
            this.guna2Button1.TabIndex = 5;
            this.guna2Button1.Text = "Add New";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.dgvBao);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 100);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Padding = new System.Windows.Forms.Padding(20);
            this.guna2Panel1.Size = new System.Drawing.Size(1271, 672);
            this.guna2Panel1.TabIndex = 1;
            // 
            // dgvBao
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvBao.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvBao.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBao.ColumnHeadersHeight = 40;
            this.dgvBao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvBao.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colmaBao,
            this.colTen,
            this.colDVT,
            this.coldonGia,
            this.colngayBatDau,
            this.coltanSuat,
            this.colsoGoc});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBao.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBao.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvBao.Location = new System.Drawing.Point(20, 20);
            this.dgvBao.Name = "dgvBao";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Black", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBao.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBao.RowHeadersVisible = false;
            this.dgvBao.RowHeadersWidth = 45;
            this.dgvBao.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBao.RowTemplate.Height = 24;
            this.dgvBao.Size = new System.Drawing.Size(1231, 632);
            this.dgvBao.TabIndex = 3;
            this.dgvBao.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvBao.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvBao.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvBao.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvBao.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvBao.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvBao.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvBao.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvBao.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvBao.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBao.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvBao.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvBao.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvBao.ThemeStyle.ReadOnly = false;
            this.dgvBao.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvBao.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvBao.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBao.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvBao.ThemeStyle.RowsStyle.Height = 24;
            this.dgvBao.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvBao.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // colmaBao
            // 
            this.colmaBao.HeaderText = "Mã báo";
            this.colmaBao.MinimumWidth = 6;
            this.colmaBao.Name = "colmaBao";
            // 
            // colTen
            // 
            this.colTen.HeaderText = "Tên báo";
            this.colTen.MinimumWidth = 6;
            this.colTen.Name = "colTen";
            // 
            // colDVT
            // 
            this.colDVT.HeaderText = "Đơn vị tính";
            this.colDVT.MinimumWidth = 6;
            this.colDVT.Name = "colDVT";
            // 
            // coldonGia
            // 
            this.coldonGia.HeaderText = "Đơn giá";
            this.coldonGia.MinimumWidth = 6;
            this.coldonGia.Name = "coldonGia";
            // 
            // colngayBatDau
            // 
            this.colngayBatDau.HeaderText = "Ngày bắt đầu";
            this.colngayBatDau.MinimumWidth = 6;
            this.colngayBatDau.Name = "colngayBatDau";
            // 
            // coltanSuat
            // 
            this.coltanSuat.HeaderText = "Tần suất";
            this.coltanSuat.MinimumWidth = 6;
            this.coltanSuat.Name = "coltanSuat";
            this.coltanSuat.ToolTipText = "Số lần phát hành trong tuần";
            // 
            // colsoGoc
            // 
            this.colsoGoc.HeaderText = "Số gốc";
            this.colsoGoc.MinimumWidth = 6;
            this.colsoGoc.Name = "colsoGoc";
            // 
            // frmPublications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 772);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.pnlHeaderPublications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPublications";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmPublications_Load);
            this.pnlHeaderPublications.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBao)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox lblSearch;
        private Guna.UI2.WinForms.Guna2Panel pnlHeaderPublications;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvBao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colmaBao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn coldonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colngayBatDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn coltanSuat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colsoGoc;
    }
}