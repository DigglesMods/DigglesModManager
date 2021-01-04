using DigglesModManager.Model;
using DigglesModManager.Properties;
using DigglesModManager.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigglesModManager
{
    public partial class FormMain : Form
    {
        public static string _language = "en";
        private static Dictionary<string, ToolStripMenuItem> languageMenuItems = new Dictionary<string, ToolStripMenuItem>();

        private bool moddingServiceRunning = false;

        private readonly ModdingService _moddingService;

        private readonly ProgressBarManipulator _progressBarManipulator;

        public FormMain()
        {
            InitializeComponent();
            Text = string.Format(Resources.FormMainTitle, typeof(FormMain).Assembly.GetName().Version);

            //add imagelist to listviews
            var imageList = new ImageList();
            imageList.Images.Add("info", Resources.InfoIcon);
            imageList.Images.Add("warning", Resources.WarningIcon);
            imageList.Images.Add("error", Resources.ErrorIcon);

            availableModsListView.SmallImageList = imageList;
            activeModsListView.SmallImageList = imageList;

            FormBorderStyle = FormBorderStyle.FixedSingle;

            //set language menu items for checking
            languageMenuItems.Add("de", deutschToolStripMenuItem);
            languageMenuItems.Add("en", englishToolStripMenuItem);
            languageMenuItems.Add("es", espanolToolStripMenuItem);
            languageMenuItems.Add("fr", francaisToolStripMenuItem);
            languageMenuItems.Add("it", italianoToolStripMenuItem);
            languageMenuItems.Add("nl", nederlandsToolStripMenuItem);
            languageMenuItems.Add("pl", polishToolStripMenuItem);
            languageMenuItems.Add("ru", russianToolStripMenuItem);

            //get language
            var appSettingsFilePath = $"{Paths.ExePath}\\{Paths.AppSettingsName}";
            if (File.Exists(appSettingsFilePath))
            {
                var json = File.ReadAllText(appSettingsFilePath, Encoding.Default);
                var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                _language = appSettings.Language;
            }
            setCheckOfLanguageInMenu(true);

            _moddingService = new ModdingService();
            _progressBarManipulator = new ProgressBarManipulator(this, modProgressStatusBar);

            // check, if the exe is in the correct directory
            var correctDirectory = false;
            foreach (var digglesExe in Paths.DigglesExecutableNames)
            {
                if (File.Exists($"{Paths.ExePath}\\{digglesExe}"))
                {
                    correctDirectory = true;
                    break;
                }
            }

            //Only start, when exe is in the correct directory
            if (correctDirectory)
            {
                ResetStatusNote();
                ReadMods();
            }
            else
            {
                Helpers.ShowMessage(Resources.FormMain_CouldNotFindDigglesExeErrorText, Resources.Error);
            }
        }

        /// <summary>
        /// Resets the status-bar-text to an empty and neutral text.
        /// </summary>
        private void ResetStatusNote()
        {
            SetMessage("", Color.Black);
        }

        /// <summary>
        /// Sets the message of the status-bar at the bottom of the window.
        /// </summary>
        /// <param name="text">The text to be set.</param>
        /// <param name="color">The color to be set to the foreground of the status bar label.</param>
        private bool SetMessage(string text, Color color)
        {
            if (InvokeRequired)
            {
                return (bool)Invoke((Func<string, Color, bool>)SetMessage, text, color);
            }
            statusBarLabelRight.Text = text;
            statusBarLabelRight.ForeColor = color;
            return true;
        }


        private void setCheckOfLanguageInMenu(bool check)
        {
            ToolStripMenuItem menuItem = null;
            if (languageMenuItems.TryGetValue(_language, out menuItem) && menuItem != null)
            {
                menuItem.Checked = check;
                if (check)
                    menuItem.CheckState = CheckState.Checked;
                else
                    menuItem.CheckState = CheckState.Unchecked;

            }
        }

        private void ReadMods()
        {
            _progressBarManipulator.Reset(7);
            _progressBarManipulator.Increment();

            availableModsListView.Items.Clear();
            activeModsListView.Items.Clear();

            var activeMods = new List<Mod>();

            _moddingService.ReadModsFromFiles(activeMods, _progressBarManipulator);

            //read mods
            var modDirectories = (new DirectoryInfo($"{Paths.ModPath}\\{Paths.ModDirectoryName}")).GetDirectories();
            foreach (var modInfo in modDirectories)
            {
                var modObject = new Mod(modInfo.Name);
                ListViewItem listViewItem = new ListViewItem(modObject.ToString(), "")
                {
                    Tag = modObject
                };
                listViewItem.ToolTipText = modObject.GetToolTipText();
                if (activeMods.Exists(mod => mod.ModDirectoryName.Equals(modInfo.Name)))
                {
                    activeModsListView.Items.Add(listViewItem);
                }
                else
                {
                    availableModsListView.Items.Add(listViewItem);
                }
            }
            _progressBarManipulator.Increment();

            _progressBarManipulator.Increment();
            _progressBarManipulator.Finish();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!moddingServiceRunning)
            {
                var selectedItems = availableModsListView.SelectedItems;
                if (selectedItems.Count > 0)
                {
                    var selectedMod = (selectedItems[0].Tag as Mod);
                    //change content of description box
                    descriptionBox.Text = selectedMod.GetDescription();
                }
                else
                {
                    descriptionBox.Text = "";
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!moddingServiceRunning)
            {
                //find settings file
                var selectedItems = activeModsListView.SelectedItems;
                if (selectedItems.Count > 0)
                {
                    var selectedMod = (selectedItems[0].Tag as Mod);
                    var hasSettings = selectedMod.Config.SettingsVariables != null
                        && selectedMod.Config.SettingsVariables.Count > 0;

                    //set settings button enabled
                    modSettingsButton.Enabled = hasSettings;
                    modSettingsMenuButton.Enabled = hasSettings;

                    //change content of description box
                    descriptionBox.Text = selectedMod.GetDescription();
                }
                else
                {
                    descriptionBox.Text = "";
                }
            }
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedItems = availableModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedItem = selectedItems[0];
                availableModsListView.Items.Remove(selectedItem); //remove element from the left
                activeModsListView.Items.Add(selectedItem); //add element on the right
            }
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedItems = activeModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedItem = selectedItems[0];
                activeModsListView.Items.Remove(selectedItem); //remove element from the right
                availableModsListView.Items.Add(selectedItem); //add element on the left
            }
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedItems = activeModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedItem = selectedItems[0];
                MoveActiveModInList(selectedItem, -1);
            }
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedItems = activeModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedItem = selectedItems[0];
                MoveActiveModInList(selectedItem, 1);
            }
        }

        /// <summary>
        /// Move an item inside of the _activeMods list up or down.
        /// </summary>
        /// <param name="index">The to-be-moved item.</param>
        /// <param name="relativePosition">The distance and direction to let the item 'travel'. -1 is up, 2 is down (two steps)</param>
        private void MoveActiveModInList(ListViewItem item, int relativePosition)
        {
            var index = activeModsListView.Items.IndexOf(item);  //get index
            var newPosition = index + relativePosition;
            if (0 <= newPosition && newPosition < activeModsListView.Items.Count)
            {
                activeModsListView.Items.RemoveAt(index); //remove mod
                activeModsListView.Items.Insert(newPosition, item); //add at new position
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            ReadMods();
        }

        private bool setUIToModdingState(bool moddingState)
        {
            if (InvokeRequired)
            {
                return (bool)Invoke((Func<bool, bool>)setUIToModdingState, moddingState);
            }
            // set wait cursor
            Application.UseWaitCursor = moddingState;

            var buttonsEnabled = !moddingState;
            // Refresh Button
            refreshButton.Enabled = buttonsEnabled;
            // Arrow Buttons
            installModButton.Enabled = buttonsEnabled;
            uninstallModButton.Enabled = buttonsEnabled;
            moveUpButton.Enabled = buttonsEnabled;
            moveDownButton.Enabled = buttonsEnabled;
            // Mod Settings Buttons
            moddingServiceRunning = moddingState;
            if (buttonsEnabled)
            {
                listBox2_SelectedIndexChanged(null, null);
            }
            else
            {
                modSettingsButton.Enabled = false;
                modSettingsMenuButton.Enabled = false;
            }
            // Lets Mod Buttons
            letsModButton.Enabled = buttonsEnabled;
            letsModMenuButton.Enabled = buttonsEnabled;
            return true;
        }


        private void button_mod_Click(object sender, EventArgs e)
        {
            // set user interface into the modding state (disable buttons etc)
            setUIToModdingState(true);
            // Modding in second task to avoid UI freeze
            var activeMods = new List<Mod>();
            //get file count of all active mods
            var fileCount = 0;
            foreach (ListViewItem modItem in activeModsListView.Items)
            {
                var mod = modItem.Tag as Mod;
                activeMods.Add(mod);
                fileCount += mod.FileCount;
            }
            Task.Factory.StartNew(() =>
            {
                SetMessage(Resources.PleaseWait, Color.Black);
                //reset progress bar to new file count
                _progressBarManipulator.Reset(fileCount + 3);
                _progressBarManipulator.Increment();

                _moddingService.Restore();
                _progressBarManipulator.Increment();

                var warning = false;
                var error = false;
                foreach (var mod in activeMods)
                {
                    var returnValue = _moddingService.LetsMod(mod, mod.ModDirectoryInfo, new DirectoryInfo(Paths.ExePath), activeMods, _progressBarManipulator);
                    warning = warning || returnValue == ModdingService.WARNING_CODE;
                    error = error || returnValue == ModdingService.ERROR_CODE;
                    //cancel when error occured
                    if (error)
                    {
                        break;
                    }
                }

                _moddingService.SaveActiveMods(_language, activeMods);
                _progressBarManipulator.Finish();

                if (error)
                {
                    SetMessage(Resources.Error, Color.Red);
                    Helpers.ShowMessage(Resources.ModdingService_ErrorMessage, Resources.Error);
                }
                else if (warning)
                {
                    SetMessage(Resources.Warning, Color.Orange);
                    Helpers.ShowMessage(Resources.ModdingService_WarningMessage, Resources.Warning);
                }
                else
                {
                    SetMessage(Resources.ModdingSuccessful, Color.Green);
                }
                // set user interface into normal state (enable buttons etc)
                setUIToModdingState(false);
            });
        }

        private void letsModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_mod_Click(sender, e);
        }

        private void button_mod_settings_Click(object sender, EventArgs e)
        {
            var selectedItems = activeModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedMod = (selectedItems[0].Tag as Mod);
                var form = new FormModSettings(selectedMod, _language);
                form.ShowDialog(this); //'this' is necessary for relative aligning
            }
        }

        private void modSettingsMenuButton_Click(object sender, EventArgs e)
        {
            button_mod_settings_Click(sender, e);
        }

        private void changeLanguage(string language)
        {
            setCheckOfLanguageInMenu(false);
            _language = language;
            setCheckOfLanguageInMenu(true);
            ReadMods();
        }

        private void deutschToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("de");
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("en");
        }

        private void espanolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("es");
        }

        private void francaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("fr");
        }

        private void italianoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("it");
        }

        private void nederlandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("nl");
        }

        private void polishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("pl");
        }

        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("ru");
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.ExitApplication();
        }

        private void wikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.OpenWebPage(Resources.FormMain_WikiLink);
        }

        private void helpModsDeutschMenuButton_Click(object sender, EventArgs e)
        {
            Helpers.OpenWebPage(Resources.FormMain_WikiDeutschLink);
        }

        private void helpModsEnglishMenuButton_Click(object sender, EventArgs e)
        {
            Helpers.OpenWebPage(Resources.FormMain_EnglishWikiLink);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.OpenWebPage(Resources.FormMain_WikiHomeLink);
        }

        private void websiteMenuButton_Click(object sender, EventArgs e)
        {
            Helpers.OpenWebPage(Resources.FormMain_WebsiteLink);
        }
    }
}
