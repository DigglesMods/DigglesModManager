using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DigglesModManager
{
    internal class Mod : IToolTipDisplayer, IComparable
    {
        public string ModDirectoryName { get; private set; }
        private DirectoryInfo ModDirectory;

        public string DisplayText { get; private set; }
        public string ToolTipText { get; private set; }
        public string Author { get; private set; }

        public List<ModVar> Vars { get; private set; }

        // Constructor
        public Mod(string modDirectory, string oldSettings)
        {
            ModDirectoryName = modDirectory;

            //get description
            ModDirectory = new DirectoryInfo(DigglesModManager.exePath + "\\" + DigglesModManager.modDirectoryName + "\\" + ModDirectoryName);
            DisplayText = modDirectory;
            ToolTipText = "";
            Author = "";
            Vars = new List<ModVar>();

            //test for description file and settings file and read it
            FileInfo[] modFiles = ModDirectory.GetFiles();

            foreach (FileInfo modFile in modFiles)
            {
                string filename = modFile.Name;
                if (filename.Equals(DigglesModManager.modDescriptionFileName))
                {
                    //read mod description
                    StreamReader reader = new StreamReader(modFile.FullName, Encoding.Default);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("name:"))
                        {
                            DisplayText = line.Substring(5);
                        }
                        else if (line.StartsWith("tooltip:"))
                        {
                            ToolTipText = line.Substring(8);
                        }
                        else if (line.StartsWith("author:"))
                        {
                            Author = line.Substring(7);
                        }
                    }
                }
                else if (filename.Equals(DigglesModManager.modSettingsFileName))
                {
                    //reading mod settings
                    StreamReader reader = new StreamReader(modFile.FullName, Encoding.Default);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //read var
                        string varName = getVarElement(line, "Var:");
                        string type = getVarElement(line, "Type:");
                        string description = getVarElement(line, "Description:");
                        string gameValue = getVarElement(line, "GameValue:");
                        string stdValue = getVarElement(line, "StdValue:");
                        string minValue = getVarElement(line, "MinValue:");
                        string maxValue = getVarElement(line, "MaxValue:");

                        if (varName != null && type != null && gameValue != null && stdValue != null) 
                        {
                            //read old settings
                            string value = stdValue;
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
                            if (type.Equals("int"))
                            {
                                if (minValue != null && maxValue != null)
                                {
                                    modVar = new ModVar<int>(varName, type, description, int.Parse(gameValue), int.Parse(stdValue), int.Parse(value), int.Parse(minValue), int.Parse(maxValue));
                                }
                                else
                                {
                                    modVar = new ModVar<int>(varName, type, description, int.Parse(gameValue), int.Parse(stdValue), int.Parse(value));
                                }
                            }
                            else if (type.Equals("bool"))
                            {
                                modVar = new ModVar<bool>(varName, type, description, bool.Parse(gameValue), bool.Parse(stdValue), bool.Parse(value));
                            }
                            else if (type.Equals("string"))
                            {
                                modVar = new ModVar<string>(varName, type, description, gameValue, stdValue, value);
                            }

                            if (modVar != null)
                            {
                                Vars.Add(modVar);
                            }
                        }
                    }
                }
            }
        }

        private string getVarElement(string line, string identifier)
        {
            int startIndex = line.IndexOf(identifier);
            if (startIndex >= 0)
            {
                startIndex += identifier.Length;
                string tmp = line.Substring(startIndex);
                int endIndex = tmp.IndexOf(';');
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
            string toolTip = ToolTipText;
            if (!toolTip.Equals("") && !Author.Equals(""))
            {
                toolTip += "\n";
            }
            if (!Author.Equals(""))
            {
                toolTip += "Autor: " + Author;
            }
            return toolTip;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Mod element = (Mod)obj;
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

            Mod element = (Mod)obj;
            return DisplayText.CompareTo(element.DisplayText);
        }
    }
}
