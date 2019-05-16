using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string ID { get; set; }

        /// <summary>
        /// The human readanle name of the variable. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName="name", Required = Required.Always)]
        private ModTranslationString NameTranslation { get; set; }

        public string Name(string language)
        {
            return NameTranslation.getString(language);
        }

        /// <summary>
        /// A short description for the ui of the mod-variable. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "description", Required = Required.Always)]
        private ModTranslationString DescriptionTranslation { get; set; }

        public string Description(string language)
        {
            return DescriptionTranslation.getString(language);
        }

        /// <summary>
        /// The data-type of the variable. Can be bool, int.. [REQUIRED]
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty(PropertyName="type", Required = Required.Always)]
        public ModVariableType Type { get; set; }

        /// <summary>
        /// The default value, which is present in Diggles-sourcecode aswell. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "default", Required = Required.Always)]
        public object DefaultValue { get; set; }

        /// <summary>
        /// The actual value. Is the default or a loaded value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// A minimum value. (only for int variables)
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "min", Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public object Min { get; set; }

        /// <summary>
        /// A maximum value. (only for int variables)
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "max", Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public object Max { get; set; }

        /// <summary>
        /// A List of possible values for this variable. (only for Select_Int and Select_String)
        /// </summary>
        [JsonProperty(PropertyName = "possibleValues", Required = Required.Default)]
        public List<ModVariableValue> PossibleValues { get; set; } = new List<ModVariableValue>();
    }
}
