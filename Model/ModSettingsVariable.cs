using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigglesModManager.Model
{
    public class ModSettingsVariable
    {
        [JsonProperty(PropertyName="name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter)), JsonProperty(PropertyName="type", Required = Required.Always)]
        public ModVariableType Type { get; set; }

        [JsonProperty(PropertyName = "value", Required = Required.Always)]
        public object Value { get; set; }

        [JsonProperty(PropertyName = "default", Required = Required.Always)]
        public object DefaultValue { get; set; }

        [JsonProperty(PropertyName = "min", Required = Required.Default)]
        public object Min { get; set; }

        [JsonProperty(PropertyName = "max", Required = Required.Default)]
        public object Max { get; set; }
    }
}
