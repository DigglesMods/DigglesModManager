using System.Text.Json.Serialization;
using System;

namespace DigglesModManager.Model
{
    /// <summary>
    /// Contains a message which is shown to inform a user.
    /// </summary>
    public class ModMessage : IComparable
    {
        /// <summary>
        /// type
        /// </summary>
        [JsonPropertyName("type")]
        public ModMessageType Type { get; set; } = ModMessageType.Info;

        /// <summary>
        /// content
        /// </summary>
        [JsonPropertyName("content"), JsonRequired]
        public ModTranslationString Content { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;

            var element = (ModMessage)obj;
            return Type - element.Type;
        }
    }
}
