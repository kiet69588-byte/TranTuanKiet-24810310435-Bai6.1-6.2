namespace Bai6_2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

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
            this.SuspendLayout();

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(8F, 20F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize =
                new System.Drawing.Size(1100, 700);

            this.Name = "Form1";
            this.Text = "HỆ THỐNG QUẢN LÝ NHÂN SỰ VÀ TÍNH LƯƠNG";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.ResumeLayout(false);
        }
    }
}