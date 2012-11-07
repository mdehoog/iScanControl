using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Conversion
{
    /// <summary>
    /// ColorVector subclass that provides the ability to label a color, for UI purposes.
    /// </summary>
    public class LabeledColorVector : ColorVector
    {
        public string label;

        public LabeledColorVector(double x, double y, double z, string label)
            : base(x, y, z)
        {
            this.label = label;
        }

        public override string ToString()
        {
            return label;
        }

        public static readonly LabeledColorVector White = new LabeledColorVector(1, 1, 1, "White");
        public static readonly LabeledColorVector Red = new LabeledColorVector(1, 0, 0, "Red");
        public static readonly LabeledColorVector Green = new LabeledColorVector(0, 1, 0, "Green");
        public static readonly LabeledColorVector Blue = new LabeledColorVector(0, 0, 1, "Blue");
        public static readonly LabeledColorVector Yellow = new LabeledColorVector(1, 1, 0, "Yellow");
        public static readonly LabeledColorVector Cyan = new LabeledColorVector(0, 1, 1, "Cyan");
        public static readonly LabeledColorVector Magenta = new LabeledColorVector(1, 0, 1, "Magenta");
        public static readonly LabeledColorVector[] Values = new LabeledColorVector[] { White, Red, Green, Blue, Yellow, Cyan, Magenta };
    }
}
