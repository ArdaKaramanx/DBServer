using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordBotServer
{
    public partial class UpdaterForm : Form
    {
        Updater updater;
        Form1 main;

        public UpdaterForm()
        {
            InitializeComponent();
            updater = new Updater();
        }

        private void UpdaterForm_Load(object sender, EventArgs e)
        {
            if (updater.CheckForUpdate())
            {
                updater.UpdateApplicationWithProgress(DownloadedProgressBar, DownloadedLabel, DownloadMbLable);
            }
            else
            {
                main = new Form1();
                main.Show();
                this.Close();
            }
        }
    }
}
