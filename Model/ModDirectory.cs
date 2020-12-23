using Newtonsoft.Json;
using System.ComponentModel;

namespace DigglesModManager.Model
{
    public class ModDirectory
    {
        /// <summary>
        /// The type of the directory. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public ModDirectoryType Type { get; set; }

        /// <summary>
        /// Path of directory relative to mod root. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "path", Required = Required.Always)]
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
        [JsonProperty(PropertyName = "condition", Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public ModDirectoryCondition Condition { get; set; }

    }
}
