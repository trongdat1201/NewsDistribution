namespace DATNWF.Views
{
    partial class frmAccess
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
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.grpRoles = new System.Windows.Forms.GroupBox();
            this.chkHT = new System.Windows.Forms.CheckBox();
            this.chkNV = new System.Windows.Forms.CheckBox();
            this.chkBC = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();

            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.grpRoles.SuspendLayout();
            this.SuspendLayout();

            // pnlLeft
            this.pnlLeft.Controls.Add(this.dgvUsers);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(15);
            this.pnlLeft.Size = new System.Drawing.Size(650, 692);
            this.pnlLeft.TabIndex = 0;

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(15, 15);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowTemplate.Height = 28;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(620, 662);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.SelectionChanged += new System.EventHandler(this.dgvUsers_SelectionChanged);

            // pnlRight
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.lblTitle);
            this.pnlRight.Controls.Add(this.lblStatus);
            this.pnlRight.Controls.Add(this.lblUsername);
            this.pnlRight.Controls.Add(this.txtUsername);
            this.pnlRight.Controls.Add(this.lblPassword);
            this.pnlRight.Controls.Add(this.txtPassword);
            this.pnlRight.Controls.Add(this.grpRoles);
            this.pnlRight.Controls.Add(this.btnSave);
            this.pnlRight.Controls.Add(this.btnDelete);
            this.pnlRight.Controls.Add(this.btnNew);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(650, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(440, 692);
            this.pnlRight.TabIndex = 1;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(349, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ QUYỀN TRUY CẬP";

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblStatus.Location = new System.Drawing.Point(33, 75);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(225, 23);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Chế độ: Thêm mới người dùng";

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(33, 130);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(129, 23);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Tên đăng nhập:";

            // txtUsername
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(37, 160);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(360, 32);
            this.txtUsername.TabIndex = 3;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(33, 215);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(91, 23);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Mật khẩu:";

            // txtPassword
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(37, 245);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(360, 32);
            this.txtPassword.TabIndex = 5;

            // grpRoles
            this.grpRoles.Controls.Add(this.chkHT);
            this.grpRoles.Controls.Add(this.chkNV);
            this.grpRoles.Controls.Add(this.chkBC);
            this.grpRoles.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRoles.Location = new System.Drawing.Point(37, 310);
            this.grpRoles.Name = "grpRoles";
            this.grpRoles.Size = new System.Drawing.Size(360, 160);
            this.grpRoles.TabIndex = 6;
            this.grpRoles.TabStop = false;
            this.grpRoles.Text = " Quyền Hạn (Roles) ";

            // chkHT
            this.chkHT.AutoSize = true;
            this.chkHT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHT.Location = new System.Drawing.Point(30, 40);
            this.chkHT.Name = "chkHT";
            this.chkHT.Size = new System.Drawing.Size(155, 27);
            this.chkHT.TabIndex = 0;
            this.chkHT.Text = "Hệ thống (Admin)";
            this.chkHT.UseVisualStyleBackColor = true;

            // chkNV
            this.chkNV.AutoSize = true;
            this.chkNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNV.Location = new System.Drawing.Point(30, 75);
            this.chkNV.Name = "chkNV";
            this.chkNV.Size = new System.Drawing.Size(155, 27);
            this.chkNV.TabIndex = 1;
            this.chkNV.Text = "Nhân viên (Staff)";
            this.chkNV.UseVisualStyleBackColor = true;

            // chkBC
            this.chkBC.AutoSize = true;
            this.chkBC.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBC.Location = new System.Drawing.Point(30, 110);
            this.chkBC.Name = "chkBC";
            this.chkBC.Size = new System.Drawing.Size(150, 27);
            this.chkBC.TabIndex = 2;
            this.chkBC.Text = "Báo cáo (Reporter)";
            this.chkBC.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.ForestGreen;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(37, 500);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 45);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnDelete
            this.btnDelete.BackColor = System.Drawing.Color.Crimson;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(162, 500);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 45);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnNew
            this.btnNew.BackColor = System.Drawing.Color.Gray;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(287, 500);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(110, 45);
            this.btnNew.TabIndex = 9;
            this.btnNew.Text = "Thêm mới";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);

            // frmAccess
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 692);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAccess";
            this.Text = "frmAccess";
            this.Load += new System.EventHandler(this.frmAccess_Load);

            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.grpRoles.ResumeLayout(false);
            this.grpRoles.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.GroupBox grpRoles;
        private System.Windows.Forms.CheckBox chkHT;
        private System.Windows.Forms.CheckBox chkNV;
        private System.Windows.Forms.CheckBox chkBC;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNew;
    }
}
