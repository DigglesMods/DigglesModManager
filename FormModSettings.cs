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

        public FormModSettings(Mod mod, string language)
        {
            _mod = mod;
            _inputControls = new List<Control>();

            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Name = _mod.ToString() + " - Settings";
            Text = Name;
            
            var i = 0;
            if (_mod.Config != null)
            {
                foreach (var modVariable in mod.Config.SettingsVariables)
                {
                    BuildVariableRow(i, modVariable, language);
                    i++;
                }
            }
        }

        private void BuildVariableRow(int i, ModSettingsVariable modVariable, string language)
        {
            BuildVariableRow(i, modVariable.ID, modVariable.Name.getString(language), 
                modVariable.Description.getString(language), modVariable.Type.ToString(), modVariable.Value);
        }

        private void BuildVariableRow(int i, string varId, string varName, string varDescription, string varType, object value)
        {
            var height = i * 23 + 12;
            //add var name as label
            var label = new Label
            {
                AutoSize = true,
                Location = new Point(12, height),
                Name = varId + "_label",
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
                    Name = varId,
                    Checked = (bool)value,
                    Size = new Size(20, 20),
                    TabIndex = i + 3
                };
                Controls.Add(checkBox);
                _inputControls.Add(checkBox);
                _inputControlMap.Add(varId, checkBox);
            }
            else
            {
                var textBox = new TextBox
                {
                    Location = new Point(162, height - 3),
                    Name = varId,
                    Text = value.ToString(),
                    Size = new Size(50, 20),
                    TabIndex = i + 3
                };
                Controls.Add(textBox);
                _inputControls.Add(textBox);
                _inputControlMap.Add(varId, textBox);
            }

            //add description as label
            var description = new Label
            {
                Location = new Point(252, height),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,         //make label grow automatically on window resize
                AutoEllipsis = true,                                                        //add '...' if text does not fit into label bounds
                AutoSize = false,                                                           //no autosizing
                Name = varId + "_desc",
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
            if (_mod.Config != null)
            {
                foreach (var modVariable in _mod.Config.SettingsVariables)
                {
                    var control = _inputControlMap[modVariable.ID];

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
            Close();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            //reset values
            if (_mod.Config != null)
            {
                foreach (var modVariable in _mod.Config.SettingsVariables)
                {
                    var control = _inputControlMap[modVariable.ID];

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
        }

        private void button_default_Click(object sender, EventArgs e)
        {
            //set values to default
            if (_mod.Config != null)
            {
                foreach (var modVariable in _mod.Config.SettingsVariables)
                {
                    var control = _inputControlMap[modVariable.ID];

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
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
