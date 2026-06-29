using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DATNWF
{
    internal static class Program
    {
        private static Process _apiProcess;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StartWebApi();

            try
            {
                bool restart;
                do
                {
                    using (var home = new Home())
                    {
                        Application.Run(home);
                        restart = Home.NeedsRestart;
                        Home.NeedsRestart = false;
                    }
                } while (restart);
            }
            finally
            {
                StopWebApi();
            }
        }

        private static void StartWebApi()
        {
            try
            {
                var existing = Process.GetProcessesByName("DATNWF_API");
                if (existing.Length > 0) return;

                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\DATNWF_API\bin\Debug\net10.0\DATNWF_API.exe");
                if (!File.Exists(exePath))
                {
                    exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATNWF_API.exe");
                }

                if (File.Exists(exePath))
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        WorkingDirectory = Path.GetDirectoryName(exePath)
                    };
                    _apiProcess = Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động Web API ngầm: " + ex.Message, "Lỗi khởi động", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void StopWebApi()
        {
            try
            {
                if (_apiProcess != null && !_apiProcess.HasExited)
                {
                    _apiProcess.Kill();
                    _apiProcess.Dispose();
                }
            }
            catch
            {
                // Ignore
            }
        }
    }
}
