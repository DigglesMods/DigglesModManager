using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DigglesManager
{
    public partial class DigglesManager : Form
    {
        string exePath = @"."; //TODO D:\programme\Wiggles
        string modDirectoryName = "Mods";
        string activeModsFileName = "mods.dm";
        string restoreFileName = "restore.dm";

        string changeFileStarting = "change_";
        string copyFileEnding = "_copy";

        bool warning = false;

        List<string> inactiveMods = new List<string>();
        List<string> activeMods = new List<string>();

        public DigglesManager()
        {
            InitializeComponent();

            if (!File.Exists(exePath + "\\" + "Wiggles.exe"))
            {
                MessageBox.Show("Legen Sie die Datei ins Wiggles Verzeichnis!");
            }
            else
            {
                setMessage("", Color.Black);
                readMods();
            }
        }

        private void setMessage(string text, Color color)
        {
            label_message.Text = text;
            label_message.ForeColor = color;
        }

        private void readMods()
        {
            inactiveMods.Clear();
            activeMods.Clear();

            //check if mod directory exists
            if (!Directory.Exists(exePath + "\\" + modDirectoryName))
            {
                Directory.CreateDirectory(exePath + "\\" + modDirectoryName);
            }

            //read last active mods
            List<string> lastActiveMods = new List<string>();
            if (File.Exists(exePath + "\\" + activeModsFileName))
            {
                StreamReader reader = new StreamReader(exePath + "\\" + activeModsFileName);
                string mod;
                while ((mod = reader.ReadLine()) != null)
                {
                    lastActiveMods.Add(mod);
                }
                reader.Close();
            }

            //add to active mods 
            foreach (string mod in lastActiveMods)
            {
                if (Directory.Exists(exePath + "\\" + modDirectoryName + "\\" + mod))
                {
                    activeMods.Add(mod);
                }
            }

            //read mods
            DirectoryInfo[] modDirectories = (new DirectoryInfo(exePath + "\\" + modDirectoryName)).GetDirectories();
            foreach (DirectoryInfo modInfo in modDirectories)
            {
                if (!activeMods.Contains(modInfo.Name))
                {
                    inactiveMods.Add(modInfo.Name);
                }
            }
            inactiveMods.Sort();

            changeDataSource();
        }
        
        private void changeDataSource()
        {
            // Change the DataSource.
            listBox1.DataSource = null;
            listBox1.DataSource = inactiveMods;
            listBox2.DataSource = null;
            listBox2.DataSource = activeMods;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < inactiveMods.Count) {
                activeMods.Insert(0, inactiveMods.ElementAt<string>(selectedIndex)); //add element right at first position
                inactiveMods.RemoveAt(selectedIndex); //remove element left

                changeDataSource();
            }
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
            int selectedIndex = listBox2.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < activeMods.Count)
            {
                inactiveMods.Add(activeMods.ElementAt<string>(selectedIndex)); //add element left
                activeMods.RemoveAt(selectedIndex); //remove element right
                inactiveMods.Sort();

                changeDataSource();
            }
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
            int selectedIndex = listBox2.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < activeMods.Count)
            {
                string mod = activeMods.ElementAt<string>(selectedIndex); //get mod
                activeMods.RemoveAt(selectedIndex); //remove mod
                selectedIndex--;
                activeMods.Insert(selectedIndex, mod); //add at new position

                changeDataSource();
                listBox2.SetSelected(selectedIndex, true);
            }
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
            int selectedIndex = listBox2.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < activeMods.Count - 1)
            {
                string mod = activeMods.ElementAt<string>(selectedIndex); //get mod
                activeMods.RemoveAt(selectedIndex); //remove mod
                selectedIndex++;
                activeMods.Insert(selectedIndex, mod); //add at new position

                changeDataSource();
                listBox2.SetSelected(selectedIndex, true);
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            setMessage("", Color.Black);
            readMods();
        }

        private void button_mod_Click(object sender, EventArgs e)
        {
            warning = false;
            setMessage("...", Color.Black);

            restore();
            foreach (string mod in activeMods)
            {
                DirectoryInfo modDir = new DirectoryInfo(exePath + "\\" + modDirectoryName + "\\" + mod);
                letsMod(modDir, new DirectoryInfo(exePath));
            }
            saveActiveMods();
            if (warning)
            {
                setMessage("Warning", Color.Orange);
            }
            else
            {
                setMessage("Success", Color.Green);
            }
        }

        private void letsMod(DirectoryInfo modDirectory, DirectoryInfo gameDirectory)
        {

            FileInfo[] modFiles = modDirectory.GetFiles();
            FileInfo[] gameFiles = gameDirectory.GetFiles();

            //if Texture directory deactivate texmaps.bin
            if (gameDirectory.Name == "Texture")
            {
                foreach (FileInfo gameFile in gameFiles)
                {
                    if (gameFile.Name == "texmaps.bin")
                    {
                        rememberForRestore(gameFile, "res");
                        gameFile.MoveTo(gameFile.FullName + copyFileEnding);
                        break;
                    }
                }
            }

            //add or override game files
            foreach (FileInfo modFile in modFiles)
            {
                //detect mode
                string mode = "replace";
                string filename = modFile.Name;

                if (filename.StartsWith(changeFileStarting))
                {
                    mode = "change";
                    filename = filename.Substring(changeFileStarting.Count());
                }

                FileInfo rightGameFile = null;
                bool copyExists = false;
                foreach (FileInfo gameFile in gameFiles)
                {
                    //check for game file
                    if (gameFile.Name == filename)
                    {
                        rightGameFile = gameFile;
                    }
                    //check for copy
                    if (gameFile.Name == filename + copyFileEnding)
                    {
                        copyExists = true;
                    }
                }


                string type = "del"; //type for delelte at restore
                if (rightGameFile != null && !copyExists)
                {
                    //rename game file
                    rightGameFile.CopyTo(rightGameFile.FullName + copyFileEnding, true);
                    type = "res"; //type for restore
                }

                FileInfo newModFile = null;
                //switch mode
                if (mode == "replace") //replacement mode
                {
                    //copy file to game folder
                    newModFile = modFile.CopyTo(gameDirectory.FullName + "\\" + filename, true);
                }
                else if (mode == "change" && rightGameFile != null) //file change mode
                {
                    //change game file
                    type = "res";
                    newModFile = rightGameFile;

                    string origFileContent = File.ReadAllText(newModFile.FullName, Encoding.Default);

                    StreamReader reader = new StreamReader(modFile.FullName, Encoding.Default);
                    string line;
                    bool started = false;
                    int commandCount = -1;
                    string[] command = {"", ""};
                    string[] commandText = {"", ""};

                    int i = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        i++;
                        //start tag
                        if (line.StartsWith("$start")) {
                            if (!started) {
                                started = true;
                                commandCount = -1;
                            } 
                            else
                            {
                                MessageBox.Show("$start an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                MessageBox.Show("$before an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                MessageBox.Show("$after an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                MessageBox.Show("$put an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                MessageBox.Show("$replace an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                MessageBox.Show("$with an der falschen Stelle\nZeile: " + i + "\nDatei: " + modFile.FullName);
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
                                string oldValue = "";
                                string newValue = "";

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

                                    MessageBox.Show("Zwei nicht zueinander passende Kommandos gefunden\nvor Zeile: " + i + "\nDatei: " + modFile.FullName);
                                    warning = true;
                                }

                                if (oldValue != "")
                                {
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
                    File.WriteAllText(newModFile.FullName, origFileContent, Encoding.Default);
                }

                //remember
                if (newModFile != null && !copyExists)
                {
                    rememberForRestore(newModFile, type);
                }
            }

            DirectoryInfo[] modDirectories = modDirectory.GetDirectories();
            DirectoryInfo[] gameDirectories = gameDirectory.GetDirectories();
            foreach (DirectoryInfo modDir in modDirectories)
            {
                //search for game directory
                DirectoryInfo rightGameDir = null;
                foreach (DirectoryInfo gameDir in gameDirectories)
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
                letsMod(modDir, rightGameDir);
            }

        }

        private void rememberForRestore(FileInfo file, string type) 
        {
            //remember
            StreamWriter writer = new StreamWriter(exePath + "\\" + restoreFileName, true, Encoding.Default);
            writer.WriteLine(type + "||" + file.FullName);
            writer.Flush();
            writer.Close();
        }

        private void restore()
        {
            if (File.Exists(exePath + "\\" + restoreFileName))
            {
                StreamReader reader = new StreamReader(exePath + "\\" + restoreFileName, Encoding.Default);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string type = parts[0];
                    string filename = parts[1];

                    //delete file if type is "del" or type is "res" and a copy exists
                    if (File.Exists(filename) && (type == "del" || (type == "res" && File.Exists(filename + copyFileEnding))))
                    {
                        //delete mod file
                        try
                        {
                            File.Delete(filename);
                        }
                        catch {}
                    }

                    //delete copy if two mods added the same NEW file
                    if (type == "del" && File.Exists(filename + copyFileEnding))
                    {
                        //delete mod file copy
                        try
                        {
                            File.Delete(filename + copyFileEnding);
                        }
                        catch {}
                    }

                    //restore file if type is res
                    if (type == "res" && File.Exists(filename + copyFileEnding))
                    {
                        //restore game file
                        try
                        {
                            File.Move(filename + copyFileEnding, filename);
                        }
                        catch {}
                    }
                }
                reader.Close();
                //delete restore file
                File.Delete(exePath + "\\" + restoreFileName);
            }

        }

        private void saveActiveMods()
        {
            //delete old file
            if (File.Exists(exePath + "\\" + activeModsFileName))
            {
                File.Delete(exePath + "\\" + activeModsFileName);
            }

            if (activeMods.Count > 0)
            {
                //save
                StreamWriter writer = new StreamWriter(exePath + "\\" + activeModsFileName, true, Encoding.Default);
                foreach (string mod in activeMods)
                {
                    writer.WriteLine(mod);
                }
                writer.Flush();
                writer.Close();
            }
        }

        private void button_mod_settings_Click(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
