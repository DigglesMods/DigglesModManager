using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    public class ModSettings
    {
        [JsonProperty(PropertyName = "variables")]
        public List<ModSettingsVariable> Variables { get; private set; }
    }
}
