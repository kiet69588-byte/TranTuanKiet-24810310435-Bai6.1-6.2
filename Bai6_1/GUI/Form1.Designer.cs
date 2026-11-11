using static System.Net.Mime.MediaTypeNames;

namespace Bai6_1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl = new System.Windows.Forms.TabControl();
            lblTitle = new System.Windows.Forms.Label();

            SuspendLayout();

            // Tiêu đề
            lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitle.Height = 65;
            lblTitle.Text =
                "HỆ THỐNG QUẢN LÝ BÁN HÀNG - SIÊU THỊ MINI";

            lblTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;

            lblTitle.Font = new System.Drawing.Font(
                "Segoe UI", 17F,
                System.Drawing.FontStyle.Bold);

            lblTitle.ForeColor = System.Drawing.Color.DarkBlue;

            // TabControl
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;

            tabControl.Font = new System.Drawing.Font(
                "Segoe UI", 10F);

            Controls.Add(tabControl);
            Controls.Add(lblTitle);

            Text = "Bai6_1 - Retail Sales System";

            StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            WindowState =
                System.Windows.Forms.FormWindowState.Maximized;

            MinimumSize = new System.Drawing.Size(1000, 650);

            ResumeLayout(false);
        }
    }
}