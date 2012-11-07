using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.Windows.Forms;
using System.Drawing;

namespace Profiler.UI
{
    public class ControlColorer
    {
        private readonly Color queryQueuedColor = Color.LightBlue;
        private readonly Color queryStartedColor = Color.Blue;
        private readonly Color queryErrorColor = Color.Red;
        private readonly Color setQueuedColor = Color.PeachPuff;
        private readonly Color setStartedColor = Color.Orange;
        private readonly Color setErrorColor = Color.Red;

        private readonly Color armedControlColor = Color.FromArgb(200, 255, 200);
        private readonly Color disarmedControlColor = Color.FromArgb(255, 200, 200);

        public enum ColorState
        {
            QueryQueued,
            QueryStarted,
            QueryError,
            SetQueued,
            SetStarted,
            SetError,
            Clear,
            Armed,
            Disarmed
        }

        public void RegisterConnector(IConnector connector)
        {
            if (connector.ICommand.IsSavable)
            {
                connector.ICommand.IsArmed = true;
                connector.Control.BackColor = armedControlColor;

                connector.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            /*if (sender is Control)
            {
                Control control = sender as Control;
                if (commands.ContainsKey(control))
                {
                    ICommand command = commands[control];
                    UpdateState(command, command.IsArmed ? ColorState.Disarmed : ColorState.Armed); //TODO
                }
            }*/
        }

        public void UpdateState(ICommand command, ColorState state)
        {
            IConnector connector = Context.ConnectorRegistry.ConnectorForCommand(command);

            //don't know about this command, as the connector hasn't been registered
            if (connector == null)
                return;

            Color? panelColor = null;

            switch (state)
            {
                case ColorState.Disarmed:
                    command.IsArmed = false;
                    connector.Control.BackColor = disarmedControlColor;
                    break;
                case ColorState.Armed:
                    command.IsArmed = true;
                    connector.Control.BackColor = armedControlColor;
                    break;

                case ColorState.QueryQueued:
                    panelColor = queryQueuedColor;
                    break;
                case ColorState.QueryStarted:
                    panelColor = queryStartedColor;
                    break;
                case ColorState.QueryError:
                    panelColor = queryErrorColor;
                    break;

                case ColorState.SetQueued:
                    panelColor = setQueuedColor;
                    break;
                case ColorState.SetStarted:
                    panelColor = setStartedColor;
                    break;
                case ColorState.SetError:
                    panelColor = setErrorColor;
                    break;

                case ColorState.Clear:
                    panelColor = Color.Transparent;
                    break;
            }

            if (panelColor.HasValue)
            {
                ControlInvoke(connector.Panel, new MethodInvoker(delegate()
                    {
                        connector.Panel.BackColor = panelColor.Value;
                    }));
            }
        }

        private void ControlInvoke(Control control, MethodInvoker invoker)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(invoker);
            else
                invoker.Invoke();
        }
    }
}
