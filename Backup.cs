using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Serial;
using Profiler.UI;
using Profiler.Protocol;
using System.Threading;
using System.Windows.Forms;

namespace Profiler
{
    public class Backup : ICommunicatorListener
    {
        private Thread thread;
        private BackupDialog dialog;
        private bool cancelled = false;
        private string filename;

        private EventWaitHandle communicationComplete = new AutoResetEvent(false);

        public void PerformBackup(Form form, string filename)
        {
            this.filename = filename;

            thread = new Thread(new ThreadStart(BackupThread));
            thread.Start();

            dialog = new BackupDialog();
            DialogResult result = dialog.ShowDialog(form);

            if (result == DialogResult.Cancel)
            {
                cancelled = true;
                communicationComplete.Set();
            }

            thread.Join();
        }

        private void CloseDialog()
        {
            MethodInvoker invoker = new MethodInvoker(delegate()
                {
                    dialog.DialogResult = DialogResult.OK;
                    dialog.Close();
                });

            if (dialog.InvokeRequired)
                dialog.Invoke(invoker);
            else
                invoker.Invoke();
        }

        private void BackupThread()
        {
            try
            {
                Context.Communicator.AddListener(this);


                /*ListConnector inputConnector = Context.ConnectorRegistry.ConnectorForCommand(DuoCommands.InputSelectCommand) as ListConnector;
                ListConnector cmsProfileConnector = Context.ConnectorRegistry.ConnectorForCommand(DuoCommands.DayNightCommand) as ListConnector;

                IList<IConnector> leftOverConnectors = new List<IConnector>(Context.ConnectorRegistry.AllConnectors());
                IList<IConnector> inputDependants = inputConnector.DependantConnectors();
                IList<IConnector> cmsDependants = cmsProfileConnector.DependantConnectors();

                leftOverConnectors.Remove(inputConnector);
                leftOverConnectors.Remove(cmsProfileConnector);
                foreach (IConnector connector in inputDependants)
                {
                    leftOverConnectors.Remove(connector);
                }
                foreach (IConnector connector in cmsDependants)
                {
                    leftOverConnectors.Remove(connector);
                }*/

                ListConnector cmsProfileConnector = Context.ConnectorRegistry.ConnectorForCommand(DuoCommands.DayNightCommand) as ListConnector;
                IList<IConnector> cmsDependants = new List<IConnector>(Context.ConnectorRegistry.ConnectorsInCategory(CommandCategory.CMS));
                cmsDependants.Remove(cmsProfileConnector);


                //refresh all commands
                /*Context.ConnectorRegistry.QueryAllCommands();
                communicationComplete.WaitOne();
                if (cancelled)
                    return;*/


                IList<ConnectorRegistry.SerializableCommand> commands = new List<ConnectorRegistry.SerializableCommand>();

                /*foreach (ListValue inputValue in DuoListValues.InputSelectValues)
                {
                    //don't backup settings for input 'Auto':
                    if (inputValue.Value == 0)
                        continue;

                    inputConnector.SetControlValue(inputValue);

                    //wait for set and all queries to complete
                    communicationComplete.WaitOne();
                    if (cancelled)
                        return;

                    commands.Add(ConnectorRegistry.SerializableCommandForConnector(inputConnector));
                    foreach (IConnector connector in inputDependants)
                    {
                        if (connector.ICommand.IsSavable)
                        {
                            commands.Add(ConnectorRegistry.SerializableCommandForConnector(connector));
                        }
                    }
                }


                foreach (IConnector connector in leftOverConnectors)
                {
                    if (connector.ICommand.IsSavable)
                    {
                        commands.Add(ConnectorRegistry.SerializableCommandForConnector(connector));
                    }
                }*/

                foreach (ListValue profileValue in DuoListValues.DayNightProfileValues)
                {
                    cmsProfileConnector.SetControlValue(profileValue);

                    communicationComplete.WaitOne();
                    if (cancelled)
                        return;

                    commands.Add(ConnectorRegistry.SerializableCommandForConnector(cmsProfileConnector));
                    foreach (IConnector connector in cmsDependants)
                    {
                        if (connector.ICommand.IsSavable)
                        {
                            commands.Add(ConnectorRegistry.SerializableCommandForConnector(connector));
                        }
                    }
                }

                ConnectorRegistry.SaveToFile(commands, filename);

                CloseDialog();
            }
            finally
            {
                Context.Communicator.RemoveListener(this);
            }
        }

        public void CommunicationBegun()
        {
        }

        public void CommunicationComplete()
        {
            communicationComplete.Set();
        }
    }
}
