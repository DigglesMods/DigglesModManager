using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    /// <summary>
    /// contains different translations of a string. english is required
    /// </summary>
    public class ModLinks
    {
        /// <summary>
        /// download link
        /// </summary>
        [JsonProperty(PropertyName = "download", Required = Required.Default)]
        public string Download { get; set; }

        /// <summary>
        /// wiki link
        /// </summary>
        [JsonProperty(PropertyName = "wiki", Required = Required.Default)]
        public string Wiki { get; set; }
    }
}
