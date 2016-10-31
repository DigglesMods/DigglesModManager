using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DigglesModManager.Model
{
    /// <summary>
    /// Holds settings for one specific mod. Contains several optional variables.
    /// </summary>
    public class ModSettings
    {
        /// <summary>
        /// The variables of a mod which can be tuned by the user.
        /// </summary>
        [JsonProperty(PropertyName = "variables")]
        public List<ModSettingsVariable> Variables { get; set; } = new List<ModSettingsVariable>();
    }
}
