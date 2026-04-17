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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomers));
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.lblSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
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
            this.guna2Button1.Location = new System.Drawing.Point(308, 280);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(206, 66);
            this.guna2Button1.TabIndex = 4;
            this.guna2Button1.Text = "Add New";
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
            this.lblSearch.Location = new System.Drawing.Point(235, 201);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.lblSearch.PlaceholderText = "\"Search dispatches, customers...\".";
            this.lblSearch.SelectedText = "";
            this.lblSearch.Size = new System.Drawing.Size(331, 48);
            this.lblSearch.TabIndex = 3;
            // 
            // frmCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.lblSearch);
            this.Name = "frmCustomers";
            this.Text = "frmCustomers";
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox lblSearch;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}