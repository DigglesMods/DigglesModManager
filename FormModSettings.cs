using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DigglesModManager.Model;

namespace DigglesModManager
{
    internal partial class FormModSettings : Form
    {
        private readonly Mod _mod;
        private readonly Dictionary<string, Control> _inputControlMap = new Dictionary<string, Control>();
        private readonly List<Control> _inputControls;

        public FormModSettings(Mod mod)
        {
            _mod = mod;
            _inputControls = new List<Control>();

            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Name = _mod.DisplayText + " - Settings";
            Text = Name;

            if (mod.Settings != null)
            {
                //NEW JSON-FORMAT, only if json-file is present
                var i = 0;
                foreach (var modVariable in mod.Settings.Variables)
                {
                    BuildVariableRow(i, modVariable);
                    i++;
                }
            }
            else
            {
                //OLD .DM-FORMAT
                //add for each ModVar control row
                var i = 0;
                foreach (var modVar in mod.Vars)
                {
                    object value = null;
                    if (modVar.Type.Equals("bool"))
                    {
                        value = ((ModVar<bool>)modVar).Value;
                    }
                    else if (modVar.Type.Equals("int"))
                    {
                        value = ((ModVar<int>)modVar).Value;
                    }
                    BuildVariableRow(i, modVar.VarName, modVar.Description, modVar.Type, value);
                    i++;
                }
            }

        }

        private void BuildVariableRow(int i, ModSettingsVariable modVariable)
        {
            BuildVariableRow(i, modVariable.Name, modVariable.Description, modVariable.Type.ToString(), modVariable.Value);
        }

        private void BuildVariableRow(int i, string varName, string varDescription, string varType, object value)
        {
            var height = i * 23 + 12;
            //add var name as label
            var label = new Label
            {
                AutoSize = true,
                Location = new Point(12, height),
                Name = varName + "_label",
                TabIndex = i + 3,
                Text = varName + @":"
            };
            Controls.Add(label);

            //add value as changeable text box or checkbox
            if (varType.ToLower().Equals("bool"))
            {
                var checkBox = new CheckBox
                {
                    Location = new Point(162, height - 3),
                    Name = varName,
                    Checked = (bool)value,
                    Size = new Size(20, 20),
                    TabIndex = i + 3
                };
                Controls.Add(checkBox);
                _inputControls.Add(checkBox);
                _inputControlMap.Add(varName, checkBox);
            }
            else
            {
                var textBox = new TextBox
                {
                    Location = new Point(162, height - 3),
                    Name = varName,
                    Text = value.ToString(),
                    Size = new Size(50, 20),
                    TabIndex = i + 3
                };
                Controls.Add(textBox);
                _inputControls.Add(textBox);
                _inputControlMap.Add(varName, textBox);
            }

            //add description as label
            var description = new Label
            {
                Location = new Point(252, height),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,         //make label grow automatically on window resize
                AutoEllipsis = true,                                                        //add '...' if text does not fit into label bounds
                AutoSize = false,                                                           //no autosizing
                Name = varName + "_desc",
                Text = varDescription,
                Cursor = Cursors.Help
            };
            description.Size = new Size(290, description.Size.Height);

            var tooltip = new ToolTip();
            tooltip.SetToolTip(description, description.Text);

            Controls.Add(description);
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            //save values

            //JSON-Format
            if (_mod.Settings != null)
            {
                foreach (var modVariable in _mod.Settings.Variables)
                {
                    var control = _inputControlMap[modVariable.Name];

                    switch (modVariable.Type)
                    {
                        case ModVariableType.Bool:
                            modVariable.Value = ((CheckBox)control).Checked;
                            break;
                        case ModVariableType.Int:
                            modVariable.Value = int.Parse(control.Text);
                            break;
                        case ModVariableType.String:
                        default:
                            modVariable.Value = control.Text;
                            break;
                    }

                }
            }
            //.dm-Format
            else
                foreach (var modVar in _mod.Vars)
                {
                    foreach (var control in _inputControls)
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

            //JSON-Format
            if (_mod.Settings != null)
            {
                foreach (var modVariable in _mod.Settings.Variables)
                {
                    var control = _inputControlMap[modVariable.Name];

                    switch (modVariable.Type)
                    {
                        case ModVariableType.Bool:
                            ((CheckBox)control).Checked = (bool)modVariable.Value;
                            break;
                        case ModVariableType.Int:
                        case ModVariableType.String:
                        default:
                            control.Text = modVariable.Value.ToString();
                            break;
                    }
                }
            }
            //.dm-Format
            else
                foreach (var modVar in _mod.Vars)
                {
                    foreach (var control in _inputControls)
                    {
                        if (modVar.VarName.Equals(control.Name))
                        {
                            if (modVar.Type.Equals("bool"))
                            {
                                ((CheckBox)control).Checked = ((ModVar<bool>)modVar).Value;
                            }
                            else
                            {
                                control.Text = modVar.GetValueAsString();
                            }
                            break;
                        }
                    }
                }
        }

        private void button_default_Click(object sender, EventArgs e)
        {
            //set values to default

            //JSON-Format
            if (_mod.Settings != null)
            {
                foreach (var modVariable in _mod.Settings.Variables)
                {
                    var control = _inputControlMap[modVariable.Name];

                    switch (modVariable.Type)
                    {
                        case ModVariableType.Bool:
                            ((CheckBox)control).Checked = (bool)modVariable.DefaultValue;
                            break;
                        case ModVariableType.Int:
                        case ModVariableType.String:
                        default:
                            control.Text = modVariable.DefaultValue.ToString();
                            break;
                    }
                }
            }
            //.dm-Format
            else
                foreach (var modVar in _mod.Vars)
                {
                    foreach (var control in _inputControls)
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
            Close();
        }
    }
}
