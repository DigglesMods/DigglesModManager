using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DigglesModManager
{
    internal partial class FormModSettings : Form
    {
        private Mod mod;
        private List<Control> inputControls;

        public FormModSettings(Mod mod)
        {
            this.mod = mod;
            inputControls = new List<Control>();

            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Name = this.mod.DisplayText + ": Settings";
            Text = Name;

            //add for each ModVar control row
            int i = 0;
            foreach (ModVar modVar in mod.Vars)
            {
                int height = i * 23 + 12;
                //add var name as label
                Label varName = new Label();
                varName.AutoSize = true;
                varName.Location = new System.Drawing.Point(12, height);
                varName.Name = modVar.VarName + "_label";
                varName.TabIndex = i + 3;
                varName.Text = modVar.VarName + ":";
                Controls.Add(varName);

                //add value as changeable text box or checkbox
                if (modVar.Type.Equals("bool"))
                {
                    CheckBox value = new CheckBox();
                    value.Location = new System.Drawing.Point(162, height - 3);
                    value.Name = modVar.VarName;
                    value.Checked = ((ModVar<bool>)modVar).Value;
                    value.Size = new System.Drawing.Size(20, 20);
                    value.TabIndex = i + 3;
                    Controls.Add(value);
                    inputControls.Add(value);
                }
                else
                {
                    TextBox value = new TextBox();
                    value.Location = new System.Drawing.Point(162, height - 3);
                    value.Name = modVar.VarName;
                    value.Text = modVar.getValueAsString();
                    value.Size = new System.Drawing.Size(50, 20);
                    value.TabIndex = i + 3;
                    Controls.Add(value);
                    inputControls.Add(value);
                }

                //add description as label
                Label description = new Label();
                description.AutoSize = true;
                description.Location = new System.Drawing.Point(252, height);
                description.Size = new System.Drawing.Size(290, description.Size.Height);           //limit size
                description.Anchor = AnchorStyles.Top | AnchorStyles.Left |  AnchorStyles.Right;    //make label grow automatically on window resize
                description.AutoEllipsis = true;                                                    //add '...' if text does not fit into label bounds
                description.AutoSize = false;                                                       //stop autosizing
                description.Name = modVar.VarName + "_desc";
                description.Text = modVar.Description;
                description.Cursor = Cursors.Help;
                ToolTip tooltip = new ToolTip();
                tooltip.SetToolTip(description, description.Text);

                Controls.Add(description);
                i++;
            }

        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            //save values
            foreach (ModVar modVar in mod.Vars)
            {
                foreach (Control control in inputControls)
                {
                    if (modVar.VarName.Equals(control.Name))
                    {
                        if (modVar.Type.Equals("bool"))
                        {
                            ((ModVar<bool>)modVar).Value = ((CheckBox)control).Checked;
                        }
                        else if (modVar.Type.Equals("int"))
                        {
                            ((ModVar<int>)modVar).Value = int.Parse(control.Text);
                        }
                        else if (modVar.Type.Equals("string"))
                        {
                            ((ModVar<string>)modVar).Value = control.Text;
                        }
                        break;
                    }
                }
            }
            Close();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            //reset values
            foreach (ModVar modVar in mod.Vars)
            {
                foreach (Control control in inputControls)
                {
                    if (modVar.VarName.Equals(control.Name))
                    {
                        if (modVar.Type.Equals("bool"))
                        {
                            ((CheckBox)control).Checked = ((ModVar<bool>)modVar).Value;
                        }
                        else
                        {
                            control.Text = modVar.getValueAsString();
                        }
                        break;
                    }
                }
            }
        }

        private void button_default_Click(object sender, EventArgs e)
        {
            //set values to default
            foreach (ModVar modVar in mod.Vars)
            {
                foreach (Control control in inputControls)
                {
                    if (modVar.VarName.Equals(control.Name))
                    {
                        if (modVar.Type.Equals("bool"))
                        {
                            ((CheckBox)control).Checked = ((ModVar<bool>)modVar).StdValue;
                        }
                        else if (modVar.Type.Equals("int"))
                        {
                            control.Text = ((ModVar<int>)modVar).StdValue.ToString();
                        }
                        else if (modVar.Type.Equals("string"))
                        {
                            control.Text = ((ModVar<string>)modVar).StdValue.ToString();
                        }
                        break;
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
