namespace ErgoTracker
{
    partial class KinectForm
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
            this.kinectVideoBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.kinectVideoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // kinectVideoBox
            // 
            this.kinectVideoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kinectVideoBox.Location = new System.Drawing.Point(0, 0);
            this.kinectVideoBox.Name = "kinectVideoBox";
            this.kinectVideoBox.Size = new System.Drawing.Size(1278, 737);
            this.kinectVideoBox.TabIndex = 0;
            this.kinectVideoBox.TabStop = false;
            // 
            // KinectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 737);
            this.Controls.Add(this.kinectVideoBox);
            this.Name = "KinectForm";
            this.Text = "KinectForm";
            this.Load += new System.EventHandler(this.KinectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kinectVideoBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox kinectVideoBox;
    }
}