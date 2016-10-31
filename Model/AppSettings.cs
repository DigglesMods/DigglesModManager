using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    public class AppSettings
    {
        [JsonProperty(PropertyName = "activeMods")]
        public List<string> ActiveMods { get; set; } = new List<string>();
    }
}
