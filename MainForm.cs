using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Profiler.UI;
using Profiler.Serial;
using Profiler.Logging;
using System.IO;
using Profiler.Protocol;
using Profiler.Conversion;
using System.IO.Ports;
using Profiler.Settings;
using System.Xml.Serialization;

namespace Profiler
{
    public partial class MainForm : Form, ICommunicatorListener
    {
        private bool changingTestPattern = false;
        private bool communicationInProgress;
        private ColorMatrix colorMatrix;
        private ColorMatrix colorInverse;
        private bool changingCTP = false;
        private bool selectingSavedColor = false;
        private Size initialFormSize;
        private static MainForm currentForm;

        public MainForm()
        {
            Context.Reset();
            InitializeComponent();
            Context.Logger = new ListLogger(log);
            Context.Logger.Info("Application started");
            currentForm = this;
            this.Disposed += new EventHandler(MainForm_Disposed);
            RefreshSavedColors();
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            currentForm = null;
            UserSettings.Save();
        }

        public static MainForm Start(SerialPort port)
        {
            if (currentForm != null)
            {
                return currentForm;
            }
            MainForm form = new MainForm();
            form.disconnectMenuItem.Visible = false;
            form.connectMenuItem.Visible = false;
            form.Show();
            Context.Communicator.Connect(port);
            form.EnableControlsForConnection(Context.Communicator.IsConnected());
            form.SetupStatusLabelForConnection(Context.Communicator.IsConnected());
            Context.ConnectorRegistry.QueryAllCommands();
            return form;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Context.Communicator.AddListener(this);
            Context.ControlLinker.ConnectControls(this);
            EnableControlsForConnection(false);
            SetupStatusLabelForConnection(false);
            SetLogLevel(Context.Logger.LogLevel);

            testPatternCombo2.Items.Clear();
            foreach (object item in testPatternCombo.Items)
                testPatternCombo2.Items.Add(item);
            testPatternCombo2.SelectedIndex = testPatternCombo.SelectedIndex;

            ConnectAutomaticTestPatternControls();
            SetupIncrements();
            SetupCTPPanel();

            initialFormSize = this.Size;
        }

        private void connectMenuItem_Click(object sender, EventArgs e)
        {
            PortSelector selector = new PortSelector();
            if (selector.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    Context.Communicator.Connect(selector.SelectedPortName, selector.SelectedBaudRate);
                    EnableControlsForConnection(Context.Communicator.IsConnected());
                    SetupStatusLabelForConnection(Context.Communicator.IsConnected());
                    Context.ConnectorRegistry.QueryAllCommands();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error connecting to device: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void disconnectMenuItem_Click(object sender, EventArgs e)
        {
            EnableControlsForConnection(false);
            SetupStatusLabelForConnection(false);
            Context.Communicator.CancelQueuedCommands();
            Context.Communicator.Disconnect();

            //queue the color reset in the main thread as an Invoke, so that it happens after the other control color operations from the Communicator
            MethodInvoker invoker = new MethodInvoker(delegate()
                {
                    Context.ConnectorRegistry.ClearAllControlColors();
                });
            this.BeginInvoke(invoker);
        }

        private void refreshMenuItem_Click(object sender, EventArgs e)
        {
            Context.ConnectorRegistry.QueryAllCommands();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void inputMenuItem_Click(object sender, EventArgs e)
        {
            inputMenuItem.Checked = Context.ConnectorRegistry.ToggleArmedStateForCategory(CommandCategory.Input);
        }

        private void aspectRatioMenuItem_Click(object sender, EventArgs e)
        {
            aspectRatioMenuItem.Checked = Context.ConnectorRegistry.ToggleArmedStateForCategory(CommandCategory.AspectRatio);
        }

        private void pictureMenuItem_Click(object sender, EventArgs e)
        {
            pictureMenuItem.Checked = Context.ConnectorRegistry.ToggleArmedStateForCategory(CommandCategory.Picture);
        }

        private void outputMenuItem_Click(object sender, EventArgs e)
        {
            outputMenuItem.Checked = Context.ConnectorRegistry.ToggleArmedStateForCategory(CommandCategory.Output);
        }

        private void configurationMenuItem_Click(object sender, EventArgs e)
        {
            configurationMenuItem.Checked = Context.ConnectorRegistry.ToggleArmedStateForCategory(CommandCategory.Configuration);
        }

        private void cmsMenuItem_Click(object sender, EventArgs e)
        {
            cmsMenuItem.Checked = Context.ConnectorRegistry.ToggleArmedStateForCategory(CommandCategory.CMS);
        }

        private void selectAllMenuItem_Click(object sender, EventArgs e)
        {
            Context.ConnectorRegistry.ArmAllCommands(true);
            CheckSaveMenuItems(true);
        }

        private void selectNoneMenuItem_Click(object sender, EventArgs e)
        {
            Context.ConnectorRegistry.ArmAllCommands(false);
            CheckSaveMenuItems(false);
        }

        private void CheckSaveMenuItems(bool check)
        {
            inputMenuItem.Checked = check;
            aspectRatioMenuItem.Checked = check;
            pictureMenuItem.Checked = check;
            outputMenuItem.Checked = check;
            configurationMenuItem.Checked = check;
            cmsMenuItem.Checked = check;
        }

        private void EnableControlsForConnection(bool connected)
        {
            connectMenuItem.Enabled = !connected;
            disconnectMenuItem.Enabled = connected;
            saveMenuItem.Enabled = connected;
            saveSelectedMenuItem.Enabled = connected;
            openMenuItem.Enabled = connected;
            backupCMSMenuItem.Enabled = connected;
            refreshMenuItem.Enabled = connected;

            copyProfileButton.Enabled = connected;
            testPatternCombo2.Enabled = connected;
            previousTestPatternButton.Enabled = connected;
            nextTestPatternButton.Enabled = connected;
            automaticTestPatternCheck.Enabled = connected;

            Context.ConnectorRegistry.EnableCommandControls(connected);

            EnableControlsInGroup(ctpColorGroup, connected, false);
            EnableControlsInGroup(ctpSizeGroup, connected, false);
            EnableControlsInGroup(ctpEnableGroup, connected, false);
            EnableControlsInGroup(ctpParametersGroup, connected, false);
            EnableControlsInGroup(ctpMatricesGroup, connected, false);
            UpdateCTPButtons(connected);
            UpdateRemoveButtonEnabled();
        }

        private string LastUsedDirectory
        {
            get
            {
                object o = Application.UserAppDataRegistry.GetValue("LastUsedDirectory");
                if (o != null && o is string)
                    return (string)o;
                return null;
            }
            set
            {
                Application.UserAppDataRegistry.SetValue("LastUsedDirectory", value);
            }
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (communicationInProgress)
            {
                if (MessageBox.Show(this, "Communication with the Duo is still in progress. It is recommended that you wait until it's finished before you load a configuration. Are you sure you want to continue loading a configuration?",
                    "Communication in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML file|*.xml";
            dialog.Title = "Load configuration";
            dialog.InitialDirectory = LastUsedDirectory;

            if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != null && dialog.FileName.Length > 0)
            {
                LastUsedDirectory = Path.GetDirectoryName(dialog.FileName);
                try
                {
                    Context.ConnectorRegistry.LoadFromFile(dialog.FileName);
                    RefreshArmCheckState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not load from file: " + ex.Message, "Load error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        private void saveSelectedMenuItem_Click(object sender, EventArgs e)
        {
            Save(false);
        }

        private void Save(bool all)
        {
            if (communicationInProgress)
            {
                if (MessageBox.Show(this, "Communication with the Duo is still in progress. It is recommended that you wait until it's finished before you save the configuration. Are you sure you want to continue saving the configuration?",
                    "Communication in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML file|*.xml";
            dialog.Title = "Save configuration";
            dialog.InitialDirectory = LastUsedDirectory;

            if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != null && dialog.FileName.Length > 0)
            {
                LastUsedDirectory = Path.GetDirectoryName(dialog.FileName);
                try
                {
                    Context.ConnectorRegistry.SaveToFile(dialog.FileName, !all);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not save to file: " + ex.Message, "Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backupCMSMenuItem_Click(object sender, EventArgs e)
        {
            if (communicationInProgress)
            {
                if (MessageBox.Show(this, "Communication with the Duo is still in progress. It is recommended that you wait until it's finished before you save the configuration. Are you sure you want to continue saving the configuration?",
                    "Communication in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML file|*.xml";
            dialog.Title = "Backup configuration";
            dialog.InitialDirectory = LastUsedDirectory;

            if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != null && dialog.FileName.Length > 0)
            {
                LastUsedDirectory = Path.GetDirectoryName(dialog.FileName);
                try
                {
                    tabControl.SelectedTab = cmsTab;
                    Backup backup = new Backup();
                    backup.PerformBackup(this, dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not save to file: " + ex.Message, "Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshArmCheckState()
        {
            inputMenuItem.Checked = Context.ConnectorRegistry.AllCommandsArmedForCategory(CommandCategory.Input);
            aspectRatioMenuItem.Checked = Context.ConnectorRegistry.AllCommandsArmedForCategory(CommandCategory.AspectRatio);
            pictureMenuItem.Checked = Context.ConnectorRegistry.AllCommandsArmedForCategory(CommandCategory.Picture);
            outputMenuItem.Checked = Context.ConnectorRegistry.AllCommandsArmedForCategory(CommandCategory.Output);
            configurationMenuItem.Checked = Context.ConnectorRegistry.AllCommandsArmedForCategory(CommandCategory.Configuration);
            cmsMenuItem.Checked = Context.ConnectorRegistry.AllCommandsArmedForCategory(CommandCategory.CMS);
        }

        private void logFineMenuItem_Click(object sender, EventArgs e)
        {
            SetLogLevel(Logger.Level.Fine);
        }

        private void logInfoMenuItem_Click(object sender, EventArgs e)
        {
            SetLogLevel(Logger.Level.Info);
        }

        private void logWarningMenuItem_Click(object sender, EventArgs e)
        {
            SetLogLevel(Logger.Level.Warning);
        }

        private void logErrorMenuItem_Click(object sender, EventArgs e)
        {
            SetLogLevel(Logger.Level.Error);
        }

        private void SetLogLevel(Logger.Level level)
        {
            logFineMenuItem.Checked = level == Logger.Level.Fine;
            logInfoMenuItem.Checked = level == Logger.Level.Info;
            logWarningMenuItem.Checked = level == Logger.Level.Warning;
            logErrorMenuItem.Checked = level == Logger.Level.Error;

            Context.Logger.LogLevel = level;
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void cmsTab_MouseUp(object sender, MouseEventArgs e)
        {
            if (userChromaticityGroup.Bounds.Contains(e.Location))
            {
                MessageBox.Show(this, "The User Chromaticity settings are disabled because the current iScan Duo firmware seems to have a bug: the device freezes every time a User Chromaticity parameter is queried or set.\n\nFeel free to report this bug to Anchor Bay.",
                    "User Chromaticity disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void testPatternCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            changingTestPattern = true;
            testPatternCombo2.SelectedItem = testPatternCombo.SelectedItem;
            UpdateCTPButtons(true);
            changingTestPattern = false;
        }

        private void testPatternCombo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changingTestPattern)
                return;
            testPatternCombo.SelectedItem = testPatternCombo2.SelectedItem;
        }

        private void previousTestPatternButton_Click(object sender, EventArgs e)
        {
            if (testPatternCombo2.SelectedIndex > 0)
                testPatternCombo2.SelectedIndex--;
            else
                testPatternCombo2.SelectedIndex = testPatternCombo2.Items.Count - 1;
        }

        private void nextTestPatternButton_Click(object sender, EventArgs e)
        {
            if (testPatternCombo2.SelectedIndex < testPatternCombo2.Items.Count - 1)
                testPatternCombo2.SelectedIndex++;
            else
                testPatternCombo2.SelectedIndex = 0;
        }

        private void ConnectAutomaticTestPatternControls()
        {
            ConnectControlsToTestPattern(28, new NumericUpDown[] { cgWhitexSpinner, cgWhiteySpinner, cgWhitecYSpinner });
            ConnectControlsToTestPattern(29, new NumericUpDown[] { cgRedxSpinner, cgRedySpinner, cgRedcYSpinner });
            ConnectControlsToTestPattern(30, new NumericUpDown[] { cgGreenxSpinner, cgGreenySpinner, cgGreencYSpinner });
            ConnectControlsToTestPattern(31, new NumericUpDown[] { cgBluexSpinner, cgBlueySpinner, cgBluecYSpinner });
            ConnectControlsToTestPattern(32, new NumericUpDown[] { cgCyanxSpinner, cgCyanySpinner, cgCyancYSpinner });
            ConnectControlsToTestPattern(33, new NumericUpDown[] { cgMagentaxSpinner, cgMagentaySpinner, cgMagentacYSpinner });
            ConnectControlsToTestPattern(34, new NumericUpDown[] { cgYellowxSpinner, cgYellowySpinner, cgYellowcYSpinner });

            ConnectControlsToTestPattern(35, new NumericUpDown[] { gRed0Spinner, gGreen0Spinner, gBlue0Spinner, gWhitex0Spinner, gWhitey0Spinner, gWhitecY0Spinner });
            ConnectControlsToTestPattern(9, new NumericUpDown[] { gRed10Spinner, gGreen10Spinner, gBlue10Spinner, gWhitex10Spinner, gWhitey10Spinner, gWhitecY10Spinner });
            ConnectControlsToTestPattern(10, new NumericUpDown[] { gRed20Spinner, gGreen20Spinner, gBlue20Spinner, gWhitex20Spinner, gWhitey20Spinner, gWhitecY20Spinner });
            ConnectControlsToTestPattern(11, new NumericUpDown[] { gRed30Spinner, gGreen30Spinner, gBlue30Spinner, gWhitex30Spinner, gWhitey30Spinner, gWhitecY30Spinner });
            ConnectControlsToTestPattern(12, new NumericUpDown[] { gRed40Spinner, gGreen40Spinner, gBlue40Spinner, gWhitex40Spinner, gWhitey40Spinner, gWhitecY40Spinner });
            ConnectControlsToTestPattern(13, new NumericUpDown[] { gRed50Spinner, gGreen50Spinner, gBlue50Spinner, gWhitex50Spinner, gWhitey50Spinner, gWhitecY50Spinner });
            ConnectControlsToTestPattern(14, new NumericUpDown[] { gRed60Spinner, gGreen60Spinner, gBlue60Spinner, gWhitex60Spinner, gWhitey60Spinner, gWhitecY60Spinner });
            ConnectControlsToTestPattern(15, new NumericUpDown[] { gRed70Spinner, gGreen70Spinner, gBlue70Spinner, gWhitex70Spinner, gWhitey70Spinner, gWhitecY70Spinner });
            ConnectControlsToTestPattern(16, new NumericUpDown[] { gRed80Spinner, gGreen80Spinner, gBlue80Spinner, gWhitex80Spinner, gWhitey80Spinner, gWhitecY80Spinner });
            ConnectControlsToTestPattern(17, new NumericUpDown[] { gRed90Spinner, gGreen90Spinner, gBlue90Spinner, gWhitex90Spinner, gWhitey90Spinner, gWhitecY90Spinner });
            ConnectControlsToTestPattern(18, new NumericUpDown[] { gRed100Spinner, gGreen100Spinner, gBlue100Spinner, gWhitex100Spinner, gWhitey100Spinner, gWhitecY100Spinner });
        }

        private void ConnectControlsToTestPattern(int testPattern, NumericUpDown[] controls)
        {
            ListValue value = DuoListValues.TestPatternValues.ValueToValue(testPattern);
            if (value != null)
            {
                foreach (NumericUpDown control in controls)
                {
                    control.GotFocus += delegate(object sender, EventArgs args)
                    {
                        if (automaticTestPatternCheck.Checked)
                        {
                            testPatternCombo2.SelectedItem = value;
                        }
                    };
                }
            }
        }

        private void SetupIncrements()
        {
            decimal increment = 0.0001M;
            if (thousandthIncrementRadio.Checked)
                increment = 0.001M;
            else if (hundrethIncrementRadio.Checked)
                increment = 0.01M;
            else if (tenthIncrementRadio.Checked)
                increment = 0.1M;
            else if (oneIncrementRadio.Checked)
                increment = 1M;

            IList<IConnector> cmsConnectors = Context.ConnectorRegistry.ConnectorsInCategory(CommandCategory.CMS);
            if (cmsConnectors != null)
            {
                foreach (IConnector connector in cmsConnectors)
                {
                    if (connector is DecimalConnector)
                    {
                        NumericUpDown nud = (connector as DecimalConnector).Control;
                        nud.Increment = increment;
                    }
                }
            }
        }

        private void tenThousandthIncrementRadio_CheckedChanged(object sender, EventArgs e)
        {
            SetupIncrements();
        }

        private void thousandthIncrementRadio_CheckedChanged(object sender, EventArgs e)
        {
            SetupIncrements();
        }

        private void hundrethIncrementRadio_CheckedChanged(object sender, EventArgs e)
        {
            SetupIncrements();
        }

        private void tenthIncrementRadio_CheckedChanged(object sender, EventArgs e)
        {
            SetupIncrements();
        }

        private void oneIncrementRadio_CheckedChanged(object sender, EventArgs e)
        {
            SetupIncrements();
        }

        private void dayNightProfileCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListValue value = (ListValue)dayNightProfileCombo.SelectedItem;
            if (value != null)
            {
                if (value.Value == 0) //day
                {
                    copyProfileButton.Text = "Copy settings to Night profile";
                }
                else if (value.Value == 1) //night
                {
                    copyProfileButton.Text = "Copy settings to Day profile";
                }
            }
        }

        private void copyProfileButton_Click(object sender, EventArgs e)
        {
            ListValue value = (ListValue)dayNightProfileCombo.SelectedItem;
            if (value == null)
                return;

            bool day = value.Value == 0;
            bool night = value.Value == 1;
            if (!(day || night))
                return;

            string from = day ? "Day" : "Night";
            string to = day ? "Night" : "Day";
            DialogResult result = MessageBox.Show(this,
                "This will override all the CMS settings in the " + to + " profile with the values in the " + from + " profile. Are you sure you want to continue?",
                "Copy profile", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            IList<IConnector> cmsConnectors = Context.ConnectorRegistry.ConnectorsInCategory(CommandCategory.CMS);
            if (cmsConnectors != null)
            {
                IList<decimal> values = new List<decimal>();
                IList<DecimalConnector> connectors = new List<DecimalConnector>();
                foreach (IConnector connector in cmsConnectors)
                {
                    if (connector is DecimalConnector && connector.ICommand.IsSavable)
                    {
                        DecimalConnector dc = connector as DecimalConnector;
                        connectors.Add(dc);
                        values.Add(dc.CurrentControlValue());
                    }
                }

                //swap the profile
                dayNightProfileCombo.SelectedItem = DuoListValues.DayNightProfileValues.ValueToValue(day ? 1 : 0);
                //plug in the old values
                for (int i = 0; i < values.Count; i++)
                {
                    connectors[i].SetControlValue(values[i]);
                }
            }
        }

        private void SetupStatusLabelForConnection(bool connected)
        {
            statusLabel.Text = connected ? "Ready" : "Disconnected";
        }

        public void CommunicationBegun()
        {
            communicationInProgress = true;

            MethodInvoker invoker = new MethodInvoker(delegate()
                {
                    statusLabel.Text = "Communicating...";
                });

            if (this.InvokeRequired)
                this.BeginInvoke(invoker);
            else
                invoker.Invoke();
        }

        public void CommunicationComplete()
        {
            communicationInProgress = false;

            MethodInvoker invoker = new MethodInvoker(delegate()
            {
                statusLabel.Text = Context.Communicator.IsConnected() ? "Ready" : "Disconnected";
            });

            if (this.InvokeRequired)
                this.BeginInvoke(invoker);
            else
                invoker.Invoke();
        }

        private void SetupCTPPanel()
        {
            NumericUpDown[] primaries = new NumericUpDown[] { ctpWx, ctpWy, ctpRx, ctpRy, ctpGx, ctpGy, ctpBx, ctpBy };
            foreach (NumericUpDown primary in primaries)
            {
                primary.ValueChanged += new EventHandler(primary_ValueChanged);
            }

            ctpColor.Items.AddRange(LabeledColorVector.Values);
            ctpColor.SelectedIndex = 0;

            ctpPresets.CheckedChanged += new EventHandler(ctpRadio_ValueChanged);
            ctpManual.CheckedChanged += new EventHandler(ctpRadio_ValueChanged);

            ctpGamma.ValueChanged += new EventHandler(ctpGeneral_ValueChanged);
            ctpLuminance.ValueChanged += new EventHandler(ctpGeneral_ValueChanged);
            ctpSaturation.ValueChanged += new EventHandler(ctpGeneral_ValueChanged);
            ctpColor.SelectedValueChanged += new EventHandler(ctpGeneral_ValueChanged);

            ctpL100.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpL75.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpL50.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpL25.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpS100.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpS75.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpS50.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpS25.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpW100.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpW75.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpW50.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpW25.Click += new EventHandler(SLQuickChangeButton_Click);
            ctpW10.Click += new EventHandler(SLQuickChangeButton_Click);

            ctpRed.ValueChanged += new EventHandler(ctpRGB_ValueChanged);
            ctpGreen.ValueChanged += new EventHandler(ctpRGB_ValueChanged);
            ctpBlue.ValueChanged += new EventHandler(ctpRGB_ValueChanged);
            ctpXYZX.ValueChanged += new EventHandler(ctpXYZ_ValueChanged);
            ctpXYZY.ValueChanged += new EventHandler(ctpXYZ_ValueChanged);
            ctpXYZZ.ValueChanged += new EventHandler(ctpXYZ_ValueChanged);
            ctpxyYx.ValueChanged += new EventHandler(ctpxyY_ValueChanged);
            ctpxyYy.ValueChanged += new EventHandler(ctpxyY_ValueChanged);
            ctpxyYcY.ValueChanged += new EventHandler(ctpxyY_ValueChanged);

            UpdateMatrices();
            ctpPresets.Checked = true;
            ctpRadio_ValueChanged(null, null);
        }

        private void SLQuickChangeButton_Click(object sender, EventArgs e)
        {
            if (sender == ctpL100) ctpLuminance.Value = 100;
            if (sender == ctpL75) ctpLuminance.Value = 75;
            if (sender == ctpL50) ctpLuminance.Value = 50;
            if (sender == ctpL25) ctpLuminance.Value = 25;
            if (sender == ctpS100) ctpSaturation.Value = 100;
            if (sender == ctpS75) ctpSaturation.Value = 75;
            if (sender == ctpS50) ctpSaturation.Value = 50;
            if (sender == ctpS25) ctpSaturation.Value = 25;
            if (sender == ctpW100) ctpSize.Value = 100;
            if (sender == ctpW75) ctpSize.Value = 75;
            if (sender == ctpW50) ctpSize.Value = 50;
            if (sender == ctpW25) ctpSize.Value = 25;
            if (sender == ctpW10) ctpSize.Value = 10;
        }

        private void ctpRadio_ValueChanged(object sender, EventArgs e)
        {
            EnableControlsInGroup(ctpPresetsGroup, ctpPresets.Checked, false);
            EnableControlsInGroup(ctpManualGroup, ctpManual.Checked, true);
            UpdateCustomTestPattern(false, false, false);
        }

        private void EnableControlsInGroup(GroupBox group, bool enable, bool useReadonlyForNumericUpDown)
        {
            foreach (Control control in group.Controls)
            {
                if (control is GroupBox)
                {
                }
                if (useReadonlyForNumericUpDown && control is NumericUpDown)
                {
                    (control as NumericUpDown).ReadOnly = !enable;
                }
                else
                {
                    control.Enabled = enable;
                }
            }
        }

        private void ctpGeneral_ValueChanged(object sender, EventArgs e)
        {
            UpdateCustomTestPattern(false, false, false);
        }

        private void ctpRGB_ValueChanged(object sender, EventArgs e)
        {
            UpdateCustomTestPattern(false, false, true);
        }

        private void ctpXYZ_ValueChanged(object sender, EventArgs e)
        {
            UpdateCustomTestPattern(false, true, false);
        }

        private void ctpxyY_ValueChanged(object sender, EventArgs e)
        {
            UpdateCustomTestPattern(true, false, false);
        }

        private void primary_ValueChanged(object sender, EventArgs e)
        {
            UpdateMatrices();
            UpdateCustomTestPattern(false, false, false);
        }

        private void UpdateMatrices()
        {
            colorMatrix = ColorMatrix.FromPrimaries((double)ctpRx.Value, (double)ctpRy.Value, (double)ctpGx.Value, (double)ctpGy.Value, (double)ctpBx.Value, (double)ctpBy.Value, (double)ctpWx.Value, (double)ctpWy.Value);
            colorInverse = colorMatrix.Inverse();
            ctpMatrix.Text = colorMatrix.ToString();
            ctpMatrixInverse.Text = colorInverse.ToString();
        }

        private void UpdateCustomTestPattern(bool xyYChanged, bool xyzChanged, bool rgbChanged)
        {
            if (changingCTP)
            {
                return;
            }
            changingCTP = true;

            decimal r = ctpRed.Value;
            decimal g = ctpGreen.Value;
            decimal b = ctpBlue.Value;
            decimal cx = ctpxyYx.Value;
            decimal cy = ctpxyYy.Value;
            decimal cY = ctpxyYcY.Value;
            decimal X = ctpXYZX.Value;
            decimal Y = ctpXYZY.Value;
            decimal Z = ctpXYZZ.Value;
            double gamma = (double)ctpGamma.Value;

            ColorVector rgb = new ColorVector((double)r / 100.0, (double)g / 100.0, (double)b / 100.0);
            ColorVector xyY = new ColorVector((double)cx, (double)cy, (double)cY);
            ColorVector XYZ = new ColorVector((double)X, (double)Y, (double)Z);
            if (ctpPresets.Checked)
            {
                ColorVector hue = (ColorVector)ctpColor.SelectedItem;
                double luminance = (double)ctpLuminance.Value / 100.0;
                double saturation = (double)ctpSaturation.Value / 100.0;
                ColorConversion.fromHSL(hue, luminance, saturation, colorMatrix, colorInverse, gamma, out rgb, out xyY, out XYZ);
            }
            else if (xyzChanged)
            {
                ColorConversion.fromXYZ(XYZ, colorInverse, gamma, out rgb, out xyY);
            }
            else if (rgbChanged)
            {
                ColorConversion.fromRGB(rgb, colorMatrix, gamma, out xyY, out XYZ);
            }
            else //if (xyYChanged || true)
            {
                ColorConversion.fromxyY(xyY, colorInverse, gamma, out rgb, out XYZ);
            }

            try
            {
                r = (decimal)Math.Round(rgb.x * 100); g = (decimal)Math.Round(rgb.y * 100); b = (decimal)Math.Round(rgb.z * 100);
                cx = (decimal)xyY.x; cy = (decimal)xyY.y; cY = (decimal)xyY.z;
                X = (decimal)XYZ.x; Y = (decimal)XYZ.y; Z = (decimal)XYZ.z;
            }
            catch (Exception)
            {
                //ignore?
            }

            SetNumericUpDownWithinRange(ctpRed, r);
            SetNumericUpDownWithinRange(ctpGreen, g);
            SetNumericUpDownWithinRange(ctpBlue, b);
            SetNumericUpDownWithinRange(ctpxyYx, cx);
            SetNumericUpDownWithinRange(ctpxyYy, cy);
            SetNumericUpDownWithinRange(ctpxyYcY, cY);
            SetNumericUpDownWithinRange(ctpXYZX, X);
            SetNumericUpDownWithinRange(ctpXYZY, Y);
            SetNumericUpDownWithinRange(ctpXYZZ, Z);

            double min = 0;
            double max = 255;
            int ir = Math.Max(0, Math.Min(255, (int)(min + ((double)r / 100.0) * (max - min))));
            int ig = Math.Max(0, Math.Min(255, (int)(min + ((double)g / 100.0) * (max - min))));
            int ib = Math.Max(0, Math.Min(255, (int)(min + ((double)b / 100.0) * (max - min))));

            ctpPreview.BackColor = Color.FromArgb(ir, ig, ib);

            if (!selectingSavedColor)
            {
                ctpSavedColors.SelectedItem = null;
            }

            changingCTP = false;
        }

        private void SetNumericUpDownWithinRange(NumericUpDown control, decimal value)
        {
            control.Value = Math.Max(control.Minimum, Math.Min(control.Maximum, value));
        }

        private void ctpSend_Click(object sender, EventArgs e)
        {
            if (testPatternCombo.SelectedItem == DuoListValues.TestPatternValueOff)
            {
                testPatternCombo.SelectedItem = DuoListValues.TestPatternValueWhite;
            }

            CustomTestPatternCommand command = new CustomTestPatternCommand();
            int R = (int)ctpRed.Value;
            int G = (int)ctpGreen.Value;
            int B = (int)ctpBlue.Value;
            int S = (int)ctpSize.Value;
            int[] value = new int[] { R, G, B, S };
            Context.Communicator.SetValue(command, value, new CustomTestPatternCommand.NullListener(), 0);
        }

        private void ctpEnable_Click(object sender, EventArgs e)
        {
            if (testPatternCombo.SelectedItem != DuoListValues.TestPatternValueOff)
            {
                testPatternCombo.SelectedItem = DuoListValues.TestPatternValueOff;
            }
            else
            {
                testPatternCombo.SelectedItem = DuoListValues.TestPatternValueWhite;
            }
        }

        private void UpdateCTPButtons(bool connected)
        {
            ctpSend.Enabled = connected && testPatternCombo.SelectedItem != DuoListValues.TestPatternValueOff;
            ctpEnable.Text = testPatternCombo.SelectedItem == DuoListValues.TestPatternValueOff ? "Enable Test Pattern" : "Disable Test Pattern";
        }

        private void windowResizeMenuItem_Click(object sender, EventArgs e)
        {
            EnableWindowResize(windowResizeMenuItem.Checked);
        }

        private void EnableWindowResize(bool enable)
        {
            this.MaximizeBox = enable;
            if (!enable)
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = initialFormSize;
            }
            this.MinimumSize = enable ? Size.Empty : initialFormSize;
            this.MaximumSize = enable ? Size.Empty : new Size(initialFormSize.Width, initialFormSize.Height + 10000);
            windowResizeMenuItem.Checked = enable;
        }

        private void log_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    LogToClipboard();
                }
                else if (e.KeyCode == Keys.A)
                {
                    foreach (ListViewItem item in log.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void log_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (log.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void contextCopyItem_Click(object sender, EventArgs e)
        {
            LogToClipboard();
        }

        private void LogToClipboard()
        {
            if (log.SelectedItems.Count == 0)
                return;

            StringBuilder buffer = new StringBuilder();
            // Loop over all the selected items
            foreach (ListViewItem currentItem in log.SelectedItems)
            {
                // Don't need to look at currentItem, because it is in subitem[0]
                // So just loop over all the subitems of this selected item
                foreach (ListViewItem.ListViewSubItem sub in currentItem.SubItems)
                {
                    // Append the text and tab
                    buffer.Append(sub.Text);
                    buffer.Append("\t");
                }
                // Annoyance: there is a trailing tab in the buffer, get rid of it
                buffer.Remove(buffer.Length - 1, 1);
                // If you only use \n, not all programs (notepad!!!) will recognize the newline
                buffer.Append("\r\n");
            }
            // Set output to clipboard.
            Clipboard.SetDataObject(buffer.ToString());
        }

        private void showLog_Click(object sender, EventArgs e)
        {
            if (showLog.Text.ToLower().Contains("show"))
            {
                this.Height = initialFormSize.Height + 200;
            }
            else
            {
                this.Height = initialFormSize.Height;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Size currentSize = this.Size;
            if (currentSize.Height <= initialFormSize.Height + 10)
            {
                showLog.Text = "Show Log";
            }
            else
            {
                showLog.Text = "Hide Log";
            }
        }

        private void ctpAddButton_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(ctpAddButton, "Save this color");
        }

        private void ctpRemoveButton_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(ctpRemoveButton, "Delete selected color");
        }

        private void ctpImportButton_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(ctpImportButton, "Import colors from file...");
        }

        private void ctpExportButton_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(ctpExportButton, "Export colors to file...");
        }

        private void RefreshSavedColors()
        {
            ctpSavedColors.Items.Clear();
            ctpSavedColors.Items.AddRange(UserSettings.Instance.SavedColors.ToArray());
            UpdateRemoveButtonEnabled();
        }

        private void ctpAddButton_Click(object sender, EventArgs e)
        {
            decimal r = ctpRed.Value;
            decimal g = ctpGreen.Value;
            decimal b = ctpBlue.Value;
            decimal cx = ctpxyYx.Value;
            decimal cy = ctpxyYy.Value;
            decimal cY = ctpxyYcY.Value;
            decimal X = ctpXYZX.Value;
            decimal Y = ctpXYZY.Value;
            decimal Z = ctpXYZZ.Value;
            double gamma = (double)ctpGamma.Value;

            ColorVector rgb = new ColorVector((double)r / 100.0, (double)g / 100.0, (double)b / 100.0);
            ColorVector xyY = new ColorVector((double)cx, (double)cy, (double)cY);
            ColorVector XYZ = new ColorVector((double)X, (double)Y, (double)Z);

            SaveColor saveColor = new SaveColor(xyY, XYZ, rgb);
            if (saveColor.ShowDialog(this) == DialogResult.OK)
            {
                UserSettings.Instance.AddSavedColor(saveColor.Color);
                RefreshSavedColors();
                ctpSavedColors.SelectedItem = saveColor.Color;
            }
        }

        private void ctpRemoveButton_Click(object sender, EventArgs e)
        {
            if (ctpSavedColors.SelectedItem != null)
            {
                int index = ctpSavedColors.SelectedIndex;
                UserSettings.Instance.RemoveSavedColor((SavedColor)ctpSavedColors.SelectedItem);
                RefreshSavedColors();
                while(index > ctpSavedColors.Items.Count - 1)
                {
                    index--;
                }
                if (index >= 0)
                {
                    ctpSavedColors.SelectedIndex = index;
                }
            }
        }

        private void ctpSavedColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRemoveButtonEnabled();

            if (selectingSavedColor)
                return;

            selectingSavedColor = true;
            try
            {
                if (ctpSavedColors.SelectedItem != null)
                {
                    ctpManual.Checked = true;

                    SavedColor color = (SavedColor)ctpSavedColors.SelectedItem;
                    switch (color.Type)
                    {
                        case SavedColor.SavedColorTypeEnum.RGB:
                            ctpRed.Value = (decimal)(color.X * 100.0);
                            ctpGreen.Value = (decimal)(color.Y * 100.0);
                            ctpBlue.Value = (decimal)(color.Z * 100.0);
                            break;
                        case SavedColor.SavedColorTypeEnum.xyY:
                            ctpxyYx.Value = (decimal)color.X;
                            ctpxyYy.Value = (decimal)color.Y;
                            ctpxyYcY.Value = (decimal)color.Z;
                            break;
                        case SavedColor.SavedColorTypeEnum.XYZ:
                            ctpXYZX.Value = (decimal)color.X;
                            ctpXYZY.Value = (decimal)color.Y;
                            ctpXYZZ.Value = (decimal)color.Z;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            selectingSavedColor = false;
        }

        private void UpdateRemoveButtonEnabled()
        {
            ctpRemoveButton.Enabled = ctpSavedColors.SelectedItem != null;
        }

        private void ctpImportButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    SavedColor[] colors = ImportSavedColors(openFileDialog.FileName);
                    foreach (SavedColor color in colors)
                    {
                        UserSettings.Instance.AddSavedColor(color);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                RefreshSavedColors();
            }
        }

        private void ctpExportButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    ExportSavedColors(saveFileDialog.FileName, UserSettings.Instance.SavedColors.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static SavedColor[] ImportSavedColors(string file)
        {
            SavedColor[] colors = null;
            FileStream stream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SavedColor[]));
                FileInfo fi = new FileInfo(file);
                if (fi.Exists)
                {
                    stream = fi.OpenRead();
                    colors = (SavedColor[])serializer.Deserialize(stream);
                }
            }
            finally
            {
                if (stream != null) stream.Close();
            }
            return colors;
        }

        private static void ExportSavedColors(string file, SavedColor[] colors)
        {
                StreamWriter writer = null;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SavedColor[]));
                    writer = new StreamWriter(file, false);
                    serializer.Serialize(writer, colors);
                }
                finally
                {
                    if (writer != null) writer.Close();
                }
        }
    }
}
