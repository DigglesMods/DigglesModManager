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
        /// <param name="modVarHeight">Height of the mod variables part.</param>
        private void InitializeComponent(int modVarHeight)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModSettings));
            this.okButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.setDefaultButton = new System.Windows.Forms.Button();
            this.process1 = new System.Diagnostics.Process();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            var buttonY = modVarHeight + 7;
            // 
            // okButton
            // 
            this.okButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.okButton.Location = new System.Drawing.Point(352, buttonY);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(98, buttonY);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // setDefaultButton
            // 
            this.setDefaultButton.Location = new System.Drawing.Point(12, buttonY);
            this.setDefaultButton.Name = "setDefaultButton";
            this.setDefaultButton.Size = new System.Drawing.Size(80, 23);
            this.setDefaultButton.TabIndex = 2;
            this.setDefaultButton.Text = "Set Default";
            this.setDefaultButton.UseVisualStyleBackColor = true;
            this.setDefaultButton.Click += new System.EventHandler(this.button_default_Click);
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
            // closeButton
            // 
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.closeButton.Location = new System.Drawing.Point(446, buttonY);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(88, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Cancel";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // FormModSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, buttonY + 35);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.setDefaultButton);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormModSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormModSettings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button setDefaultButton;
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.Button closeButton;
    }
}