using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Conversion
{
    /// <summary>
    /// Represents a single color, in either RGB, xyY, or XYZ space. RGB values must be scaled between 0 and 1.
    /// </summary>
    public class ColorVector
    {
        public double x;
        public double y;
        public double z;

        public ColorVector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void MinLocal(double min)
        {
            x = Math.Min(x, min);
            y = Math.Min(y, min);
            z = Math.Min(z, min);
        }

        public void MaxLocal(double max)
        {
            x = Math.Max(x, max);
            y = Math.Max(y, max);
            z = Math.Max(z, max);
        }

        public void MultiplyLocal(double s)
        {
            x *= s;
            y *= s;
            z *= s;
        }

        public void MultiplyLocal(ColorVector v)
        {
            x *= v.x;
            y *= v.y;
            z *= v.z;
        }

        public ColorVector Multiply(double s)
        {
            return new ColorVector(x * s, y * s, z * s);
        }

        public ColorVector Multiply(ColorVector v)
        {
            return new ColorVector(x * v.x, y * v.y, z * v.z);
        }

        public void PowerLocal(double power)
        {
            x = Math.Pow(x, power);
            y = Math.Pow(y, power);
            z = Math.Pow(z, power);
        }

        public ColorVector Power(double power)
        {
            return new ColorVector(Math.Pow(x, power), Math.Pow(y, power), Math.Pow(z, power));
        }

        public override string ToString()
        {
            string n = "10:0.0000000";
            string f = String.Format("{{0,{0}}} {{1,{0}}} {{2,{0}}}", n);
            return String.Format(f, x, y, z);
        }

        public string ToRGBString(double multiplier)
        {
            return (int)(x * multiplier) + " " + (int)(y * multiplier) + " " + (int)(z * multiplier);
        }
    }
}
