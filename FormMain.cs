using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DigglesModManager.Properties;
using DigglesModManager.Service;
using Newtonsoft.Json;
using System.Text;
using DigglesModManager.Model;
using System.Threading.Tasks;

namespace DigglesModManager
{
    public partial class FormMain : Form
    {
        private readonly List<Mod> _inactiveMods = new List<Mod>();
        private readonly List<Mod> _activeMods = new List<Mod>();

        public static string _language = "en";
        private static Dictionary<string, ToolStripMenuItem> languageMenuItems = new Dictionary<string, ToolStripMenuItem>();

        private bool moddingServiceRunning = false;

        private readonly ModdingService _moddingService;

        private readonly ProgressBarManipulator _progressBarManipulator;

        public FormMain()
        {
            InitializeComponent();
            Text = string.Format(Resources.FormMainTitle, typeof(FormMain).Assembly.GetName().Version);

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
            _progressBarManipulator = new ProgressBarManipulator(modProgressStatusBar);

            if (!File.Exists($"{Paths.ExePath}\\{Paths.WigglesExecutableName}"))
            {
                Helpers.ShowErrorMessage(Resources.FormMain_CouldNotFindWigglesExeErrorText, Resources.Error);
            }
            else
            {
                ResetStatusNote();
                ReadMods();
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
        private void SetMessage(string text, Color color)
        {
            statusBarLabelRight.Text = text;
            statusBarLabelRight.ForeColor = color;
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

            _inactiveMods.Clear();
            _activeMods.Clear();

            _moddingService.ReadModsFromFiles(_activeMods, _progressBarManipulator);

            //read mods
            var modDirectories = (new DirectoryInfo(Paths.ModPath + "\\" + Paths.ModDirectoryName)).GetDirectories();
            foreach (var modInfo in modDirectories)
            {
                if (!_activeMods.Exists(mod => mod.ModDirectoryName.Equals(modInfo.Name)))
                {
                    _inactiveMods.Add(new Mod(modInfo.Name));
                }
            }
            _inactiveMods.Sort();
            _progressBarManipulator.Increment();

            ChangeDataSource();

            _progressBarManipulator.Increment();
            _progressBarManipulator.Finish();
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
            //do nothing
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.moddingServiceRunning)
            {
                //find settings file
                int selectedIndex = installedModsListBox.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < _activeMods.Count)
                {
                    var hasSettings = _activeMods.ElementAt(selectedIndex).Config.SettingsVariables != null
                        && _activeMods.ElementAt(selectedIndex).Config.SettingsVariables.Count > 0;

                    //set settings button enabled
                    modSettingsButton.Enabled = hasSettings;
                    modSettingsMenuButton.Enabled = hasSettings;
                }
            }
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedIndex = availableModsListBox.SelectedIndex;
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
            var selectedIndex = installedModsListBox.SelectedIndex;
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
            var selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < _activeMods.Count)
            {
                MoveActiveModInList(selectedIndex, -1);
            }
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _activeMods.Count - 1)
            {
                MoveActiveModInList(selectedIndex, 1);
            }
        }

        /// <summary>
        /// Move an item inside of the _activeMods list up or down.
        /// </summary>
        /// <param name="index">The to-be-moved item.</param>
        /// <param name="relativePosition">The distance and direction to let the item 'travel'. -1 is up, 2 is down (two steps)</param>
        private void MoveActiveModInList(int index, int relativePosition)
        {
            var mod = _activeMods.ElementAt(index); //get mod
            _activeMods.RemoveAt(index); //remove mod
            index += relativePosition;
            _activeMods.Insert(index, mod); //add at new position

            ChangeDataSource();
            installedModsListBox.SetSelected(index, true);
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            ReadMods();
        }

        private void setUIToModdingState(bool moddingState)
        {
            // set wait cursor
            Application.UseWaitCursor = moddingState;

            var buttonsEnabled = !moddingState;
            // Refresh Button
            this.refreshButton.Enabled = buttonsEnabled;
            // Arrow Buttons
            this.installModButton.Enabled = buttonsEnabled;
            this.uninstallModButton.Enabled = buttonsEnabled;
            this.moveUpButton.Enabled = buttonsEnabled;
            this.moveDownButton.Enabled = buttonsEnabled;
            // Mod Settings Buttons
            this.moddingServiceRunning = moddingState;
            if (buttonsEnabled)
            {
                this.listBox2_SelectedIndexChanged(null, null);
            }
            else
            {
                this.modSettingsButton.Enabled = false;
                this.modSettingsMenuButton.Enabled = false;
            }
            // Lets Mod Buttons
            this.letsModButton.Enabled = buttonsEnabled;
            this.letsModMenuButton.Enabled = buttonsEnabled;
        }

        private void button_mod_Click(object sender, EventArgs e)
        {
            // set user interface into the modding state (disable buttons etc)
            this.setUIToModdingState(true);
            // Modding in second task to avoid UI freeze
            Task.Factory.StartNew(() => {
                var warning = false;
                SetMessage("Please wait", Color.Black);
                //get file count of all active mods
                var fileCount = 0;
                foreach (var mod in _activeMods)
                {
                    fileCount += mod.FileCount;
                }
                //reset progress bar to new file count
                _progressBarManipulator.Reset(fileCount + 3);
                _progressBarManipulator.Increment();

                _moddingService.Restore();
                _progressBarManipulator.Increment();
                foreach (var mod in _activeMods)
                {
                    var modDir = new DirectoryInfo(Paths.ModPath + "\\" + Paths.ModDirectoryName + "\\" + mod.ModDirectoryName);
                    warning = _moddingService.LetsMod(mod, modDir, new DirectoryInfo(Paths.ExePath), _activeMods, _progressBarManipulator) || warning;
                }

                _moddingService.SaveActiveMods(_language, _activeMods);
                _progressBarManipulator.Finish();

                if (warning)
                {
                    SetMessage("Warning", Color.Orange);
                }
                else
                {
                    SetMessage("Modding was successful", Color.Green);
                }
                // set user interface into normal state (enable buttons etc)
                this.setUIToModdingState(false);
            });
        }

        private void letsModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_mod_Click(sender, e);
        }

        private void button_mod_settings_Click(object sender, EventArgs e)
        {
            var selectedIndex = installedModsListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _activeMods.Count)
            {
                var mod = _activeMods.ElementAt(selectedIndex); //get mod
                var form = new FormModSettings(mod, _language);
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
            //refresh view
            _inactiveMods.Sort();
            ChangeDataSource();
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
