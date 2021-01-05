using DigglesModManager.Model;
using DigglesModManager.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DigglesModManager
{
    public class Mod : IToolTipDisplayer, IComparable
    {
        public string ModDirectoryName { get; private set; }

        public DirectoryInfo ModDirectoryInfo { get; private set; }

        public ModConfig Config { get; private set; }

        public string ToolTipText { get; private set; }
        public string Author { get; private set; }

        public int FileCount { get; private set; }

        public Mod(string modDirectory) : this(modDirectory, new Dictionary<string, object>())
        {
        }

        // Constructor
        public Mod(string modDirectory, Dictionary<string, object> oldSettings)
        {
            ModDirectoryName = modDirectory;
            // the last backslash is important for path cutting!
            ModDirectoryInfo = new DirectoryInfo($"{Paths.ModPath}\\{Paths.ModDirectoryName}\\{ModDirectoryName}\\");
            FileCount = ModDirectoryInfo.GetFiles("*", SearchOption.AllDirectories).Length;

            //get description
            ToolTipText = "";
            Author = "";

            //test for config file and read it
            var modFiles = ModDirectoryInfo.GetFiles();

            foreach (var modFile in modFiles)
            {
                var filename = modFile.Name;
                if (filename.Equals(Paths.ModConfigName))
                {
                    ReadModConfigAndMergeWithOldValues(modFile, oldSettings);
                }
            }

            if (Config == null)
            {
                Config = new ModConfig()
                {
                    Name = new ModTranslationString($"{ modDirectory }-ERROR"),
                    Author = "",
                    Description = new ModTranslationString("")
                };
            }
        }

        /// <summary>
        /// Reads from the given JSON-File and parses the content to ModSettings. The resulting Settings-Variables 
        /// are then merged with the given oldSettings.
        /// Opens the json file containing the metadata for a specific mod and parses the content to ModMetaData.
        /// If there is any Error, an Error Message is shown.
        /// </summary>
        /// <param name="oldSettings">The old/pre-existing variable settings.</param>
        /// <param name="modFile">The file to be read from.</param>
        private void ReadModConfigAndMergeWithOldValues(FileInfo modFile, Dictionary<string, object> oldSettings)
        {
            try
            {
                var json = File.ReadAllText(modFile.FullName, Encoding.Default);
                Config = JsonConvert.DeserializeObject<ModConfig>(json);

                foreach (var modSettingsVariable in Config.SettingsVariables)
                {
                    //check saved application status and overwrite with values of last session or set the default value
                    object oldValue = null;
                    if (!oldSettings.TryGetValue(modSettingsVariable.ID, out oldValue) // set oldValue at success
                        || oldValue == null)
                        oldValue = modSettingsVariable.DefaultValue;

                    switch (modSettingsVariable.Type)
                    {
                        case ModVariableType.Bool:
                            oldValue = bool.Parse(oldValue.ToString());
                            break;
                        case ModVariableType.Int:
                            oldValue = int.Parse(oldValue.ToString());
                            break;
                        default:
                        case ModVariableType.String:
                            break;
                    }
                    modSettingsVariable.Value = oldValue;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Could not parse config-file of '{ModDirectoryName}'!\nError is {ex}");
            }
        }

        // Returns the mod messages.
        public List<ModMessage> GetMessages()
        {
            var messages = new List<ModMessage>(Config.Messages);
            //TODO add incompatibility warnings & errors
            messages.Sort();
            return messages;
        }

        // Returns the display text of this item.
        public override string ToString()
        {
            return Config.Name.getString(FormMain._language);
        }

        // Returns the mod author.
        private string GetAuthor()
        {
            if (!Author.Equals(""))
            {
                return Author;
            }
            else if (!string.IsNullOrEmpty(Config.Author))
            {
                return Config.Author;
            }
            return null;
        }

        // Returns the mod messages as listed string with an initial \n.
        private string GetMessagesString()
        {
            var result = "";
            foreach (var message in GetMessages())
            {
                result += $"\n- {message.Type}: {message.Content.getString(FormMain._language)}";
            }
            return result;
        }

        // Returns the tooltip text of this mod.
        public string GetToolTipText()
        {
            var text = ToolTipText;
            if (!string.IsNullOrWhiteSpace(Config.Description.getString(FormMain._language)))
            {
                text = Config.Description.getString(FormMain._language);
            }
            var messages = GetMessagesString();
            if (!string.IsNullOrWhiteSpace(messages))
            {
                if (!string.IsNullOrWhiteSpace(text)) text += "\n\n";
                text += $"Messages:{messages}";
            }
            return text;
        }

        // Returns the description of this mod.
        public string GetDescription()
        {
            var text = ToString();
            if (!string.IsNullOrWhiteSpace(Config.Description.getString(FormMain._language)))
            {
                text += "\n\n";
                text += Config.Description.getString(FormMain._language);
            }
            var messages = GetMessagesString();
            if (!string.IsNullOrWhiteSpace(messages))
            {
                text += "\n\n";
                text += $"Messages:{messages}";
            }
            var author = GetAuthor();
            if (!string.IsNullOrWhiteSpace(author))
            {
                text += "\n\n";
                text += $"Author: {author}";
            }
            return text;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            var element = (Mod)obj;
            return ToString().Equals(element.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;

            var element = (Mod)obj;
            return string.Compare(ToString(), element.ToString(), StringComparison.Ordinal);
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, Resources.Error);
        }

    }
}
