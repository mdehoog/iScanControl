using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.Windows.Forms;
using Profiler.Serial;
using System.Drawing;

namespace Profiler.UI
{
    public class ButtonConnector : ICommandListener<ListValue>
    {
        private readonly ListCommand command;
        private readonly ListValue value;
        private readonly Button button;
        private readonly CommandCategory category;

        public ButtonConnector(ListCommand command, ListValue value, Button button, CommandCategory category)
        {
            this.command = command;
            this.value = value;
            this.button = button;
            this.category = category;

            Context.ConnectorRegistry.RegisterOtherControl(button);

            button.Click += new EventHandler(button_Click);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Context.Communicator.SetValue<ListValue>(command, value, this, 0);
        }

        public void CommandQueued()
        {
        }

        public void CommandCancelled()
        {
        }

        public void CommandStarted()
        {
        }

        public void CommandCompleted(ListValue value)
        {
        }

        public void CommandError(ErrorCode code, ListValue value)
        {
        }
    }
}
