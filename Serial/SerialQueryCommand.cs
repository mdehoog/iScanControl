using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qixle.iScanDuo.Controller.Protocol;
using System.IO.Ports;

namespace Qixle.iScanDuo.Controller.Serial
{
    public class SerialQueryCommand<T> : AbstractSerialCommand<T>
    {
        public SerialQueryCommand(Command<T> command, T currentValue, ICommandListener<T> listener, long commandId)
            : base(command, currentValue, listener, commandId)
        {
        }

        public override void Run(ICommunicator communicator)
        {
            listener.CommandStarted();

            byte[] packet = DuoProtocol.QueryPacket(command);
            Context.Logger.Fine("Querying '" + command + "': " + BitConverter.ToString(packet));

            string commandId = null, response = null;
            ErrorCode errorCode = ErrorCode.NoError;

            for (int i = -1; i < timeoutRetries; i++)
            {
                errorCode = communicator.SendPacket(this, packet, out commandId, out response);
                if (errorCode == ErrorCode.Timeout && i < timeoutRetries - 1)
                    Context.Logger.Warning("Timeout querying '" + command + "', retrying");
                else
                    break;
            }

            if (errorCode == ErrorCode.NoError && commandId != command.Id)
            {
                errorCode = ErrorCode.UnknownResponse;
            }

            string translated = DuoProtocol.TranslateQueryErrorCode(command, errorCode);
            if (translated != null)
            {
                response = translated;
                errorCode = ErrorCode.NoError;
            }

            if (errorCode == ErrorCode.NoError)
            {
                T value = command.StringToValue(response);
                Context.Logger.Info("Query '" + command + "' result: " + value + " (" + response + ")");
                listener.CommandCompleted(value);
            }
            else
            {
                Context.Logger.Error("Error querying '" + command + "': " + errorCode);
                listener.CommandError(errorCode, this.value);
            }
        }
    }
}
