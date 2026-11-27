using System.Text.Json.Serialization;
using System.ComponentModel;

namespace DigglesModManager.Model
{
    public class ModDirectory
    {
        /// <summary>
        /// The type of the directory. [REQUIRED]
        /// </summary>
        [JsonPropertyName("type"), JsonRequired]
        public ModDirectoryType Type { get; set; }

        /// <summary>
        /// Path of directory relative to mod root. [REQUIRED]
        /// </summary>
        [JsonPropertyName("path"), JsonRequired]
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                //replace backslashes with slashes
                path = value.Replace("\\", "/");
            }
        }

        private string path;

        /// <summary>
        /// Condition that has to be true to add this directory.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("condition")]
        public ModDirectoryCondition Condition { get; set; }

    }
}
