using System;
using System.IO;
using System.Linq;

namespace DigglesModManager
{
    /// <summary>
    /// Holds several constant file-paths and directory names.
    /// </summary>
    public static class Paths
    {
        private static string _exePath = Path.GetFullPath(".");

        /// <summary>
        /// The game directory (containing Diggles.exe/Wiggles.exe). Always an absolute path.
        /// </summary>
        public static string ExePath
        {
            get => _exePath;
            set => _exePath = Path.GetFullPath(value);
        }

        /// <summary>
        /// The directory containing the mod directory (normally the game directory).
        /// </summary>
        public static string ModPath => ExePath;
        public static string ModDirectoryName = "Mods";
        public static string[] DigglesExecutableNames = { "Diggles.exe", "Wiggles.exe" };
        public static string DataPath => $"{ExePath}\\Data";

        //.dm
        public static string RestoreFileName = "restore.dm";

        //JSON
        public static string AppSettingsName = "diggles-mod-manager.json";
        public static string ModConfigName = "config.json";

        //log
        public static string ErrorLogFileName = "diggles-mod-manager-errors.log";

        /// <summary>
        /// Checks if the given directory contains the game executable.
        /// </summary>
        public static bool IsGameDirectory(string directory)
        {
            return DigglesExecutableNames.Any(exe => File.Exists(Path.Combine(directory, exe)));
        }

        /// <summary>
        /// Sets <see cref="ExePath"/> to the game directory: the current working directory if it contains the game,
        /// otherwise the directory of the mod manager executable (e.g. if a launcher like Lutris/Proton starts
        /// the mod manager with another working directory).
        /// </summary>
        /// <returns>true, if a game directory was found</returns>
        public static bool DetectGameDirectory()
        {
            var candidates = new[] { Environment.CurrentDirectory, AppContext.BaseDirectory };
            foreach (var candidate in candidates)
            {
                if (!string.IsNullOrEmpty(candidate) && IsGameDirectory(candidate))
                {
                    ExePath = candidate;
                    return true;
                }
            }
            ExePath = Environment.CurrentDirectory;
            return false;
        }
    }
}
