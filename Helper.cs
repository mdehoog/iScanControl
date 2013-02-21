using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Qixle.iScanDuo.Controller.Protocol;
using Qixle.iScanDuo.Controller.Serial;
using System.Threading;
using Qixle.iScanDuo.Controller.Logging;

namespace Qixle.iScanDuo.Controller
{
    /// <summary>
    /// Helper class for ChromaPure.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Connect to the given serial port. The baud rate must match the value set in the Duo's settings. The port read and write timeouts should be appropriately set.
        /// </summary>
        /// <param name="port">Serial port to connect to</param>
        public void Connect(SerialPort port)
        {
            Context.Communicator.Connect(port);
        }

        /// <summary>
        /// Disconnect from the serial port. This does not close the port, but does cancel any commands queued.
        /// </summary>
        public void Disconnect()
        {
            Context.Communicator.CancelQueuedCommands();
            Context.Communicator.Disconnect();
        }

        /// <summary>
        /// The Logger to log messages to.
        /// </summary>
        public Logger Logger
        {
            get { return Context.Logger; }
            set { Context.Logger = value; }
        }

        /// <summary>
        /// Query the given parameter from the iScan Duo connected to the serial port, and return the result immediately.
        /// Be careful calling this method as it can block until a timeout occurs if the communication fails. Also the return value doesn't indicate if an error has occurred.
        /// </summary>
        /// <param name="parameter">Parameter to query</param>
        /// <returns>The value of the parameter in the iScan Duo</returns>
        public decimal Query(Parameter parameter)
        {
            return QueryCommand(ParameterToCommand(parameter));
        }

        protected decimal QueryCommand(DecimalCommand command)
        {
            if (command == null)
                return 0;

            WaitDecimalCommandListener listener = new WaitDecimalCommandListener();
            Context.Communicator.QueryValue<decimal>(command, 0, listener);
            ErrorCode? error;
            decimal? value = listener.Get(out error);
            return value ?? 0;
        }

        /// <summary>
        /// Set the given parameter's value in the iScan Duo connected to the serial port. Be careful calling this method as it can block until a timeout occurs if the communication fails.
        /// </summary>
        /// <param name="parameter">Paramter to set</param>
        /// <param name="value">Value to set the parameter to</param>
        /// <returns>An error code if an error occurs setting the parameter's value</returns>
        public ErrorCode Set(Parameter parameter, decimal value)
        {
            return SetCommand(ParameterToCommand(parameter), value);
        }

        protected ErrorCode SetCommand(DecimalCommand command, decimal value)
        {
            if (command == null)
                return ErrorCode.InvalidSettingId;

            WaitDecimalCommandListener listener = new WaitDecimalCommandListener();
            Context.Communicator.SetValue<decimal>(command, value, listener, 0);
            ErrorCode? error;
            listener.Get(out error);
            return error ?? ErrorCode.NoError;
        }

        /// <summary>
        /// Query the parameter's value asynchronously, notifying the given listener of the returned value (or any errors that occur).
        /// </summary>
        /// <param name="parameter">Parameter to query</param>
        /// <param name="listener">Listener to notify of the query's result</param>
        public void QueryAsync(Parameter parameter, ICommandListener<decimal> listener)
        {
            QueryCommandAsync(ParameterToCommand(parameter), listener);
        }

        protected void QueryCommandAsync(DecimalCommand command, ICommandListener<decimal> listener)
        {
            if (command == null)
                return;

            Context.Communicator.QueryValue<decimal>(command, 0, listener);
        }

        /// <summary>
        /// Set the parameter's value asynchronously, notifying the given listener of the set command's result.
        /// </summary>
        /// <param name="parameter">Parameter to set</param>
        /// <param name="value">Value to set the parameter to</param>
        /// <param name="listener">Listener to notify the result of the set</param>
        public void SetAsync(Parameter parameter, decimal value, ICommandListener<decimal> listener)
        {
            SetCommandAsync(ParameterToCommand(parameter), value, listener);
        }

        protected void SetCommandAsync(DecimalCommand command, decimal value, ICommandListener<decimal> listener)
        {
            if (command == null)
                return;

            Context.Communicator.SetValue<decimal>(command, value, listener, 0);
        }

        /// <summary>
        /// iScan Duo setting parameters
        /// </summary>
        public enum Parameter
        {
            ColorGamutRedx,
            ColorGamutRedy,
            ColorGamutRedcY,
            ColorGamutGreenx,
            ColorGamutGreeny,
            ColorGamutGreencY,
            ColorGamutBluex,
            ColorGamutBluey,
            ColorGamutBluecY,
            ColorGamutCyanx,
            ColorGamutCyany,
            ColorGamutCyancY,
            ColorGamutMagentax,
            ColorGamutMagentay,
            ColorGamutMagentacY,
            ColorGamutYellowx,
            ColorGamutYellowy,
            ColorGamutYellowcY,
            ColorGamutWhitex,
            ColorGamutWhitey,
            ColorGamutWhitecY,

            GrayscaleRed0IRE,
            GrayscaleRed10IRE,
            GrayscaleRed20IRE,
            GrayscaleRed30IRE,
            GrayscaleRed40IRE,
            GrayscaleRed50IRE,
            GrayscaleRed60IRE,
            GrayscaleRed70IRE,
            GrayscaleRed80IRE,
            GrayscaleRed90IRE,
            GrayscaleRed100IRE,

            GrayscaleGreen0IRE,
            GrayscaleGreen10IRE,
            GrayscaleGreen20IRE,
            GrayscaleGreen30IRE,
            GrayscaleGreen40IRE,
            GrayscaleGreen50IRE,
            GrayscaleGreen60IRE,
            GrayscaleGreen70IRE,
            GrayscaleGreen80IRE,
            GrayscaleGreen90IRE,
            GrayscaleGreen100IRE,

            GrayscaleBlue0IRE,
            GrayscaleBlue10IRE,
            GrayscaleBlue20IRE,
            GrayscaleBlue30IRE,
            GrayscaleBlue40IRE,
            GrayscaleBlue50IRE,
            GrayscaleBlue60IRE,
            GrayscaleBlue70IRE,
            GrayscaleBlue80IRE,
            GrayscaleBlue90IRE,
            GrayscaleBlue100IRE,

            GrayscaleWhitex0IRE,
            GrayscaleWhitex10IRE,
            GrayscaleWhitex20IRE,
            GrayscaleWhitex30IRE,
            GrayscaleWhitex40IRE,
            GrayscaleWhitex50IRE,
            GrayscaleWhitex60IRE,
            GrayscaleWhitex70IRE,
            GrayscaleWhitex80IRE,
            GrayscaleWhitex90IRE,
            GrayscaleWhitex100IRE,

            GrayscaleWhitey0IRE,
            GrayscaleWhitey10IRE,
            GrayscaleWhitey20IRE,
            GrayscaleWhitey30IRE,
            GrayscaleWhitey40IRE,
            GrayscaleWhitey50IRE,
            GrayscaleWhitey60IRE,
            GrayscaleWhitey70IRE,
            GrayscaleWhitey80IRE,
            GrayscaleWhitey90IRE,
            GrayscaleWhitey100IRE,

            GrayscaleWhitecY0IRE,
            GrayscaleWhitecY10IRE,
            GrayscaleWhitecY20IRE,
            GrayscaleWhitecY30IRE,
            GrayscaleWhitecY40IRE,
            GrayscaleWhitecY50IRE,
            GrayscaleWhitecY60IRE,
            GrayscaleWhitecY70IRE,
            GrayscaleWhitecY80IRE,
            GrayscaleWhitecY90IRE,
            GrayscaleWhitecY100IRE
        }

        protected DecimalCommand ParameterToCommand(Parameter parameter)
        {
            switch (parameter)
            {
                case Parameter.ColorGamutRedx: return DuoCommands.CGRedxCommand;
                case Parameter.ColorGamutRedy: return DuoCommands.CGRedyCommand;
                case Parameter.ColorGamutRedcY: return DuoCommands.CGRedcYCommand;
                case Parameter.ColorGamutGreenx: return DuoCommands.CGGreenxCommand;
                case Parameter.ColorGamutGreeny: return DuoCommands.CGGreenyCommand;
                case Parameter.ColorGamutGreencY: return DuoCommands.CGGreencYCommand;
                case Parameter.ColorGamutBluex: return DuoCommands.CGBluexCommand;
                case Parameter.ColorGamutBluey: return DuoCommands.CGBlueyCommand;
                case Parameter.ColorGamutBluecY: return DuoCommands.CGBluecYCommand;
                case Parameter.ColorGamutCyanx: return DuoCommands.CGCyanxCommand;
                case Parameter.ColorGamutCyany: return DuoCommands.CGCyanyCommand;
                case Parameter.ColorGamutCyancY: return DuoCommands.CGCyancYCommand;
                case Parameter.ColorGamutMagentax: return DuoCommands.CGMagentaxCommand;
                case Parameter.ColorGamutMagentay: return DuoCommands.CGMagentayCommand;
                case Parameter.ColorGamutMagentacY: return DuoCommands.CGMagentacYCommand;
                case Parameter.ColorGamutYellowx: return DuoCommands.CGYellowxCommand;
                case Parameter.ColorGamutYellowy: return DuoCommands.CGYellowyCommand;
                case Parameter.ColorGamutYellowcY: return DuoCommands.CGYellowcYCommand;
                case Parameter.ColorGamutWhitex: return DuoCommands.CGWhitexCommand;
                case Parameter.ColorGamutWhitey: return DuoCommands.CGWhiteyCommand;
                case Parameter.ColorGamutWhitecY: return DuoCommands.CGWhitecYCommand;

                case Parameter.GrayscaleRed0IRE: return DuoCommands.GRed0Command;
                case Parameter.GrayscaleRed10IRE: return DuoCommands.GRed10Command;
                case Parameter.GrayscaleRed20IRE: return DuoCommands.GRed20Command;
                case Parameter.GrayscaleRed30IRE: return DuoCommands.GRed30Command;
                case Parameter.GrayscaleRed40IRE: return DuoCommands.GRed40Command;
                case Parameter.GrayscaleRed50IRE: return DuoCommands.GRed50Command;
                case Parameter.GrayscaleRed60IRE: return DuoCommands.GRed60Command;
                case Parameter.GrayscaleRed70IRE: return DuoCommands.GRed70Command;
                case Parameter.GrayscaleRed80IRE: return DuoCommands.GRed80Command;
                case Parameter.GrayscaleRed90IRE: return DuoCommands.GRed90Command;
                case Parameter.GrayscaleRed100IRE: return DuoCommands.GRed100Command;

                case Parameter.GrayscaleGreen0IRE: return DuoCommands.GGreen0Command;
                case Parameter.GrayscaleGreen10IRE: return DuoCommands.GGreen10Command;
                case Parameter.GrayscaleGreen20IRE: return DuoCommands.GGreen20Command;
                case Parameter.GrayscaleGreen30IRE: return DuoCommands.GGreen30Command;
                case Parameter.GrayscaleGreen40IRE: return DuoCommands.GGreen40Command;
                case Parameter.GrayscaleGreen50IRE: return DuoCommands.GGreen50Command;
                case Parameter.GrayscaleGreen60IRE: return DuoCommands.GGreen60Command;
                case Parameter.GrayscaleGreen70IRE: return DuoCommands.GGreen70Command;
                case Parameter.GrayscaleGreen80IRE: return DuoCommands.GGreen80Command;
                case Parameter.GrayscaleGreen90IRE: return DuoCommands.GGreen90Command;
                case Parameter.GrayscaleGreen100IRE: return DuoCommands.GGreen100Command;

                case Parameter.GrayscaleBlue0IRE: return DuoCommands.GBlue0Command;
                case Parameter.GrayscaleBlue10IRE: return DuoCommands.GBlue10Command;
                case Parameter.GrayscaleBlue20IRE: return DuoCommands.GBlue20Command;
                case Parameter.GrayscaleBlue30IRE: return DuoCommands.GBlue30Command;
                case Parameter.GrayscaleBlue40IRE: return DuoCommands.GBlue40Command;
                case Parameter.GrayscaleBlue50IRE: return DuoCommands.GBlue50Command;
                case Parameter.GrayscaleBlue60IRE: return DuoCommands.GBlue60Command;
                case Parameter.GrayscaleBlue70IRE: return DuoCommands.GBlue70Command;
                case Parameter.GrayscaleBlue80IRE: return DuoCommands.GBlue80Command;
                case Parameter.GrayscaleBlue90IRE: return DuoCommands.GBlue90Command;
                case Parameter.GrayscaleBlue100IRE: return DuoCommands.GBlue100Command;

                case Parameter.GrayscaleWhitex0IRE: return DuoCommands.GWhitex0Command;
                case Parameter.GrayscaleWhitex10IRE: return DuoCommands.GWhitex10Command;
                case Parameter.GrayscaleWhitex20IRE: return DuoCommands.GWhitex20Command;
                case Parameter.GrayscaleWhitex30IRE: return DuoCommands.GWhitex30Command;
                case Parameter.GrayscaleWhitex40IRE: return DuoCommands.GWhitex40Command;
                case Parameter.GrayscaleWhitex50IRE: return DuoCommands.GWhitex50Command;
                case Parameter.GrayscaleWhitex60IRE: return DuoCommands.GWhitex60Command;
                case Parameter.GrayscaleWhitex70IRE: return DuoCommands.GWhitex70Command;
                case Parameter.GrayscaleWhitex80IRE: return DuoCommands.GWhitex80Command;
                case Parameter.GrayscaleWhitex90IRE: return DuoCommands.GWhitex90Command;
                case Parameter.GrayscaleWhitex100IRE: return DuoCommands.GWhitex100Command;

                case Parameter.GrayscaleWhitey0IRE: return DuoCommands.GWhitey0Command;
                case Parameter.GrayscaleWhitey10IRE: return DuoCommands.GWhitey10Command;
                case Parameter.GrayscaleWhitey20IRE: return DuoCommands.GWhitey20Command;
                case Parameter.GrayscaleWhitey30IRE: return DuoCommands.GWhitey30Command;
                case Parameter.GrayscaleWhitey40IRE: return DuoCommands.GWhitey40Command;
                case Parameter.GrayscaleWhitey50IRE: return DuoCommands.GWhitey50Command;
                case Parameter.GrayscaleWhitey60IRE: return DuoCommands.GWhitey60Command;
                case Parameter.GrayscaleWhitey70IRE: return DuoCommands.GWhitey70Command;
                case Parameter.GrayscaleWhitey80IRE: return DuoCommands.GWhitey80Command;
                case Parameter.GrayscaleWhitey90IRE: return DuoCommands.GWhitey90Command;
                case Parameter.GrayscaleWhitey100IRE: return DuoCommands.GWhitey100Command;

                case Parameter.GrayscaleWhitecY0IRE: return DuoCommands.GWhitecY0Command;
                case Parameter.GrayscaleWhitecY10IRE: return DuoCommands.GWhitecY10Command;
                case Parameter.GrayscaleWhitecY20IRE: return DuoCommands.GWhitecY20Command;
                case Parameter.GrayscaleWhitecY30IRE: return DuoCommands.GWhitecY30Command;
                case Parameter.GrayscaleWhitecY40IRE: return DuoCommands.GWhitecY40Command;
                case Parameter.GrayscaleWhitecY50IRE: return DuoCommands.GWhitecY50Command;
                case Parameter.GrayscaleWhitecY60IRE: return DuoCommands.GWhitecY60Command;
                case Parameter.GrayscaleWhitecY70IRE: return DuoCommands.GWhitecY70Command;
                case Parameter.GrayscaleWhitecY80IRE: return DuoCommands.GWhitecY80Command;
                case Parameter.GrayscaleWhitecY90IRE: return DuoCommands.GWhitecY90Command;
                case Parameter.GrayscaleWhitecY100IRE: return DuoCommands.GWhitecY100Command;
            }
            return null;
        }
    }
}

