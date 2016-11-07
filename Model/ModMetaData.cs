using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    /// <summary>
    /// MetaData of a Diggles-Modification.
    /// </summary>
    public class ModMetaData
    {
        /// <summary>
        /// The name of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// A short description of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// The author(s) of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }
    }
}
