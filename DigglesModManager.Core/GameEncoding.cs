using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace DigglesModManager
{
    /// <summary>
    /// Encodings used to read and write the different kinds of files.
    /// </summary>
    /// <remarks>
    /// The game files (messages.txt, *.tcl, ...) are stored in a single byte ANSI code page
    /// (cp1252 for most languages, cp1250 for cz/pl, cp1251 for ru). The .NET Framework versions of the
    /// DigglesModManager used <c>Encoding.Default</c>, which was the ANSI code page of the system.
    /// Since .NET Core <c>Encoding.Default</c> is UTF-8, which destroys umlauts and other special characters.
    /// Therefore the ANSI code page of the system is determined explicitly here.
    /// All single byte code pages used here map each byte to exactly one character, so reading and writing
    /// a file keeps all bytes that were not changed by a mod intact - independent of the game language.
    /// The code page is only relevant for non-ASCII characters inserted by mods (e.g. "$print:" variables).
    /// </remarks>
    public static class GameEncoding
    {
        private const int FallbackCodePage = 1252;

        private static readonly Lazy<Encoding> _ansi = new Lazy<Encoding>(CreateAnsiEncoding);

        /// <summary>
        /// Encoding of the game files and of the mod files that are copied into or merged with game files.
        /// </summary>
        public static Encoding Ansi => _ansi.Value;

        /// <summary>
        /// Encoding of JSON files (mod config.json, diggles-mod-manager.json). Reading detects a BOM.
        /// </summary>
        public static readonly Encoding Json = new UTF8Encoding(false);

        /// <summary>
        /// Encoding of the log file.
        /// </summary>
        public static readonly Encoding Log = new UTF8Encoding(false);

        private static Encoding CreateAnsiEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var codePage = DetectAnsiCodePage();
            try
            {
                return Encoding.GetEncoding(codePage);
            }
            catch (Exception)
            {
                return Encoding.GetEncoding(FallbackCodePage);
            }
        }

        private static int DetectAnsiCodePage()
        {
            // Same semantic as Encoding.Default of the .NET Framework: the ANSI code page of the system.
            // This also works under Wine, which derives the code page from the locale (LANG).
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    var acp = (int)GetACP();
                    if (IsSupportedSingleByteCodePage(acp))
                    {
                        return acp;
                    }
                }
                catch (Exception)
                {
                    //ignore and use fallback
                }
            }
            var cultureCodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
            if (IsSupportedSingleByteCodePage(cultureCodePage))
            {
                return cultureCodePage;
            }
            // e.g. UTF-8 (65001) when "Beta: Use Unicode UTF-8 for worldwide language support" is active,
            // or a multi byte code page. Both would destroy the game files.
            return FallbackCodePage;
        }

        /// <summary>
        /// Windows single byte code pages: Thai (874) and 1250-1258.
        /// </summary>
        private static bool IsSupportedSingleByteCodePage(int codePage)
        {
            return codePage == 874 || (codePage >= 1250 && codePage <= 1258);
        }

        [DllImport("kernel32.dll")]
        private static extern uint GetACP();
    }
}
