using DigglesModManager.Model;
using DigglesModManager.Properties;
using DigglesModManager.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// All installed mods (active and available).
        /// </summary>
        private List<Mod> _installedMods = new List<Mod>();

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
            languageMenuItems.Add("cz", cestinaToolStripMenuItem);
            languageMenuItems.Add("de", deutschToolStripMenuItem);
            languageMenuItems.Add("en", englishToolStripMenuItem);
            languageMenuItems.Add("es", espanolToolStripMenuItem);
            languageMenuItems.Add("fr", francaisToolStripMenuItem);
            languageMenuItems.Add("it", italianoToolStripMenuItem);
            languageMenuItems.Add("nl", nederlandsToolStripMenuItem);
            languageMenuItems.Add("pl", polishToolStripMenuItem);
            languageMenuItems.Add("ru", russianToolStripMenuItem);

            // check, if the exe is in the correct directory
            var correctDirectory = Paths.DetectGameDirectory();

            _moddingService = new ModdingService();
            _progressBarManipulator = new ProgressBarManipulator(this, modProgressStatusBar);

            //get language
            var appSettings = correctDirectory ? _moddingService.ReadAppSettings() : null;
            if (!string.IsNullOrEmpty(appSettings?.Language))
            {
                _language = appSettings.Language;
            }
            Mod.DisplayLanguage = _language;
            setCheckOfLanguageInMenu(true);

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
        /// Updates the images of the mod lists.
        /// </summary>
        private void UpdateModImages()
        {
            foreach (var mod in availableModsListView.Items)
            {
                UpdateModImage(mod as ListViewItem);
            }
            foreach (var mod in activeModsListView.Items)
            {
                UpdateModImage(mod as ListViewItem);
            }
        }

        /// <summary>
        /// Updates the images of a mod list item.
        /// </summary>
        /// <param name="item">ListViewItem which should be updated.</param>
        private void UpdateModImage(ListViewItem item)
        {
            var mod = item.Tag as Mod;
            var messages = mod.GetMessages();
            if (messages.Count > 0)
            {
                item.ImageKey = messages[0].Type.ToString();
            }
            else
            {
                item.ImageKey = "";
            }
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

            //read mods: all other mods are available
            var availableMods = new List<Mod>();
            var modDirectories = (new DirectoryInfo($"{Paths.ModPath}\\{Paths.ModDirectoryName}")).GetDirectories();
            foreach (var modInfo in modDirectories)
            {
                if (!activeMods.Exists(mod => mod.ModDirectoryName.Equals(modInfo.Name)))
                {
                    availableMods.Add(new Mod(modInfo.Name));
                }
            }
            _installedMods = activeMods.Concat(availableMods).ToList();
            foreach (var mod in _installedMods)
            {
                mod.DependencyMessages = ModDependencyResolver.GetDependencyMessages(mod, _installedMods);
            }

            var configErrors = new List<string>();
            //active mods in the saved order (it is the order in which the mods are applied), adjusted to the dependencies
            foreach (var modObject in SortByDependencies(activeMods))
            {
                activeModsListView.Items.Add(CreateModListViewItem(modObject, configErrors));
            }
            foreach (var modObject in availableMods)
            {
                availableModsListView.Items.Add(CreateModListViewItem(modObject, configErrors));
            }
            _progressBarManipulator.Increment();

            _progressBarManipulator.Increment();
            _progressBarManipulator.Finish();

            UpdateModImages();

            foreach (var configError in configErrors)
            {
                Helpers.ShowErrorMessage(configError);
            }
        }

        /// <summary>
        /// Sorts the mods so that each mod is applied after the mods it depends on.
        /// </summary>
        private static List<Mod> SortByDependencies(IList<Mod> mods)
        {
            var sorted = ModDependencyResolver.SortByDependencies(mods, out var circularMods);
            if (circularMods.Count > 0)
            {
                Log.Warning($"Circular mod dependencies: {string.Join(", ", circularMods.Select(m => m.ModDirectoryName))}");
            }
            return sorted;
        }

        private static List<Mod> GetMods(ListView listView)
        {
            return listView.Items.Cast<ListViewItem>().Select(item => (Mod)item.Tag).ToList();
        }

        private ListViewItem FindItem(ListView listView, Mod mod)
        {
            return listView.Items.Cast<ListViewItem>().FirstOrDefault(item => item.Tag == mod);
        }

        /// <summary>
        /// Sorts the active mod list according to the mod dependencies (the selection is kept).
        /// </summary>
        /// <returns>true, if the order was changed</returns>
        private bool ApplyDependencyOrder()
        {
            var current = GetMods(activeModsListView);
            var sorted = SortByDependencies(current);
            if (current.SequenceEqual(sorted))
            {
                return false;
            }
            var items = sorted.Select(mod => FindItem(activeModsListView, mod)).ToList();
            var selected = activeModsListView.SelectedItems.Count > 0 ? activeModsListView.SelectedItems[0] : null;
            activeModsListView.BeginUpdate();
            activeModsListView.Items.Clear();
            activeModsListView.Items.AddRange(items.ToArray());
            activeModsListView.EndUpdate();
            if (selected != null)
            {
                selected.Selected = true;
                selected.EnsureVisible();
            }
            return true;
        }

        private ListViewItem CreateModListViewItem(Mod modObject, List<string> configErrors)
        {
            if (modObject.ConfigError != null)
            {
                configErrors.Add(modObject.ConfigError);
            }
            return new ListViewItem(modObject.GetName(_language), "")
            {
                Tag = modObject,
                ToolTipText = modObject.GetToolTipText(_language)
            };
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
                    descriptionBox.Text = selectedMod.GetDescription(_language);
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
                    descriptionBox.Text = selectedMod.GetDescription(_language);
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
                var selectedMod = (Mod)selectedItem.Tag;
                //required mods are activated, too
                var requiredMods = ModDependencyResolver.GetRequiredMods(selectedMod, _installedMods, out var missingMods);
                if (missingMods.Count > 0)
                {
                    Helpers.ShowErrorMessage(Resources.Dependencies_NotInstalled
                        .Replace("MODS", string.Join("\n", missingMods.Select(id => $"- {id}")))
                        .Replace("MOD", selectedMod.GetName(_language)));
                    return;
                }
                //incompatibilities ("error") between the mods to be activated (e.g. a required mod is incompatible)
                var modsToActivate = requiredMods.Append(selectedMod).ToList();
                var internalConflicts = ModDependencyResolver.GetIncompatiblePairs(modsToActivate, ModIncompatibilityType.Error);
                if (internalConflicts.Count > 0)
                {
                    Helpers.ShowErrorMessage(Resources.Incompatibilities_NotPossible
                        .Replace("MODS", FormatPairs(internalConflicts))
                        .Replace("MOD", selectedMod.GetName(_language)));
                    return;
                }
                //incompatible active mods ("error"): the mod cannot be activated
                var activeMods = GetMods(activeModsListView).Except(modsToActivate).ToList();
                var conflictingMods = activeMods
                    .Where(active => modsToActivate.Any(mod => ModDependencyResolver.AreIncompatible(mod, active, ModIncompatibilityType.Error)))
                    .ToList();
                if (conflictingMods.Count > 0)
                {
                    Helpers.ShowErrorMessage(Resources.Incompatibilities_ErrorActive
                        .Replace("MODS", string.Join("\n", conflictingMods.Select(m => $"- {m.GetName(_language)}")))
                        .Replace("MOD", selectedMod.GetName(_language)));
                    return;
                }
                //not fully compatible active mods ("warning"): the mod is activated, but the user is warned
                var warningMods = activeMods
                    .Where(active => modsToActivate.Any(mod => ModDependencyResolver.AreIncompatible(mod, active, ModIncompatibilityType.Warning)))
                    .ToList();
                if (warningMods.Count > 0)
                {
                    Helpers.ShowMessage(Resources.Incompatibilities_WarningActive
                        .Replace("MODS", string.Join("\n", warningMods.Select(m => $"- {m.GetName(_language)}")))
                        .Replace("MOD", selectedMod.GetName(_language)), Resources.Warning);
                }
                foreach (var requiredMod in requiredMods)
                {
                    var requiredItem = FindItem(availableModsListView, requiredMod);
                    if (requiredItem != null)
                    {
                        availableModsListView.Items.Remove(requiredItem);
                        activeModsListView.Items.Add(requiredItem);
                    }
                }
                availableModsListView.Items.Remove(selectedItem); //remove element from the left
                activeModsListView.Items.Add(selectedItem); //add element on the right
                activeModsListView.SelectedItems.Clear();
                selectedItem.Selected = true;
                ApplyDependencyOrder();
                selectedItem.EnsureVisible();
                UpdateModImages();
            }
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedItems = activeModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedItem = selectedItems[0];
                var selectedMod = (Mod)selectedItem.Tag;
                //mods which require this mod are deactivated, too
                var dependentMods = ModDependencyResolver.GetDependentMods(selectedMod, GetMods(activeModsListView));
                if (dependentMods.Count > 0)
                {
                    var text = Resources.Dependencies_DeactivateDependents
                        .Replace("MODS", string.Join("\n", dependentMods.Select(m => $"- {m.GetName(_language)}")))
                        .Replace("MOD", selectedMod.GetName(_language));
                    if (MessageBox.Show(text, Resources.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                DeactivateMods(dependentMods.Append(selectedMod));
                UpdateModImages();
            }
        }

        /// <summary>
        /// Moves the given mods from the active to the available mods.
        /// </summary>
        private void DeactivateMods(IEnumerable<Mod> mods)
        {
            foreach (var mod in mods)
            {
                var item = FindItem(activeModsListView, mod);
                if (item != null)
                {
                    activeModsListView.Items.Remove(item); //remove element from the right
                    availableModsListView.Items.Add(item); //add element on the left
                }
            }
        }

        private string FormatPairs(IEnumerable<(Mod, Mod)> pairs)
        {
            return string.Join("\n", pairs.Select(pair => $"- {pair.Item1.GetName(_language)} / {pair.Item2.GetName(_language)}"));
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            ResetStatusNote();
            var selectedItems = activeModsListView.SelectedItems;
            if (selectedItems.Count > 0)
            {
                var selectedItem = selectedItems[0];
                MoveActiveModInList(selectedItem, -1);
                if (ApplyDependencyOrder())
                {
                    SetMessage(Resources.Dependencies_OrderChanged, Color.Orange);
                }
                UpdateModImages();
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
                if (ApplyDependencyOrder())
                {
                    SetMessage(Resources.Dependencies_OrderChanged, Color.Orange);
                }
                UpdateModImages();
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
            // check the mod dependencies (e.g. if the json file was edited manually)
            ApplyDependencyOrder();
            var currentActiveMods = GetMods(activeModsListView);
            var modsWithMissingDependencies = currentActiveMods
                .Select(mod => (mod, missing: ModDependencyResolver.GetMissingRequiredMods(mod, currentActiveMods)))
                .Where(entry => entry.missing.Count > 0)
                .Select(entry => $"- {entry.mod.GetName(_language)}: {string.Join(", ", entry.missing)}")
                .ToList();
            if (modsWithMissingDependencies.Count > 0)
            {
                SetMessage(Resources.Error, Color.Red);
                Helpers.ShowErrorMessage(Resources.Dependencies_MissingActive.Replace("MODS", string.Join("\n", modsWithMissingDependencies)));
                return;
            }
            // check the incompatibilities
            var incompatibleMods = ModDependencyResolver.GetIncompatiblePairs(currentActiveMods, ModIncompatibilityType.Error);
            if (incompatibleMods.Count > 0)
            {
                SetMessage(Resources.Error, Color.Red);
                Helpers.ShowErrorMessage(Resources.Incompatibilities_Active.Replace("MODS", FormatPairs(incompatibleMods)));
                return;
            }
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

                var restoreErrors = _moddingService.Restore();
                _progressBarManipulator.Increment();
                if (restoreErrors.Count > 0)
                {
                    Invoke(() => Helpers.ShowErrorMessage(string.Join("\n\n", restoreErrors)));
                }

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
            Mod.DisplayLanguage = language;
            setCheckOfLanguageInMenu(true);
            ReadMods();
        }

        private void cestinaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeLanguage("cz");
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
