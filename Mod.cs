using DigglesModManager.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DigglesModManager.Properties;

namespace DigglesModManager
{
    public class Mod : IToolTipDisplayer, IComparable
    {
        public string ModDirectoryName { get; private set; }

        public ModMetaData MetaData { get; private set; }
        public ModSettings Settings { get; private set; } = new ModSettings();

        public string DisplayText { get; private set; }
        public string ToolTipText { get; private set; }
        public string Author { get; private set; }

        public List<ModVar> Vars { get; private set; }

        public Mod(string modDirectory, ModSettings modSettings)
            : this(modDirectory, DecodeModSettings(modSettings))
        {
        }

        private static string DecodeModSettings(ModSettings modSettings)
        {
            return modSettings.Variables.Aggregate("", (current, variable) => current + $"{variable.Name}:{variable.Value};");
        }

        // Constructor
        public Mod(string modDirectory, string oldSettings)
        {
            ModDirectoryName = modDirectory;

            //get description
            var modDirectoryInfo = new DirectoryInfo(Paths.ExePath + "\\" + Paths.ModDirectoryName + "\\" + ModDirectoryName);
            DisplayText = modDirectory;
            ToolTipText = "";
            Author = "";
            Vars = new List<ModVar>();

            //test for description file and settings file and read it
            var modFiles = modDirectoryInfo.GetFiles();

            foreach (var modFile in modFiles)
            {
                var filename = modFile.Name;
                if (filename.Equals(Paths.ModDescriptionName)) //.json-Format
                {
                    ReadModMetaDataFromJson(modFile);
                }
                else if (filename.Equals(Paths.ModSettingsName))
                {
                    ReadModSettingsFromJsonAndMergeWithOldValues(oldSettings, modFile);
                }
                else if (filename.Equals(Paths.ModDescriptionFileName)) //.dm-Settings-Format
                {
                    //read mod description
                    ReadModDescriptionFromDm(modFile);
                }
                else if (filename.Equals(Paths.ModSettingsFileName))
                {
                    ReadModSettingsFromDm(oldSettings, modFile);
                }
            }

            if (MetaData == null)
            {
                MetaData = new ModMetaData()
                {
                    Name = $"{ modDirectory }-ERROR",
                    Author = "",
                    Description = ""
                };
            }
        }

        /// <summary>
        /// Reads from the given JSON-File and parses the content to ModSettings. The resulting Settings-Variables 
        /// are then merged with the given oldSettings.
        /// </summary>
        /// <param name="oldSettings">The old/pre-existing variable settings.</param>
        /// <param name="modFile">The file to be read from.</param>
        private void ReadModSettingsFromJsonAndMergeWithOldValues(string oldSettings, FileInfo modFile)
        {
            try
            {
                var json = File.ReadAllText(modFile.FullName, Encoding.Default);
                Settings = JsonConvert.DeserializeObject<ModSettings>(json);

                foreach (var modSettingsVariable in Settings.Variables)
                {
                    //check saved application status and overwrite with values of last session
                    if (oldSettings == null)
                        continue;

                    object oldValue = GetVarElement(oldSettings, modSettingsVariable.Name + ":");
                    if (oldValue == null)
                        continue;

                    switch (modSettingsVariable.Type)
                    {
                        case ModVariableType.Bool:
                            oldValue = bool.Parse((string)oldValue);
                            break;
                        case ModVariableType.Int:
                            oldValue = int.Parse((string)oldValue);
                            break;
                        default:
                        case ModVariableType.String:
                            break;
                    }
                    modSettingsVariable.Value = oldValue;
                }
            }
            catch (JsonReaderException ex)
            {
                ShowErrorMessage($"Could not parse settings-file of '{ModDirectoryName}'!\nError is {ex}");
            }
        }

        /// <summary>
        /// Opens the json file containing the metadata for a specific mod and parses the content to ModMetaData.
        /// If there is any Error, an Error Message is shown.
        /// </summary>
        /// <param name="modFile">The File Info for the JSON-File</param>
        private void ReadModMetaDataFromJson(FileInfo modFile)
        {
            try
            {
                var json = File.ReadAllText(modFile.FullName, Encoding.Default);
                MetaData = JsonConvert.DeserializeObject<ModMetaData>(json);
            }
            catch (JsonReaderException ex)
            {
                ShowErrorMessage($"Could not parse metadata-file of '{ModDirectoryName}'!\nError is {ex}");
            }
        }

        /// <summary>
        /// LEGACY, DEPRECATED
        /// </summary>
        /// <param name="oldSettings"></param>
        /// <param name="modFile"></param>
        private void ReadModSettingsFromDm(string oldSettings, FileInfo modFile)
        {
            //reading mod settings
            var reader = new StreamReader(modFile.FullName, Encoding.Default);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //read var
                var varName = GetVarElement(line, "Var:");
                var type = GetVarElement(line, "Type:");
                var description = GetVarElement(line, "Description:");
                var gameValue = GetVarElement(line, "GameValue:");
                var stdValue = GetVarElement(line, "StdValue:");
                var minValue = GetVarElement(line, "MinValue:");
                var maxValue = GetVarElement(line, "MaxValue:");

                if (varName != null && type != null && gameValue != null && stdValue != null)
                {
                    //read old settings
                    var value = stdValue;
                    if (oldSettings != null)
                    {
                        value = GetVarElement(oldSettings, varName + ":");
                        if (value == null)
                        {
                            value = stdValue;
                        }
                    }
                    //generate mod var
                    ModVar modVar = null;
                    ModSettingsVariable modVariable = null;
                    if (type.Equals("int"))
                    {
                        if (minValue != null && maxValue != null)
                        {
                            modVar = new ModVar<int>(varName, type, description, int.Parse(gameValue), int.Parse(stdValue),
                                int.Parse(value), int.Parse(minValue), int.Parse(maxValue));
                            modVariable = new ModSettingsVariable()
                            {
                                Name = varName,
                                Type = ModVariableType.Int,
                                Description = description,
                                Value = int.Parse(gameValue),
                                DefaultValue = int.Parse(stdValue),
                                Min = int.Parse(minValue),
                                Max = int.Parse(maxValue)
                            };
                        }
                        else
                        {
                            modVar = new ModVar<int>(varName, type, description, int.Parse(gameValue), int.Parse(stdValue),
                                int.Parse(value));
                            modVariable = new ModSettingsVariable()
                            {
                                Name = varName,
                                Type = ModVariableType.Int,
                                Description = description,
                                Value = int.Parse(gameValue),
                                DefaultValue = int.Parse(stdValue)
                            };
                        }
                    }
                    else if (type.Equals("bool"))
                    {
                        modVar = new ModVar<bool>(varName, type, description, bool.Parse(gameValue), bool.Parse(stdValue),
                            bool.Parse(value));
                        modVariable = new ModSettingsVariable()
                        {
                            Name = varName,
                            Type = ModVariableType.Bool,
                            Description = description,
                            Value = bool.Parse(gameValue),
                            DefaultValue = bool.Parse(stdValue)
                        };
                    }
                    else if (type.Equals("string"))
                    {
                        modVar = new ModVar<string>(varName, type, description, gameValue, stdValue, value);
                        modVariable = new ModSettingsVariable()
                        {
                            Name = varName,
                            Type = ModVariableType.String,
                            Description = description,
                            Value = gameValue,
                            DefaultValue = stdValue
                        };
                    }

                    if (modVar != null)
                    {
                        //.dm-Format
                        Vars.Add(modVar);
                        //JSON-Format
                        Settings.Variables.Add(modVariable);
                    }
                }
            }
        }

        private void ReadModDescriptionFromDm(FileInfo modFile)
        {
            var reader = new StreamReader(modFile.FullName, Encoding.Default);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("name:"))
                {
                    DisplayText = line.Substring("name:".Length);
                }
                else if (line.StartsWith("tooltip:"))
                {
                    ToolTipText = line.Substring("tooltip:".Length);
                }
                else if (line.StartsWith("author:"))
                {
                    Author = line.Substring("author:".Length);
                }
            }
        }

        private static string GetVarElement(string line, string identifier)
        {
            var startIndex = line.IndexOf(identifier, StringComparison.Ordinal);
            if (startIndex >= 0)
            {
                startIndex += identifier.Length;
                var tmp = line.Substring(startIndex);
                var endIndex = tmp.IndexOf(';');
                if (endIndex > 0)
                {
                    return tmp.Substring(0, endIndex);
                }
                else
                {
                    return tmp;
                }
            }
            return null;
        }


        // Returns the display text of this item.
        public override string ToString()
        {
            return DisplayText;
        }
        // Returns the tooltip text of this item.
        public string GetToolTipText()
        {
            var toolTip = ToolTipText;
            if (!string.IsNullOrWhiteSpace(MetaData.Description))
                toolTip = MetaData.Description;
            if (!string.IsNullOrWhiteSpace(toolTip) && !string.IsNullOrWhiteSpace(Author))
            {
                toolTip += "\n";
            }
            if (!Author.Equals(""))
            {
                toolTip += "Author: " + Author;
            }
            else if (!string.IsNullOrEmpty(MetaData.Author))
            {
                toolTip += "Author: " + MetaData.Author;
            }

            return toolTip;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            var element = (Mod)obj;
            return DisplayText.Equals(element.DisplayText);
        }

        public override int GetHashCode()
        {
            return DisplayText.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;

            var element = (Mod)obj;
            return string.Compare(DisplayText, element.DisplayText, StringComparison.Ordinal);
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, Resources.Error);
        }

    }
}
