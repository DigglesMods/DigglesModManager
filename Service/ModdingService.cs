using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public const int SUCCESS_CODE = 0;
        public const int WARNING_CODE = 1;
        public const int ERROR_CODE = 2;

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
            var appSettingsFilePath = $"{Paths.ExePath}\\{Paths.AppSettingsName}";
            if (File.Exists(appSettingsFilePath))
            {
                var json = File.ReadAllText(appSettingsFilePath, Encoding.Default);
                var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                foreach (var pair in appSettings.ActiveMods)
                    if (Directory.Exists($"{Paths.ModPath}\\{Paths.ModDirectoryName}\\{pair.Key}"))
                    {
                        activeMods.Add(new Mod(pair.Key, pair.Value));
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

        public void SaveActiveMods(string language, List<Mod> activeMods)
        {
            var appSettingsFilePath = $"{Paths.ExePath}\\{Paths.AppSettingsName}";
            if (activeMods.Count > 0)
            {
                var appSettings = new AppSettings();
                appSettings.Language = language;
                foreach (var mod in activeMods)
                {
                    var variables = new Dictionary<string, object>();
                    foreach (var variable in mod.Config.SettingsVariables)
                    {
                        variables.Add(variable.ID, variable.Value);
                    }
                    appSettings.ActiveMods.Add(mod.ModDirectoryName, variables);
                }
                    
                var json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
                File.WriteAllText(appSettingsFilePath, json, Encoding.UTF8); //overwrites any existing files, if present
            }
            else
            {
                File.Delete(appSettingsFilePath);
            }
        }

        public int LetsMod(Mod mod, DirectoryInfo modDirectory, DirectoryInfo gameDirectory, List<Mod> activeMods, ProgressBarManipulator progressBarManipulator)
        {
            var modFiles = modDirectory.GetFiles();
            var gameFiles = gameDirectory.GetFiles();
            var returnValue = SUCCESS_CODE;

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

                //skip some files in root directory
                if (modDirectory.Name.Equals(mod.ModDirectoryName))
                {
                    if (filename.Equals(Paths.ModConfigName) || filename.Equals("LICENSE") || filename.Equals("README.md"))
                    {
                        progressBarManipulator.Increment();
                        continue;
                    }
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
                    //Only compare lower case strings to support other languages of the game.
                    //Polish game version has i.e. LAGER.TCL instead of lager.tcl.
                    var lowerCaseFilename = filename.ToLower();
                    var lowerCaseGameFilename = gameFile.Name.ToLower();
                    //check for game file
                    if (lowerCaseGameFilename == lowerCaseFilename)
                    {
                        rightGameFile = gameFile;
                    }
                    //check for copy
                    if (lowerCaseGameFilename == lowerCaseFilename + CopyFileSuffix)
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

                    var lineNumber = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;
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
                                    foreach (var modVariable in mod.Config.SettingsVariables)
                                    {
                                        if (!modVariable.ID.Equals(ifStatement))
                                            continue;

                                        //supports only bool variables
                                        if (modVariable.Type == ModVariableType.Bool)
                                        {
                                            modVarFound = true;
                                            ifStack.Push(((bool)modVariable.Value) != not);
                                        }
                                        break;
                                    }
                                    if (!modVarFound)
                                    {
                                        Log.Error("Boolean mod variable '" + ifStatement + "' not found for '$if' statement", modFile, lineNumber);
                                        returnValue = ERROR_CODE;
                                        break;
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
                                Log.Error("'$start' at wrong place", modFile, lineNumber);
                                returnValue = ERROR_CODE;
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
                                Log.Error("'$before' at wrong place", modFile, lineNumber);
                                returnValue = ERROR_CODE;
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
                                Log.Error("'$after' at wrong place", modFile, lineNumber);
                                returnValue = ERROR_CODE;
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
                                Log.Error("'$put' at wrong place", modFile, lineNumber);
                                returnValue = ERROR_CODE;
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
                                Log.Error("'$replace' at wrong place", modFile, lineNumber);
                                returnValue = ERROR_CODE;
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
                                Log.Error("'$with' at wrong place", modFile, lineNumber);
                                returnValue = ERROR_CODE;
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
                                    if (commandCount > -1)
                                    {
                                        var commandsAsString = command[0];
                                        for (int i = 1; i < commandCount; i++)
                                        {
                                            commandsAsString += ", " + command[i];
                                        }
                                        Log.Error("Found commands that do not match: " + commandsAsString, modFile, lineNumber);
                                    }
                                    else
                                    {
                                        Log.Error("'$end' at wrong place", modFile, lineNumber);
                                    }
                                    returnValue = ERROR_CODE;
                                    break;
                                }

                                //replace old value with new value
                                if (oldValue != "")
                                {
                                    //before: replace variables
                                    foreach (var modVar in mod.Config.SettingsVariables)
                                    {
                                        newValue = newValue.Replace("$print:" + modVar.ID, modVar.Value.ToString());
                                    }

                                    //replace old value with new value
                                    Console.WriteLine($"Replace content for {newModFile.FullName}");

                                    origFileContent = origFileContent.Replace(oldValue, newValue);

                                }
                            }
                            else
                            {
                                Log.Warning("Unnecessary '$end' found", modFile, lineNumber);
                                returnValue = WARNING_CODE;
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
                        else if (!string.IsNullOrEmpty(line) && !line.StartsWith("$if:"))
                        {
                            Log.Warning("Unnecessary content found: '" + line + "'", modFile, lineNumber);
                            returnValue = WARNING_CODE;
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

                progressBarManipulator.Increment();
            }

            //if an error occurred do not mod sub directories
            if (returnValue == ERROR_CODE)
            {
                return returnValue;
            }

            var modDirectories = modDirectory.GetDirectories();
            var gameDirectories = gameDirectory.GetDirectories();
            foreach (var modDir in modDirectories)
            {
                //skip .git directories
                if (modDir.Name.Equals(".git"))
                {
                    //increment progress bar for all files in git directory
                    progressBarManipulator.Increment(modDir.GetFiles("*", SearchOption.AllDirectories).Length);
                    continue;
                }

                DirectoryInfo rightGameDir = null;

                //check for mod directories, that are handled in a different way
                if (mod.Config.Directories.Count > 0)
                {
                    var relativePath = modDir.FullName.Replace(mod.ModDirectoryPath, "");
                    //replace all backslashes with slashes
                    relativePath = relativePath.Replace("\\", "/");
                    var skipDirectory = false;
                    foreach (var directory in mod.Config.Directories)
                    {
                        if (directory.Path.Equals(relativePath))
                        {
                            // if this is a special directory, check the condition
                            if (directory.Condition != null && !directory.Condition.isTrue(mod, activeMods))
                            {
                                //if a condition exists and is false, skip this directory
                                skipDirectory = true;
                                break;
                            }
                            // if the condition is true or no condition exists
                            switch (directory.Type)
                            {
                                case ModDirectoryType.Data:
                                    //set the data directory as game directory
                                    rightGameDir = new DirectoryInfo(Paths.DataPath);
                                    break;
                                case ModDirectoryType.Optional:
                                    if (directory.Condition == null)
                                    {
                                        Log.Warning(mod.ModDirectoryName + ": The optional directory " + directory.Path  + " should have a condition");
                                        returnValue = WARNING_CODE;
                                    }
                                    //do nothing, because it is now a normal directory
                                    break;
                                default:
                                    Log.Warning(mod.ModDirectoryName + ": Unhandled ModDirectoryType \"" + directory.Type + "\"");
                                    returnValue = WARNING_CODE;
                                    break;
                            }
                            break;
                        }
                    }
                    if (skipDirectory)
                    {
                        continue;
                    }
                }

                if (rightGameDir == null)
                {
                    //search for game directory
                    foreach (var gameDir in gameDirectories)
                    {
                        if (gameDir.Name == modDir.Name)
                        {
                            rightGameDir = gameDir;
                            break;
                        }
                    }
                }
                //if game directory does not exist, create it
                if (rightGameDir == null)
                {
                    rightGameDir = gameDirectory.CreateSubdirectory(modDir.Name);
                    //TODO merke neues verzeichnis fuer wiederherstellung
                }
                //same procedure for subdirectrory
                var subReturnValue = LetsMod(mod, modDir, rightGameDir, activeMods, progressBarManipulator);

                if (subReturnValue == ERROR_CODE)
                {
                    //cancel when error occurred
                    returnValue = subReturnValue;
                    break;
                }
                else if (subReturnValue == WARNING_CODE)
                {
                    //remember warning state
                    returnValue = subReturnValue;
                }
            }
            return returnValue;
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
