using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSB_U
{
    public partial class UpdaterForm : Form
    {
        Updater updater;
        private string currentAppPath = AppDomain.CurrentDomain.BaseDirectory;

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
                Updater.CleanupOldVersion();

                try
                {
                    // Başlatılacak uygulamanın tam yolunu oluşturuyoruz
                    string appToRun = Path.Combine(currentAppPath, "DiscordBotServer.exe");

                    // Dosyanın mevcut olup olmadığını kontrol ediyoruz
                    if (File.Exists(appToRun))
                    {
                        // Uygulamayı başlatıyoruz
                        Process.Start(appToRun);
                    }
                    else
                    {
                        MessageBox.Show("Uygulama bulunamadı: " + appToRun, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Başka uygulama başlatılırken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Application.Exit();
            }
        }
    }
}
