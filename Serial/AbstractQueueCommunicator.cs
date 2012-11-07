using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Profiler.Protocol;
using System.IO.Ports;

namespace Profiler.Serial
{
    public abstract class AbstractQueueCommunicator : ICommunicator
    {
        private Thread thread;

        private readonly SortedList<ISerialCommand, ISerialCommand> setCommandQueue = new SortedList<ISerialCommand, ISerialCommand>();
        private readonly IDictionary<ICommand, ISerialCommand> currentSetCommands = new Dictionary<ICommand, ISerialCommand>();
        private readonly SortedList<ISerialCommand, ISerialCommand> queryCommandQueue = new SortedList<ISerialCommand, ISerialCommand>();
        private readonly IDictionary<ICommand, ISerialCommand> currentQueryCommands = new Dictionary<ICommand, ISerialCommand>();
        private readonly Object semaphore = new Object();
        private readonly EventWaitHandle commandAvailable = new AutoResetEvent(false);
        private readonly EventWaitHandle waitingForCommand = new ManualResetEvent(false);
        private readonly IList<ICommunicatorListener> listeners = new List<ICommunicatorListener>();
        private long commandId = 0;
        private bool firstLoop = true;
        private bool queueDuplicateSetCommands = false;

        public AbstractQueueCommunicator()
        {
            thread = new Thread(new ThreadStart(SerialThread));
            thread.IsBackground = true;
            thread.Start();
        }

        private void SerialThread()
        {
            while (true)
            {
                ISerialCommand setCommand = DequeueSetCommand();
                if (setCommand != null)
                {
                    setCommand.Run(this);
                    continue;
                }

                ISerialCommand queryCommand = DequeueQueryCommand();
                if(queryCommand != null)
                {
                    queryCommand.Run(this);
                    continue;
                }

                if (firstLoop)
                {
                    firstLoop = false;
                }
                else
                {
                    NotifyCommunicationComplete();
                }

                //no set or query commands; wait for one
                waitingForCommand.Set();
                commandAvailable.WaitOne();
                waitingForCommand.Reset();

                NotifyCommunicationBegun();
            }
        }

        private ISerialCommand DequeueSetCommand()
        {
            lock (semaphore)
            {
                if (setCommandQueue.Count > 0)
                {
                    ISerialCommand command = setCommandQueue.Keys[0];
                    setCommandQueue.Remove(command);
                    currentSetCommands.Remove(command.ICommand);
                    return command;
                }
                return null;
            }
        }

        private ISerialCommand DequeueQueryCommand()
        {
            lock (semaphore)
            {
                if (queryCommandQueue.Count > 0)
                {
                    ISerialCommand command = queryCommandQueue.Keys[0];
                    queryCommandQueue.Remove(command);
                    currentQueryCommands.Remove(command.ICommand);
                    return command;
                }
                return null;
            }
        }

        private void NotifyCommunicationBegun()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].CommunicationBegun();
        }

        private void NotifyCommunicationComplete()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].CommunicationComplete();
        }

        public void AddListener(ICommunicatorListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(ICommunicatorListener listener)
        {
            listeners.Remove(listener);
        }

        public void CancelQueuedCommands()
        {
            lock (semaphore)
            {
                foreach (ISerialCommand command in queryCommandQueue.Keys)
                {
                    command.NotifyCancelled();
                }
                foreach (ISerialCommand command in setCommandQueue.Keys)
                {
                    command.NotifyCancelled();
                }

                queryCommandQueue.Clear();
                currentQueryCommands.Clear();
                setCommandQueue.Clear();
                currentSetCommands.Clear();

                commandAvailable.Reset();
            }

            //wait for the SerialThread function to be waiting for a new command, which means it has completed any command it was currently running
            waitingForCommand.WaitOne();
        }

        public void QueryValue<T>(Command<T> command, T currentValue, ICommandListener<T> listener)
        {
            try
            {
                ISerialCommand serialCommand = new SerialQueryCommand<T>(command, currentValue, listener, commandId++);
                lock (semaphore)
                {
                    if (!currentQueryCommands.ContainsKey(command))
                    {
                        currentQueryCommands.Add(command, serialCommand);
                        queryCommandQueue.Add(serialCommand, serialCommand);
                        serialCommand.NotifyQueued();
                    }
                }
                commandAvailable.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SetValue<T>(Command<T> command, T value, ICommandListener<T> listener, int delayAfterCommandInMillis)
        {
            ISerialCommand serialCommand = new SerialSetCommand<T>(command, value, listener, commandId++, delayAfterCommandInMillis);
            lock (semaphore)
            {
                if (!queueDuplicateSetCommands)
                {
                    if (currentSetCommands.ContainsKey(command))
                    {
                        ISerialCommand key = currentSetCommands[command];
                        setCommandQueue.Remove(key);
                        currentSetCommands.Remove(command);
                        key.NotifyCancelled();
                    }
                    currentSetCommands.Add(command, serialCommand);
                }
                setCommandQueue.Add(serialCommand, serialCommand);
                serialCommand.NotifyQueued();
            }
            commandAvailable.Set();
        }

        public bool QueueDuplicateSetCommands
        {
            get { return queueDuplicateSetCommands; }
            set { queueDuplicateSetCommands = value; }
        }

        public abstract ErrorCode SendPacket(ISerialCommand sourceComand, byte[] packet, out string commandId, out string response);

        public abstract void Connect(string portName, int baudRate);
        public abstract void Connect(SerialPort port);
        public abstract void Disconnect();
        public abstract bool IsConnected();
    }
}
