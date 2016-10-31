using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    /// <summary>
    /// Data-Model-Object for an application-status. It holds the Application Settings, such as activated mods.
    /// </summary>
    public class AppSettings
    {
        [JsonProperty(PropertyName = "activeMods")]
        public List<string> ActiveMods { get; set; } = new List<string>();
    }
}
