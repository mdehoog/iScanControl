using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    public class DuoListValues
    {
        public static readonly ListValues InputSelectValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Video 1", 1),
            new ListValue("Video 2", 2),
            new ListValue("Video 3", 3),
            new ListValue("S-Video", 4),
            new ListValue("Component 1", 5),
            new ListValue("Component 2", 6),
            new ListValue("HDMI 1", 7),
            new ListValue("HDMI 2", 8),
            new ListValue("HDMI 3", 9),
            new ListValue("HDMI 4", 10),
            new ListValue("HDMI 5", 11),
            new ListValue("HDMI 6", 12),
            new ListValue("HDMI 7", 13),
            new ListValue("HDMI 8", 14),
            new ListValue("VGA", 15));
        public static readonly ListValues CUECorrectionValues = new ListValues(2,
            new ListValue("Auto", 2),
            new ListValue("Off", 0),
            new ListValue("On", 1));
        public static readonly ListValues MosquitoNRValues = new ListValues(0,
            new ListValue("Off", 0),
            new ListValue("Low", 1),
            new ListValue("High", 3));
        public static readonly ListValues AudioInputValues = new ListValues(1,
            new ListValue("HDMI", 1),
            new ListValue("Coax", 2),
            new ListValue("Optical 1", 3),
            new ListValue("Optical 2", 4),
            new ListValue("Optical 3", 5),
            new ListValue("Stereo 1", 6),
            new ListValue("Stereo 2", 7));
        public static readonly ListValues OutputSelectValues = new ListValues(1,
            new ListValue("HDMI 1", 1),
            new ListValue("HDMI 2", 2));
        public static readonly ListValues OutputVideoFormatValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("480p", 3),
            new ListValue("576p", 4),
            new ListValue("720p50", 5),
            new ListValue("720p60", 6),
            new ListValue("1080i50", 7),
            new ListValue("1080i60", 8),
            new ListValue("1080p24", 9),
            new ListValue("1080p25", 10),
            new ListValue("1080p30", 18),
            new ListValue("1080p50", 12),
            new ListValue("1080p60", 13),
            new ListValue("VGA", 14),
            new ListValue("SVGA", 15),
            new ListValue("XGA", 16),
            new ListValue("SXGA", 17));
        public static readonly ListValues DisplayAspectRatioValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("16:9", 1),
            new ListValue("4:3", 2));
        public static readonly ListValues OutputColorSpaceValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("RGB", 1),
            new ListValue("YCbCr 4:2:2", 2),
            new ListValue("YCbCr 4:4:4", 3));
        public static readonly ListValues OutputColorimetryValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("ITU BT.601", 1),
            new ListValue("ITU BT.709", 2));
        public static readonly ListValues OutputDeepColorValues = new ListValues(1,
            new ListValue("Off", 0),
            new ListValue("Auto", 1),
            new ListValue("10-bit", 2),
            new ListValue("12-bit", 3));
        public static readonly ListValues OutputVideoLevelValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Video", 1),
            new ListValue("Computer", 2));
        public static readonly ListValues OutputHDCPModeValues = new ListValues(2,
            new ListValue("Auto", 2),
            new ListValue("Off", 0),
            new ListValue("On", 1));
        public static readonly ListValues InputVideoLevelValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Video", 1),
            new ListValue("Computer", 2));
        public static readonly ListValues InputColorSpaceValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("RGB", 1),
            new ListValue("YCbCr 4:2:2", 2),
            new ListValue("YCbCr 4:4:4", 3));
        public static readonly ListValues InputDeepColorValues = new ListValues(0,
            new ListValue("Off", 0),
            new ListValue("30-bit", 1),
            new ListValue("36-bit", 2));
        public static readonly ListValues InputColorimetryValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("ITU BT.601", 1),
            new ListValue("ITU BT.709", 2));
        public static readonly ListValues AutoWakeupValues = new ListValues(1,
            new ListValue("Off", 0),
            new ListValue("Mode 1", 1),
            new ListValue("Mode 2", 2));
        public static readonly ListValue MenuTimeoutValueOff = new ListValue("Off", 0);
        public static readonly ListValue MenuTimeoutValue40 = new ListValue("40 seconds", 1);
        public static readonly ListValue MenuTimeoutValue160 = new ListValue("160 seconds", 2);
        public static readonly ListValues MenuTimeoutValues = new ListValues(1,
            MenuTimeoutValueOff,
            MenuTimeoutValue40,
            MenuTimeoutValue160);
        public static readonly ListValue TestPatternValueOff = new ListValue("Off", 0);
        public static readonly ListValue TestPatternValueWhite = new ListValue("White", 28);
        public static readonly ListValues TestPatternValues = new ListValues(0,
            TestPatternValueOff,
            new ListValue("Frame and Geometry", 1),
            new ListValue("Brightness Contrast", 2),
            new ListValue("Alternating pixels", 3),
            new ListValue("Vertical Lines", 4),
            new ListValue("Horizontal Lines", 5),
            new ListValue("Judder", 6),
            new ListValue("8 Color Bars 75 IRE", 7),
            new ListValue("8 Color Bars 100 IRE", 8),
            new ListValue("10 IRE", 9),
            new ListValue("20 IRE", 10),
            new ListValue("30 IRE", 11),
            new ListValue("40 IRE", 12),
            new ListValue("50 IRE", 13),
            new ListValue("60 IRE", 14),
            new ListValue("70 IRE", 15),
            new ListValue("80 IRE", 16),
            new ListValue("90 IRE", 17),
            new ListValue("100 IRE", 18),
            new ListValue("Gray Ramp", 19),
            new ListValue("Cross Hatch Coarse", 20),
            new ListValue("Cross Hatch Fine", 21),
            new ListValue("Focus", 22),
            new ListValue("Half Pattern Black White", 23),
            new ListValue("Half Pattern 7-Color Bars 75 IRE", 24),
            new ListValue("Half Pattern 7-Color Bars 100 IRE", 25),
            new ListValue("Half Pattern 8-Color Bars 75 IRE", 26),
            new ListValue("Half Pattern 8-Color Bars 100 IRE", 27),
            TestPatternValueWhite,
            new ListValue("Red", 29),
            new ListValue("Green", 30),
            new ListValue("Blue", 31),
            new ListValue("Cyan", 32),
            new ListValue("Magenta", 33),
            new ListValue("Yellow", 34),
            new ListValue("Black", 35));
        public static readonly ListValues InfoScreenValues = new ListValues(0,
            new ListValue("Off", 0),
            new ListValue("Input Status", 1),
            new ListValue("Picture Controls", 2),
            new ListValue("Output Status", 3),
            new ListValue("About", 4));
        public static readonly ListValues FactoryDefaultValues = new ListValues(-1,
            new ListValue("All", 0),
            new ListValue("Name", 1),
            new ListValue("Picture Controls", 2),
            new ListValue("Output Format", 3));
        public static readonly ListValues AudioOutputValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("HDMI Video", 1),
            new ListValue("HDMI Audio", 2),
            new ListValue("Optical", 3));
        public static readonly ListValues InputPictureARValues = new ListValues(2,
            new ListValue("4:3", 1),
            new ListValue("5:4", 3),
            new ListValue("16:9", 2));
        public static readonly ListValues InputActiveARValues = new ListValues(4,
            new ListValue("4:3", 1),
            new ListValue("1.55:1", 2),
            new ListValue("1.66:1", 3),
            new ListValue("16:9", 4),
            new ListValue("1.85:1", 5),
            new ListValue("2.35:1", 6));
        public static readonly ListValues InputARPresetValues = new ListValues(1,
            new ListValue("16:9 Full Frame", 1),
            new ListValue("4:3 Full Frame", 2),
            new ListValue("4:3 Letter Box", 3),
            new ListValue("Panorama", 4),
            new ListValue("User Preset", 5));
        public static readonly ListValues PREP480pValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Off", 1));
        public static readonly ListValues PREP1080pValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Off", 1));
        public static readonly ListValues HotPlugSourceValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Off", 1));
        public static readonly ListValues DeinterlacerModeValues = new ListValues(6,
            new ListValue("Auto", 6),
            new ListValue("Film Bias", 0),
            new ListValue("Forced 3:2", 8),
            new ListValue("Forced 2:2", 10),
            new ListValue("2:2 Even", 2),
            new ListValue("2:2 Odd", 3),
            new ListValue("Video", 1));
        public static readonly ListValues FrameLockModeValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("Unlock", 1));
        public static readonly ListValues InputChromaticityValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("RGBs/709", 1),
            new ListValue("NTSC", 2),
            new ListValue("PAL/SECAM", 3),
            new ListValue("SMPTE-C", 4),
            new ListValue("CIE 1931", 5),
            new ListValue("AppleRGB", 6),
            new ListValue("Adobe 1998", 7));
        public static readonly ListValues OutputChromaticityValues = new ListValues(0,
            new ListValue("Auto", 0),
            new ListValue("RGBs/709", 1),
            new ListValue("NTSC", 2),
            new ListValue("PAL/SECAM", 3),
            new ListValue("SMPTE-C", 4),
            new ListValue("CIE 1931", 5),
            new ListValue("AppleRGB", 6),
            new ListValue("Adobe 1998", 7),
            new ListValue("User", 8));
        public static readonly ListValues DayNightProfileValues = new ListValues(0,
            new ListValue("Day", 0),
            new ListValue("Night", 1));
        public static readonly ListValues BitRateValues = new ListValues(4,
            new ListValue("4800", 1),
            new ListValue("9600", 2),
            new ListValue("14400", 3),
            new ListValue("19200", 4),
            new ListValue("38400", 5),
            new ListValue("57600", 6),
            new ListValue("115200", 7));
        public static readonly ListValues ComponentInputValues = new ListValues(0,
            new ListValue("Single Sync", 0),
            new ListValue("Triple Sync", 1),
            new ListValue("AGC Disable", 2));
        public static readonly ListValues FrontPanelBrightnessValues = new ListValues(3,
            new ListValue("Off", 0),
            new ListValue("Low", 1),
            new ListValue("Medium", 2),
            new ListValue("High", 3));
        public static readonly ListValues PassThruValues = new ListValues(2,
            new ListValue("Auto", 2),
            new ListValue("On", 1));

        public static readonly ListValues RemoteButtonValues = new ListValues(-1,
            new ListValue("Menu Button", 5),
            new ListValue("Exit Button", 7),
            new ListValue("Enter Button", 6),
            new ListValue("Up Button", 3),
            new ListValue("Down Button", 4),
            new ListValue("Left Button", 1),
            new ListValue("Right Button", 2));
        public static readonly ListValues PowerValues = new ListValues(-1,
            new ListValue("Power On", 1),
            new ListValue("Power Off", 0));
        public static readonly ListValues ResetValues = new ListValues(-1,
            new ListValue("Reset", 0));
        public static readonly ListValues FirmwareUpdateValues = new ListValues(-1,
            new ListValue("Firmware Update", 0));
    }
}
