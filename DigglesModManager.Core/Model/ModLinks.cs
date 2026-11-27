using System.Text.Json.Serialization;

namespace DigglesModManager.Model
{
    /// <summary>
    /// contains different translations of a string. english is required
    /// </summary>
    public class ModLinks
    {
        /// <summary>
        /// download link
        /// </summary>
        [JsonPropertyName("download")]
        public string Download { get; set; }

        /// <summary>
        /// wiki link
        /// </summary>
        [JsonPropertyName("wiki")]
        public string Wiki { get; set; }
    }
}
