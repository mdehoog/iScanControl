using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Logging;
using Profiler.Serial;
using Profiler.UI;

namespace Profiler
{
    public class Context
    {
        //start with some valid defaults
        private static Logger logger = new ConsoleLogger();
        private static ICommunicator communicator = new DummyQueueCommunicator();
        //private static ICommunicator communicator = new SerialCommunicator();
        private static readonly ControlLinker controlLinker = new ControlLinker();
        private static readonly ControlColorer controlColorer = new ControlColorer();
        private static readonly ConnectorRegistry connectorRegistry = new ConnectorRegistry();

        public static Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public static ICommunicator Communicator
        {
            get { return communicator; }
            set { communicator = value; }
        }

        public static ControlLinker ControlLinker
        {
            get { return controlLinker; }
        }

        public static ControlColorer ControlColorer
        {
            get { return controlColorer; }
        }

        public static ConnectorRegistry ConnectorRegistry
        {
            get { return connectorRegistry; }
        }
    }
}
