namespace DigglesModManager
{
    partial class DigglesModManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DigglesModManager));
            this.listBox1 = new ToolTipListBox();
            this.listBox2 = new ToolTipListBox();
            this.button_right = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_mod = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_refresh = new System.Windows.Forms.Button();
            this.button_up = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.label_message = new System.Windows.Forms.Label();
            this.button_mod_settings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 28);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(282, 147);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(340, 28);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(282, 147);
            this.listBox2.TabIndex = 1;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // button_right
            // 
            this.button_right.Location = new System.Drawing.Point(300, 76);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(34, 23);
            this.button_right.TabIndex = 2;
            this.button_right.Text = ">";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Click += new System.EventHandler(this.button_right_Click);
            // 
            // button_left
            // 
            this.button_left.Location = new System.Drawing.Point(300, 105);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(34, 23);
            this.button_left.TabIndex = 3;
            this.button_left.Text = "<";
            this.button_left.UseVisualStyleBackColor = true;
            this.button_left.Click += new System.EventHandler(this.button_left_Click);
            // 
            // button_mod
            // 
            this.button_mod.Location = new System.Drawing.Point(547, 182);
            this.button_mod.Name = "button_mod";
            this.button_mod.Size = new System.Drawing.Size(75, 23);
            this.button_mod.TabIndex = 4;
            this.button_mod.Text = "Let\'s Mod";
            this.button_mod.UseVisualStyleBackColor = true;
            this.button_mod.Click += new System.EventHandler(this.button_mod_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Inactive Mods:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(340, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Active Mods:";
            // 
            // button_refresh
            // 
            this.button_refresh.Location = new System.Drawing.Point(12, 182);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(77, 23);
            this.button_refresh.TabIndex = 7;
            this.button_refresh.Text = "Refresh";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // button_up
            // 
            this.button_up.Location = new System.Drawing.Point(628, 75);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(43, 23);
            this.button_up.TabIndex = 8;
            this.button_up.Text = "Up";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_down
            // 
            this.button_down.Location = new System.Drawing.Point(628, 105);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(43, 23);
            this.button_down.TabIndex = 9;
            this.button_down.Text = "Down";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // label_message
            // 
            this.label_message.AutoSize = true;
            this.label_message.Location = new System.Drawing.Point(491, 187);
            this.label_message.Name = "label_message";
            this.label_message.Size = new System.Drawing.Size(50, 13);
            this.label_message.TabIndex = 10;
            this.label_message.Text = "Message";
            // 
            // button_mod_settings
            // 
            this.button_mod_settings.Enabled = false;
            this.button_mod_settings.Location = new System.Drawing.Point(340, 182);
            this.button_mod_settings.Name = "button_mod_settings";
            this.button_mod_settings.Size = new System.Drawing.Size(80, 23);
            this.button_mod_settings.TabIndex = 11;
            this.button_mod_settings.Text = "Mod Settings";
            this.button_mod_settings.UseVisualStyleBackColor = true;
            this.button_mod_settings.Click += new System.EventHandler(this.button_mod_settings_Click);
            // 
            // DigglesModManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 224);
            this.Controls.Add(this.button_mod_settings);
            this.Controls.Add(this.label_message);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_mod);
            this.Controls.Add(this.button_left);
            this.Controls.Add(this.button_right);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DigglesModManager";
            this.Text = "DigglesModManager v0.2.5";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolTipListBox listBox1;
        private ToolTipListBox listBox2;
        private System.Windows.Forms.Button button_right;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_mod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Label label_message;
        private System.Windows.Forms.Button button_mod_settings;
    }
}

