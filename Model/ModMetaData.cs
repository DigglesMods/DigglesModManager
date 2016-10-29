using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    public class ModMetaData
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; private set; }
    }
}
