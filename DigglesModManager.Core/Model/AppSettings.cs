using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DigglesModManager.Model
{
    /// <summary>
    /// Data-Model-Object for an application-status. It holds the Application Settings, such as activated mods.
    /// </summary>
    public class AppSettings
    {
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("activeMods")]
        public Dictionary<string, Dictionary<string, object>> ActiveMods { get; set; } = new Dictionary<string, Dictionary<string, object>>();
    }
}
