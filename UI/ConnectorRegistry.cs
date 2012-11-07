using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Windows.Forms;

namespace Profiler.UI
{
    public class ConnectorRegistry
    {
        private readonly IList<IConnector> connectors = new List<IConnector>();
        private readonly IList<Control> otherControls = new List<Control>();
        private readonly IDictionary<CommandCategory, IList<IConnector>> categoryConnectors = new Dictionary<CommandCategory, IList<IConnector>>();

        private readonly Dictionary<ICommand, IConnector> commandToConnector = new Dictionary<ICommand, IConnector>();
        private readonly IDictionary<string, IConnector> commandKeyToConnector = new Dictionary<string, IConnector>();

        public void RegisterConnector(IConnector connector)
        {
            connectors.Add(connector);

            IList<IConnector> categoryList;
            if (categoryConnectors.ContainsKey(connector.Category))
            {
                categoryList = categoryConnectors[connector.Category];
            }
            else
            {
                categoryList = new List<IConnector>();
                categoryConnectors[connector.Category] = categoryList;
            }
            categoryList.Add(connector);

            string commandKey = CommandKeyForCommand(connector.ICommand);
            if (commandKeyToConnector.ContainsKey(commandKey))
            {
                throw new Exception("Command key dictionary already contains a value for key '" + commandKey + "' (command: " + connector.ICommand.Name + ")");
            }
            commandKeyToConnector[commandKey] = connector;

            commandToConnector[connector.ICommand] = connector;
        }

        public void RegisterOtherControl(Control control)
        {
            otherControls.Add(control);
        }

        public IList<IConnector> AllConnectors()
        {
            return connectors;
        }

        public IList<IConnector> ConnectorsInCategory(CommandCategory category)
        {
            if(categoryConnectors.ContainsKey(category))
                return categoryConnectors[category];
            return null;
        }

        public IConnector ConnectorForCommand(ICommand command)
        {
            if (commandToConnector.ContainsKey(command))
                return commandToConnector[command];
            return null;
        }

        public void QueryAllCommands()
        {
            foreach (IConnector connector in connectors)
            {
                connector.QueryValue();
            }
        }

        public void EnableCommandControls(bool enabled)
        {
            foreach (IConnector connector in connectors)
            {
                connector.Control.Enabled = enabled;
            }
            foreach (Control control in otherControls)
            {
                control.Enabled = enabled;
            }
        }

        public bool AllCommandsArmedForCategory(CommandCategory category)
        {
            if (!categoryConnectors.ContainsKey(category))
                return false;

            IList<IConnector> connectors = categoryConnectors[category];
            bool anyDisarmed = false;
            foreach (IConnector connector in connectors)
            {
                if (connector.ICommand.IsSavable && !connector.ICommand.IsArmed)
                {
                    anyDisarmed = true;
                    break;
                }
            }

            return !anyDisarmed;
        }

        public bool ToggleArmedStateForCategory(CommandCategory category)
        {
            if (!categoryConnectors.ContainsKey(category))
                return false;

            IList<IConnector> connectors = categoryConnectors[category];
            bool arm = !AllCommandsArmedForCategory(category);
            foreach (IConnector connector in connectors)
            {
                if (connector.ICommand.IsSavable)
                {
                    Context.ControlColorer.UpdateState(connector.ICommand, arm ? ControlColorer.ColorState.Armed : ControlColorer.ColorState.Disarmed);
                }
            }
            return arm;
        }

        public void ArmAllCommands(bool arm)
        {
            foreach (IConnector connector in connectors)
            {
                if (connector.ICommand.IsSavable)
                {
                    Context.ControlColorer.UpdateState(connector.ICommand, arm ? ControlColorer.ColorState.Armed : ControlColorer.ColorState.Disarmed);
                }
            }
        }

        public void ClearAllControlColors()
        {
            foreach (IConnector connector in connectors)
            {
                Context.ControlColorer.UpdateState(connector.ICommand, ControlColorer.ColorState.Clear);
            }
        }

        private static string CommandKeyForCommand(ICommand command)
        {
            return command.Id + (command.HasValuePrefix ? " " + command.ValuePrefix : "");
        }

        public void LoadFromFile(string filename)
        {
            try
            {
                Context.Communicator.QueueDuplicateSetCommands = true;

                FileStream fs = new FileStream(filename, FileMode.Open);
                XmlReader reader = new XmlTextReader(fs);
                XmlSerializer ser = new XmlSerializer(typeof(List<SerializableCommand>), new XmlRootAttribute("Commands"));
                List<SerializableCommand> commands = (List<SerializableCommand>)ser.Deserialize(reader);
                reader.Close();
                fs.Close();

                foreach (IConnector connector in connectors)
                {
                    if (connector.ICommand.IsSavable)
                    {
                        Context.ControlColorer.UpdateState(connector.ICommand, ControlColorer.ColorState.Disarmed);
                    }
                }

                foreach (SerializableCommand sc in commands)
                {
                    if (commandKeyToConnector.ContainsKey(sc.CommandKey))
                    {
                        IConnector connector = commandKeyToConnector[sc.CommandKey];
                        if (connector.ICommand.IsSavable)
                        {
                            Context.ControlColorer.UpdateState(connector.ICommand, ControlColorer.ColorState.Armed);
                            connector.SetControlStringValue(sc.Value);
                        }
                    }
                }
            }
            finally
            {
                Context.Communicator.QueueDuplicateSetCommands = false;
            }
        }

        public void SaveToFile(string filename, bool onlyArmed)
        {
            List<SerializableCommand> commands = new List<SerializableCommand>();
            foreach (IConnector connector in connectors)
            {
                if (connector.ICommand.IsSavable && (!onlyArmed || connector.ICommand.IsArmed))
                {
                    commands.Add(SerializableCommandForConnector(connector));
                }
            }

            SaveToFile(commands, filename);
        }

        public static SerializableCommand SerializableCommandForConnector(IConnector connector)
        {
            return new SerializableCommand(CommandKeyForCommand(connector.ICommand), connector.CurrentControlStringValue());
        }

        public static void SaveToFile(IList<SerializableCommand> commands, string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator nav = xmlDoc.CreateNavigator();
            using (XmlWriter writer = nav.AppendChild())
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<SerializableCommand>), new XmlRootAttribute("Commands"));
                ser.Serialize(writer, commands);
            }

            xmlDoc.Save(filename);
        }

        [Serializable]
        [XmlType("Command")]
        public class SerializableCommand
        {
            [XmlElement("Key")]
            public string CommandKey;
            [XmlElement("Value")]
            public string Value;

            public SerializableCommand()
            {
            }

            public SerializableCommand(string commandKey, string value)
            {
                this.CommandKey = commandKey;
                this.Value = value;
            }
        }
    }
}
