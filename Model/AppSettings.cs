using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    /// <summary>
    /// Data-Model-Object for an application-status. It holds the Application Settings, such as activated mods.
    /// </summary>
    public class AppSettings
    {
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "activeMods")]
        public Dictionary<string, Dictionary<string, object>> ActiveMods { get; set; } = new Dictionary<string, Dictionary<string, object>>();
    }
}
