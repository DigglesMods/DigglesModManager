namespace DigglesModManager
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.installModButton = new System.Windows.Forms.Button();
            this.uninstallModButton = new System.Windows.Forms.Button();
            this.letsModButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.label_message = new System.Windows.Forms.Label();
            this.modSettingsButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarLabelLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarLabelSpring1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarLabelRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.modProgressStatusBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.letsModMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.quitMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modSettingsMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wikiMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpModsMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpModsDeutschMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpModsEnglishMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.availableModsListBox = new DigglesModManager.ToolTipListBox();
            this.installedModsListBox = new DigglesModManager.ToolTipListBox();
            this.mainPanel.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // installModButton
            // 
            this.installModButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installModButton.Location = new System.Drawing.Point(256, 53);
            this.installModButton.Name = "installModButton";
            this.installModButton.Size = new System.Drawing.Size(40, 32);
            this.installModButton.TabIndex = 2;
            this.installModButton.Text = "▶";
            this.installModButton.UseVisualStyleBackColor = true;
            this.installModButton.Click += new System.EventHandler(this.button_right_Click);
            // 
            // uninstallModButton
            // 
            this.uninstallModButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uninstallModButton.Location = new System.Drawing.Point(256, 91);
            this.uninstallModButton.Name = "uninstallModButton";
            this.uninstallModButton.Size = new System.Drawing.Size(40, 32);
            this.uninstallModButton.TabIndex = 3;
            this.uninstallModButton.Text = "◀";
            this.uninstallModButton.UseVisualStyleBackColor = true;
            this.uninstallModButton.Click += new System.EventHandler(this.button_left_Click);
            // 
            // letsModButton
            // 
            this.letsModButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.letsModButton.Location = new System.Drawing.Point(604, 27);
            this.letsModButton.Name = "letsModButton";
            this.letsModButton.Size = new System.Drawing.Size(94, 183);
            this.letsModButton.TabIndex = 4;
            this.letsModButton.Text = "Let\'s Mod";
            this.letsModButton.UseVisualStyleBackColor = true;
            this.letsModButton.Click += new System.EventHandler(this.button_mod_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.Location = new System.Drawing.Point(256, 143);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(40, 40);
            this.refreshButton.TabIndex = 7;
            this.refreshButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moveUpButton.Location = new System.Drawing.Point(558, 53);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 2);
            this.moveUpButton.Size = new System.Drawing.Size(40, 32);
            this.moveUpButton.TabIndex = 8;
            this.moveUpButton.Text = "▲";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.button_up_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moveDownButton.Location = new System.Drawing.Point(558, 91);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 2);
            this.moveDownButton.Size = new System.Drawing.Size(40, 32);
            this.moveDownButton.TabIndex = 9;
            this.moveDownButton.Text = "▼";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.button_down_Click);
            // 
            // label_message
            // 
            this.label_message.AutoSize = true;
            this.label_message.Location = new System.Drawing.Point(555, 18);
            this.label_message.Name = "label_message";
            this.label_message.Size = new System.Drawing.Size(50, 13);
            this.label_message.TabIndex = 10;
            this.label_message.Text = "Message";
            this.label_message.Visible = false;
            // 
            // modSettingsButton
            // 
            this.modSettingsButton.BackColor = System.Drawing.SystemColors.Control;
            this.modSettingsButton.Enabled = false;
            this.modSettingsButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modSettingsButton.Image = ((System.Drawing.Image)(resources.GetObject("modSettingsButton.Image")));
            this.modSettingsButton.Location = new System.Drawing.Point(558, 143);
            this.modSettingsButton.Name = "modSettingsButton";
            this.modSettingsButton.Size = new System.Drawing.Size(40, 40);
            this.modSettingsButton.TabIndex = 11;
            this.modSettingsButton.UseVisualStyleBackColor = false;
            this.modSettingsButton.Click += new System.EventHandler(this.button_mod_settings_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainPanel.Controls.Add(this.label_message);
            this.mainPanel.Controls.Add(this.refreshButton);
            this.mainPanel.Controls.Add(this.modSettingsButton);
            this.mainPanel.Controls.Add(this.availableModsListBox);
            this.mainPanel.Controls.Add(this.uninstallModButton);
            this.mainPanel.Controls.Add(this.moveDownButton);
            this.mainPanel.Controls.Add(this.installModButton);
            this.mainPanel.Controls.Add(this.moveUpButton);
            this.mainPanel.Controls.Add(this.installedModsListBox);
            this.mainPanel.Location = new System.Drawing.Point(0, 27);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(598, 188);
            this.mainPanel.TabIndex = 12;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLabelLeft,
            this.statusBarLabelSpring1,
            this.statusBarLabelRight,
            this.modProgressStatusBar});
            this.statusBar.Location = new System.Drawing.Point(0, 215);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(703, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 14;
            this.statusBar.Text = "Hello!";
            // 
            // statusBarLabelLeft
            // 
            this.statusBarLabelLeft.Name = "statusBarLabelLeft";
            this.statusBarLabelLeft.Size = new System.Drawing.Size(42, 17);
            this.statusBarLabelLeft.Text = "Ready.";
            // 
            // statusBarLabelSpring1
            // 
            this.statusBarLabelSpring1.Name = "statusBarLabelSpring1";
            this.statusBarLabelSpring1.Size = new System.Drawing.Size(535, 17);
            this.statusBarLabelSpring1.Spring = true;
            // 
            // statusBarLabelRight
            // 
            this.statusBarLabelRight.Margin = new System.Windows.Forms.Padding(0, 3, 10, 2);
            this.statusBarLabelRight.Name = "statusBarLabelRight";
            this.statusBarLabelRight.Size = new System.Drawing.Size(16, 17);
            this.statusBarLabelRight.Text = "...";
            // 
            // modProgressStatusBar
            // 
            this.modProgressStatusBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.modProgressStatusBar.Margin = new System.Windows.Forms.Padding(1, 3, -8, 3);
            this.modProgressStatusBar.Name = "modProgressStatusBar";
            this.modProgressStatusBar.Size = new System.Drawing.Size(92, 16);
            this.modProgressStatusBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.modProgressStatusBar.Value = 48;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(703, 24);
            this.mainMenuStrip.TabIndex = 15;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.letsModMenuButton,
            this.quitMenuButton});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // letsModMenuButton
            // 
            this.letsModMenuButton.Name = "letsModMenuButton";
            this.letsModMenuButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.letsModMenuButton.Size = new System.Drawing.Size(166, 22);
            this.letsModMenuButton.Text = "Let\'s Mod";
            this.letsModMenuButton.Click += new System.EventHandler(this.letsModToolStripMenuItem_Click);
            // 
            // quitMenuButton
            // 
            this.quitMenuButton.Name = "quitMenuButton";
            this.quitMenuButton.Size = new System.Drawing.Size(166, 22);
            this.quitMenuButton.Text = "Quit";
            this.quitMenuButton.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modSettingsMenuButton});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // modSettingsMenuButton
            // 
            this.modSettingsMenuButton.Enabled = false;
            this.modSettingsMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("modSettingsMenuButton.Image")));
            this.modSettingsMenuButton.Name = "modSettingsMenuButton";
            this.modSettingsMenuButton.Size = new System.Drawing.Size(144, 22);
            this.modSettingsMenuButton.Text = "Mod Settings";
            this.modSettingsMenuButton.Click += new System.EventHandler(this.modSettingsMenuButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wikiMenuButton,
            this.helpModsMenuButton,
            this.aboutMenuButton});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // wikiMenuButton
            // 
            this.wikiMenuButton.Name = "wikiMenuButton";
            this.wikiMenuButton.Size = new System.Drawing.Size(107, 22);
            this.wikiMenuButton.Text = "Wiki";
            this.wikiMenuButton.Click += new System.EventHandler(this.wikiToolStripMenuItem_Click);
            // 
            // helpModsMenuButton
            // 
            this.helpModsMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpModsDeutschMenuButton,
            this.helpModsEnglishMenuButton});
            this.helpModsMenuButton.Name = "helpModsMenuButton";
            this.helpModsMenuButton.Size = new System.Drawing.Size(107, 22);
            this.helpModsMenuButton.Text = "Mods";
            // 
            // helpModsDeutschMenuButton
            // 
            this.helpModsDeutschMenuButton.Name = "helpModsDeutschMenuButton";
            this.helpModsDeutschMenuButton.Size = new System.Drawing.Size(117, 22);
            this.helpModsDeutschMenuButton.Text = "Deutsch";
            this.helpModsDeutschMenuButton.Click += new System.EventHandler(this.deutschToolStripMenuItem_Click);
            // 
            // helpModsEnglishMenuButton
            // 
            this.helpModsEnglishMenuButton.Name = "helpModsEnglishMenuButton";
            this.helpModsEnglishMenuButton.Size = new System.Drawing.Size(117, 22);
            this.helpModsEnglishMenuButton.Text = "English";
            this.helpModsEnglishMenuButton.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // aboutMenuButton
            // 
            this.aboutMenuButton.Name = "aboutMenuButton";
            this.aboutMenuButton.Size = new System.Drawing.Size(107, 22);
            this.aboutMenuButton.Text = "About";
            this.aboutMenuButton.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // availableModsListBox
            // 
            this.availableModsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.availableModsListBox.FormattingEnabled = true;
            this.availableModsListBox.Location = new System.Drawing.Point(0, 0);
            this.availableModsListBox.Name = "availableModsListBox";
            this.availableModsListBox.Size = new System.Drawing.Size(250, 251);
            this.availableModsListBox.TabIndex = 0;
            this.availableModsListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // installedModsListBox
            // 
            this.installedModsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.installedModsListBox.FormattingEnabled = true;
            this.installedModsListBox.Location = new System.Drawing.Point(302, 0);
            this.installedModsListBox.Name = "installedModsListBox";
            this.installedModsListBox.Size = new System.Drawing.Size(250, 251);
            this.installedModsListBox.TabIndex = 1;
            this.installedModsListBox.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 237);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.letsModButton);
            this.Controls.Add(this.mainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "FormMain";
            this.Text = "DigglesModManager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolTipListBox availableModsListBox;
        private ToolTipListBox installedModsListBox;
        private System.Windows.Forms.Button installModButton;
        private System.Windows.Forms.Button uninstallModButton;
        private System.Windows.Forms.Button letsModButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Label label_message;
        private System.Windows.Forms.Button modSettingsButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBarLabelLeft;
        private System.Windows.Forms.ToolStripStatusLabel statusBarLabelRight;
        private System.Windows.Forms.ToolStripProgressBar modProgressStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBarLabelSpring1;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem letsModMenuButton;
        private System.Windows.Forms.ToolStripMenuItem quitMenuButton;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modSettingsMenuButton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuButton;
        private System.Windows.Forms.ToolStripMenuItem helpModsMenuButton;
        private System.Windows.Forms.ToolStripMenuItem helpModsDeutschMenuButton;
        private System.Windows.Forms.ToolStripMenuItem helpModsEnglishMenuButton;
        private System.Windows.Forms.ToolStripMenuItem wikiMenuButton;
    }
}

