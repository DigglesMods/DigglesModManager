using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    public class ModVariableValue
    {

        /// <summary>
        /// The value that is used for modding. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "value", Required = Required.Always)]
        public object Value { get; set; }

        public string Name(string language)
        {
            return NameTranslation.getString(language);
        }

        /// <summary>
        /// The human readanle name of the value. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        private ModTranslationString NameTranslation { get; set; }

    }
}
