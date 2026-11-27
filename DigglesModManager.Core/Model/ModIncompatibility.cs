using System.Text.Json.Serialization;

namespace DigglesModManager.Model
{
    /// <summary>
    /// How strict an incompatibility between two mods is.
    /// </summary>
    public enum ModIncompatibilityType
    {
        /// <summary>
        /// The mods cannot be active at the same time.
        /// </summary>
        Error,

        /// <summary>
        /// The mods can be active at the same time, but the user is warned: where both mods change the same code,
        /// the changes of the mod that is applied first (higher in the list of active mods) win.
        /// </summary>
        Warning
    }

    /// <summary>
    /// Another mod that is not (fully) compatible with this mod. The incompatibility applies in both directions,
    /// so it is enough if one of both mods declares it.
    /// </summary>
    /// <example>
    /// "incompatibilities": [ { "id": "CommandF9", "type": "warning", "reason": { "en": "Both mods change the F9 key." } } ]
    /// </example>
    public class ModIncompatibility
    {
        /// <summary>
        /// The directory name of the other mod (as in "$if:mod:"). [REQUIRED]
        /// </summary>
        [JsonPropertyName("id"), JsonRequired]
        public string Id { get; set; }

        /// <summary>
        /// "error" (default): the mods cannot be active at the same time.
        /// "warning": the mods can be active at the same time, the mod applied first wins.
        /// </summary>
        [JsonPropertyName("type")]
        public ModIncompatibilityType Type { get; set; } = ModIncompatibilityType.Error;

        /// <summary>
        /// Optional explanation, which is shown to the user.
        /// </summary>
        [JsonPropertyName("reason")]
        public ModTranslationString Reason { get; set; }
    }
}
