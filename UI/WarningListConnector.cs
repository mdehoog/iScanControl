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
        private bool clearingControl = false;

        public WarningListConnector(ListCommand command, ComboBox control, Label label, CommandCategory category, string message, string caption)
            : base(command, control, label, category)
        {
            this.message = message;
            this.caption = caption;
        }

        protected override void ControlChanged(object sender, EventArgs e)
        {
            if (clearingControl)
                return;

            if (MessageBox.Show(string.Format(message, CurrentControlValue().Name), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                base.ControlChanged(sender, e);
            }

            clearingControl = true;
            Control.SelectedItem = null;
            clearingControl = false;
        }
    }
}
