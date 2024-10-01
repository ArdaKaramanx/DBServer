using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSB_U
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        private const int SW_RESTORE = 9;

        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
                return null;
            };

            bool createdNew;
            using (Mutex mutex = new Mutex(true, "DSBUpdater", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new UpdaterForm());
                }
                else
                {
                    // Uygulama zaten çalışıyorsa mevcut örneği getir
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            IntPtr hWnd = process.MainWindowHandle;

                            // Eğer pencere gizlenmişse göster
                            if (IsIconic(hWnd))
                            {
                                // Eğer pencere simge durumundaysa geri yükle
                                ShowWindow(hWnd, SW_RESTORE);
                            }

                            // Uygulama penceresini ön plana getir
                            SetForegroundWindow(hWnd);
                            break;
                        }
                    }
                }
            }
        }
    }
}
