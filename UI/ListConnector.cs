using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Profiler.Protocol;

namespace Profiler.UI
{
    public class ListConnector : Connector<ListValue>
    {
        public ListConnector(ListCommand command, ComboBox control, Label label, CommandCategory category)
            : base(command, control, label, category)
        {
        }

        public new ComboBox Control
        {
            get { return (ComboBox)base.Control; }
        }

        public new ListCommand Command
        {
            get { return (ListCommand)base.Command; }
        }

        public override ListValue DoCurrentControlValue()
        {
            return (ListValue)Control.SelectedItem;
        }

        protected override void DoSetControlValue(ListValue value)
        {
            Control.SelectedItem = value;
        }

        public override string CurrentControlStringValue()
        {
            return CurrentControlValue().Value.ToString();
        }

        public override void SetControlStringValue(string value)
        {
            ListValue v = Command.ListValues.StringToValue(value) ?? Command.DefaultValue;
            SetControlValue(v);
        }

        protected override void AddControlChangeListener(EventHandler eventHandler)
        {
            Control.SelectedValueChanged += eventHandler;
        }

        protected override void SetupControl()
        {
            Control.Items.Clear();
            Control.Items.AddRange(Command.ListValues.ToArray<ListValue>());
            Control.SelectedItem = Command.DefaultValue;
        }

        public void AddListControlChangeListener(EventHandler eventHandler)
        {
            Control.SelectedValueChanged += eventHandler;
        }
    }
}
