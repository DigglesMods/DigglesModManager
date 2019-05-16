namespace DigglesModManager
{
    /// <summary>
    /// Holds several constant file-paths and directory names.
    /// </summary>
    public class Paths
    {
        public static string ExePath = @".";                //dyn: @"." | local:  @"D:\Programme\Wiggles-Test"
        public static string ModPath = ExePath;             //dyn: ExePath | local: @"D:\Projekte\DigglesMods\DigglesModManager"
        public static string ModDirectoryName = "Mods";
        public static string[] DigglesExecutableNames = { "Diggles.exe", "Wiggles.exe" };

        //.dm
        public static string RestoreFileName = "restore.dm";

        //JSON
        public static string AppSettingsName = "diggles-mod-manager.json";
        public static string ModConfigName = "config.json";

        //log
        public static string ErrorLogFileName = "diggles-mod-manager-errors.log";
    }
}
