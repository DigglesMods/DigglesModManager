using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    /// <summary>
    /// contains different translations of a string. english is required
    /// </summary>
    public class ModTranslationString
    {
        /// <summary>
        /// german translation
        /// </summary>
        [JsonProperty(PropertyName = "de", Required = Required.Default)]
        public string de { get; set; }

        /// <summary>
        /// english translation [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "en", Required = Required.Always)]
        public string en { get; set; }

        /// <summary>
        /// spanish translation
        /// </summary>
        [JsonProperty(PropertyName = "es", Required = Required.Default)]
        public string es { get; set; }

        /// <summary>
        /// french translation
        /// </summary>
        [JsonProperty(PropertyName = "fr", Required = Required.Default)]
        public string fr { get; set; }

        /// <summary>
        /// italian translation
        /// </summary>
        [JsonProperty(PropertyName = "it", Required = Required.Default)]
        public string it { get; set; }

        /// <summary>
        /// dutch translation
        /// </summary>
        [JsonProperty(PropertyName = "nl", Required = Required.Default)]
        public string nl { get; set; }

        /// <summary>
        /// polish translation
        /// </summary>
        [JsonProperty(PropertyName = "pl", Required = Required.Default)]
        public string pl { get; set; }
        
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
            string str = null;
            switch (language)
            {
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
