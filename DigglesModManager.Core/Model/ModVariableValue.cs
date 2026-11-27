using System.Text.Json.Serialization;

namespace DigglesModManager.Model
{
    public class ModVariableValue
    {

        /// <summary>
        /// The value that is used for modding. [REQUIRED]
        /// </summary>
        [JsonPropertyName("value"), JsonRequired]
        public object Value { get; set; }

        public string Name(string language)
        {
            return NameTranslation.getString(language);
        }

        /// <summary>
        /// The human readanle name of the value. [REQUIRED]
        /// </summary>
        [JsonPropertyName("name"), JsonRequired, JsonInclude]
        private ModTranslationString NameTranslation { get; set; }

    }
}
