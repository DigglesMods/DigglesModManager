using System;
using System.Drawing;
using System.Windows.Forms;

namespace DigglesModManager
{
    /// <summary>
    /// Interface for objects used in ToolTipListBox.
    /// </summary>
    internal interface IToolTipDisplayer
    {
        string GetToolTipText();
    }

    /// <summary>
    /// ListBox that displays item-specific tooltips.
    /// </summary>
    internal partial class ToolTipListBox : ListBox
    {
        // The item index that the mouse is currently over
        private int _currentItem;

        // A value indicating if the current item has been set
        private bool _currentItemSet;

        // A value indicating if a tooltip is currently being displayed
        private bool _toolTipDisplayed;

        // Timer that is used to wait for the mouse to hover over an item
        private Timer _toolTipDisplayTimer;

        // Tooltip control
        private ToolTip _toolTip;

        public ToolTipListBox()
        {
            InitializeComponent();

            this.MouseMove += listBox_MouseMove;
            this.MouseLeave += listBox_MouseLeave;

            _currentItemSet = false;
            _toolTipDisplayed = false;
            _toolTipDisplayTimer = new Timer();
            _toolTip = new ToolTip();

            // Set the timer interval to the system time that it takes for a tooltip to appear
            _toolTipDisplayTimer.Interval = SystemInformation.MouseHoverTime;
            _toolTipDisplayTimer.Tick += _toolTipDisplayTimer_Tick;
        }

        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the item that the mouse is currently over
            Point cursorPoint = Cursor.Position;
            cursorPoint = this.PointToClient(cursorPoint);
            int itemIndex = this.IndexFromPoint(cursorPoint);

            if (itemIndex == ListBox.NoMatches)
            {
                // Mouse is over empty space in the listbox so hide tooltip
                _toolTip.Hide(this);
                _currentItemSet = false;
                _toolTipDisplayed = false;
                _toolTipDisplayTimer.Stop();
            }
            else if (!_currentItemSet)
            {
                // Mouse is over a new item so start timer to display tooltip
                _currentItem = itemIndex;
                _currentItemSet = true;
                _toolTipDisplayTimer.Start();
            }
            else if (itemIndex != _currentItem)
            {
                // Mouse is over a different item so hide tooltip and restart timer
                _currentItem = itemIndex;
                _toolTipDisplayTimer.Stop();
                _toolTipDisplayTimer.Start();
                _toolTip.Hide(this);
                _toolTipDisplayed = false;
            }
        }

        private void listBox_MouseLeave(object sender, EventArgs e)
        {
            // Mouse has left listbox so stop timer (tooltip is automatically hidden)
            _currentItemSet = false;
            _toolTipDisplayed = false;
            _toolTipDisplayTimer.Stop();
        }

        void _toolTipDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Display tooltip text since the mouse has hovered over an item
            if (!_toolTipDisplayed && _currentItem != ListBox.NoMatches && _currentItem < this.Items.Count)
            {
                IToolTipDisplayer toolTipDisplayer = this.Items[_currentItem] as IToolTipDisplayer;
                if (toolTipDisplayer != null)
                {
                    _toolTip.SetToolTip(this, toolTipDisplayer.GetToolTipText());
                    _toolTipDisplayed = true;
                }
            }
        }
    }
}
