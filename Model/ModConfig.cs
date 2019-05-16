using Newtonsoft.Json;
using System.Collections.Generic;

namespace DigglesModManager.Model
{
    /// <summary>
    /// MetaData of a Diggles-Modification.
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// The version of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// The author(s) of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        /// <summary>
        /// The name of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public ModTranslationString Name { get; set; }

        /// <summary>
        /// A short description of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public ModTranslationString Description { get; set; }

        /// <summary>
        /// A short description of the mod.
        /// </summary>
        [JsonProperty(PropertyName = "links")]
        public ModLinks Links { get; set; }

        /// <summary>
        /// The variables of a mod which can be tuned by the user.
        /// </summary>
        [JsonProperty(PropertyName = "settings")]
        public List<ModSettingsVariable> SettingsVariables { get; set; } = new List<ModSettingsVariable>();

        /// <summary>
        /// A list of directories, that are handled in a different way.
        /// </summary>
        [JsonProperty(PropertyName = "directories")]
        public List<ModDirectory> Directories { get; set; } = new List<ModDirectory>();
    }
}
