using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.IO.Ports;

namespace Profiler.Serial
{
    public interface ICommunicator
    {
        void Connect(string portName, int baudRate);
        void Connect(SerialPort port);
        void Disconnect();
        bool IsConnected();

        void CancelQueuedCommands();
        void QueryValue<T>(Command<T> command, T currentValue, ICommandListener<T> listener);
        void SetValue<T>(Command<T> command, T value, ICommandListener<T> listener, int delayAfterCommandInMillis);
        ErrorCode SendPacket(ISerialCommand sourceComand, byte[] packet, out string commandId, out string response);

        bool QueueDuplicateSetCommands { get; set; }

        void AddListener(ICommunicatorListener listener);
        void RemoveListener(ICommunicatorListener listener);
    }
}
