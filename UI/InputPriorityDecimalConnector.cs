using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qixle.iScanDuo.Controller.Protocol;
using System.Windows.Forms;

namespace Qixle.iScanDuo.Controller.UI
{
    public class InputPriorityDecimalConnector : DecimalConnector
    {
        private readonly ListConnector inputSelectConnector;

        public InputPriorityDecimalConnector(DecimalCommand command, NumericUpDown control, Label label, CommandCategory category, ListConnector inputSelectConnector)
            : base(command, control, label, category)
        {
            this.inputSelectConnector = inputSelectConnector;
            inputSelectConnector.AddListControlChangeListener(new EventHandler(InputSelectControlChanged));
        }

        public override void QueryValue()
        {
            if (inputSelectConnector.CurrentControlValue().Value > 0)
            {
                base.QueryValue();
            }
        }

        private void InputSelectControlChanged(object sender, EventArgs e)
        {
            QueryValue();
            Control.Enabled = inputSelectConnector.CurrentControlValue().Value > 0;
        }
    }
}
