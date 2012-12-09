using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.Windows.Forms;

namespace Profiler.UI
{
    public class WarningListConnector : ListConnector
    {
        private readonly string message;
        private readonly string caption;
        private readonly bool clearSelectedItem;
        private bool clearingControl = false;

        public WarningListConnector(ListCommand command, ComboBox control, Label label, CommandCategory category, string message, string caption, bool clearSelectedItem)
            : base(command, control, label, category)
        {
            this.message = message;
            this.caption = caption;
            this.clearSelectedItem = clearSelectedItem;
        }

        protected override void ControlChanged(object sender, EventArgs e)
        {
            if (clearingControl)
                return;

            if (settingControl || MessageBox.Show(string.Format(message, CurrentControlValue().Name), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                base.ControlChanged(sender, e);
            }

            if (clearSelectedItem)
            {
                clearingControl = true;
                Control.SelectedItem = null;
                clearingControl = false;
            }
        }
    }
}
