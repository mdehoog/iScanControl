using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Profiler.Protocol;

namespace Profiler.UI
{
    public class ControlLinker
    {
        private IConnector inputSelectConnector;
        private IList<IConnector> inputConnectors = new List<IConnector>();

        private IConnector verticalShiftConnector;
        private IConnector horizontalShiftConnector;
        private IConnector verticalStretchConnector;
        private IConnector horizontalStretchConnector;
        private IConnector dayNightProfileConnector;
        private IConnector[][][] colorGamutConnectors;
        private IConnector[][] grayscaleConnectors;

        private BooleanConnector autoARConnector;
        private IConnector activeARConnector;
        private IConnector pictureARConnector;
        private IConnector panoramaARConnector;
        private IConnector presetARConnector;

        public void ConnectControls(MainForm form)
        {
            ConnectInputSelectControls(form);
            ConnectOutputSelectControls(form);
            ConnectAspectRatioControls(form);
            ConnectInputControls(form);
            ConnectPictureControls(form);
            ConnectOutputControls(form);
            ConnectConfigurationControls(form);
            ConnectInputLabelControls(form);
            ConnectRemoteControls(form);
            ConnectOtherControls(form);
            ConnectCMSControls(form);

            ConnectDependants();
        }

        private void ConnectInputSelectControls(MainForm form)
        {
            inputSelectConnector = ConnectListControl(DuoCommands.InputSelectCommand, form.inputSelectCombo, form.inputSelectLabel, CommandCategory.Input);
            inputSelectConnector.DelayAfterSet = 2000;
        }

        private void ConnectOutputSelectControls(MainForm form)
        {
            ConnectListControl(DuoCommands.OutputSelectCommand, form.outputSelectCombo, form.outputSelectLabel, CommandCategory.Output);
        }

        private void ConnectAspectRatioControls(MainForm form)
        {
            inputConnectors.Add(autoARConnector = ConnectBooleanControl(DuoCommands.AutoInputARCommand, form.autoARCheck, form.autoARLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(pictureARConnector = ConnectListControl(DuoCommands.InputPictureARCommand, form.pictureARCombo, form.pictureARLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(activeARConnector = ConnectListControl(DuoCommands.InputActiveARCommand, form.activeARCombo, form.activeARLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(horizontalStretchConnector = ConnectDecimalControl(DuoCommands.HorizontalStretchCommand, form.horizontalStretchSpinner, form.horizontalStretchLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(verticalStretchConnector = ConnectDecimalControl(DuoCommands.VerticalStretchCommand, form.verticalStretchSpinner, form.verticalStretchLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(horizontalShiftConnector = ConnectDecimalControl(DuoCommands.HorizontalShiftCommand, form.horizontalShiftSpinner, form.horizontalShiftLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(verticalShiftConnector = ConnectDecimalControl(DuoCommands.VerticalShiftCommand, form.verticalShiftSpinner, form.verticalShiftLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.ZoomCommand, form.zoomSpinner, form.zoomLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(panoramaARConnector = ConnectBooleanControl(DuoCommands.PanoramaCommand, form.panoramaCheck, form.panoramaLabel, CommandCategory.AspectRatio));
            inputConnectors.Add(presetARConnector = ConnectListControl(DuoCommands.InputARPresetsCommand, form.inputARPresetsCombo, form.inputARPresetsLabel, CommandCategory.AspectRatio));
        }

        private void ConnectInputControls(MainForm form)
        {
            inputConnectors.Add(ConnectListControl(DuoCommands.Prep480pCommand, form.prep480pCombo, form.prep480pLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.Prep1080pCommand, form.prep1080pCombo, form.prep1080pLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.DeinterlacerModeCommand, form.deinterlacerModeCombo, form.deinterlacerModeLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectBooleanControl(DuoCommands.GameModeCommand, form.gameModeCheck, form.gameModeLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.InputColorSpaceCommand, form.inputColorSpaceCombo, form.inputColorSpaceLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.InputColorimetryCommand, form.inputColorimetryCombo, form.inputColorimetryLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.InputVideoLevelCommand, form.inputVideoLevelCombo, form.inputVideoLevelLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.InputDeepColorCommand, form.inputDeepColorCombo, form.inputDeepColorLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.InputChromaticityCommand, form.inputChromaticityCombo, form.inputChromaticityLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.HotPlugSourceCommand, form.hotPlugSourceCombo, form.hotPlugSourceLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectBooleanControl(DuoCommands.InputHDCPModeCommand, form.inputHDCPModeCheck, form.inputHDCPModeLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.PassThruCommand, form.passThruCombo, form.passThruLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectListControl(DuoCommands.AudioInputCommand, form.audioInputCombo, form.audioInputLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.AudioDelayCommand, form.audioDelaySpinner, form.audioDelayLabel, CommandCategory.Input));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.InputPriorityCommand, form.prioritySpinner, form.priorityLabel, CommandCategory.Input));
        }

        private void ConnectPictureControls(MainForm form)
        {
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.BrightnessCommand, form.brightnessSpinner, form.brightnessLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.ContrastCommand, form.contrastSpinner, form.contrastLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.SaturationCommand, form.saturationSpinner, form.saturationLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.HueCommand, form.hueSpinner, form.hueLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.YCDelayCommand, form.ycDelaySpinner, form.ycDelayLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.DetailEnhancementCommand, form.detailEnhancementSpinner, form.detailEnhancementLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectDecimalControl(DuoCommands.EdgeEnhancementCommand, form.edgeEnhancementSpinner, form.edgeEnhancementLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectListControl(DuoCommands.MosquitoNRCommand, form.mosquitoNRCombo, form.mosquitoNRLabel, CommandCategory.Picture));
            inputConnectors.Add(ConnectListControl(DuoCommands.CueCorrectionCommand, form.cueCorrectionCombo, form.cueCorrectionLabel, CommandCategory.Picture));
        }

        private void ConnectOutputControls(MainForm form)
        {
            ConnectBooleanControl(DuoCommands.SecondOutputCommand, form.secondOutputCheck, form.secondOutputLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputVideoFormatCommand, form.outputVideoFormatCombo, form.outputVideoFormatLabel, CommandCategory.Output);
            ConnectDecimalControl(DuoCommands.UnderscanCommand, form.underscanSpinner, form.underscanLabel, CommandCategory.Output);
            ConnectBooleanControl(DuoCommands.OneOneFrameRateCommand, form.oneOneFrameRateCheck, form.oneOneFrameRateLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.FrameLockModeCommand, form.frameLockModeCombo, form.frameLockModeLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.DisplayAspectRatioCommand, form.displayAspectRatioCombo, form.displayAspectRatioLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputColorSpaceCommand, form.outputColorSpaceCombo, form.outputColorSpaceLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputColorimetryCommand, form.outputColorimetryCombo, form.outputColorimetryLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputVideoLevelCommand, form.outputVideoLevelCombo, form.outputVideoLevelLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputChromaticityCommand, form.outputChromaticityCombo, form.outputChromaticityLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputDeepColorCommand, form.outputDeepColorCombo, form.outputDeepColorLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.OutputHDCPModeCommand, form.outputHDCPModeCombo, form.outputHDCPModeLabel, CommandCategory.Output);
            ConnectDecimalControl(DuoCommands.BorderLevelCommand, form.borderLevelSpinner, form.borderLevelLabel, CommandCategory.Output);
            ConnectDecimalControl(DuoCommands.MaskLevelCommand, form.maskLevelSpinner, form.maskLevelLabel, CommandCategory.Output);
            ConnectListControl(DuoCommands.AudioOutputCommand, form.audioOutputCombo, form.audioOutputLabel, CommandCategory.Output);
        }

        private void ConnectConfigurationControls(MainForm form)
        {
            ConnectBooleanControl(DuoCommands.AutoStandbyCommand, form.autoStandbyCheck, form.autoStandbyLabel, CommandCategory.Configuration);
            ConnectListControl(DuoCommands.AutoWakeupCommand, form.autoWakeupCombo, form.autoWakeupLabel, CommandCategory.Configuration);
            ConnectWarningListControl(DuoCommands.BitRateCommand, form.bitRateCombo, form.bitRateLabel, CommandCategory.Configuration,
                 "Setting the " + DuoCommands.BitRateCommand.Name + " to '{0}' will interrupt the serial connection. You will have to reconnect using the new value. Are you sure?", DuoCommands.BitRateCommand.Name, false);
            ConnectListControl(DuoCommands.ComponentInputsCommand, form.componentInputsCombo, form.componentInputsLabel, CommandCategory.Configuration);
            ConnectBooleanControl(DuoCommands.RGBsComponent1Command, form.rgbsComponent1Check, form.rgbsComponent1Label, CommandCategory.Configuration);
            ConnectBooleanControl(DuoCommands.RGBsComponent2Command, form.rgbsComponent2Check, form.rgbsComponent2Label, CommandCategory.Configuration);
            ConnectListControl(DuoCommands.MenuTimeoutCommand, form.menuTimeoutCombo, form.menuTimeoutLabel, CommandCategory.Configuration);
            ConnectListControl(DuoCommands.FrontPanelBrightnessCommand, form.frontPanelBrightnessCombo, form.frontPanelBrightnessLabel, CommandCategory.Configuration);
            ConnectBooleanControl(DuoCommands.OSDInputIndicatorCommand, form.osdInputIndicatorCheck, form.osdInputIndicatorLabel, CommandCategory.Configuration);
        }

        private void ConnectInputLabelControls(MainForm form)
        {
            ConnectStringControl(DuoCommands.Video1InputLabelCommand, form.video1LabelText, form.video1LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.Video2InputLabelCommand, form.video2LabelText, form.video2LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.Video3InputLabelCommand, form.video3LabelText, form.video3LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.SVideoInputLabelCommand, form.svideoLabelText, form.svideoLabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.Component1InputLabelCommand, form.component1LabelText, form.component1LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.Component2InputLabelCommand, form.component2LabelText, form.component2LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI1InputLabelCommand, form.hdmi1LabelText, form.hdmi1LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI2InputLabelCommand, form.hdmi2LabelText, form.hdmi2LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI3InputLabelCommand, form.hdmi3LabelText, form.hdmi3LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI4InputLabelCommand, form.hdmi4LabelText, form.hdmi4LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI5InputLabelCommand, form.hdmi5LabelText, form.hdmi5LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI6InputLabelCommand, form.hdmi6LabelText, form.hdmi6LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI7InputLabelCommand, form.hdmi7LabelText, form.hdmi7LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.HDMI8InputLabelCommand, form.hdmi8LabelText, form.hdmi8LabelLabel, CommandCategory.Configuration);
            ConnectStringControl(DuoCommands.VGAInputLabelCommand, form.vgaLabelText, form.vgaLabelLabel, CommandCategory.Configuration);
        }

        private void ConnectRemoteControls(MainForm form)
        {
            ConnectListControl(DuoCommands.InfoScreenCommand, form.infoScreenCombo, form.infoScreenLabel, CommandCategory.Remote);
            ConnectListControl(DuoCommands.TestPatternCommand, form.testPatternCombo, form.testPatternLabel, CommandCategory.Remote);

            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 1, form.leftButton, CommandCategory.Remote);
            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 2, form.rightButton, CommandCategory.Remote);
            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 3, form.upButton, CommandCategory.Remote);
            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 4, form.downButton, CommandCategory.Remote);
            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 5, form.menuButton, CommandCategory.Remote);
            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 6, form.enterButton, CommandCategory.Remote);
            ConnectButtonControl(DuoCommands.RemoteButtonCommand, 7, form.exitButton, CommandCategory.Remote);
        }

        private void ConnectOtherControls(MainForm form)
        {
            ConnectStringControl(DuoCommands.ProductNameCommand, form.productNameText, form.productNameLabel, CommandCategory.Other);
            ConnectStringControl(DuoCommands.VersionNumberCommand, form.versionNumberText, form.versionNumberLabel, CommandCategory.Other);

            ConnectButtonControl(DuoCommands.PowerCommand, 0, form.offButton, CommandCategory.Other);
            ConnectButtonControl(DuoCommands.PowerCommand, 1, form.onButton, CommandCategory.Other);
            ConnectButtonControl(DuoCommands.ResetCommand, 0, form.resetButton, CommandCategory.Other);
            ConnectButtonControl(DuoCommands.FirmwareUpdateCommand, 0, form.firmwareUpdateButton, CommandCategory.Other);

            ConnectWarningListControl(DuoCommands.FactoryDefaultCommand, form.factoryDefaultCombo, form.factoryDefaultLabel, CommandCategory.Other,
                "This will reset the '{0}' settings to their factory defaults. Are you sure?", "Factory defaults", true);
        }

        private void ConnectCMSControls(MainForm form)
        {
            dayNightProfileConnector = ConnectListControl(DuoCommands.DayNightCommand, form.profileCombo, form.dayNightProfileLabel, CommandCategory.CMS);
            ConnectBooleanControl(DuoCommands.CMSBypassCommand, form.bypassCMSCheck, null, CommandCategory.CMS);

            ConnectMultiParameterControls(new DecimalCommand[][]
            {
                new DecimalCommand[]{ DuoCommands.UCRedxCommand, DuoCommands.UCRedyCommand },
                new DecimalCommand[]{ DuoCommands.UCGreenxCommand, DuoCommands.UCGreenyCommand },
                new DecimalCommand[]{ DuoCommands.UCBluexCommand, DuoCommands.UCBlueyCommand },
                new DecimalCommand[]{ DuoCommands.UCWhitexCommand, DuoCommands.UCWhiteyCommand }
            }, new NumericUpDown[][]
            {
                new NumericUpDown[]{ form.ucRedxSpinner, form.ucRedySpinner },
                new NumericUpDown[]{ form.ucGreenxSpinner, form.ucGreenySpinner },
                new NumericUpDown[]{ form.ucBluexSpinner, form.ucBlueySpinner },
                new NumericUpDown[]{ form.ucWhitexSpinner, form.ucWhiteySpinner }
            }, new Label[]
            {
                form.ucRedLabel, form.ucGreenLabel, form.ucBlueLabel, form.ucWhiteLabel
            }, new Label[]
            {
                form.ucxLabel, form.ucyLabel
            }, CommandCategory.CMS);

            colorGamutConnectors = new IConnector[2][][];
            colorGamutConnectors[0] = ConnectMultiParameterControls(new DecimalCommand[][]
            {
                new DecimalCommand[]{ DuoCommands.CGRedxCommand, DuoCommands.CGRedyCommand, DuoCommands.CGRedcYCommand },
                new DecimalCommand[]{ DuoCommands.CGGreenxCommand, DuoCommands.CGGreenyCommand, DuoCommands.CGGreencYCommand },
                new DecimalCommand[]{ DuoCommands.CGBluexCommand, DuoCommands.CGBlueyCommand, DuoCommands.CGBluecYCommand }
            }, new NumericUpDown[][]
            {
                new NumericUpDown[]{ form.cgRedxSpinner, form.cgRedySpinner, form.cgRedcYSpinner },
                new NumericUpDown[]{ form.cgGreenxSpinner, form.cgGreenySpinner, form.cgGreencYSpinner },
                new NumericUpDown[]{ form.cgBluexSpinner, form.cgBlueySpinner, form.cgBluecYSpinner }
            }, new Label[]
            {
                form.cgRedLabel, form.cgGreenLabel, form.cgBlueLabel
            }, new Label[]
            {
                form.cgx1Label, form.cgy1Label, form.cgcY1Label
            }, CommandCategory.CMS);

            colorGamutConnectors[1] = ConnectMultiParameterControls(new DecimalCommand[][]
            {
                new DecimalCommand[]{ DuoCommands.CGCyanxCommand, DuoCommands.CGCyanyCommand, DuoCommands.CGCyancYCommand },
                new DecimalCommand[]{ DuoCommands.CGMagentaxCommand, DuoCommands.CGMagentayCommand, DuoCommands.CGMagentacYCommand },
                new DecimalCommand[]{ DuoCommands.CGYellowxCommand, DuoCommands.CGYellowyCommand, DuoCommands.CGYellowcYCommand },
                new DecimalCommand[]{ DuoCommands.CGWhitexCommand, DuoCommands.CGWhiteyCommand, DuoCommands.CGWhitecYCommand }
            }, new NumericUpDown[][]
            {
                new NumericUpDown[]{ form.cgCyanxSpinner, form.cgCyanySpinner, form.cgCyancYSpinner },
                new NumericUpDown[]{ form.cgMagentaxSpinner, form.cgMagentaySpinner, form.cgMagentacYSpinner },
                new NumericUpDown[]{ form.cgYellowxSpinner, form.cgYellowySpinner, form.cgYellowcYSpinner },
                new NumericUpDown[]{ form.cgWhitexSpinner, form.cgWhiteySpinner, form.cgWhitecYSpinner }
            }, new Label[]
            {
                form.cgCyanLabel, form.cgMagentaLabel, form.cgYellowLabel, form.cgWhiteLabel
            }, new Label[]
            {
                form.cgx2Label, form.cgy2Label, form.cgcY2Label
            }, CommandCategory.CMS);

            grayscaleConnectors = ConnectMultiParameterControls(new DecimalCommand[][]
            {
                new DecimalCommand[]{ DuoCommands.GRed0Command, DuoCommands.GRed10Command, DuoCommands.GRed20Command, DuoCommands.GRed30Command, DuoCommands.GRed40Command, DuoCommands.GRed50Command, DuoCommands.GRed60Command, DuoCommands.GRed70Command, DuoCommands.GRed80Command, DuoCommands.GRed90Command, DuoCommands.GRed100Command },
                new DecimalCommand[]{ DuoCommands.GGreen0Command, DuoCommands.GGreen10Command, DuoCommands.GGreen20Command, DuoCommands.GGreen30Command, DuoCommands.GGreen40Command, DuoCommands.GGreen50Command, DuoCommands.GGreen60Command, DuoCommands.GGreen70Command, DuoCommands.GGreen80Command, DuoCommands.GGreen90Command, DuoCommands.GGreen100Command },
                new DecimalCommand[]{ DuoCommands.GBlue0Command, DuoCommands.GBlue10Command, DuoCommands.GBlue20Command, DuoCommands.GBlue30Command, DuoCommands.GBlue40Command, DuoCommands.GBlue50Command, DuoCommands.GBlue60Command, DuoCommands.GBlue70Command, DuoCommands.GBlue80Command, DuoCommands.GBlue90Command, DuoCommands.GBlue100Command },
                new DecimalCommand[]{ DuoCommands.GWhitex0Command, DuoCommands.GWhitex10Command, DuoCommands.GWhitex20Command, DuoCommands.GWhitex30Command, DuoCommands.GWhitex40Command, DuoCommands.GWhitex50Command, DuoCommands.GWhitex60Command, DuoCommands.GWhitex70Command, DuoCommands.GWhitex80Command, DuoCommands.GWhitex90Command, DuoCommands.GWhitex100Command },
                new DecimalCommand[]{ DuoCommands.GWhitey0Command, DuoCommands.GWhitey10Command, DuoCommands.GWhitey20Command, DuoCommands.GWhitey30Command, DuoCommands.GWhitey40Command, DuoCommands.GWhitey50Command, DuoCommands.GWhitey60Command, DuoCommands.GWhitey70Command, DuoCommands.GWhitey80Command, DuoCommands.GWhitey90Command, DuoCommands.GWhitey100Command },
                new DecimalCommand[]{ DuoCommands.GWhitecY0Command, DuoCommands.GWhitecY10Command, DuoCommands.GWhitecY20Command, DuoCommands.GWhitecY30Command, DuoCommands.GWhitecY40Command, DuoCommands.GWhitecY50Command, DuoCommands.GWhitecY60Command, DuoCommands.GWhitecY70Command, DuoCommands.GWhitecY80Command, DuoCommands.GWhitecY90Command, DuoCommands.GWhitecY100Command }
            }, new NumericUpDown[][]
            {
                new NumericUpDown[]{ form.gRed0Spinner, form.gRed10Spinner, form.gRed20Spinner, form.gRed30Spinner, form.gRed40Spinner, form.gRed50Spinner, form.gRed60Spinner, form.gRed70Spinner, form.gRed80Spinner, form.gRed90Spinner, form.gRed100Spinner },
                new NumericUpDown[]{ form.gGreen0Spinner, form.gGreen10Spinner, form.gGreen20Spinner, form.gGreen30Spinner, form.gGreen40Spinner, form.gGreen50Spinner, form.gGreen60Spinner, form.gGreen70Spinner, form.gGreen80Spinner, form.gGreen90Spinner, form.gGreen100Spinner },
                new NumericUpDown[]{ form.gBlue0Spinner, form.gBlue10Spinner, form.gBlue20Spinner, form.gBlue30Spinner, form.gBlue40Spinner, form.gBlue50Spinner, form.gBlue60Spinner, form.gBlue70Spinner, form.gBlue80Spinner, form.gBlue90Spinner, form.gBlue100Spinner },
                new NumericUpDown[]{ form.gWhitex0Spinner, form.gWhitex10Spinner, form.gWhitex20Spinner, form.gWhitex30Spinner, form.gWhitex40Spinner, form.gWhitex50Spinner, form.gWhitex60Spinner, form.gWhitex70Spinner, form.gWhitex80Spinner, form.gWhitex90Spinner, form.gWhitex100Spinner },
                new NumericUpDown[]{ form.gWhitey0Spinner, form.gWhitey10Spinner, form.gWhitey20Spinner, form.gWhitey30Spinner, form.gWhitey40Spinner, form.gWhitey50Spinner, form.gWhitey60Spinner, form.gWhitey70Spinner, form.gWhitey80Spinner, form.gWhitey90Spinner, form.gWhitey100Spinner },
                new NumericUpDown[]{ form.gWhitecY0Spinner, form.gWhitecY10Spinner, form.gWhitecY20Spinner, form.gWhitecY30Spinner, form.gWhitecY40Spinner, form.gWhitecY50Spinner, form.gWhitecY60Spinner, form.gWhitecY70Spinner, form.gWhitecY80Spinner, form.gWhitecY90Spinner, form.gWhitecY100Spinner }
            }, new Label[]
            {
                form.gRedLabel, form.gGreenLabel, form.gBlueLabel, form.gWhitexLabel, form.gWhiteyLabel, form.gWhitecYLabel
            }, new Label[]
            {
                form.g0Label, form.g10Label, form.g20Label, form.g30Label, form.g40Label, form.g50Label, form.g60Label, form.g70Label, form.g80Label, form.g90Label, form.g100Label
            }, CommandCategory.CMS);
        }

        private ListConnector ConnectListControl(ListCommand command, ComboBox control, Label label, CommandCategory category)
        {
            return new ListConnector(command, control, label, category);
        }

        private WarningListConnector ConnectWarningListControl(ListCommand command, ComboBox control, Label label, CommandCategory category, string message, string caption, bool clearSelectedItem)
        {
            return new WarningListConnector(command, control, label, category, message, caption, clearSelectedItem);
        }

        private BooleanConnector ConnectBooleanControl(BooleanCommand command, CheckBox control, Label label, CommandCategory category)
        {
            return new BooleanConnector(command, control, label, category);
        }

        private DecimalConnector ConnectDecimalControl(DecimalCommand command, NumericUpDown control, Label label, CommandCategory category)
        {
            return new DecimalConnector(command, control, label, category);
        }

        private RowColumnDecimalConnector[][] ConnectMultiParameterControls(DecimalCommand[][] commands, NumericUpDown[][] controls, Label[] rowLabels, Label[] columnLabels, CommandCategory category)
        {
            RowColumnDecimalConnector[][] connectors = new RowColumnDecimalConnector[rowLabels.Length][];
            for(int y = 0; y < rowLabels.Length; y++)
            {
                connectors[y] = new RowColumnDecimalConnector[columnLabels.Length];
                for(int x = 0; x < columnLabels.Length; x++)
                {
                    connectors[y][x] = ConnectMultiParameterControl(commands[y][x], controls[y][x], rowLabels[y], columnLabels[x], category);
                }
            }
            return connectors;
        }

        private RowColumnDecimalConnector ConnectMultiParameterControl(DecimalCommand command, NumericUpDown control, Label rowLabel, Label columnLabel, CommandCategory category)
        {
            return new RowColumnDecimalConnector(command, control, rowLabel, columnLabel, category);
        }

        private StringConnector ConnectStringControl(StringCommand command, TextBox control, Label label, CommandCategory category)
        {
            return new StringConnector(command, control, label, category);
        }

        private void ConnectButtonControl(ListCommand command, int v, Button button, CommandCategory category)
        {
            ListValue value = command.ListValues.ValueToValue(v);
            if(value != null)
            {
                new ButtonConnector(command, value, button, category);
            }
        }

        private void ConnectDependants()
        {
            horizontalStretchConnector.AddDependantConnector(horizontalShiftConnector);
            verticalStretchConnector.AddDependantConnector(verticalShiftConnector);
            verticalShiftConnector.AddDependantConnector(verticalShiftConnector);
            horizontalShiftConnector.AddDependantConnector(horizontalShiftConnector);

            for (int z = 0; z < colorGamutConnectors.Length; z++)
            {
                for (int y = 0; y < colorGamutConnectors[z].Length; y++)
                {
                    for (int x = 0; x < colorGamutConnectors[z][y].Length; x++)
                    {
                        dayNightProfileConnector.AddDependantConnector(colorGamutConnectors[z][y][x]);
                    }
                }
            }

            for (int x = 0; x < grayscaleConnectors[0].Length; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    dayNightProfileConnector.AddDependantConnector(grayscaleConnectors[y][x]);
                    grayscaleConnectors[y][x].AddDependantConnector(grayscaleConnectors[3][x]);
                    grayscaleConnectors[y][x].AddDependantConnector(grayscaleConnectors[4][x]);
                    grayscaleConnectors[y][x].AddDependantConnector(grayscaleConnectors[5][x]);
                }
                for (int y = 3; y < 6; y++)
                {
                    dayNightProfileConnector.AddDependantConnector(grayscaleConnectors[y][x]);
                    grayscaleConnectors[y][x].AddDependantConnector(grayscaleConnectors[0][x]);
                    grayscaleConnectors[y][x].AddDependantConnector(grayscaleConnectors[1][x]);
                    grayscaleConnectors[y][x].AddDependantConnector(grayscaleConnectors[2][x]);
                }
            }

            foreach (IConnector connector in inputConnectors)
            {
                inputSelectConnector.AddDependantConnector(connector);
            }

            autoARConnector.AddDependantConnector(presetARConnector);
            autoARConnector.AddDependantConnector(activeARConnector);
            autoARConnector.AddDependantConnector(pictureARConnector);
            autoARConnector.AddDependantConnector(panoramaARConnector);
            
            presetARConnector.AddDependantConnector(autoARConnector);
            presetARConnector.AddDependantConnector(activeARConnector);
            presetARConnector.AddDependantConnector(pictureARConnector);
            presetARConnector.AddDependantConnector(panoramaARConnector);

            activeARConnector.AddDependantConnector(autoARConnector);
            activeARConnector.AddDependantConnector(panoramaARConnector);

            pictureARConnector.AddDependantConnector(autoARConnector);
            panoramaARConnector.AddDependantConnector(autoARConnector);

            autoARConnector.EnableableCheckValue = false;
            autoARConnector.AddEnableableConnector(presetARConnector);
            autoARConnector.AddEnableableConnector(activeARConnector);
            autoARConnector.AddEnableableConnector(pictureARConnector);
            autoARConnector.AddEnableableConnector(panoramaARConnector);
        }
    }
}
