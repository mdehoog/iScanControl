using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Settings
{
    public class SavedColor
    {
        public enum SavedColorTypeEnum
        {
            RGB,
            xyY,
            XYZ
        }

        private double x;
        private double y;
        private double z;
        private SavedColorTypeEnum type;
        private String name;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public SavedColorTypeEnum Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return "" + Name;
        }
    }
}
