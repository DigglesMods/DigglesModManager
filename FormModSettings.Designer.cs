namespace DigglesModManager
{
    partial class FormModSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModSettings));
            this.button_save = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_default = new System.Windows.Forms.Button();
            this.process1 = new System.Diagnostics.Process();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_save.Location = new System.Drawing.Point(12, 180);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Apply";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(94, 180);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 23);
            this.button_reset.TabIndex = 1;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // button_default
            // 
            this.button_default.Location = new System.Drawing.Point(437, 180);
            this.button_default.Name = "button_default";
            this.button_default.Size = new System.Drawing.Size(96, 23);
            this.button_default.TabIndex = 2;
            this.button_default.Text = "Set Default";
            this.button_default.UseVisualStyleBackColor = true;
            this.button_default.Click += new System.EventHandler(this.button_default_Click);
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // FormModSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 215);
            this.Controls.Add(this.button_default);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormModSettings";
            this.Text = "FormModSettings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_default;
        private System.Diagnostics.Process process1;
    }
}