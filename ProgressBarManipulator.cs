using System;
using System.Windows.Forms;

namespace DigglesModManager
{
    public class ProgressBarManipulator
    {
        private readonly Form _form;
        private readonly ToolStripProgressBar _modProgressStatusBar;

        public ProgressBarManipulator(Form form, ToolStripProgressBar modProgressStatusBar)
        {
            _form = form;
            _modProgressStatusBar = modProgressStatusBar;
        }

        public bool Reset(int max = 1)
        {
            if (_form.InvokeRequired)
            {
                return (bool)_form.Invoke((Func<int, bool>)Reset, max);
            }
            _modProgressStatusBar.Value = 0;
            _modProgressStatusBar.Maximum = max;
            _modProgressStatusBar.Visible = true;
            return true;
        }

        public bool Increment(int increment = 1)
        {
            if (_form.InvokeRequired)
            {
                return (bool)_form.Invoke((Func<int, bool>)Increment, increment);
            }
            if (_modProgressStatusBar.Maximum > _modProgressStatusBar.Value)
            {
                _modProgressStatusBar.Value += increment;
            }
            return true;
        }

        public bool Finish()
        {
            if (_form.InvokeRequired)
            {
                return (bool)_form.Invoke((Func<bool>)Finish);
            }
            _modProgressStatusBar.Value = _modProgressStatusBar.Maximum;
            //modProgressStatusBar.Visible = false;
            return true;
        }
    }
}
