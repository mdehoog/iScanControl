using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Profiler.Protocol;

namespace Profiler.UI
{
    public class BooleanConnector : Connector<bool>
    {
        public BooleanConnector(BooleanCommand command, CheckBox control, Label label, CommandCategory category)
            : base(command, control, label, category)
        {
        }

        public new CheckBox Control
        {
            get { return (CheckBox)base.Control; }
        }

        public new BooleanCommand Command
        {
            get { return (BooleanCommand)base.Command; }
        }

        public override bool DoCurrentControlValue()
        {
            return Control.Checked;
        }

        protected override void DoSetControlValue(bool value)
        {
            Control.Checked = value;
        }

        public override string CurrentControlStringValue()
        {
            return CurrentControlValue().ToString();
        }

        public override void SetControlStringValue(string value)
        {
            bool v = Command.DefaultValue;
            bool.TryParse(value, out v);
            SetControlValue(v);
        }

        protected override void AddControlChangeListener(EventHandler eventHandler)
        {
            Control.CheckedChanged += eventHandler;
        }

        protected override void SetupControl()
        {
            Control.Checked = Command.DefaultValue;
        }
    }
}
