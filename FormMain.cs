using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using DigglesModManager.Model;
using DigglesModManager.Properties;
using Newtonsoft.Json;

namespace DigglesModManager
{
    public partial class FormMain : Form
    {
        public static string ChangeFileStarting = "change_";
        public static string CopyFileEnding = "_copy";

        private bool _warning = false;

        private readonly List<Mod> _inactiveMods = new List<Mod>();
        private readonly List<Mod> _activeMods = new List<Mod>();

        public FormMain()
        {
            InitializeComponent();
            Text = string.Format(Resources.FormMainTitle, typeof(FormMain).Assembly.GetName().Version);

            FormBorderStyle = FormBorderStyle.FixedSingle;

            if (!File.Exists($"{Paths.ExePath}\\{Paths.WigglesExecutableName}"))
            {
                ShowErrorMessage(Resources.FormMain_CouldNotFindWigglesExeErrorText, Resources.Error);
            }
            else
            {
                ResetStatusNote();
                ReadMods();
            }
        }

        private void ResetStatusNote()
        {
            SetMessage("", Color.Black);
        }

        private void SetMessage(string text, Color color)
        {
            label_message.Text = text;
            label_message.ForeColor = color;
            statusBarLabelRight.Text = text;
            statusBarLabelRight.ForeColor = color;
        }

        private void ResetProgressBar(int max = 1)
        {
            modProgressStatusBar.Value = 0;
            modProgressStatusBar.Maximum = max;
            modProgressStatusBar.Visible = true;
        }

        private void IncrementProgressBar(int increment = 1)
        {
            if (modProgressStatusBar.Maximum > modProgressStatusBar.Value)
                modProgressStatusBar.Value += increment;
        }

        private void FinalizeProgressBar()
        {
            modProgressStatusBar.Value = modProgressStatusBar.Maximum;
            //modProgressStatusBar.Visible = false;
        }

        private void ReadMods()
        {
            ResetProgressBar(7);
            IncrementProgressBar();

            _inactiveMods.Clear();
            _activeMods.Clear();

            IncrementProgressBar();
            //check if mod directory exists
            if (!Directory.Exists(Paths.ModPath + "\\" + Paths.ModDirectoryName))
            {
                Directory.CreateDirectory(Paths.ModPath + "\\" + Paths.ModDirectoryName);
            }
            IncrementProgressBar();

            //read last active mods
            List<string> lastActiveMods = new List<string>();
            if (File.Exists(Paths.ExePath + "\\" + Paths.ActiveModsFileName))
            {
                StreamReader reader = new StreamReader(Paths.ExePath + "\\" + Paths.ActiveModsFileName);
                string mod;
                while ((mod = reader.ReadLine()) != null)
                {
                    lastActiveMods.Add(mod);
                }
                reader.Close();
            }
            //read JSON-AppSettings
            if (File.Exists($"{Paths.ExePath}\\{Paths.AppSettingsName}"))
            {
                var json = File.ReadAllText($"{Paths.ExePath}\\{Paths.AppSettingsName}", Encoding.Default);
                var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                lastActiveMods = new List<string>();
                foreach (var pair in appSettings.ActiveMods)
                    if (Directory.Exists($"{Paths.ModPath}\\{Paths.ModDirectoryName}\\{pair.Key}"))
                        _activeMods.Add(new Mod(pair.Key, pair.Value));

            }
            IncrementProgressBar();

            //add to active mods 
            foreach (string modAndSettings in lastActiveMods)
            {
                //read settings values
                int separatorIndex = modAndSettings.IndexOf('|');
                string mod = modAndSettings;
                string settings = null;
                if (separatorIndex > 0)
                {
                    settings = modAndSettings.Substring(separatorIndex + 1);
                    mod = modAndSettings.Substring(0, separatorIndex);
                }

                //add active mod
                if (Directory.Exists(Paths.ModPath + "\\" + Paths.ModDirectoryName + "\\" + mod))
                {
                    _activeMods.Add(new Mod(mod, settings));
                }
            }
            IncrementProgressBar();

            //read mods
            DirectoryInfo[] modDirectories = (new DirectoryInfo(Paths.ModPath + "\\" + Paths.ModDirectoryName)).GetDirectories();
            foreach (DirectoryInfo modInfo in modDirectories)
            {
                //if (!_activeMods.Contains(new Mod(modInfo.Name, (string)null)))
                if (!_activeMods.Exists(mod => mod.MetaData.Name.Equals(modInfo.Name)))
                {
                    _inactiveMods.Add(new Mod(modInfo.Name, (string)null));
                }
            }
            _inactiveMods.Sort();
            IncrementProgressBar();

            ChangeDataSource();

            IncrementProgressBar();
            FinalizeProgressBar();
        }

        private void ChangeDataSource()
        {
            // Change the DataSource.
            availableModsListBox.DataSource = null;
            availableModsListBox.DataSource = _inactiveMods;
            installedModsListBox.DataSource = null;
            installedModsListBox.DataSource = _activeMods;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetStatusNote();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetStatusNote();

            //find settings file
            int selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _activeMods.Count)
            {
                Mod mod = _activeMods.ElementAt(selectedIndex);

                DirectoryInfo modDir = new DirectoryInfo(Paths.ModPath + "\\" + Paths.ModDirectoryName + "\\" + mod.ModDirectoryName);
                FileInfo[] modFiles = modDir.GetFiles();

                bool hasSettings = false;
                foreach (FileInfo gameFile in modFiles)
                {
                    if (gameFile.Name.Equals(Paths.ModSettingsFileName) || Paths.ModSettingsName.Equals(gameFile.Name))
                    {
                        hasSettings = true;
                    }
                }
                //set settings button enabled
                modSettingsButton.Enabled = hasSettings;
                modSettingsMenuButton.Enabled = hasSettings;
            }
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            int selectedIndex = availableModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _inactiveMods.Count)
            {
                _activeMods.Insert(0, _inactiveMods.ElementAt(selectedIndex)); //add element right at first position
                _inactiveMods.RemoveAt(selectedIndex); //remove element left

                ChangeDataSource();
            }
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            int selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _activeMods.Count)
            {
                _inactiveMods.Add(_activeMods.ElementAt(selectedIndex)); //add element left
                _activeMods.RemoveAt(selectedIndex); //remove element right
                _inactiveMods.Sort();

                ChangeDataSource();
            }
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            int selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < _activeMods.Count)
            {
                Mod mod = _activeMods.ElementAt(selectedIndex); //get mod
                _activeMods.RemoveAt(selectedIndex); //remove mod
                selectedIndex--;
                _activeMods.Insert(selectedIndex, mod); //add at new position

                ChangeDataSource();
                installedModsListBox.SetSelected(selectedIndex, true);
            }
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            int selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _activeMods.Count - 1)
            {
                Mod mod = _activeMods.ElementAt(selectedIndex); //get mod
                _activeMods.RemoveAt(selectedIndex); //remove mod
                selectedIndex++;
                _activeMods.Insert(selectedIndex, mod); //add at new position

                ChangeDataSource();
                installedModsListBox.SetSelected(selectedIndex, true);
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            ReadMods();
        }

        private void button_mod_Click(object sender, EventArgs e)
        {
            _warning = false;
            SetMessage("...", Color.Black);
            ResetProgressBar(3 + _activeMods.Count);
            IncrementProgressBar();

            Restore();
            IncrementProgressBar();
            foreach (Mod mod in _activeMods)
            {
                DirectoryInfo modDir = new DirectoryInfo(Paths.ModPath + "\\" + Paths.ModDirectoryName + "\\" + mod.ModDirectoryName);
                LetsMod(mod, modDir, new DirectoryInfo(Paths.ExePath));
                IncrementProgressBar();
            }

            SaveActiveMods();
            IncrementProgressBar();

            if (_warning)
            {
                SetMessage("Warning", Color.Orange);
            }
            else
            {
                SetMessage("Modding was successful", Color.Green);
            }
            FinalizeProgressBar();
        }

        private void letsModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_mod_Click(sender, e);
        }

        private void LetsMod(Mod mod, DirectoryInfo modDirectory, DirectoryInfo gameDirectory)
        {
            var modFiles = modDirectory.GetFiles();
            var gameFiles = gameDirectory.GetFiles();

            //if Texture directory deactivate texmaps.bin
            if (gameDirectory.Name == "Texture")
            {
                foreach (var gameFile in gameFiles)
                {
                    if (gameFile.Name == "texmaps.bin")
                    {
                        RememberForRestore(gameFile, "res");
                        gameFile.MoveTo(gameFile.FullName + CopyFileEnding);
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

                if (filename.StartsWith(ChangeFileStarting))
                {
                    mode = "change";
                    filename = filename.Substring(ChangeFileStarting.Count());
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
                    if (gameFile.Name == filename + CopyFileEnding)
                    {
                        copyExists = true;
                    }
                }


                var type = "del"; //type for delete at restore
                if (rightGameFile != null && !copyExists)
                {
                    //rename game file
                    Console.WriteLine($"Rename game file: {rightGameFile.FullName} --> {rightGameFile.FullName}{CopyFileEnding}");
                    rightGameFile.CopyTo(rightGameFile.FullName + CopyFileEnding, true);
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
                                string ifStatement = line.Substring("$if:".Length).TrimEnd();
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
                                    var modFound = false;
                                    foreach (var m in _activeMods)
                                    {
                                        if (m.ModDirectoryName.Equals(ifStatement))
                                        {
                                            modFound = true;
                                            break;
                                        }
                                    }
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
                                                ShowErrorMessage($"$if unterstuetzt nur boole'sche Variablen: {i}\nDatei: {modFile.FullName}");
                                                break;
                                        }
                                        break;
                                    }
                                    if (!modVarFound)
                                    {
                                        ShowErrorMessage("$if boolsche Variable '" + ifStatement + "' nicht gefunden: " + i + "\nDatei: " + modFile.FullName);
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
                                ShowErrorMessage("$start an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                _warning = true;
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
                                ShowErrorMessage("$before an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                _warning = true;
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
                                ShowErrorMessage("$after an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                _warning = true;
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
                                ShowErrorMessage("$put an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                _warning = true;
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
                                ShowErrorMessage("$replace an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                ShowErrorMessage("$with an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
                                _warning = true;
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

                                    ShowErrorMessage("Zwei nicht zueinander passende Kommandos gefunden\nvor Zeile: " + i + "\nDatei: " + modFile.FullName);
                                    _warning = true;
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
                    RememberForRestore(newModFile, type);
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
                LetsMod(mod, modDir, rightGameDir);
            }
        }

        private static void ShowErrorMessage(string text, string title = null)
        {
            if (title == null)
                title = Resources.Error;
            MessageBox.Show(text, title);
        }

        private void RememberForRestore(FileInfo file, string type)
        {
            //remember
            var writer = new StreamWriter(Paths.ExePath + "\\" + Paths.RestoreFileName, true, Encoding.Default);
            writer.WriteLine(type + "||" + file.FullName);
            writer.Flush();
            writer.Close();
        }

        private void Restore()
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
                    if (File.Exists(filename) && (type == "del" || (type == "res" && File.Exists(filename + CopyFileEnding))))
                    {
                        //delete mod file
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage($"Error occurred: {ex}");
                        }
                    }

                    //delete copy if two mods added the same NEW file
                    if (type == "del" && File.Exists(filename + CopyFileEnding))
                    {
                        //delete mod file copy
                        try
                        {
                            File.Delete(filename + CopyFileEnding);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage($"Error occurred: {ex}");
                        }
                    }

                    //restore file if type is res
                    if (type == "res" && File.Exists(filename + CopyFileEnding))
                    {
                        //restore game file
                        try
                        {
                            File.Move(filename + CopyFileEnding, filename);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage($"Error occurred: {ex}");
                        }
                    }
                }
                reader.Close();
                //delete restore file
                File.Delete(Paths.ExePath + "\\" + Paths.RestoreFileName);
            }
        }

        private void SaveActiveMods()
        {
            //delete old file
            if (File.Exists(Paths.ExePath + "\\" + Paths.ActiveModsFileName))
            {
                File.Delete(Paths.ExePath + "\\" + Paths.ActiveModsFileName);
            }

            if (_activeMods.Count > 0)
            {
                //JSON-Format
                var appSettings = new AppSettings();
                foreach (var mod in _activeMods)
                    appSettings.ActiveMods.Add(mod.ModDirectoryName, mod.Settings);
                var json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
                File.WriteAllText($"{Paths.ExePath}\\{Paths.AppSettingsName}", json, Encoding.UTF8);   //overwrites any existing files, if present


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
                File.Delete($"{Paths.ExePath}\\{Paths.AppSettingsName}");
            }
        }

        private void button_mod_settings_Click(object sender, EventArgs e)
        {
            var selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _activeMods.Count)
            {
                var mod = _activeMods.ElementAt(selectedIndex); //get mod
                var form = new FormModSettings(mod);
                form.ShowDialog(this); //'this' is necessary for relative aligning
            }
        }

        private void modSettingsMenuButton_Click(object sender, EventArgs e)
        {
            button_mod_settings_Click(sender, e);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void wikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("https://github.com/DigglesMods/DigglesModManager/wiki");
        }

        private void deutschToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("https://github.com/DigglesMods/DigglesModManager/wiki/Mods-(de)");
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("https://github.com/DigglesMods/DigglesModManager/wiki/Mods-(en)");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("https://github.com/DigglesMods/DigglesModManager/blob/master/README.md");
        }

        private void websiteMenuButton_Click(object sender, EventArgs e)
        {
            OpenWebPage("https://digglesmods.github.io/DigglesModManager/");
        }

        private static void OpenWebPage(string uri)
        {
            Process.Start(new ProcessStartInfo(uri));
        }

    }
}