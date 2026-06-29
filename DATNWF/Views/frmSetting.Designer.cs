namespace DATNWF.Views
{
    partial class frmSetting
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtSearchSohd = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSearchPrompt = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlLeftInfo = new System.Windows.Forms.Panel();
            this.lblThanhToanVal = new System.Windows.Forms.Label();
            this.lblThanhToan = new System.Windows.Forms.Label();
            this.lblGhiChuVal = new System.Windows.Forms.Label();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.lblTongTienVal = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblKyHieuVal = new System.Windows.Forms.Label();
            this.lblKyHieu = new System.Windows.Forms.Label();
            this.lblNgayLapVal = new System.Windows.Forms.Label();
            this.lblNgayLap = new System.Windows.Forms.Label();
            this.lblKHVal = new System.Windows.Forms.Label();
            this.lblKH = new System.Windows.Forms.Label();
            this.lblSohdVal = new System.Windows.Forms.Label();
            this.lblSohd = new System.Windows.Forms.Label();
            this.dgvDetails = new System.Windows.Forms.DataGridView();

            this.pnlTop.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlLeftInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1090, 60);
            this.pnlHeader.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(15, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(256, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CHI TIẾT HÓA ĐƠN";

            // pnlTop
            this.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTop.Controls.Add(this.lblSearchPrompt);
            this.pnlTop.Controls.Add(this.txtSearchSohd);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 60);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1090, 75);
            this.pnlTop.TabIndex = 1;

            // lblSearchPrompt
            this.lblSearchPrompt.AutoSize = true;
            this.lblSearchPrompt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchPrompt.Location = new System.Drawing.Point(18, 26);
            this.lblSearchPrompt.Name = "lblSearchPrompt";
            this.lblSearchPrompt.Size = new System.Drawing.Size(159, 23);
            this.lblSearchPrompt.TabIndex = 0;
            this.lblSearchPrompt.Text = "Nhập Số hóa đơn:";

            // txtSearchSohd
            this.txtSearchSohd.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchSohd.Location = new System.Drawing.Point(185, 21);
            this.txtSearchSohd.Name = "txtSearchSohd";
            this.txtSearchSohd.Size = new System.Drawing.Size(300, 32);
            this.txtSearchSohd.TabIndex = 1;

            // btnSearch
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(500, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 42);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tra cứu";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // pnlLeftInfo
            this.pnlLeftInfo.BackColor = System.Drawing.Color.White;
            this.pnlLeftInfo.Controls.Add(this.lblThanhToanVal);
            this.pnlLeftInfo.Controls.Add(this.lblThanhToan);
            this.pnlLeftInfo.Controls.Add(this.lblGhiChuVal);
            this.pnlLeftInfo.Controls.Add(this.lblGhiChu);
            this.pnlLeftInfo.Controls.Add(this.lblTongTienVal);
            this.pnlLeftInfo.Controls.Add(this.lblTongTien);
            this.pnlLeftInfo.Controls.Add(this.lblKyHieuVal);
            this.pnlLeftInfo.Controls.Add(this.lblKyHieu);
            this.pnlLeftInfo.Controls.Add(this.lblNgayLapVal);
            this.pnlLeftInfo.Controls.Add(this.lblNgayLap);
            this.pnlLeftInfo.Controls.Add(this.lblKHVal);
            this.pnlLeftInfo.Controls.Add(this.lblKH);
            this.pnlLeftInfo.Controls.Add(this.lblSohdVal);
            this.pnlLeftInfo.Controls.Add(this.lblSohd);
            this.pnlLeftInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftInfo.Location = new System.Drawing.Point(0, 135);
            this.pnlLeftInfo.Name = "pnlLeftInfo";
            this.pnlLeftInfo.Size = new System.Drawing.Size(380, 557);
            this.pnlLeftInfo.TabIndex = 2;

            // lblSohd
            this.lblSohd.AutoSize = true;
            this.lblSohd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSohd.ForeColor = System.Drawing.Color.Gray;
            this.lblSohd.Location = new System.Drawing.Point(18, 20);
            this.lblSohd.Name = "lblSohd";
            this.lblSohd.Size = new System.Drawing.Size(107, 20);
            this.lblSohd.TabIndex = 0;
            this.lblSohd.Text = "SỐ HÓA ĐƠN:";

            // lblSohdVal
            this.lblSohdVal.AutoSize = true;
            this.lblSohdVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSohdVal.Location = new System.Drawing.Point(18, 43);
            this.lblSohdVal.Name = "lblSohdVal";
            this.lblSohdVal.Size = new System.Drawing.Size(20, 25);
            this.lblSohdVal.TabIndex = 1;
            this.lblSohdVal.Text = "-";

            // lblKH
            this.lblKH.AutoSize = true;
            this.lblKH.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKH.ForeColor = System.Drawing.Color.Gray;
            this.lblKH.Location = new System.Drawing.Point(18, 90);
            this.lblKH.Name = "lblKH";
            this.lblKH.Size = new System.Drawing.Size(113, 20);
            this.lblKH.TabIndex = 2;
            this.lblKH.Text = "KHÁCH HÀNG:";

            // lblKHVal
            this.lblKHVal.AutoSize = true;
            this.lblKHVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKHVal.Location = new System.Drawing.Point(18, 113);
            this.lblKHVal.Name = "lblKHVal";
            this.lblKHVal.Size = new System.Drawing.Size(20, 25);
            this.lblKHVal.TabIndex = 3;
            this.lblKHVal.Text = "-";

            // lblNgayLap
            this.lblNgayLap.AutoSize = true;
            this.lblNgayLap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayLap.ForeColor = System.Drawing.Color.Gray;
            this.lblNgayLap.Location = new System.Drawing.Point(18, 160);
            this.lblNgayLap.Name = "lblNgayLap";
            this.lblNgayLap.Size = new System.Drawing.Size(139, 20);
            this.lblNgayLap.TabIndex = 4;
            this.lblNgayLap.Text = "NGÀY LẬP PHIẾU:";

            // lblNgayLapVal
            this.lblNgayLapVal.AutoSize = true;
            this.lblNgayLapVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayLapVal.Location = new System.Drawing.Point(18, 183);
            this.lblNgayLapVal.Name = "lblNgayLapVal";
            this.lblNgayLapVal.Size = new System.Drawing.Size(20, 25);
            this.lblNgayLapVal.TabIndex = 5;
            this.lblNgayLapVal.Text = "-";

            // lblKyHieu
            this.lblKyHieu.AutoSize = true;
            this.lblKyHieu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKyHieu.ForeColor = System.Drawing.Color.Gray;
            this.lblKyHieu.Location = new System.Drawing.Point(18, 230);
            this.lblKyHieu.Name = "lblKyHieu";
            this.lblKyHieu.Size = new System.Drawing.Size(167, 20);
            this.lblKyHieu.TabIndex = 6;
            this.lblKyHieu.Text = "KỲ THANH TOÁN (Từ-Đến):";

            // lblKyHieuVal
            this.lblKyHieuVal.AutoSize = true;
            this.lblKyHieuVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKyHieuVal.Location = new System.Drawing.Point(18, 253);
            this.lblKyHieuVal.Name = "lblKyHieuVal";
            this.lblKyHieuVal.Size = new System.Drawing.Size(20, 25);
            this.lblKyHieuVal.TabIndex = 7;
            this.lblKyHieuVal.Text = "-";

            // lblTongTien
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.ForeColor = System.Drawing.Color.Gray;
            this.lblTongTien.Location = new System.Drawing.Point(18, 300);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(95, 20);
            this.lblTongTien.TabIndex = 8;
            this.lblTongTien.Text = "TỔNG TIỀN:";

            // lblTongTienVal
            this.lblTongTienVal.AutoSize = true;
            this.lblTongTienVal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTienVal.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblTongTienVal.Location = new System.Drawing.Point(18, 323);
            this.lblTongTienVal.Name = "lblTongTienVal";
            this.lblTongTienVal.Size = new System.Drawing.Size(56, 32);
            this.lblTongTienVal.TabIndex = 9;
            this.lblTongTienVal.Text = "0 đ";

            // lblGhiChu
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhiChu.ForeColor = System.Drawing.Color.Gray;
            this.lblGhiChu.Location = new System.Drawing.Point(18, 375);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(76, 20);
            this.lblGhiChu.TabIndex = 10;
            this.lblGhiChu.Text = "GHI CHÚ:";

            // lblGhiChuVal
            this.lblGhiChuVal.AutoSize = true;
            this.lblGhiChuVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhiChuVal.Location = new System.Drawing.Point(18, 398);
            this.lblGhiChuVal.Name = "lblGhiChuVal";
            this.lblGhiChuVal.Size = new System.Drawing.Size(20, 25);
            this.lblGhiChuVal.TabIndex = 11;
            this.lblGhiChuVal.Text = "-";

            // lblThanhToan
            this.lblThanhToan.AutoSize = true;
            this.lblThanhToan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhToan.ForeColor = System.Drawing.Color.Gray;
            this.lblThanhToan.Location = new System.Drawing.Point(18, 445);
            this.lblThanhToan.Name = "lblThanhToan";
            this.lblThanhToan.Size = new System.Drawing.Size(189, 20);
            this.lblThanhToan.TabIndex = 12;
            this.lblThanhToan.Text = "TRẠNG THÁI THANH TOÁN:";

            // lblThanhToanVal
            this.lblThanhToanVal.AutoSize = true;
            this.lblThanhToanVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhToanVal.Location = new System.Drawing.Point(18, 468);
            this.lblThanhToanVal.Name = "lblThanhToanVal";
            this.lblThanhToanVal.Size = new System.Drawing.Size(20, 25);
            this.lblThanhToanVal.TabIndex = 13;
            this.lblThanhToanVal.Text = "-";

            // dgvDetails
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.Location = new System.Drawing.Point(380, 135);
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.RowTemplate.Height = 28;
            this.dgvDetails.Size = new System.Drawing.Size(710, 557);
            this.dgvDetails.TabIndex = 3;

            // frmSetting
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 692);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.pnlLeftInfo);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSetting";
            this.Text = "frmSetting";
            this.Load += new System.EventHandler(this.frmSetting_Load);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLeftInfo.ResumeLayout(false);
            this.pnlLeftInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblSearchPrompt;
        private System.Windows.Forms.TextBox txtSearchSohd;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlLeftInfo;
        private System.Windows.Forms.Label lblSohd;
        private System.Windows.Forms.Label lblSohdVal;
        private System.Windows.Forms.Label lblKH;
        private System.Windows.Forms.Label lblKHVal;
        private System.Windows.Forms.Label lblNgayLap;
        private System.Windows.Forms.Label lblNgayLapVal;
        private System.Windows.Forms.Label lblKyHieu;
        private System.Windows.Forms.Label lblKyHieuVal;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblTongTienVal;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.Label lblGhiChuVal;
        private System.Windows.Forms.Label lblThanhToan;
        private System.Windows.Forms.Label lblThanhToanVal;
        private System.Windows.Forms.DataGridView dgvDetails;
    }
}