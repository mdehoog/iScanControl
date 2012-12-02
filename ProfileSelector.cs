using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Profiler.Protocol;

namespace Profiler
{
    public partial class ProfileSelector : Form
    {
        private ListValue selectedProfile;

        public ProfileSelector()
        {
            InitializeComponent();
        }

        public ListValue SelectedProfile
        {
            get { return selectedProfile; }
            private set
            {
                selectedProfile = value;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void profileCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProfile = (ListValue)profileCombo.SelectedItem;
        }
    }
}
