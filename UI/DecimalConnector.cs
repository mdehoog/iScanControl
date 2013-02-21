using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qixle.iScanDuo.Controller.Protocol;
using System.Windows.Forms;
using System.Drawing;

namespace Qixle.iScanDuo.Controller.UI
{
    public class DecimalConnector : Connector<decimal>
    {
        public DecimalConnector(DecimalCommand command, NumericUpDown control, Label label, CommandCategory category)
            : base(command, control, label, category)
        {
        }

        public new NumericUpDown Control
        {
            get { return (NumericUpDown)base.Control; }
        }

        public new DecimalCommand Command
        {
            get { return (DecimalCommand)base.Command; }
        }

        public override decimal DoCurrentControlValue()
        {
            return Control.Value;
        }

        protected override void DoSetControlValue(decimal value)
        {
            if (value < Control.Minimum)
                Control.Minimum = value;
            if (value > Control.Maximum)
                Control.Maximum = value;
            Control.Value = value;
        }

        public override string CurrentControlStringValue()
        {
            return CurrentControlValue().ToString();
        }

        public override void SetControlStringValue(string value)
        {
            decimal v = Command.DefaultValue;
            decimal.TryParse(value, out v);
            SetControlValue(v);
        }

        protected override void AddControlChangeListener(EventHandler eventHandler)
        {
            Control.ValueChanged += eventHandler;
        }

        protected override void SetupControl()
        {
            Control.DecimalPlaces = Command.DecimalPlaces;
        }
    }
}
