using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigglesModManager.Model;
using Newtonsoft.Json;

namespace DigglesModManager.Service
{
    /// <summary>
    /// Service holding several methods and functions to manipulate the modding status of the wiggles-instance.
    /// This service is able to read mods, apply mods to the game files and restores the changes of the mod manager.
    /// </summary>
    public class ModdingService
    {

        public static string ChangeFilePrefix = "change_";
        public static string CopyFileSuffix = "_copy";

        public void ReadModsFromFiles(List<Mod> activeMods, ProgressBarManipulator progressBarManipulator)
        {
            progressBarManipulator.Increment();
            //check if mod directory exists
            var modDirectoryPath = Paths.ModPath + "\\" + Paths.ModDirectoryName;
            if (!Directory.Exists(modDirectoryPath))
            {
                Directory.CreateDirectory(modDirectoryPath);
            }
            progressBarManipulator.Increment();

            //read last active mods
            var lastActiveMods = new List<string>();
            var activeModsFilePath = Paths.ExePath + "\\" + Paths.ActiveModsFileName;
            if (File.Exists(activeModsFilePath))
            {
                var reader = new StreamReader(activeModsFilePath);
                string mod;
                while ((mod = reader.ReadLine()) != null)
                {
                    lastActiveMods.Add(mod);
                }
                reader.Close();
            }
            //read JSON-AppSettings
            var appSettingsFilePath = $"{Paths.ExePath}\\{Paths.AppSettingsName}";
            if (File.Exists(appSettingsFilePath))
            {
                var json = File.ReadAllText(appSettingsFilePath, Encoding.Default);
                var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                lastActiveMods = new List<string>();
                foreach (var pair in appSettings.ActiveMods)
                    if (Directory.Exists($"{Paths.ModPath}\\{Paths.ModDirectoryName}\\{pair.Key}"))
                        activeMods.Add(new Mod(pair.Key, pair.Value));
            }
            progressBarManipulator.Increment();

            //add to active mods 
            foreach (var modAndSettings in lastActiveMods)
            {
                //read settings values
                var separatorIndex = modAndSettings.IndexOf('|');
                var mod = modAndSettings;
                string settings = null;
                if (separatorIndex > 0)
                {
                    settings = modAndSettings.Substring(separatorIndex + 1);
                    mod = modAndSettings.Substring(0, separatorIndex);
                }

                //add active mod
                if (Directory.Exists(Paths.ModPath + "\\" + Paths.ModDirectoryName + "\\" + mod))
                {
                    activeMods.Add(new Mod(mod, settings));
                }
            }
            progressBarManipulator.Increment();
        }

        public void RememberForRestore(FileInfo file, string type)
        {
            //remember
            var writer = new StreamWriter(Paths.ExePath + "\\" + Paths.RestoreFileName, true, Encoding.Default);
            writer.WriteLine(type + "||" + file.FullName);
            writer.Flush();
            writer.Close();
        }

        public void SaveActiveMods(List<Mod> activeMods)
        {
            //delete old file (.dm)
            var oldFilePath = Paths.ExePath + "\\" + Paths.ActiveModsFileName;
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            var appSettingsFilePath = $"{Paths.ExePath}\\{Paths.AppSettingsName}";
            if (activeMods.Count > 0)
            {
                //JSON-Format
                var appSettings = new AppSettings();
                foreach (var mod in activeMods)
                    appSettings.ActiveMods.Add(mod.ModDirectoryName, mod.Settings);
                var json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
                File.WriteAllText(appSettingsFilePath, json, Encoding.UTF8);   //overwrites any existing files, if present


                //save
                //StreamWriter writer = new StreamWriter(Paths.ExePath + "\\" + Paths.ActiveModsFileName, true, Encoding.Default);
                //foreach (Mod mod in _activeMods)
                //{
                //    string line = mod.ModDirectoryName;
                //    if (mod.Settings.Variables.Count > 0)
                //    {
                //        line += "|";
                //        foreach (var modVar in mod.Settings.Variables)
                //        {
                //            line += modVar.Name + ":" + modVar.Value.ToString() + ";";
                //        }
                //    }
                //    writer.WriteLine(line);
                //}
                //writer.Flush();
                //writer.Close();
            }
            else
            {
                File.Delete(appSettingsFilePath);
            }
        }

        public bool LetsMod(Mod mod, DirectoryInfo modDirectory, DirectoryInfo gameDirectory, List<Mod> activeMods)
        {
            var modFiles = modDirectory.GetFiles();
            var gameFiles = gameDirectory.GetFiles();
            var warning = false;

            //if Texture directory deactivate texmaps.bin
            if (gameDirectory.Name == "Texture")
            {
                foreach (var gameFile in gameFiles)
                {
                    if (gameFile.Name == "texmaps.bin")
                    {
                        this.RememberForRestore(gameFile, "res");
                        gameFile.MoveTo(gameFile.FullName + CopyFileSuffix);
                        break;
                    }
                }
            }

            //add or override game files
            foreach (var modFile in modFiles)
            {
                //detect mode
                var mode = "replace";
                var filename = modFile.Name;

                //skip modmanager files
                if (filename.Equals(Paths.ModSettingsFileName) || filename.Equals(Paths.ModDescriptionFileName) ||
                    filename.Equals(Paths.ModSettingsName) || filename.Equals(Paths.ModDescriptionName))
                {
                    continue;
                }

                if (filename.StartsWith(ChangeFilePrefix))
                {
                    mode = "change";
                    filename = filename.Substring(ChangeFilePrefix.Count());
                }

                FileInfo rightGameFile = null;
                var copyExists = false;
                foreach (var gameFile in gameFiles)
                {
                    //check for game file
                    if (gameFile.Name == filename)
                    {
                        rightGameFile = gameFile;
                    }
                    //check for copy
                    if (gameFile.Name == filename + CopyFileSuffix)
                    {
                        copyExists = true;
                    }
                }


                var type = "del"; //type for delete at restore
                if (rightGameFile != null && !copyExists)
                {
                    //rename game file
                    Console.WriteLine($"Rename game file: {rightGameFile.FullName} --> {rightGameFile.FullName}{CopyFileSuffix}");
                    rightGameFile.CopyTo(rightGameFile.FullName + CopyFileSuffix, true);
                    type = "res"; //type for restore
                }

                FileInfo newModFile = null;
                //switch mode
                if (mode == "replace") //replacement mode
                {
                    //copy file to game folder
                    Console.WriteLine($"Copy file: {modFile.FullName} --> {gameDirectory.FullName}\\{filename}");
                    newModFile = modFile.CopyTo(gameDirectory.FullName + "\\" + filename, true);
                }
                else if (mode == "change" && rightGameFile != null) //file change mode
                {
                    //change game file
                    type = "res";
                    newModFile = rightGameFile;

                    var origFileContent = File.ReadAllText(newModFile.FullName, Encoding.Default);

                    var reader = new StreamReader(modFile.FullName, Encoding.Default);
                    string line;
                    var started = false;
                    var ifStack = new Stack<bool>();
                    var commandCount = -1;
                    string[] command = { "", "" };
                    string[] commandText = { "", "" };

                    var i = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        i++;
                        //check for if ending
                        if (line.StartsWith("$ifend") && ifStack.Count > 0)
                        {
                            ifStack.Pop();
                        }
                        //if clause
                        if (line.StartsWith("$if:"))
                        {
                            if (ifStack.Count > 0 && ifStack.Contains(false))
                            {
                                //skip check, because it is already false
                                ifStack.Push(false);
                            }
                            else
                            {
                                //check if statement
                                var ifStatement = line.Substring("$if:".Length).TrimEnd();
                                var not = false;
                                if (ifStatement.StartsWith("!"))
                                {
                                    not = true;
                                    ifStatement = ifStatement.Substring(1);
                                }

                                //check if-type
                                if (ifStatement.StartsWith("mod:"))
                                {
                                    //$if:mod:
                                    ifStatement = ifStatement.Substring("mod:".Length);
                                    //search mod
                                    var modFound = activeMods.Any(m => m.ModDirectoryName.Equals(ifStatement));
                                    ifStack.Push(modFound != not);
                                }
                                else
                                {
                                    //$if:varname
                                    //look for variable
                                    var modVarFound = false;
                                    foreach (var modVariable in mod.Settings.Variables)
                                    {
                                        if (!modVariable.Name.Equals(ifStatement))
                                            continue;

                                        modVarFound = true;
                                        //supports only bool variables
                                        switch (modVariable.Type)
                                        {
                                            case ModVariableType.Bool:
                                                ifStack.Push(((bool)modVariable.Value) != not);
                                                break;
                                            case ModVariableType.Int:
                                            case ModVariableType.String:
                                            default:
                                                Helpers.ShowErrorMessage($"$if unterstuetzt nur boole'sche Variablen: {i}\nDatei: {modFile.FullName}");
                                                break;
                                        }
                                        break;
                                    }
                                    if (!modVarFound)
                                    {
                                        Helpers.ShowErrorMessage("$if boolsche Variable '" + ifStatement + "' nicht gefunden: " + i + "\nDatei: " + modFile.FullName);
                                    }
                                }
                            }
                        }
                        //skip when if clause was false
                        if (ifStack.Count > 0 && ifStack.Contains(false))
                        {
                            continue;
                        }

                        //start tag
                        if (line.StartsWith("$start"))
                        {
                            if (!started)
                            {
                                started = true;
                                commandCount = -1;
                            }
                            else
                            {
                                Helpers.ShowErrorMessage("$start an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                warning = true;
                                break;
                            }
                            //end tag
                        }
                        else if (line.StartsWith("$before"))
                        {
                            commandCount++;
                            if (started && commandCount < 2)
                            {
                                command[commandCount] = "before";
                                commandText[commandCount] = "";
                            }
                            else
                            {
                                Helpers.ShowErrorMessage("$before an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                warning = true;
                                break;
                            }
                        }
                        else if (line.StartsWith("$after"))
                        {
                            commandCount++;
                            if (started && commandCount < 2)
                            {
                                command[commandCount] = "after";
                                commandText[commandCount] = "";
                            }
                            else
                            {
                                Helpers.ShowErrorMessage("$after an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                warning = true;
                                break;
                            }
                        }
                        else if (line.StartsWith("$put"))
                        {
                            commandCount++;
                            if (started && commandCount < 2)
                            {
                                command[commandCount] = "put";
                                commandText[commandCount] = "";
                            }
                            else
                            {
                                Helpers.ShowErrorMessage("$put an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                warning = true;
                                break;
                            }
                        }
                        else if (line.StartsWith("$replace"))
                        {
                            commandCount++;
                            if (started && commandCount < 2)
                            {
                                command[commandCount] = "replace";
                                commandText[commandCount] = "";
                            }
                            else
                            {
                                Helpers.ShowErrorMessage("$replace an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                break;
                            }
                        }
                        else if (line.StartsWith("$with"))
                        {
                            commandCount++;
                            if (started && commandCount < 2)
                            {
                                command[commandCount] = "with";
                                commandText[commandCount] = "";
                            }
                            else
                            {
                                Helpers.ShowErrorMessage("$with an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                warning = true;
                                break;
                            }
                        }
                        else if (line.StartsWith("$end"))
                        {
                            if (started)
                            {
                                started = false;
                                //replace
                                var oldValue = "";
                                var newValue = "";

                                if (command.Contains("before") && command.Contains("put"))
                                {
                                    if (command[0] == "before")
                                    {
                                        oldValue = commandText[0];
                                        newValue = commandText[1] + commandText[0];
                                    }
                                    else
                                    {
                                        oldValue = commandText[1];
                                        newValue = commandText[0] + commandText[1];
                                    }
                                }
                                else if (command.Contains("after") && command.Contains("put"))
                                {
                                    if (command[0] == "after")
                                    {
                                        oldValue = commandText[0];
                                        newValue = commandText[0] + commandText[1];
                                    }
                                    else
                                    {
                                        oldValue = commandText[1];
                                        newValue = commandText[1] + commandText[0];
                                    }
                                }
                                else if (command.Contains("replace") && command.Contains("with"))
                                {
                                    if (command[0] == "replace")
                                    {
                                        oldValue = commandText[0];
                                        newValue = commandText[1];
                                    }
                                    else
                                    {
                                        oldValue = commandText[1];
                                        newValue = commandText[0];
                                    }
                                }
                                else
                                {

                                    Helpers.ShowErrorMessage("Zwei nicht zueinander passende Kommandos gefunden\nvor Zeile: " + i + "\nDatei: " + modFile.FullName);
                                    warning = true;
                                }

                                //replace old value with new value
                                if (oldValue != "")
                                {
                                    //old code had: if (mod.Vars.Count > 0). Why?
                                    //before: replace variables
                                    foreach (var modVar in mod.Settings.Variables)
                                    {
                                        newValue = newValue.Replace("$print:" + modVar.Name, modVar.Value.ToString());
                                    }

                                    //replace old value with new value
                                    Console.WriteLine($"Replace content for {newModFile.FullName}");

                                    origFileContent = origFileContent.Replace(oldValue, newValue);

                                }
                            }
                            //other commands
                        }
                        else if (commandCount == 0 || commandCount == 1)
                        {
                            //add text 
                            if (commandText[commandCount] != "")
                            {
                                //add break line 
                                commandText[commandCount] += "\r\n";
                            }
                            commandText[commandCount] += line;
                        }
                    }
                    reader.Close();

                    //write content to file
                    Console.WriteLine($"Write to file: {newModFile.FullName} --> {gameDirectory.FullName}\\{filename}");

                    File.WriteAllText(newModFile.FullName, origFileContent, Encoding.Default);
                }

                //remember
                if (newModFile != null && !copyExists)
                {
                    this.RememberForRestore(newModFile, type);
                }
            }

            var modDirectories = modDirectory.GetDirectories();
            var gameDirectories = gameDirectory.GetDirectories();
            foreach (var modDir in modDirectories)
            {
                //search for game directory
                DirectoryInfo rightGameDir = null;
                foreach (var gameDir in gameDirectories)
                {
                    if (gameDir.Name == modDir.Name)
                    {
                        rightGameDir = gameDir;
                        break;
                    }
                }
                //if game directory does not exist, create it
                if (rightGameDir == null)
                {
                    rightGameDir = gameDirectory.CreateSubdirectory(modDir.Name);
                    //TODO merke neues verzeichnis fuer wiederherstellung
                }
                //same procedure for subdirectrory
                warning = warning || LetsMod(mod, modDir, rightGameDir, activeMods);
            }
            return warning;
        }

        public void Restore()
        {
            if (File.Exists(Paths.ExePath + "\\" + Paths.RestoreFileName))
            {
                var reader = new StreamReader(Paths.ExePath + "\\" + Paths.RestoreFileName, Encoding.Default);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var type = parts[0];
                    var filename = parts[1];

                    //delete file if type is "del" or type is "res" and a copy exists
                    if (File.Exists(filename) && (type == "del" || (type == "res" && File.Exists(filename + CopyFileSuffix))))
                    {
                        //delete mod file
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ShowErrorMessage($"Error occurred: {ex}");
                        }
                    }

                    //delete copy if two mods added the same NEW file
                    if (type == "del" && File.Exists(filename + CopyFileSuffix))
                    {
                        //delete mod file copy
                        try
                        {
                            File.Delete(filename + CopyFileSuffix);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ShowErrorMessage($"Error occurred: {ex}");
                        }
                    }

                    //restore file if type is res
                    if (type == "res" && File.Exists(filename + CopyFileSuffix))
                    {
                        //restore game file
                        try
                        {
                            File.Move(filename + CopyFileSuffix, filename);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ShowErrorMessage($"Error occurred: {ex}");
                        }
                    }
                }
                reader.Close();
                //delete restore file
                File.Delete(Paths.ExePath + "\\" + Paths.RestoreFileName);
            }
        }
    }
}
