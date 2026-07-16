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

            // TỰ ĐỘNG MÃ HÓA CẤU HÌNH KẾT NỐI QUA DPAPI
            EncryptConnectionString();

            // ĐĂNG KÝ BỘ XỬ LÝ LỖI TOÀN CỤC CHỐNG CRASH VÀ HẾT HẠN TOKEN
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

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

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void HandleException(Exception ex)
        {
            Exception current = ex;
            bool isUnauthorized = false;
            while (current != null)
            {
                if (current is UnauthorizedAccessException)
                {
                    isUnauthorized = true;
                    break;
                }
                current = current.InnerException;
            }

            if (isUnauthorized)
            {
                MessageBox.Show("Phiên đăng nhập đã hết hạn hoặc không hợp lệ. Vui lòng đăng nhập lại!", 
                                "Thông báo bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // XÓA TOKEN VÀ PHIÊN LÀM VIỆC CŨ
                DATNWF.Models.UserSession.Clear();

                // ĐÁNH DẤU CẦN RESTART ĐỂ HIỆN LẠI FRMLOGIN
                Home.NeedsRestart = true;

                // ĐÓNG FORM CHÍNH TRÊN UI THREAD
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Home)
                    {
                        if (form.InvokeRequired)
                        {
                            form.BeginInvoke(new Action(() => form.Close()));
                        }
                        else
                        {
                            form.Close();
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Hệ thống gặp lỗi không mong muốn: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static void EncryptConnectionString()
        {
            try
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                var section = config.GetSection("connectionStrings") as System.Configuration.ConnectionStringsSection;

                if (section != null && !section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("connectionStrings");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Không thể mã hóa cấu hình: " + ex.Message);
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
