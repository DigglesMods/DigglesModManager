using System.Text.Json.Serialization;

namespace DigglesModManager.Model
{
    /// <summary>
    /// Another mod this mod depends on. The mod is always applied after the mods it depends on.
    /// </summary>
    /// <example>
    /// "dependencies": [ { "id": "BugFix" }, { "id": "DigglesHDmod", "required": false } ]
    /// </example>
    public class ModDependency
    {
        /// <summary>
        /// The directory name of the other mod (as in "$if:mod:"). [REQUIRED]
        /// </summary>
        [JsonPropertyName("id"), JsonRequired]
        public string Id { get; set; }

        /// <summary>
        /// true: the other mod is activated together with this mod and has to be installed.
        /// false (optional dependency): if the other mod is active, it is applied before this mod.
        /// Default: true
        /// </summary>
        [JsonPropertyName("required")]
        public bool Required { get; set; } = true;
    }
}
