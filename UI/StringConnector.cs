using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.Windows.Forms;

namespace Profiler.UI
{
    public class StringConnector : Connector<string>
    {
        public StringConnector(StringCommand command, TextBox control, Label label, CommandCategory category)
            : base(command, control, label, category)
        {
        }

        public new TextBox Control
        {
            get { return (TextBox)base.Control; }
        }

        public new StringCommand Command
        {
            get { return (StringCommand)base.Command; }
        }

        public override string DoCurrentControlValue()
        {
            return Control.Text;
        }

        protected override void DoSetControlValue(string value)
        {
            Control.Text = value;
        }

        public override string CurrentControlStringValue()
        {
            return CurrentControlValue();
        }

        public override void SetControlStringValue(string value)
        {
            SetControlValue(value);
        }

        protected override void AddControlChangeListener(EventHandler eventHandler)
        {
            Control.TextChanged += eventHandler;
        }

        protected override void SetupControl()
        {
            Control.Text = Command.DefaultValue;
        }
    }
}
