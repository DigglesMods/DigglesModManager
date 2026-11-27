using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace DigglesModManager.Model
{
    /// <summary>
    /// One variable of a mod which can be tuned by the user.
    /// </summary>
    public class ModSettingsVariable
    {
        /// <summary>
        /// The mod-unique name/id of the variable. Is referenced/used in tcl-code aswell. [REQUIRED]
        /// </summary>
        [JsonPropertyName("id"), JsonRequired]
        public string ID { get; set; }

        /// <summary>
        /// The human readanle name of the variable. [REQUIRED]
        /// </summary>
        [JsonPropertyName("name"), JsonRequired, JsonInclude]
        private ModTranslationString NameTranslation { get; set; }

        public string Name(string language)
        {
            return NameTranslation.getString(language);
        }

        /// <summary>
        /// A short description for the ui of the mod-variable. [REQUIRED]
        /// </summary>
        [JsonPropertyName("description"), JsonRequired, JsonInclude]
        private ModTranslationString DescriptionTranslation { get; set; }

        public string Description(string language)
        {
            return DescriptionTranslation.getString(language);
        }

        /// <summary>
        /// The data-type of the variable. Can be bool, int.. [REQUIRED]
        /// </summary>
        [JsonPropertyName("type"), JsonRequired]
        public ModVariableType Type { get; set; }

        /// <summary>
        /// The default value, which is present in Diggles-sourcecode aswell. [REQUIRED]
        /// </summary>
        [JsonPropertyName("default"), JsonRequired]
        public object DefaultValue { get; set; }

        /// <summary>
        /// The actual value. Is the default or a loaded value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// A minimum value. (only for int variables)
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("min")]
        public object Min { get; set; }

        /// <summary>
        /// A maximum value. (only for int variables)
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("max")]
        public object Max { get; set; }

        /// <summary>
        /// A List of possible values for this variable. (only for Select_Int and Select_String)
        /// </summary>
        [JsonPropertyName("possibleValues")]
        public List<ModVariableValue> PossibleValues { get; set; } = new List<ModVariableValue>();
    }
}
