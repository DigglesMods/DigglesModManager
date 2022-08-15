using DigglesModManager.Properties;
using System;
using System.Windows.Forms;

namespace DigglesModManager
{
    public static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new FormMain());
            }
            catch (System.IO.FileNotFoundException e)
            {
                Helpers.ShowErrorMessage(Resources.FormMain_CouldNotFindFile.Replace("FILENAME", e.FileName));
            }

        }
    }
}
