using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace DSB_U
{
    public class Updater
    {
        private string updateUrl = "https://jeakdev.com.tr/dsb/update/latestversion.zip"; // Güncelleme dosyasının URL'si
        private string tempFilePath = Path.Combine(Path.GetTempPath(), "latestversion.zip"); // Geçici dosya (zip) yolu
        private string currentAppPath = AppDomain.CurrentDomain.BaseDirectory; // Mevcut uygulama dizini
        private string tempAppPath = Path.Combine(Path.GetTempPath(), "tempApp"); // Geçici uygulama yolu
        private string currentVersion = "1.0.0"; // Uygulamanın mevcut sürümü
        private string versionCheckUrl = "https://jeakdev.com.tr/dsb/dsb_version.txt"; // Sunucudaki sürüm bilgisi

        public bool CheckForUpdate()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Sunucudan en son sürümü al
                    string latestVersion = client.DownloadString(versionCheckUrl).Trim();

                    // Sürüm kontrolü yap
                    if (latestVersion != currentVersion)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "DSB Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void UpdateApplicationWithProgress(ProgressBar progressBar, Label labelPercentage, Label labelSpeed)
        {
            if (CheckForUpdate())
            {
                try
                {
                    MoveCurrentAppToTemp();

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadProgressChanged += (sender, e) =>
                        {
                            // İlerlemeyi progress bar'da ve etiketlerde güncelle
                            progressBar.Value = e.ProgressPercentage;
                            labelPercentage.Text = $"%{e.ProgressPercentage}";
                            labelSpeed.Text = $"{(e.BytesReceived / 1024d / 1024d).ToString("0.00")} MB / {(e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")} MB";
                        };

                        client.DownloadFileCompleted += (sender, e) =>
                        {
                            if (e.Error == null)
                            {
                                try
                                {
                                    // Zip dosyasını aç
                                    using (ZipArchive archive = ZipFile.OpenRead(tempFilePath))
                                    {
                                        foreach (ZipArchiveEntry entry in archive.Entries)
                                        {
                                            // 'config.json' ve 'plugins' klasörünü atla
                                            if (entry.FullName.Equals("config.json", StringComparison.OrdinalIgnoreCase) ||
                                                entry.FullName.StartsWith("plugins/", StringComparison.OrdinalIgnoreCase) ||
                                                entry.FullName.StartsWith("plugins\\", StringComparison.OrdinalIgnoreCase))
                                            {
                                                continue;
                                            }

                                            string entryPath = Path.Combine(currentAppPath, entry.FullName);

                                            // Hedef dizini oluştur
                                            string destinationDir = Path.GetDirectoryName(entryPath);
                                            if (!Directory.Exists(destinationDir))
                                            {
                                                Directory.CreateDirectory(destinationDir);
                                            }

                                            // Var olan dosyaları sil ve yenileriyle değiştir
                                            if (File.Exists(entryPath))
                                            {
                                                File.SetAttributes(entryPath, FileAttributes.Normal);
                                                File.Delete(entryPath);
                                            }

                                            // Dosyayı çıkar
                                            if (string.IsNullOrEmpty(entry.Name))
                                            {
                                                // Eğer dizinse, dizini oluştur
                                                if (!Directory.Exists(entryPath))
                                                {
                                                    Directory.CreateDirectory(entryPath);
                                                }
                                            }
                                            else
                                            {
                                                entry.ExtractToFile(entryPath, true);
                                            }
                                        }
                                    }

                                    // Yeni uygulamayı çalıştır
                                    Process.Start(Path.Combine(currentAppPath, "DSBUpdater.exe"));
                                    Environment.Exit(0);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Güncelleme sırasında bir hata oluştu: " + ex.Message);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Güncelleme sırasında bir hata oluştu: " + e.Error.Message);
                            }
                        };

                        // Güncelleme dosyasını indir
                        client.DownloadFileAsync(new Uri(updateUrl), tempFilePath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme sırasında bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void MoveCurrentAppToTemp()
        {
            if (!Directory.Exists(tempAppPath))
            {
                Directory.CreateDirectory(tempAppPath);
            }

            string currentExePath = Process.GetCurrentProcess().MainModule.FileName;
            string newExePath = Path.Combine(tempAppPath, Path.GetFileName(currentExePath));

            File.Copy(currentExePath, newExePath, true);
        }

        public static void CleanupOldVersion()
        {
            string tempAppPath = Path.Combine(Path.GetTempPath(), "tempApp");

            try
            {
                if (Directory.Exists(tempAppPath))
                {
                    Directory.Delete(tempAppPath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eski sürüm silinirken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
