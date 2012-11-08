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
        private static Logger logger;
        private static ICommunicator communicator;
        private static ControlLinker controlLinker;
        private static ControlColorer controlColorer;
        private static ConnectorRegistry connectorRegistry;

        static Context()
        {
            Reset();
        }

        public static void Reset()
        {
            //start with some valid defaults
            logger = new ConsoleLogger();
            //communicator = new DummyQueueCommunicator();
            communicator = new SerialCommunicator();
            controlLinker = new ControlLinker();
            controlColorer = new ControlColorer();
            connectorRegistry = new ConnectorRegistry();
        }

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
