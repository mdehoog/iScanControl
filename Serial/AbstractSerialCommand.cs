using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.IO.Ports;

namespace Profiler.Serial
{
    public abstract class AbstractSerialCommand<T> : ISerialCommand
    {
        protected const int timeoutRetries = 2;

        protected readonly Command<T> command;
        protected readonly T value;
        protected readonly ICommandListener<T> listener;
        protected readonly long commandId;

        public AbstractSerialCommand(Command<T> command, T value, ICommandListener<T> listener, long commandId)
        {
            this.command = command;
            this.value = value;
            this.listener = listener;
            this.commandId = commandId;
        }

        public void NotifyQueued()
        {
            listener.CommandQueued();
        }

        public void NotifyCancelled()
        {
            listener.CommandCancelled();
        }

        public long CommandId
        {
            get { return commandId; }
        }

        public ICommand ICommand
        {
            get { return command; }
        }

        public int CompareTo(ISerialCommand other)
        {
            return this.CommandId.CompareTo(other.CommandId);
        }

        public abstract void Run(ICommunicator communicator);
    }
}
