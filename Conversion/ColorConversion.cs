using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Conversion
{
    /// <summary>
    /// Class used to convert between different color spaces (RGB, XYZ, and xyY). Uses a standard power-law gamma function, and a ColorMatrix that converts between RGB and XYZ.
    /// </summary>
    public class ColorConversion
    {
        public static readonly ColorMatrix bradfordCtoD50 = new ColorMatrix(1.0377, 0.0154, -0.0385, 0.0171, 1.0057, -0.0189, -0.0120, 0.0204, 0.6906);
        public static readonly ColorMatrix bradfordD50toD65 = new ColorMatrix(0.9556, -0.0230, 0.0632, -0.0283, 1.0099, 0.0210, 0.0123, -0.0205, 1.3299);

        /// <summary>
        /// Convert an RGB color to xyY and XYZ, using a color matrix and a power-law gamma function.
        /// </summary>
        /// <param name="rgb">RGB color to convert</param>
        /// <param name="matrix">RGB to XYZ color matrix</param>
        /// <param name="gamma">Gamma exponent</param>
        /// <param name="xyY">Variable to store xyY result</param>
        /// <param name="XYZ">Variable to store XYZ result</param>
        public static void fromRGB(ColorVector rgb, ColorMatrix matrix, double gamma, out ColorVector xyY, out ColorVector XYZ)
        {
            ColorVector gammafied = rgb.Power(gamma);
            XYZ = matrix.Multiply(gammafied);
            xyY = XYZtoxyY(XYZ);
        }

        /// <summary>
        /// Convert an xyY color to XYZ and RGB, using a color matrix and a power-law gamma function.
        /// </summary>
        /// <param name="xyY">xyY color to convert</param>
        /// <param name="inverse">XYZ to RGB color matrix</param>
        /// <param name="gamma">Gamma exponent</param>
        /// <param name="rgb">Variable to store RGB result</param>
        /// <param name="XYZ">Variable to store XYZ result</param>
        public static void fromxyY(ColorVector xyY, ColorMatrix inverse, double gamma, out ColorVector rgb, out ColorVector XYZ)
        {
            XYZ = xyYtoXYZ(xyY);
            ColorVector gammafied = inverse.Multiply(XYZ);
            gammafied.MaxLocal(0.0);
            rgb = gammafied.Power(1.0 / gamma);
        }

        /// <summary>
        /// Convert an XYZ color to xyY and RGB, using a color matrix and a power-law gamma function.
        /// </summary>
        /// <param name="XYZ">XYZ color to convert</param>
        /// <param name="inverse">XYZ to RGB color matrix</param>
        /// <param name="gamma">Gamma exponent</param>
        /// <param name="rgb">Variable to store RGB result</param>
        /// <param name="xyY">Variable to store xyY result</param>
        public static void fromXYZ(ColorVector XYZ, ColorMatrix inverse, double gamma, out ColorVector rgb, out ColorVector xyY)
        {
            xyY = XYZtoxyY(XYZ);
            ColorVector gammafied = inverse.Multiply(XYZ);
            gammafied.MaxLocal(0.0);
            rgb = gammafied.Power(1.0 / gamma);
        }

        /// <summary>
        /// <para>Convert a hue,luminance,saturation value to RGB, xyY and XYZ, using a color matrix (and its inverse) and a power-law gamma function.</para>
        /// <para>Steps taken by this function:</para>
        /// <list type="number">
        /// <item>Hue (RGB) color is multiplied by the luminance.</item>
        /// <item>Power-law gamma function applied (color^gamma).</item>
        /// <item>Color is converted from RGB to XYZ using the matrix.</item>
        /// <item>XYZ is converted to xyY.</item>
        /// <item>Saturation is applied to the xyY color, moving it a certain percentage to the xyY white point defined by the matrix. This value is stored in the xyY out variable.</item>
        /// <item>The saturated xyY is converted back to XYZ, and stored in the XYZ out variable.</item>
        /// <item>The saturated XYZ is converted back to RGB using the inverse matrix.</item>
        /// <item>The RGB color is gamma corrected using the inverse gamma (color^(1/gamma)). This value is stored in the rgb out variable.</item>
        /// </list>
        /// </summary>
        /// <param name="hue">Hue (color) to convert (build-in luminance should be 1)</param>
        /// <param name="luminance">Luminance of the color</param>
        /// <param name="saturation">Saturation to apply</param>
        /// <param name="matrix">RGB to XYZ color matrix</param>
        /// <param name="inverse">XYZ to RGB color matrix</param>
        /// <param name="gamma">Gamma exponent</param>
        /// <param name="rgb">Variable to store RGB result</param>
        /// <param name="xyY">Variable to store xyY result</param>
        /// <param name="XYZ">Variable to store XYZ result</param>
        public static void fromHSL(ColorVector hue, double luminance, double saturation, ColorMatrix matrix, ColorMatrix inverse, double gamma, out ColorVector rgb, out ColorVector xyY, out ColorVector XYZ)
        {
            ColorVector luminanced = hue.Multiply(luminance);
            ColorVector gammafied = luminanced.Power(gamma);
            ColorVector usXYZ = matrix.Multiply(gammafied);
            ColorVector usxyY = XYZtoxyY(usXYZ);
            xyY = SaturatexyY(usxyY, saturation, matrix);
            XYZ = xyYtoXYZ(xyY);
            ColorVector sgammafied = inverse.Multiply(XYZ);
            sgammafied.MaxLocal(0.0);
            rgb = sgammafied.Power(1.0 / gamma);
        }

        public static ColorVector xyYtoXYZ(ColorVector xyY)
        {
            return new ColorVector(xyY.x * (xyY.z / xyY.y), xyY.z, (1 - xyY.x - xyY.y) * (xyY.z / xyY.y));
        }

        public static ColorVector XYZtoxyY(ColorVector XYZ)
        {
            return new ColorVector(XYZ.x / (XYZ.x + XYZ.y + XYZ.z), XYZ.y / (XYZ.x + XYZ.y + XYZ.z), XYZ.y);
        }

        public static ColorVector SaturatexyY(ColorVector xyY, double saturation, ColorMatrix matrix)
        {
            //calculate white point:
            ColorVector wXYZ = new ColorVector(matrix.m00 + matrix.m01 + matrix.m02, matrix.m10 + matrix.m11 + matrix.m12, matrix.m20 + matrix.m21 + matrix.m22);
            ColorVector wxyY = XYZtoxyY(wXYZ);
            return new ColorVector(saturation * (xyY.x - wxyY.x) + wxyY.x, saturation * (xyY.y - wxyY.y) + wxyY.y, xyY.z);
        }

        public static ColorVector d50toD65(ColorVector XYZ)
        {
            return bradfordD50toD65.Multiply(XYZ);
        }
    }
}
