using System.Text.Json.Serialization;
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
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// The author(s) of the mod.
        /// </summary>
        [JsonPropertyName("author")]
        public string Author { get; set; }

        /// <summary>
        /// The name of the mod.
        /// </summary>
        [JsonPropertyName("name"), JsonRequired]
        public ModTranslationString Name { get; set; }

        /// <summary>
        /// A short description of the mod.
        /// </summary>
        [JsonPropertyName("description")]
        public ModTranslationString Description { get; set; }

        /// <summary>
        /// A short description of the mod.
        /// </summary>
        [JsonPropertyName("links")]
        public ModLinks Links { get; set; }

        /// <summary>
        /// A list of messages, that are shown to the user to inform about specific mod behavior. (incompatibilities, ...)
        /// </summary>
        [JsonPropertyName("messages")]
        public List<ModMessage> Messages { get; set; } = new List<ModMessage>();

        /// <summary>
        /// The variables of a mod which can be tuned by the user.
        /// </summary>
        [JsonPropertyName("settings")]
        public List<ModSettingsVariable> SettingsVariables { get; set; } = new List<ModSettingsVariable>();

        /// <summary>
        /// Other mods this mod depends on (required or optional).
        /// </summary>
        [JsonPropertyName("dependencies")]
        public List<ModDependency> Dependencies { get; set; } = new List<ModDependency>();

        /// <summary>
        /// Other mods that cannot be used together with this mod (applies in both directions).
        /// </summary>
        [JsonPropertyName("incompatibilities")]
        public List<ModIncompatibility> Incompatibilities { get; set; } = new List<ModIncompatibility>();

        /// <summary>
        /// A list of directories, that are handled in a different way.
        /// </summary>
        [JsonPropertyName("directories")]
        public List<ModDirectory> Directories { get; set; } = new List<ModDirectory>();
    }
}
