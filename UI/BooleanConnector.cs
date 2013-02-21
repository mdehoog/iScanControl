using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Qixle.iScanDuo.Controller.Protocol;

namespace Qixle.iScanDuo.Controller.UI
{
    public class BooleanConnector : Connector<bool>
    {
        private readonly IList<IConnector> enableableConnectors = new List<IConnector>();
        private bool enableableCheckValue = true;

        public BooleanConnector(BooleanCommand command, CheckBox control, Label label, CommandCategory category)
            : base(command, control, label, category)
        {
            Control.CheckedChanged += new EventHandler(UpdateEnableables);
        }

        public void AddEnableableConnector(IConnector connector)
        {
            enableableConnectors.Add(connector);
        }

        public bool EnableableCheckValue
        {
            get { return enableableCheckValue; }
            set { enableableCheckValue = value; }
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

        public override void SetControlValue(bool value)
        {
            base.SetControlValue(value);
            UpdateEnableables(null, null);
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

        void UpdateEnableables(object sender, EventArgs e)
        {
            foreach (IConnector connector in enableableConnectors)
            {
                connector.Control.Enabled = Control.Checked == EnableableCheckValue;
            }
        }
    }
}
