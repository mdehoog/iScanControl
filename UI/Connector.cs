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
    public abstract class Connector<T> : IConnector
    {
        private readonly Command<T> command;
        private readonly Control control;
        private readonly Panel panel;
        private readonly Label label;
        private readonly CommandCategory category;
        private T lastGoodValue;
        protected bool settingControl = false;
        private readonly IList<IConnector> dependantConnectors = new List<IConnector>();

        private readonly ConnectorQueryCommandListener queryListener;
        private readonly ConnectorSetCommandListener setListener;
        private int queriesQueued = 0;
        private int setsQueued = 0;

        private int delayAfterSet = 0;

        public Connector(Command<T> command, Control control, Label label, CommandCategory category)
        {
            this.command = command;
            this.control = control;
            this.label = label;
            this.category = category;
            this.lastGoodValue = command.DefaultValue;

            this.queryListener = new ConnectorQueryCommandListener(this);
            this.setListener = new ConnectorSetCommandListener(this);

            panel = new Panel();
            panel.Location = new Point(control.Location.X - 2, control.Location.Y - 2);
            panel.Size = new Size(control.Size.Width + 4, control.Size.Height + 4);
            panel.BackColor = Color.Transparent;
            Control.Parent.Controls.Add(panel);
            Control.BringToFront();

            Context.ControlColorer.RegisterConnector(this);
            Context.ConnectorRegistry.RegisterConnector(this);

            SetupControl();
            AddControlChangeListener(new EventHandler(ControlChanged));
        }

        public virtual void SetControlValue(T value)
        {
            if (CurrentControlValue().Equals(value))
            {
                ControlChanged(null, null);
            }
            else
            {
                if (Control.InvokeRequired)
                    Control.Invoke(new DoSetControlValueDelegate(DoSetControlValue), value);
                else
                    DoSetControlValue(value);
            }
        }

        public T CurrentControlValue()
        {
            if (Control.InvokeRequired)
                return (T)Control.Invoke(new DoCurrentControlValueDelegate(DoCurrentControlValue));
            return DoCurrentControlValue();
        }

        private delegate T DoCurrentControlValueDelegate();
        private delegate void DoSetControlValueDelegate(T value);
        public abstract T DoCurrentControlValue();
        protected abstract void DoSetControlValue(T value);
        public abstract string CurrentControlStringValue();
        public abstract void SetControlStringValue(string value);
        protected abstract void SetupControl();
        protected abstract void AddControlChangeListener(EventHandler eventHandler);

        public virtual Command<T> Command
        {
            get { return command; }
        }

        public ICommand ICommand
        {
            get { return Command; }
        }

        public Control Control
        {
            get { return control; }
        }

        public Panel Panel
        {
            get { return panel; }
        }

        public Label Label
        {
            get { return label; }
        }

        public CommandCategory Category
        {
            get { return category; }
        }

        public int DelayAfterSet
        {
            get { return delayAfterSet; }
            set { delayAfterSet = value; }
        }

        public void AddDependantConnector(IConnector dependant)
        {
            dependantConnectors.Add(dependant);
        }

        public IList<IConnector> DependantConnectors()
        {
            return dependantConnectors;
        }

        public virtual void QueryValue()
        {
            if (Command.IsQueryable)
            {
                Context.Communicator.QueryValue<T>(Command, CurrentControlValue(), queryListener);
            }
        }

        protected virtual void ControlChanged(object sender, EventArgs e)
        {
            if (!settingControl)
            {
                Context.Communicator.SetValue<T>(Command, CurrentControlValue(), setListener, DelayAfterSet);

                foreach (IConnector dependant in dependantConnectors)
                {
                    dependant.QueryValue();
                }
            }
        }

        protected void InvokeSetControlValue(T value)
        {
            MethodInvoker invoker = new MethodInvoker(delegate()
                {
                    settingControl = true;
                    SetControlValue(value);
                    settingControl = false;
                });

            if (Control.InvokeRequired)
                Control.BeginInvoke(invoker);
            else
                invoker.Invoke();
        }

        private void UpdateQueuedColorState()
        {
            setsQueued = Math.Max(0, setsQueued);
            queriesQueued = Math.Max(0, queriesQueued);

            if (setsQueued > 0)
            {
                Context.ControlColorer.UpdateState(Command, ControlColorer.ColorState.SetQueued);
            }
            else if (queriesQueued > 0)
            {
                Context.ControlColorer.UpdateState(Command, ControlColorer.ColorState.QueryQueued);
            }
            else
            {
                Context.ControlColorer.UpdateState(Command, ControlColorer.ColorState.Clear);
            }
        }

        public class ConnectorQueryCommandListener : ICommandListener<T>
        {
            private readonly Connector<T> connector;

            public ConnectorQueryCommandListener(Connector<T> connector)
            {
                this.connector = connector;
            }

            public void CommandQueued()
            {
                connector.queriesQueued++;
                connector.UpdateQueuedColorState();
            }

            public void CommandCancelled()
            {
                connector.queriesQueued--;
                connector.UpdateQueuedColorState();
            }

            public void CommandStarted()
            {
                connector.queriesQueued--;
                Context.ControlColorer.UpdateState(connector.Command, ControlColorer.ColorState.QueryStarted);
            }

            public void CommandCompleted(T value)
            {
                connector.InvokeSetControlValue(value);
                connector.lastGoodValue = value;
                connector.UpdateQueuedColorState();
            }

            public void CommandError(ErrorCode code, T value)
            {
                Context.ControlColorer.UpdateState(connector.Command, ControlColorer.ColorState.QueryError);
            }
        }

        public class ConnectorSetCommandListener : ICommandListener<T>
        {
            private readonly Connector<T> connector;

            public ConnectorSetCommandListener(Connector<T> connector)
            {
                this.connector = connector;
            }

            public void CommandQueued()
            {
                connector.setsQueued++;
                connector.UpdateQueuedColorState();
            }

            public void CommandCancelled()
            {
                connector.setsQueued--;
                connector.UpdateQueuedColorState();
            }

            public void CommandStarted()
            {
                connector.setsQueued--;
                Context.ControlColorer.UpdateState(connector.Command, ControlColorer.ColorState.SetStarted);
            }

            public void CommandCompleted(T value)
            {
                connector.lastGoodValue = value;
                connector.UpdateQueuedColorState();
            }

            public void CommandError(ErrorCode code, T value)
            {
                connector.InvokeSetControlValue(connector.lastGoodValue);
                Context.ControlColorer.UpdateState(connector.Command, ControlColorer.ColorState.SetError);
            }
        }
    }
}
