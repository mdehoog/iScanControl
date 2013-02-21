using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Conversion
{
    /// <summary>
    /// Represents a matrix that can be used to convert between color spaces (used for RGB to XYZ conversion and vice versa).
    /// </summary>
    public class ColorMatrix
    {
        public double m00;
        public double m01;
        public double m02;
        public double m10;
        public double m11;
        public double m12;
        public double m20;
        public double m21;
        public double m22;

        public ColorMatrix(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        public ColorVector Multiply(ColorVector v)
        {
            double x = v.x * m00 + v.y * m01 + v.z * m02;
            double y = v.x * m10 + v.y * m11 + v.z * m12;
            double z = v.x * m20 + v.y * m21 + v.z * m22;
            return new ColorVector(x, y, z);
        }

        public ColorMatrix Inverse()
        {
            // calculate the minors for the first row
            double minor00 = m11 * m22 - m12 * m21;
            double minor01 = m12 * m20 - m10 * m22;
            double minor02 = m10 * m21 - m11 * m20;

            // calculate the determinant
            double determinant = m00 * minor00 + m01 * minor01 + m02 * minor02;

            // check if the input is a singular matrix (non-invertable)
            double epsilon = 0.0000001;
            if (Math.Abs(determinant) < epsilon)
                return null;

            // the inverse of inMat is (1 / determinant) * adjoint(inMat)
            double invDet = 1.0 / determinant;
            double om00 = invDet * minor00;
            double om01 = invDet * (m21 * m02 - m22 * m01);
            double om02 = invDet * (m01 * m12 - m02 * m11);
            double om10 = invDet * minor01;
            double om11 = invDet * (m22 * m00 - m20 * m02);
            double om12 = invDet * (m02 * m10 - m00 * m12);
            double om20 = invDet * minor02;
            double om21 = invDet * (m20 * m01 - m21 * m00);
            double om22 = invDet * (m00 * m11 - m01 * m10);

            return new ColorMatrix(om00, om01, om02, om10, om11, om12, om20, om21, om22);
        }

        /// <summary>
        /// Create a new ColorMatrix instance from the given xy primaries and white point (in xyY space).
        /// </summary>
        /// <param name="rx">x-coordinate of red primary</param>
        /// <param name="ry">y-coordinate of red primary</param>
        /// <param name="gx">x-coordinate of green primary</param>
        /// <param name="gy">y-coordinate of green primary</param>
        /// <param name="bx">x-coordinate of blue primary</param>
        /// <param name="by">y-coordinate of blue primary</param>
        /// <param name="wx">x-coordinate of white point</param>
        /// <param name="wy">y-coordinate of white point</param>
        /// <returns>New ColorMatrix calculated from primaries and white point</returns>
        public static ColorMatrix FromPrimaries(double rx, double ry, double gx, double gy, double bx, double by, double wx, double wy)
        {
            ColorVector r = new ColorVector(rx, ry, 1.0 - (rx + ry));
            ColorVector g = new ColorVector(gx, gy, 1.0 - (gx + gy));
            ColorVector b = new ColorVector(bx, by, 1.0 - (bx + by));
            ColorVector w = new ColorVector(wx, wy, 1.0 - (wx + wy));
            w.MultiplyLocal(1.0 / w.y);

            ColorMatrix m = new ColorMatrix(r.x, g.x, b.x, r.y, g.y, b.y, r.z, g.z, b.z);
            ColorMatrix i = m.Inverse();
            ColorVector s = i.Multiply(w);

            m.m00 *= s.x;
            m.m10 *= s.x;
            m.m20 *= s.x;
            m.m01 *= s.y;
            m.m11 *= s.y;
            m.m21 *= s.y;
            m.m02 *= s.z;
            m.m12 *= s.z;
            m.m22 *= s.z;

            return m;
        }

        public override string ToString()
        {
            string n = "10:0.0000000";
            string f = String.Format("{{0,{0}}} {{1,{0}}} {{2,{0}}}" + Environment.NewLine + "{{3,{0}}} {{4,{0}}} {{5,{0}}}" + Environment.NewLine + "{{6,{0}}} {{7,{0}}} {{8,{0}}}", n);
            return String.Format(f, m00, m01, m02, m10, m11, m12, m20, m21, m22);
        }
    }
}
