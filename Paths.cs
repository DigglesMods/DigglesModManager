using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigglesModManager
{
    public class Paths
    {
        public static string ExePath = @".";                //dyn: @"." | local: D:\Programme\Wiggles
        public static string ModPath = ExePath;             //dyn: exePath | local: @"D:\Projekte\DigglesModManager"
        public static string ModDirectoryName = "Mods";
        public static string ActiveModsFileName = "mods.dm";
        public static string RestoreFileName = "restore.dm";
        public static string ModSettingsFileName = "settings.dm";
        public static string ModDescriptionFileName = "description.dm";
        public static string WigglesExecutableName = "Wiggles.exe";
    }
}
