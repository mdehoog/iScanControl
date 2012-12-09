using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    public class DuoCommands
    {
        public static readonly ListCommand InputSelectCommand = new ListCommand("Input Select", "4C", true, DuoListValues.InputSelectValues);
        public static readonly ListCommand Prep480pCommand = new ListCommand("PReP 480p/576p", "B6", true, DuoListValues.PREP480pValues);
        public static readonly ListCommand Prep1080pCommand = new ListCommand("PReP 1080p", "B7", true, DuoListValues.PREP1080pValues);
        public static readonly ListCommand DeinterlacerModeCommand = new ListCommand("Deinterlacer Mode", "49", true, DuoListValues.DeinterlacerModeValues);
        public static readonly ListCommand InputColorSpaceCommand = new ListCommand("Input Color Space", "87", true, DuoListValues.InputColorSpaceValues);
        public static readonly ListCommand InputColorimetryCommand = new ListCommand("Input Colorimetry", "EF", true, DuoListValues.InputColorimetryValues);
        public static readonly ListCommand InputVideoLevelCommand = new ListCommand("Input Video Level", "F0", true, DuoListValues.InputVideoLevelValues);
        public static readonly ListCommand InputDeepColorCommand = new ListCommand("Input Deep Color", "F8", true, DuoListValues.InputDeepColorValues);
        public static readonly ListCommand InputChromaticityCommand = new ListCommand("Input Chromaticity", "DB", true, DuoListValues.InputChromaticityValues);
        public static readonly ListCommand HotPlugSourceCommand = new ListCommand("Hot Plug Source", "71", true, DuoListValues.HotPlugSourceValues);
        public static readonly ListCommand AudioInputCommand = new ListCommand("Audio Input", "4A", true, DuoListValues.AudioInputValues);
        public static readonly ListCommand InputPictureARCommand = new ListCommand("Input Picture Aspect Ratio", "4E", true, DuoListValues.InputPictureARValues); //serial automation header says pictureAR is 4E, masking is F7
        public static readonly ListCommand InputActiveARCommand = new ListCommand("Input Active Aspect Ratio", "50", true, DuoListValues.InputActiveARValues);
        public static readonly ListCommand InputARPresetsCommand = new ListCommand("Input Aspect Ratio Presets", "30", true, DuoListValues.InputARPresetValues);
        public static readonly ListCommand MosquitoNRCommand = new ListCommand("Mosquito Noise Reduction", "CA" /*"C8"*/, true, DuoListValues.MosquitoNRValues); //docs say C8, but works with CA!
        public static readonly ListCommand CueCorrectionCommand = new ListCommand("CUE Correction", "28", true, DuoListValues.CUECorrectionValues);
        public static readonly ListCommand OutputSelectCommand = new ListCommand("Output Select", "60", true, DuoListValues.OutputSelectValues);
        public static readonly ListCommand OutputVideoFormatCommand = new ListCommand("Output Video Format", "61", true, DuoListValues.OutputVideoFormatValues);
        public static readonly ListCommand FrameLockModeCommand = new ListCommand("Frame Lock Mode", "74", true, DuoListValues.FrameLockModeValues);
        public static readonly ListCommand DisplayAspectRatioCommand = new ListCommand("Display Aspect Ratio", "6A", true, DuoListValues.DisplayAspectRatioValues);
        public static readonly ListCommand OutputColorSpaceCommand = new ListCommand("Output Color Space", "6C", true, DuoListValues.OutputColorSpaceValues);
        public static readonly ListCommand OutputColorimetryCommand = new ListCommand("Output Colorimetry", "E5", true, DuoListValues.OutputColorimetryValues);
        public static readonly ListCommand OutputVideoLevelCommand = new ListCommand("Output Video Level", "E6", true, DuoListValues.OutputVideoLevelValues);
        public static readonly ListCommand OutputChromaticityCommand = new ListCommand("Output Chromaticity", "DC", true, DuoListValues.OutputChromaticityValues);
        public static readonly ListCommand OutputDeepColorCommand = new ListCommand("Output Deep Color", "F9", true, DuoListValues.OutputDeepColorValues);
        public static readonly ListCommand OutputHDCPModeCommand = new ListCommand("Output HDCP Mode", "EA", true, DuoListValues.OutputHDCPModeValues);
        public static readonly ListCommand AudioOutputCommand = new ListCommand("Audio Output", "BA", true, DuoListValues.AudioOutputValues);
        public static readonly ListCommand AutoWakeupCommand = new ListCommand("Auto Wakeup", "2E", true, DuoListValues.AutoWakeupValues);
        public static readonly ListCommand MenuTimeoutCommand = new ListCommand("Menu Timeout", "F5", true, DuoListValues.MenuTimeoutValues);
        public static readonly ListCommand DayNightCommand = new ListCommand("Day/Night Profile", "C4", true, DuoListValues.DayNightProfileValues);

        public static readonly ListCommand InfoScreenCommand = new ListCommand("Info Screen", "A5", false, DuoListValues.InfoScreenValues);
        public static readonly ListCommand TestPatternCommand = new ListCommand("Test Pattern", "80", false, DuoListValues.TestPatternValues);

        public static readonly ListCommand RemoteButtonCommand = new ListCommand("Remote Button", "A2", false, false, DuoListValues.RemoteButtonValues);
        public static readonly ListCommand PowerCommand = new ListCommand("Power", "A1", false, false, DuoListValues.PowerValues);
        public static readonly ListCommand ResetCommand = new ListCommand("Reset", "AE", false, false, DuoListValues.ResetValues);
        public static readonly ListCommand FactoryDefaultCommand = new ListCommand("Factory Default", "AC", false, false, DuoListValues.FactoryDefaultValues);
        public static readonly ListCommand FirmwareUpdateCommand = new ListCommand("Firmware Update", "AD", false, false, DuoListValues.FirmwareUpdateValues);
        public static readonly ListCommand BitRateCommand = new ListCommand("RS232 Baud Rate", "A3", true, DuoListValues.BitRateValues);
        public static readonly ListCommand ComponentInputsCommand = new ListCommand("Component Inputs", "FD", true, DuoListValues.ComponentInputValues);
        public static readonly ListCommand FrontPanelBrightnessCommand = new ListCommand("Front Panel Brightness", "EC", true, DuoListValues.FrontPanelBrightnessValues);
        public static readonly ListCommand PassThruCommand = new ListCommand("3D Pass Through", "A7", true, DuoListValues.PassThruValues);

        public static readonly DecimalCommand AudioDelayCommand = new DecimalCommand("Audio Delay", "4B", 0);
        public static readonly DecimalCommand HorizontalStretchCommand = new DecimalCommand("Horizontal Stretch", "40", 1, 3);
        public static readonly DecimalCommand VerticalStretchCommand = new DecimalCommand("Vertical Stretch", "41", 1, 3);
        public static readonly DecimalCommand HorizontalShiftCommand = new DecimalCommand("Horizontal Shift", "42", 0);
        public static readonly DecimalCommand VerticalShiftCommand = new DecimalCommand("Vertical Shift", "43", 0);
        public static readonly DecimalCommand ZoomCommand = new DecimalCommand("Zoom", "46", 1, 3);
        public static readonly DecimalCommand BrightnessCommand = new DecimalCommand("Brightness", "21", 0);
        public static readonly DecimalCommand ContrastCommand = new DecimalCommand("Contrast", "22", 0);
        public static readonly DecimalCommand SaturationCommand = new DecimalCommand("Saturation", "23", 0);
        public static readonly DecimalCommand HueCommand = new DecimalCommand("Hue", "24", 0);
        public static readonly DecimalCommand YCDelayCommand = new DecimalCommand("Y/C Delay", "27", 0);
        public static readonly DecimalCommand DetailEnhancementCommand = new DecimalCommand("Detail Enhancement", "C8" /*"C9"*/, 0); //docs say C9, but Duo works with C8!
        public static readonly DecimalCommand EdgeEnhancementCommand = new DecimalCommand("Edge Enhancement", "C9" /*"CA"*/, 0); //docs say CA, but Duo works with C9!
        public static readonly DecimalCommand UnderscanCommand = new DecimalCommand("Underscan", "8B", 0);
        public static readonly DecimalCommand BorderLevelCommand = new DecimalCommand("Border Level", "4F", 0);
        public static readonly DecimalCommand MaskLevelCommand = new DecimalCommand("Mask Level", "F7", 0); //serial automation header says pictureAR is 4E, masking is F7
        public static readonly DecimalCommand InputPriorityCommand = new DecimalCommand("Input Priority", "81", 0);

        public static readonly BooleanCommand GameModeCommand = new BooleanCommand("Game Mode", "2D", true, false);
        public static readonly BooleanCommand InputHDCPModeCommand = new BooleanCommand("Input HDCP Mode", "86", true, true);
        public static readonly BooleanCommand OneOneFrameRateCommand = new BooleanCommand("1:1 Frame Rate", "2F", true, false);
        public static readonly BooleanCommand SecondOutputCommand = new BooleanCommand("Second Output", "75", true, false);
        public static readonly BooleanCommand AutoStandbyCommand = new BooleanCommand("Auto Standby", "83", true, true);
        public static readonly BooleanCommand AutoInputARCommand = new BooleanCommand("Auto Input Aspect Ratio", "B0", true, true);
        public static readonly BooleanCommand PanoramaCommand = new BooleanCommand("Panorama", "A6", true, false);
        public static readonly BooleanCommand CMSBypassCommand = new BooleanCommand("CMS Bypass", "FA", true, false);
        public static readonly BooleanCommand RGBsComponent1Command = new BooleanCommand("RGBs Component 1", "FB", true, false);
        public static readonly BooleanCommand RGBsComponent2Command = new BooleanCommand("RGBs Component 2", "FC", true, false);
        public static readonly BooleanCommand OSDInputIndicatorCommand = new BooleanCommand("OSD Input Indicator", "F6", true, true);

        public static readonly StringCommand ProductNameCommand = new StringCommand("Product Name", "A8");
        public static readonly StringCommand VersionNumberCommand = new StringCommand("Version Number", "A9");

        public static readonly StringCommand Video1InputLabelCommand = new StringCommand("Video 1 Input Label", "FE", "1", true);
        public static readonly StringCommand Video2InputLabelCommand = new StringCommand("Video 2 Input Label", "FE", "2", true);
        public static readonly StringCommand Video3InputLabelCommand = new StringCommand("Video 3 Input Label", "FE", "3", true);
        public static readonly StringCommand SVideoInputLabelCommand = new StringCommand("S-Video Input Label", "FE", "4", true);
        public static readonly StringCommand Component1InputLabelCommand = new StringCommand("Component 1 Input Label", "FE", "5", true);
        public static readonly StringCommand Component2InputLabelCommand = new StringCommand("Component 2 Input Label", "FE", "6", true);
        public static readonly StringCommand HDMI1InputLabelCommand = new StringCommand("HDMI 1 Input Label", "FE", "7", true);
        public static readonly StringCommand HDMI2InputLabelCommand = new StringCommand("HDMI 2 Input Label", "FE", "8", true);
        public static readonly StringCommand HDMI3InputLabelCommand = new StringCommand("HDMI 3 Input Label", "FE", "9", true);
        public static readonly StringCommand HDMI4InputLabelCommand = new StringCommand("HDMI 4 Input Label", "FE", "10", true);
        public static readonly StringCommand HDMI5InputLabelCommand = new StringCommand("HDMI 5 Input Label", "FE", "11", true);
        public static readonly StringCommand HDMI6InputLabelCommand = new StringCommand("HDMI 6 Input Label", "FE", "12", true);
        public static readonly StringCommand HDMI7InputLabelCommand = new StringCommand("HDMI 7 Input Label", "FE", "13", true);
        public static readonly StringCommand HDMI8InputLabelCommand = new StringCommand("HDMI 8 Input Label", "FE", "14", true);
        public static readonly StringCommand VGAInputLabelCommand = new StringCommand("VGA Input Label", "FE", "15", true);

        public static readonly DecimalCommand UCRedxCommand = new DecimalCommand("User Chromaticity - Red-x", "DD", "0", 4);
        public static readonly DecimalCommand UCRedyCommand = new DecimalCommand("User Chromaticity - Red-y", "DD", "1", 4);
        public static readonly DecimalCommand UCGreenxCommand = new DecimalCommand("User Chromaticity - Green-x", "DD", "2", 4);
        public static readonly DecimalCommand UCGreenyCommand = new DecimalCommand("User Chromaticity - Green-y", "DD", "3", 4);
        public static readonly DecimalCommand UCBluexCommand = new DecimalCommand("User Chromaticity - Blue-x", "DD", "4", 4);
        public static readonly DecimalCommand UCBlueyCommand = new DecimalCommand("User Chromaticity - Blue-y", "DD", "5", 4);
        public static readonly DecimalCommand UCWhitexCommand = new DecimalCommand("User Chromaticity - White-x", "DD", "6", 4);
        public static readonly DecimalCommand UCWhiteyCommand = new DecimalCommand("User Chromaticity - White-y", "DD", "7", 4);

        public static readonly DecimalCommand CGRedxCommand = new DecimalCommand("Color Gamut - Red-x", "DE", "0", 4);
        public static readonly DecimalCommand CGRedyCommand = new DecimalCommand("Color Gamut - Red-y", "DE", "1", 4);
        public static readonly DecimalCommand CGRedcYCommand = new DecimalCommand("Color Gamut - Red-Y", "DE", "8", 4);
        public static readonly DecimalCommand CGGreenxCommand = new DecimalCommand("Color Gamut - Green-x", "DE", "2", 4);
        public static readonly DecimalCommand CGGreenyCommand = new DecimalCommand("Color Gamut - Green-y", "DE", "3", 4);
        public static readonly DecimalCommand CGGreencYCommand = new DecimalCommand("Color Gamut - Green-Y", "DE", "9", 4);
        public static readonly DecimalCommand CGBluexCommand = new DecimalCommand("Color Gamut - Blue-x", "DE", "4", 4);
        public static readonly DecimalCommand CGBlueyCommand = new DecimalCommand("Color Gamut - Blue-y", "DE", "5", 4);
        public static readonly DecimalCommand CGBluecYCommand = new DecimalCommand("Color Gamut - Blue-Y", "DE", "10", 4);
        public static readonly DecimalCommand CGCyanxCommand = new DecimalCommand("Color Gamut - Cyan-x", "DE", "12", 4);
        public static readonly DecimalCommand CGCyanyCommand = new DecimalCommand("Color Gamut - Cyan-y", "DE", "13", 4);
        public static readonly DecimalCommand CGCyancYCommand = new DecimalCommand("Color Gamut - Cyan-Y", "DE", "14", 4);
        public static readonly DecimalCommand CGMagentaxCommand = new DecimalCommand("Color Gamut - Magenta-x", "DE", "15", 4);
        public static readonly DecimalCommand CGMagentayCommand = new DecimalCommand("Color Gamut - Magenta-y", "DE", "16", 4);
        public static readonly DecimalCommand CGMagentacYCommand = new DecimalCommand("Color Gamut - Magenta-Y", "DE", "17", 4);
        public static readonly DecimalCommand CGYellowxCommand = new DecimalCommand("Color Gamut - Yellow-x", "DE", "18", 4);
        public static readonly DecimalCommand CGYellowyCommand = new DecimalCommand("Color Gamut - Yellow-y", "DE", "19", 4);
        public static readonly DecimalCommand CGYellowcYCommand = new DecimalCommand("Color Gamut - Yellow-Y", "DE", "20", 4);
        public static readonly DecimalCommand CGWhitexCommand = new DecimalCommand("Color Gamut - White-x", "DE", "6", 4);
        public static readonly DecimalCommand CGWhiteyCommand = new DecimalCommand("Color Gamut - White-y", "DE", "7", 4);
        public static readonly DecimalCommand CGWhitecYCommand = new DecimalCommand("Color Gamut - White-Y", "DE", "11", 4);

        public static readonly DecimalCommand GRed0Command = new DecimalCommand("Grayscale - Red - 0 IRE", "DF", "0 0", 4);
        public static readonly DecimalCommand GRed10Command = new DecimalCommand("Grayscale - Red - 10 IRE", "DF", "0 1", 4);
        public static readonly DecimalCommand GRed20Command = new DecimalCommand("Grayscale - Red - 20 IRE", "DF", "0 2", 4);
        public static readonly DecimalCommand GRed30Command = new DecimalCommand("Grayscale - Red - 30 IRE", "DF", "0 3", 4);
        public static readonly DecimalCommand GRed40Command = new DecimalCommand("Grayscale - Red - 40 IRE", "DF", "0 4", 4);
        public static readonly DecimalCommand GRed50Command = new DecimalCommand("Grayscale - Red - 50 IRE", "DF", "0 5", 4);
        public static readonly DecimalCommand GRed60Command = new DecimalCommand("Grayscale - Red - 60 IRE", "DF", "0 6", 4);
        public static readonly DecimalCommand GRed70Command = new DecimalCommand("Grayscale - Red - 70 IRE", "DF", "0 7", 4);
        public static readonly DecimalCommand GRed80Command = new DecimalCommand("Grayscale - Red - 80 IRE", "DF", "0 8", 4);
        public static readonly DecimalCommand GRed90Command = new DecimalCommand("Grayscale - Red - 90 IRE", "DF", "0 9", 4);
        public static readonly DecimalCommand GRed100Command = new DecimalCommand("Grayscale - Red - 100 IRE", "DF", "0 10", 4);

        public static readonly DecimalCommand GGreen0Command = new DecimalCommand("Grayscale - Green - 0 IRE", "DF", "1 0", 4);
        public static readonly DecimalCommand GGreen10Command = new DecimalCommand("Grayscale - Green - 10 IRE", "DF", "1 1", 4);
        public static readonly DecimalCommand GGreen20Command = new DecimalCommand("Grayscale - Green - 20 IRE", "DF", "1 2", 4);
        public static readonly DecimalCommand GGreen30Command = new DecimalCommand("Grayscale - Green - 30 IRE", "DF", "1 3", 4);
        public static readonly DecimalCommand GGreen40Command = new DecimalCommand("Grayscale - Green - 40 IRE", "DF", "1 4", 4);
        public static readonly DecimalCommand GGreen50Command = new DecimalCommand("Grayscale - Green - 50 IRE", "DF", "1 5", 4);
        public static readonly DecimalCommand GGreen60Command = new DecimalCommand("Grayscale - Green - 60 IRE", "DF", "1 6", 4);
        public static readonly DecimalCommand GGreen70Command = new DecimalCommand("Grayscale - Green - 70 IRE", "DF", "1 7", 4);
        public static readonly DecimalCommand GGreen80Command = new DecimalCommand("Grayscale - Green - 80 IRE", "DF", "1 8", 4);
        public static readonly DecimalCommand GGreen90Command = new DecimalCommand("Grayscale - Green - 90 IRE", "DF", "1 9", 4);
        public static readonly DecimalCommand GGreen100Command = new DecimalCommand("Grayscale - Green - 100 IRE", "DF", "1 10", 4);

        public static readonly DecimalCommand GBlue0Command = new DecimalCommand("Grayscale - Blue - 0 IRE", "DF", "2 0", 4);
        public static readonly DecimalCommand GBlue10Command = new DecimalCommand("Grayscale - Blue - 10 IRE", "DF", "2 1", 4);
        public static readonly DecimalCommand GBlue20Command = new DecimalCommand("Grayscale - Blue - 20 IRE", "DF", "2 2", 4);
        public static readonly DecimalCommand GBlue30Command = new DecimalCommand("Grayscale - Blue - 30 IRE", "DF", "2 3", 4);
        public static readonly DecimalCommand GBlue40Command = new DecimalCommand("Grayscale - Blue - 40 IRE", "DF", "2 4", 4);
        public static readonly DecimalCommand GBlue50Command = new DecimalCommand("Grayscale - Blue - 50 IRE", "DF", "2 5", 4);
        public static readonly DecimalCommand GBlue60Command = new DecimalCommand("Grayscale - Blue - 60 IRE", "DF", "2 6", 4);
        public static readonly DecimalCommand GBlue70Command = new DecimalCommand("Grayscale - Blue - 70 IRE", "DF", "2 7", 4);
        public static readonly DecimalCommand GBlue80Command = new DecimalCommand("Grayscale - Blue - 80 IRE", "DF", "2 8", 4);
        public static readonly DecimalCommand GBlue90Command = new DecimalCommand("Grayscale - Blue - 90 IRE", "DF", "2 9", 4);
        public static readonly DecimalCommand GBlue100Command = new DecimalCommand("Grayscale - Blue - 100 IRE", "DF", "2 10", 4);

        public static readonly DecimalCommand GWhitex0Command = new DecimalCommand("Grayscale - White-x - 0 IRE", "DF", "3 0", false, 4);
        public static readonly DecimalCommand GWhitex10Command = new DecimalCommand("Grayscale - White-x - 10 IRE", "DF", "3 1", false, 4);
        public static readonly DecimalCommand GWhitex20Command = new DecimalCommand("Grayscale - White-x - 20 IRE", "DF", "3 2", false, 4);
        public static readonly DecimalCommand GWhitex30Command = new DecimalCommand("Grayscale - White-x - 30 IRE", "DF", "3 3", false, 4);
        public static readonly DecimalCommand GWhitex40Command = new DecimalCommand("Grayscale - White-x - 40 IRE", "DF", "3 4", false, 4);
        public static readonly DecimalCommand GWhitex50Command = new DecimalCommand("Grayscale - White-x - 50 IRE", "DF", "3 5", false, 4);
        public static readonly DecimalCommand GWhitex60Command = new DecimalCommand("Grayscale - White-x - 60 IRE", "DF", "3 6", false, 4);
        public static readonly DecimalCommand GWhitex70Command = new DecimalCommand("Grayscale - White-x - 70 IRE", "DF", "3 7", false, 4);
        public static readonly DecimalCommand GWhitex80Command = new DecimalCommand("Grayscale - White-x - 80 IRE", "DF", "3 8", false, 4);
        public static readonly DecimalCommand GWhitex90Command = new DecimalCommand("Grayscale - White-x - 90 IRE", "DF", "3 9", false, 4);
        public static readonly DecimalCommand GWhitex100Command = new DecimalCommand("Grayscale - White-x - 100 IRE", "DF", "3 10", false, 4);

        public static readonly DecimalCommand GWhitey0Command = new DecimalCommand("Grayscale - White-y - 0 IRE", "DF", "4 0", false, 4);
        public static readonly DecimalCommand GWhitey10Command = new DecimalCommand("Grayscale - White-y - 10 IRE", "DF", "4 1", false, 4);
        public static readonly DecimalCommand GWhitey20Command = new DecimalCommand("Grayscale - White-y - 20 IRE", "DF", "4 2", false, 4);
        public static readonly DecimalCommand GWhitey30Command = new DecimalCommand("Grayscale - White-y - 30 IRE", "DF", "4 3", false, 4);
        public static readonly DecimalCommand GWhitey40Command = new DecimalCommand("Grayscale - White-y - 40 IRE", "DF", "4 4", false, 4);
        public static readonly DecimalCommand GWhitey50Command = new DecimalCommand("Grayscale - White-y - 50 IRE", "DF", "4 5", false, 4);
        public static readonly DecimalCommand GWhitey60Command = new DecimalCommand("Grayscale - White-y - 60 IRE", "DF", "4 6", false, 4);
        public static readonly DecimalCommand GWhitey70Command = new DecimalCommand("Grayscale - White-y - 70 IRE", "DF", "4 7", false, 4);
        public static readonly DecimalCommand GWhitey80Command = new DecimalCommand("Grayscale - White-y - 80 IRE", "DF", "4 8", false, 4);
        public static readonly DecimalCommand GWhitey90Command = new DecimalCommand("Grayscale - White-y - 90 IRE", "DF", "4 9", false, 4);
        public static readonly DecimalCommand GWhitey100Command = new DecimalCommand("Grayscale - White-y - 100 IRE", "DF", "4 10", false, 4);

        public static readonly DecimalCommand GWhitecY0Command = new DecimalCommand("Grayscale - White-Y - 0 IRE", "DF", "5 0", false, 4);
        public static readonly DecimalCommand GWhitecY10Command = new DecimalCommand("Grayscale - White-Y - 10 IRE", "DF", "5 1", false, 4);
        public static readonly DecimalCommand GWhitecY20Command = new DecimalCommand("Grayscale - White-Y - 20 IRE", "DF", "5 2", false, 4);
        public static readonly DecimalCommand GWhitecY30Command = new DecimalCommand("Grayscale - White-Y - 30 IRE", "DF", "5 3", false, 4);
        public static readonly DecimalCommand GWhitecY40Command = new DecimalCommand("Grayscale - White-Y - 40 IRE", "DF", "5 4", false, 4);
        public static readonly DecimalCommand GWhitecY50Command = new DecimalCommand("Grayscale - White-Y - 50 IRE", "DF", "5 5", false, 4);
        public static readonly DecimalCommand GWhitecY60Command = new DecimalCommand("Grayscale - White-Y - 60 IRE", "DF", "5 6", false, 4);
        public static readonly DecimalCommand GWhitecY70Command = new DecimalCommand("Grayscale - White-Y - 70 IRE", "DF", "5 7", false, 4);
        public static readonly DecimalCommand GWhitecY80Command = new DecimalCommand("Grayscale - White-Y - 80 IRE", "DF", "5 8", false, 4);
        public static readonly DecimalCommand GWhitecY90Command = new DecimalCommand("Grayscale - White-Y - 90 IRE", "DF", "5 9", false, 4);
        public static readonly DecimalCommand GWhitecY100Command = new DecimalCommand("Grayscale - White-Y - 100 IRE", "DF", "5 10", false, 4);
    }
}
