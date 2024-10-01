namespace DSB_U
{
    partial class UpdaterForm
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
            this.DownloadedProgressBar = new System.Windows.Forms.ProgressBar();
            this.DownloadedLabel = new System.Windows.Forms.Label();
            this.DownloadMbLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DownloadedProgressBar
            // 
            this.DownloadedProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadedProgressBar.Location = new System.Drawing.Point(16, 13);
            this.DownloadedProgressBar.Name = "DownloadedProgressBar";
            this.DownloadedProgressBar.Size = new System.Drawing.Size(532, 27);
            this.DownloadedProgressBar.TabIndex = 0;
            // 
            // DownloadedLabel
            // 
            this.DownloadedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadedLabel.AutoSize = true;
            this.DownloadedLabel.BackColor = System.Drawing.SystemColors.Control;
            this.DownloadedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.DownloadedLabel.Location = new System.Drawing.Point(17, 17);
            this.DownloadedLabel.Name = "DownloadedLabel";
            this.DownloadedLabel.Size = new System.Drawing.Size(33, 20);
            this.DownloadedLabel.TabIndex = 1;
            this.DownloadedLabel.Text = "%0";
            // 
            // DownloadMbLable
            // 
            this.DownloadMbLable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadMbLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.DownloadMbLable.Location = new System.Drawing.Point(12, 43);
            this.DownloadMbLable.Name = "DownloadMbLable";
            this.DownloadMbLable.Size = new System.Drawing.Size(536, 20);
            this.DownloadMbLable.TabIndex = 2;
            this.DownloadMbLable.Text = "0.00 mb";
            this.DownloadMbLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(560, 72);
            this.Controls.Add(this.DownloadMbLable);
            this.Controls.Add(this.DownloadedLabel);
            this.Controls.Add(this.DownloadedProgressBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdaterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Updater DBS";
            this.Load += new System.EventHandler(this.UpdaterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar DownloadedProgressBar;
        private System.Windows.Forms.Label DownloadedLabel;
        private System.Windows.Forms.Label DownloadMbLable;
    }
}