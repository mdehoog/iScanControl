using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Qixle.iScanDuo.Controller.Settings;

namespace Qixle.iScanDuo.Controller
{
    public partial class PortSelector : Form
    {
        private string selectedPortName;
        private int selectedBaudRate;

        public PortSelector()
        {
            InitializeComponent();
        }

        private void PortSelector_Load(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();

            if (portNames == null || portNames.Length <= 0)
            {
                MessageBox.Show("No serial ports found. If your hardware appears to have a serial port, ensure it is enabled in the BIOS.", "No serial ports", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }

            Array.Sort<string>(portNames);
            object[] baudRates = new object[] { 4800, 9600, 14400, 19200, 38400, 57600, 115200 };

            int selectedPortIndex = 0;
            int selectedBaudIndex = 3;

            string lastPortName = UserSettings.Instance.LastPortName;
            if (lastPortName != null)
            {
                int index = portNames.ToList<string>().IndexOf(lastPortName);
                if (index >= 0)
                    selectedPortIndex = index;
            }

            int lastBaudRate = UserSettings.Instance.LastBaudRate;
            {
                int index = baudRates.ToList<object>().IndexOf(lastBaudRate);
                if (index >= 0)
                    selectedBaudIndex = index;
            }

            portCombo.Items.Clear();
            portCombo.Items.AddRange(portNames);
            portCombo.SelectedIndex = selectedPortIndex;

            baudCombo.Items.Clear();
            baudCombo.Items.AddRange(baudRates);
            baudCombo.SelectedIndex = selectedBaudIndex;
        }

        public string SelectedPortName
        {
            get { return selectedPortName; }
            private set
            {
                selectedPortName = value;
                UserSettings.Instance.LastPortName = value;
            }
        }

        public int SelectedBaudRate
        {
            get { return selectedBaudRate; }
            private set
            {
                selectedBaudRate = value;
                UserSettings.Instance.LastBaudRate = value;
            }
        }

        private void portCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedPortName = (string)portCombo.SelectedItem;
        }

        private void baudCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedBaudRate = (int)baudCombo.SelectedItem;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
