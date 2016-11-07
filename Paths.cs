namespace DigglesModManager
{
    /// <summary>
    /// Holds several constant file-paths and directory names.
    /// </summary>
    public class Paths
    {
        public static string ExePath = @".";                //dyn: @"." | local: D:\Programme\Wiggles
        public static string ModPath = ExePath;             //dyn: exePath | local: @"D:\Projekte\DigglesModManager"
        public static string ModDirectoryName = "Mods";
        public static string WigglesExecutableName = "Wiggles.exe";

        //.dm
        public static string ActiveModsFileName = "mods.dm";
        public static string RestoreFileName = "restore.dm";
        public static string ModSettingsFileName = "settings.dm";
        public static string ModDescriptionFileName = "description.dm";

        //JSON
        public static string AppSettingsName = AsJsonFileName("diggles-mod-manager");
        public static string ModDescriptionName = AsJsonFileName("metadata");
        public static string ModSettingsName = AsJsonFileName("settings");

        private static string AsJsonFileName(string fileName)
        {
            return $"{fileName}.json";
        }
    }
}
