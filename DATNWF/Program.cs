using System;
using System.Windows.Forms;

namespace DATNWF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Must be called before any IWin32Window is created — call ONCE, outside the loop
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
    }
}
