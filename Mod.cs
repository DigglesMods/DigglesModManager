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
    internal class Mod : IToolTipDisplayer, IComparable
    {
        public string ModDirectoryName { get; private set; }
        private DirectoryInfo ModDirectory;

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
            return modSettings.Variables.Aggregate("", (current, bla) => current + $"{bla.Name}:{bla.Value};");
        }

        // Constructor
        public Mod(string modDirectory, string oldSettings)
        {
            ModDirectoryName = modDirectory;

            //get description
            ModDirectory = new DirectoryInfo(Paths.ExePath + "\\" + Paths.ModDirectoryName + "\\" + ModDirectoryName);
            DisplayText = modDirectory;
            ToolTipText = "";
            Author = "";
            Vars = new List<ModVar>();

            //test for description file and settings file and read it
            var modFiles = ModDirectory.GetFiles();

            foreach (var modFile in modFiles)
            {
                var filename = modFile.Name;
                if (filename.Equals(Paths.AsJsonFileName(Paths.ModDescriptionName))) //.json-Format
                {
                    try
                    {
                        var json = File.ReadAllText(modFile.FullName, Encoding.Default);
                        MetaData = JsonConvert.DeserializeObject<ModMetaData>(json);
                    }
                    catch (JsonReaderException e)
                    {
                        ShowErrorMessage($"Could not parse metadata-file of '{ModDirectoryName}'!");
                    }
                }
                else if (filename.Equals(Paths.AsJsonFileName(Paths.ModSettingsName)))
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

                            object oldValue = getVarElement(oldSettings, modSettingsVariable.Name + ":");
                            if (oldValue == null)
                                continue;

                            switch (modSettingsVariable.Type)
                            {
                                case ModVariableType.Bool:
                                    oldValue = bool.Parse((string) oldValue);
                                    break;
                                case ModVariableType.Int:
                                    oldValue = int.Parse((string) oldValue);
                                    break;
                                default:
                                case ModVariableType.String:
                                    break;
                            }
                            modSettingsVariable.Value = oldValue;
                        }
                    } catch(JsonReaderException e)
                    {
                        ShowErrorMessage($"Could not parse settings-file of '{ModDirectoryName}'!");
                    }
                }
                else if (filename.Equals(Paths.ModDescriptionFileName)) //.dm-Settings-Format
                {
                    //read mod description
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
                else if (filename.Equals(Paths.ModSettingsFileName))
                {
                    //reading mod settings
                    var reader = new StreamReader(modFile.FullName, Encoding.Default);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //read var
                        var varName = getVarElement(line, "Var:");
                        var type = getVarElement(line, "Type:");
                        var description = getVarElement(line, "Description:");
                        var gameValue = getVarElement(line, "GameValue:");
                        var stdValue = getVarElement(line, "StdValue:");
                        var minValue = getVarElement(line, "MinValue:");
                        var maxValue = getVarElement(line, "MaxValue:");

                        if (varName != null && type != null && gameValue != null && stdValue != null)
                        {
                            //read old settings
                            var value = stdValue;
                            if (oldSettings != null)
                            {
                                value = getVarElement(oldSettings, varName + ":");
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
                                    modVar = new ModVar<int>(varName, type, description, int.Parse(gameValue), int.Parse(stdValue), int.Parse(value), int.Parse(minValue), int.Parse(maxValue));
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
                                    modVar = new ModVar<int>(varName, type, description, int.Parse(gameValue), int.Parse(stdValue), int.Parse(value));
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
                                modVar = new ModVar<bool>(varName, type, description, bool.Parse(gameValue), bool.Parse(stdValue), bool.Parse(value));
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

        private string getVarElement(string line, string identifier)
        {
            var startIndex = line.IndexOf(identifier);
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
            return DisplayText.CompareTo(element.DisplayText);
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, Resources.Error);
        }

    }
}
