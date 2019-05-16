using System;
using System.IO;
using System.Text;

namespace DigglesModManager
{
    class Log
    {
        private const string ERROR = "ERROR";
        private const string WARNING = "WARNING";

        private static void LogToErrorFile(string text)
        {
            var writer = new StreamWriter(Paths.ExePath + "\\" + Paths.ErrorLogFileName, true, Encoding.Default);
            writer.WriteLine(DateTime.Now + " - " + text);
            writer.Flush();
            writer.Close();
        }

        private static void LogToErrorFile(string type, string message, FileInfo fileInfo, int line)
        {
            LogToErrorFile(type + " in \"" + fileInfo.FullName + "\" at line " + line + ": " + message);
        }

        public static void Error(string message)
        {
            LogToErrorFile(ERROR + " " + message);
        }

        public static void Error(string message, FileInfo fileInfo, int line)
        {
            LogToErrorFile(ERROR, message, fileInfo, line);
        }

        public static void Warning(string message)
        {
            LogToErrorFile(WARNING + " " + message);
        }

        public static void Warning(string message, FileInfo fileInfo, int line)
        {
            LogToErrorFile(WARNING, message, fileInfo, line);
        }
    }

}
