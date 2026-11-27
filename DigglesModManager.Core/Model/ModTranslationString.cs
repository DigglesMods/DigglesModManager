using System.Text.Json.Serialization;

namespace DigglesModManager.Model
{
    /// <summary>
    /// contains different translations of a string. english is required
    /// </summary>
    public class ModTranslationString
    {
        /// <summary>
        /// czech translation
        /// </summary>
        [JsonPropertyName("cz")]
        public string cz { get; set; }

        /// <summary>
        /// german translation
        /// </summary>
        [JsonPropertyName("de")]
        public string de { get; set; }

        /// <summary>
        /// english translation [REQUIRED]
        /// </summary>
        [JsonPropertyName("en"), JsonRequired]
        public string en { get; set; }

        /// <summary>
        /// spanish translation
        /// </summary>
        [JsonPropertyName("es")]
        public string es { get; set; }

        /// <summary>
        /// french translation
        /// </summary>
        [JsonPropertyName("fr")]
        public string fr { get; set; }

        /// <summary>
        /// italian translation
        /// </summary>
        [JsonPropertyName("it")]
        public string it { get; set; }

        /// <summary>
        /// dutch translation
        /// </summary>
        [JsonPropertyName("nl")]
        public string nl { get; set; }

        /// <summary>
        /// polish translation
        /// </summary>
        [JsonPropertyName("pl")]
        public string pl { get; set; }

        /// <summary>
        /// russian translation
        /// </summary>
        [JsonPropertyName("ru")]
        public string ru { get; set; }

        /// <summary>
        /// Constructor used for JSON deserialization
        /// </summary>
        [JsonConstructor]
        public ModTranslationString()
        {
        }

        /// <summary>
        /// Constructor with intial englisch string
        /// </summary>
        public ModTranslationString(string enText)
        {
            en = enText;
        }

        /// <summary>
        /// get translated string (supported languages: de, en, er, fr, it, nl; default: en)
        /// </summary>
        public string getString(string language)
        {
            string str;
            switch (language)
            {
                case "cz":
                    str = cz;
                    break;
                case "de":
                    str = de;
                    break;
                case "en":
                    str = en;
                    break;
                case "es":
                    str = es;
                    break;
                case "fr":
                    str = fr;
                    break;
                case "it":
                    str = it;
                    break;
                case "nl":
                    str = nl;
                    break;
                case "pl":
                    str = pl;
                    break;
                case "ru":
                    str = ru;
                    break;
                default:
                    str = en;
                    break;
            }
            if (!string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            return en;
        }
    }
}
