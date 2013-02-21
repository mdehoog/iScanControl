using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;

namespace Qixle.iScanDuo.Controller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort("COM1", 19200);
            port.Open();
            //byte[] buffer = new byte[] { 2, (byte)'3', (byte)'0', (byte)'0', (byte)'5', (byte)'A', (byte)'1', 0, (byte)'1', 0, 3 };
            //byte[] buffer = new byte[] { 2, (byte)'2', (byte)'0', (byte)'0', (byte)'3', (byte)'2', (byte)'1', 0, 3 };
            //byte[] buffer = Serial.DuoProtocol.QueryPacket(Protocol.DuoCommands.UCRedxCommand);
            byte[] buffer = Serial.DuoProtocol.QueryPacket(Protocol.DuoCommands.InputLabelCommand);
            port.Write(buffer, 0, buffer.Length);

            string commandId, response;
            Console.WriteLine(Serial.DuoProtocol.ReadResponse(port, out commandId, out response));
            Console.WriteLine("command id = " + commandId + ", response = " + response);
            
            //System.Threading.Thread.Sleep(1000);
            //Console.WriteLine(port.ReadExisting());

            port.Close();
            
            Console.ReadKey();*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
