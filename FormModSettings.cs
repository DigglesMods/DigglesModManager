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

        private void BuildVariableRow(int i, ModSettingsVariable variable, string language)
        {
            var height = i * 23 + 12;
            var variableType = variable.Type.ToString().ToLower();
            //add var name as label
            var label = new Label
            {
                AutoSize = true,
                Location = new Point(12, height),
                Name = variable.ID + "_label",
                TabIndex = i + 3,
                Text = variable.Name(language) + @":"
            };
            Controls.Add(label);

            //add value as changeable text box or checkbox
            Control controlElement;
            if (variableType.Equals("bool"))
            {
                controlElement = new CheckBox
                {
                    Location = new Point(162, height - 3),
                    Name = variable.ID,
                    Checked = (bool)variable.Value,
                    Size = new Size(20, 20),
                    TabIndex = i + 3
                };
            }
            else if (variableType.Equals("select"))
            {
                // Drop Down Menu
                // Create ComboBox object
                controlElement = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Location = new Point(162, height - 3),
                    Name = variable.ID,
                    Size = new Size(70, 20),
                    TabIndex = i + 3
                };

                //Create list entries
                var possibleValueId = 0;
                foreach (ModVariableValue possibleValue in variable.PossibleValues)
                {
                    ((ComboBox)controlElement).Items.Add(new ComboBoxItem
                    {
                        Text = possibleValue.Name(language),
                        Value = possibleValue.Value
                    });
                    //select default or last selected value
                    if (variable.Value.Equals(possibleValue.Value))
                    {
                        ((ComboBox)controlElement).SelectedIndex = possibleValueId;
                    }
                    possibleValueId++;
                }
            }
            else //if (variableType.Equals("int") || variableType.Equals("string"))
            {
                //Text Box
                controlElement = new TextBox
                {
                    Location = new Point(162, height - 3),
                    Name = variable.ID,
                    Text = variable.Value.ToString(),
                    Size = new Size(50, 20),
                    TabIndex = i + 3
                };
            }
            Controls.Add(controlElement);
            _inputControls.Add(controlElement);
            _inputControlMap.Add(variable.ID, controlElement);

            //add description as label
            var description = new Label
            {
                Location = new Point(252, height),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,         //make label grow automatically on window resize
                AutoEllipsis = true,                                                        //add '...' if text does not fit into label bounds
                AutoSize = false,                                                           //no autosizing
                Name = variable.ID + "_desc",
                Text = variable.Description(language),
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
                        case ModVariableType.Select:
                            modVariable.Value = (((ComboBox)control).SelectedItem as ComboBoxItem).Value;
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
                        case ModVariableType.Select:
                            foreach (ComboBoxItem item in ((ComboBox)control).Items)
                            {
                                if (item.Value.Equals(modVariable.Value))
                                {
                                    ((ComboBox)control).SelectedItem = item;
                                    break;
                                }
                            }
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
                        case ModVariableType.Select:
                            foreach (ComboBoxItem item in ((ComboBox)control).Items)
                            {
                                if (item.Value.Equals(modVariable.DefaultValue))
                                {
                                    ((ComboBox)control).SelectedItem = item;
                                    break;
                                }
                            }
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

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
