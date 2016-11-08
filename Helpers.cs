using System.Diagnostics;
using System.Windows.Forms;
using DigglesModManager.Properties;

namespace DigglesModManager
{
    public class Helpers
    {
        public static void OpenWebPage(string uri)
        {
            Process.Start(new ProcessStartInfo(uri));
        }

        public static void ExitApplication()
        {
            Application.Exit();
        }

        public static void ShowErrorMessage(string text, string title = null)
        {
            if (title == null)
                title = Resources.Error;
            MessageBox.Show(text, title);
        }
    }
}
