using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigglesModManager
{
    public class ProgressBarManipulator
    {
        private readonly ToolStripProgressBar _modProgressStatusBar;

        public ProgressBarManipulator(ToolStripProgressBar modProgressStatusBar)
        {
            _modProgressStatusBar = modProgressStatusBar;
        }

        public void Reset(int max = 1)
        {
            _modProgressStatusBar.Value = 0;
            _modProgressStatusBar.Maximum = max;
            _modProgressStatusBar.Visible = true;
        }

        public void Increment(int increment = 1)
        {
            if (_modProgressStatusBar.Maximum > _modProgressStatusBar.Value)
                _modProgressStatusBar.Value += increment;
        }

        public void Finish()
        {
            _modProgressStatusBar.Value = _modProgressStatusBar.Maximum;
            //modProgressStatusBar.Visible = false;
        }
    }
}
