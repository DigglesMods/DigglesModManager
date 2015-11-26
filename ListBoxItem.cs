using System;

namespace DigglesModManager
{
    internal class ListBoxItem : IToolTipDisplayer, System.IComparable
    {
        public string DisplayText { get; private set; }
        public string ToolTipText { get; private set; }

        // Constructor
        public ListBoxItem(string displayText, string toolTipText)
        {
            DisplayText = displayText;
            ToolTipText = toolTipText;
        }

        // Returns the display text of this item.
        public override string ToString()
        {
            return DisplayText;
        }
        // Returns the tooltip text of this item.
        public string GetToolTipText()
        {
            return ToolTipText;
        }

        public override bool Equals(System.Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            ListBoxItem element = (ListBoxItem)obj;
            return DisplayText.Equals(element.DisplayText);
        }

        public override int GetHashCode()
        {
            return DisplayText.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;

            ListBoxItem element = (ListBoxItem)obj;
            return DisplayText.CompareTo(element.DisplayText);
        }
    }
}
