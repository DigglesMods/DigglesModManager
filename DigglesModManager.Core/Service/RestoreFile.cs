using System;
using System.Collections.Generic;
using System.IO;

namespace DigglesModManager.Service
{
    /// <summary>
    /// Reads and writes the restore file (restore.dm), which remembers all game files changed by the mod manager.
    /// </summary>
    /// <remarks>
    /// Each line has the format <c>type||path</c>. type is "res" (restore the original from "path_copy") or
    /// "del" (delete the file added by a mod).
    /// Since version 1.4 the paths are written relative to the game directory. Older versions wrote absolute paths
    /// (e.g. <c>res||D:\Games\Wiggles\Data\messages.txt</c>), which break if the game directory was moved or copied
    /// or if Wine maps the drives differently. Absolute paths are still supported while reading.
    /// </remarks>
    public static class RestoreFile
    {
        private const string Separator = "||";

        public sealed class Entry
        {
            public string Type { get; }

            /// <summary>
            /// Absolute path of the file in the (current) game directory.
            /// </summary>
            public string FullPath { get; }

            public Entry(string type, string fullPath)
            {
                Type = type;
                FullPath = fullPath;
            }
        }

        /// <summary>
        /// Appends an entry to the restore file. The path is stored relative to the game directory if possible.
        /// </summary>
        public static void Append(string gameDirectory, string restoreFilePath, string type, string fileFullPath)
        {
            using (var writer = new StreamWriter(restoreFilePath, true, GameEncoding.Ansi))
            {
                writer.WriteLine($"{type}{Separator}{ToStoredPath(gameDirectory, fileFullPath)}");
            }
        }

        /// <summary>
        /// Converts an absolute file path into the path stored in the restore file.
        /// </summary>
        public static string ToStoredPath(string gameDirectory, string fileFullPath)
        {
            var relativePath = Path.GetRelativePath(Path.GetFullPath(gameDirectory), fileFullPath);
            if (Path.IsPathRooted(relativePath) || relativePath == ".." || relativePath.StartsWith("..\\") || relativePath.StartsWith("../"))
            {
                // file is outside of the game directory (should not happen): keep the absolute path
                return fileFullPath;
            }
            return relativePath;
        }

        /// <summary>
        /// Reads all entries of the restore file and resolves their paths in the given game directory.
        /// </summary>
        public static List<Entry> Read(string gameDirectory, string restoreFilePath)
        {
            var gameDirectoryFullPath = Path.GetFullPath(gameDirectory);
            var entries = new List<Entry>();
            string oldGameDirectory = null;
            foreach (var line in File.ReadAllLines(restoreFilePath, GameEncoding.Ansi))
            {
                var separatorIndex = line.IndexOf(Separator, StringComparison.Ordinal);
                if (separatorIndex <= 0)
                {
                    continue;
                }
                var type = line.Substring(0, separatorIndex);
                var storedPath = line.Substring(separatorIndex + Separator.Length).Trim();
                if (storedPath.Length == 0)
                {
                    continue;
                }
                var fullPath = ResolveStoredPath(gameDirectoryFullPath, storedPath, ref oldGameDirectory);
                entries.Add(new Entry(type, fullPath));
            }
            return entries;
        }

        /// <summary>
        /// Resolves a path of the restore file to an absolute path in the current game directory.
        /// </summary>
        /// <param name="gameDirectory">absolute path of the current game directory</param>
        /// <param name="storedPath">path as stored in the restore file (relative or absolute)</param>
        /// <param name="oldGameDirectory">cache of the old game directory detected for absolute paths of older versions</param>
        public static string ResolveStoredPath(string gameDirectory, string storedPath, ref string oldGameDirectory)
        {
            if (!Path.IsPathRooted(storedPath))
            {
                return Path.GetFullPath(Path.Combine(gameDirectory, storedPath));
            }

            // absolute path (written by version 1.3.x or older)
            if (IsInDirectory(storedPath, gameDirectory))
            {
                return storedPath;
            }

            // The game directory was moved/copied or the drive letter changed (e.g. Wine).
            // Use the old game directory detected for a previous entry.
            if (oldGameDirectory != null && IsInDirectory(storedPath, oldGameDirectory))
            {
                return Path.Combine(gameDirectory, storedPath.Substring(oldGameDirectory.Length).TrimStart('\\', '/'));
            }

            // Detect the old game directory: remove leading directories until the remaining relative path
            // exists in the current game directory (as file or as backup copy).
            var parts = storedPath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 1; i < parts.Length; i++)
            {
                var relativePath = string.Join("\\", parts, i, parts.Length - i);
                var candidate = Path.Combine(gameDirectory, relativePath);
                if (File.Exists(candidate) || File.Exists(candidate + ModdingService.CopyFileSuffix))
                {
                    var oldRoot = string.Join("\\", parts, 0, i);
                    oldGameDirectory = storedPath.StartsWith("\\\\") ? "\\\\" + oldRoot : oldRoot;
                    return candidate;
                }
            }

            // could not be resolved: keep the path (it is skipped if it does not exist)
            Log.Warning($"Could not resolve path \"{storedPath}\" of {Paths.RestoreFileName} in the game directory \"{gameDirectory}\"");
            return storedPath;
        }

        private static bool IsInDirectory(string path, string directory)
        {
            var normalizedDirectory = directory.TrimEnd('\\', '/') + "\\";
            return path.Replace('/', '\\').StartsWith(normalizedDirectory, StringComparison.OrdinalIgnoreCase);
        }
    }
}
