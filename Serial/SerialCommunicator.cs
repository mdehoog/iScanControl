using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.IO.Ports;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace Profiler.Serial
{
    public class SerialCommunicator : AbstractQueueCommunicator
    {
        private SerialPort port;
        private bool portOpened = false;

        public SerialCommunicator()
        {
        }

        public override ErrorCode SendPacket(ISerialCommand sourceComand, byte[] packet, out string commandId, out string response)
        {
            if (!IsConnected())
            {
                commandId = null;
                response = null;
                return ErrorCode.NotConnected;
            }

            port.DiscardOutBuffer();
            try
            {
                port.Write(packet, 0, packet.Length);
            }
            catch (Exception e)
            {
                Context.Logger.Error(e);
                commandId = null;
                response = null;
                return ErrorCode.UnknownError;
            }
            return DuoProtocol.ReadResponse(port, out commandId, out response);
        }

        public override void Connect(string portName, int baudRate)
        {
            port = new SerialPort(portName, baudRate);
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.Open();
            portOpened = true;
        }

        public override void Connect(SerialPort port)
        {
            this.port = port;
        }

        public override void Disconnect()
        {
            if (portOpened)
            {
                port.Close();
            }
            port = null;
        }

        public override bool IsConnected()
        {
            return port != null && port.IsOpen;
        }
    }
}
