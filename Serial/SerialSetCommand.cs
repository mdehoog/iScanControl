using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.IO.Ports;
using System.Threading;

namespace Profiler.Serial
{
    public class SerialSetCommand<T> : AbstractSerialCommand<T>
    {
        private readonly int delayAfterCommandInMillis;

        public SerialSetCommand(Command<T> command, T value, ICommandListener<T> listener, long commandId, int delayAfterCommandInMillis)
            : base(command, value, listener, commandId)
        {
            this.delayAfterCommandInMillis = delayAfterCommandInMillis;
        }

        public override void Run(ICommunicator communicator)
        {
            listener.CommandStarted();

            byte[] packet = DuoProtocol.CommandPacket(command, value);
            
            string valueString = value + "";
            if (value is Array)
            {
                valueString = "";
                foreach (Object o in value as Array)
                {
                    valueString += " " + o;
                }
                if (valueString.Length > 0)
                {
                    valueString = valueString.Substring(1);
                }
            }

            Context.Logger.Fine("Setting '" + command + "' to '" + valueString + "': " + BitConverter.ToString(packet));

            string commandId = null, response = null;
            ErrorCode errorCode = ErrorCode.NoError;

            for (int i = -1; i < timeoutRetries; i++)
            {
                errorCode = communicator.SendPacket(this, packet, out commandId, out response);
                if (errorCode == ErrorCode.Timeout && i < timeoutRetries - 1)
                    Context.Logger.Warning("Timeout setting '" + command + "' to '" + valueString + "', retrying");
                else
                    break;
            }

            errorCode = DuoProtocol.TranslateSetErrorCode(command, value, errorCode);
            
            if (errorCode == ErrorCode.NoError)
            {
                Context.Logger.Info("Set '" + command + "' to '" + valueString + "'");
                listener.CommandCompleted(value);

                Thread.Sleep(delayAfterCommandInMillis);
            }
            else
            {
                Context.Logger.Error("Error setting '" + command + "' to '" + valueString + "': " + errorCode);
                listener.CommandError(errorCode, value);
            }
        }
    }
}
