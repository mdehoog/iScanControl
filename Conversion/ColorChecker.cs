using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Conversion
{
    public class ColorChecker
    {
        public static readonly LabeledColorVector DarkSkin = new LabeledColorVector(0.4316, 0.3777, 0.1008, "Dark skin");
        public static readonly LabeledColorVector LightSkin = new LabeledColorVector(0.4197, 0.3744, 0.3495, "Light skin");
        public static readonly LabeledColorVector BlueSky = new LabeledColorVector(0.276, 0.3016, 0.1836, "Blue sky");
        public static readonly LabeledColorVector Foliage = new LabeledColorVector(0.3703, 0.4499, 0.1325, "Foliage");
        public static readonly LabeledColorVector BlueFlower = new LabeledColorVector(0.2999, 0.2856, 0.2304, "Blue flower");
        public static readonly LabeledColorVector BluishGreen = new LabeledColorVector(0.2848, 0.3911, 0.4178, "Bluish green");
        public static readonly LabeledColorVector Orange = new LabeledColorVector(0.5295, 0.4055, 0.3118, "Orange");
        public static readonly LabeledColorVector PurplishBlue = new LabeledColorVector(0.2305, 0.2106, 0.1126, "Purplish blue");
        public static readonly LabeledColorVector ModerateRed = new LabeledColorVector(0.5012, 0.3273, 0.1938, "Moderate red");
        public static readonly LabeledColorVector Purple = new LabeledColorVector(0.3319, 0.2482, 0.0637, "Purple");
        public static readonly LabeledColorVector YellowGreen = new LabeledColorVector(0.3984, 0.5008, 0.4446, "Yellow green");
        public static readonly LabeledColorVector OrangeYellow = new LabeledColorVector(0.4957, 0.4427, 0.4357, "Orange yellow");
        public static readonly LabeledColorVector Blue = new LabeledColorVector(0.2018, 0.1692, 0.0575, "Blue");
        public static readonly LabeledColorVector Green = new LabeledColorVector(0.3253, 0.5032, 0.2318, "Green");
        public static readonly LabeledColorVector Red = new LabeledColorVector(0.5686, 0.3303, 0.1257, "Red");
        public static readonly LabeledColorVector Yellow = new LabeledColorVector(0.4697, 0.4734, 0.5981, "Yellow");
        public static readonly LabeledColorVector Magenta = new LabeledColorVector(0.4159, 0.2688, 0.2009, "Magenta");
        public static readonly LabeledColorVector Cyan = new LabeledColorVector(0.2131, 0.3023, 0.193, "Cyan");
        public static readonly LabeledColorVector White95 = new LabeledColorVector(0.3469, 0.3608, 0.9131, "White 9.5");
        public static readonly LabeledColorVector Neutral80 = new LabeledColorVector(0.344, 0.3584, 0.5894, "Neutral 8");
        public static readonly LabeledColorVector Neutral65 = new LabeledColorVector(0.3432, 0.3581, 0.3632, "Neutral 6.5");
        public static readonly LabeledColorVector Neutral50 = new LabeledColorVector(0.3446, 0.3579, 0.1915, "Neutral 5");
        public static readonly LabeledColorVector Neutral35 = new LabeledColorVector(0.3401, 0.3548, 0.0883, "Neutral 3.5");
        public static readonly LabeledColorVector Black20 = new LabeledColorVector(0.3406, 0.3537, 0.0311, "Black 2");

        public static readonly LabeledColorVector[] Values = new LabeledColorVector[] { DarkSkin, LightSkin, BlueSky, Foliage, BlueFlower, BluishGreen, Orange, PurplishBlue, ModerateRed, Purple, YellowGreen, OrangeYellow, Blue, Green, Red, Yellow, Magenta, Cyan, White95, Neutral80, Neutral65, Neutral50, Neutral35, Black20 };
    }
}
