using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public ModTranslationString Name { get; set; }

        /// <summary>
        /// A short description for the ui of the mod-variable. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "description", Required = Required.Always)]
        public ModTranslationString Description { get; set; }

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
        /// A maximum value. (only for int variables)
        /// </summary>
        [JsonProperty(PropertyName = "min", Required = Required.Default)]
        public object Min { get; set; }

        /// <summary>
        /// A minimum value. (only for int variables)
        /// </summary>
        [JsonProperty(PropertyName = "max", Required = Required.Default)]
        public object Max { get; set; }
    }
}
