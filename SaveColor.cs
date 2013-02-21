using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Qixle.iScanDuo.Controller.Conversion;
using Qixle.iScanDuo.Controller.Settings;

namespace Qixle.iScanDuo.Controller
{
    public partial class SaveColor : Form
    {
        private readonly ColorVector xyY;
        private readonly ColorVector XYZ;
        private readonly ColorVector RGB;
        private readonly SavedColor color = new SavedColor();

        public SaveColor(ColorVector xyY, ColorVector XYZ, ColorVector RGB)
        {
            InitializeComponent();
            EnableButtons();

            this.xyY = xyY;
            this.XYZ = XYZ;
            this.RGB = RGB;
            this.xyYRadio.Text += " (" + xyY + ")";
            this.XYZRadio.Text += " (" + XYZ + ")";
            this.RGBRadio.Text += " (" + RGB.ToRGBString(100) + ")";

            UpdateColor();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void name_TextChanged(object sender, EventArgs e)
        {
            EnableButtons();
            UpdateColor();
        }

        private void EnableButtons()
        {
            okButton.Enabled = nameText.Text.Length > 0;
        }

        private void xyYRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColor();
        }

        private void XYZRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColor();
        }

        private void RGBRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            color.Type = xyYRadio.Checked ? SavedColor.SavedColorTypeEnum.xyY : XYZRadio.Checked ? SavedColor.SavedColorTypeEnum.XYZ : SavedColor.SavedColorTypeEnum.RGB;
            if (xyYRadio.Checked)
            {
                color.X = xyY.x; color.Y = xyY.y; color.Z = xyY.z;
            }
            else if (XYZRadio.Checked)
            {
                color.X = XYZ.x; color.Y = XYZ.y; color.Z = XYZ.z;
            }
            else
            {
                color.X = RGB.x; color.Y = RGB.y; color.Z = RGB.z;
            }
            color.Name = nameText.Text;
        }

        public SavedColor Color
        {
            get { return color; }
        }
    }
}
