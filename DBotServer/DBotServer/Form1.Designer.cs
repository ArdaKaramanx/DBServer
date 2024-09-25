namespace DiscordBotServer
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.discordServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBoxConsole = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Discord Bot Server";
            this.notifyIcon1.BalloonTipTitle = "Discord Bot Server";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "Discord Bot Server";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.discordServerToolStripMenuItem,
            this.openConsoleToolStripMenuItem,
            this.serverStartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 92);
            // 
            // discordServerToolStripMenuItem
            // 
            this.discordServerToolStripMenuItem.Name = "discordServerToolStripMenuItem";
            this.discordServerToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.discordServerToolStripMenuItem.Text = " ";
            // 
            // openConsoleToolStripMenuItem
            // 
            this.openConsoleToolStripMenuItem.Name = "openConsoleToolStripMenuItem";
            this.openConsoleToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openConsoleToolStripMenuItem.Text = "Open Console";
            this.openConsoleToolStripMenuItem.Click += new System.EventHandler(this.openConsoleToolStripMenuItem_Click);
            // 
            // serverStartToolStripMenuItem
            // 
            this.serverStartToolStripMenuItem.Name = "serverStartToolStripMenuItem";
            this.serverStartToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.serverStartToolStripMenuItem.Text = "Server Start";
            this.serverStartToolStripMenuItem.Click += new System.EventHandler(this.serverStartToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // richTextBoxConsole
            // 
            this.richTextBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxConsole.BackColor = System.Drawing.Color.Black;
            this.richTextBoxConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.richTextBoxConsole.ForeColor = System.Drawing.Color.White;
            this.richTextBoxConsole.Location = new System.Drawing.Point(13, 12);
            this.richTextBoxConsole.Name = "richTextBoxConsole";
            this.richTextBoxConsole.ReadOnly = true;
            this.richTextBoxConsole.Size = new System.Drawing.Size(759, 506);
            this.richTextBoxConsole.TabIndex = 0;
            this.richTextBoxConsole.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(13, 525);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(759, 29);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBoxConsole);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Discord Bot Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxConsole;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discordServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverStartToolStripMenuItem;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

