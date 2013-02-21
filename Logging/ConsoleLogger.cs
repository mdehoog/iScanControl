using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Logging
{
    /// <summary>
    /// Logger implementation that logs messages to the Console.
    /// </summary>
    public class ConsoleLogger : Logger
    {
        public override void Log(string message, Logger.Level level)
        {
            if (ShouldLog(level))
            {
                Console.WriteLine(level.ToString().ToUpper() + ": " + message);
            }
        }
    }
}
