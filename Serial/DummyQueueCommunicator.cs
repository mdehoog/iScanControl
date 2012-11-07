using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Profiler.Serial
{
    public class DummyQueueCommunicator : AbstractQueueCommunicator
    {
        private bool connected = false;
        private SerialPort port;
        
        public override ErrorCode SendPacket(ISerialCommand sourceComand, byte[] packet, out string commandId, out string response)
        {
            //return the default value as the response
            commandId = sourceComand.ICommand.Id;
            response = sourceComand.ICommand.DefaultValueAsString;

            if (!IsConnected())
                return ErrorCode.NotConnected;

            //Context.Logger.Info("Command bytes: " + BitConverter.ToString(packet));

            //simulate a serial port wait
            Thread.Sleep(20);
            return ErrorCode.NoError;
        }

        public override void Connect(string portName, int baudRate)
        {
            Context.Logger.Info("Connected to device on port " + portName + " at " + baudRate + " baud");
            connected = true;
        }

        public override void Connect(SerialPort port)
        {
            Context.Logger.Info("Connected to device on port " + port.PortName + " at " + port.BaudRate + " baud");
            this.port = port;
            connected = true;
        }

        public override void Disconnect()
        {
            Context.Logger.Info("Disconnected from device");
            connected = false;
        }

        public override bool IsConnected()
        {
            return connected;
        }
    }
}
