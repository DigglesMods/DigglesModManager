using DigglesModManager.Properties;
using System.Diagnostics;
using System.Windows.Forms;

namespace DigglesModManager
{
    public class Helpers
    {
        public static void OpenWebPage(string uri)
        {
            // .NET (Core) defaults UseShellExecute to false, which would try to run the URL as an executable
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }

        public static void ExitApplication()
        {
            Application.Exit();
        }

        public static void ShowMessage(string text, string title)
        {
            MessageBox.Show(text, title);
        }

        public static void ShowErrorMessage(string text)
        {
            ShowMessage(text, Resources.Error);
        }
    }
}
