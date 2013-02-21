using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Logging
{
    /// <summary>
    /// Abstract class that provides a interface for logging messages.
    /// </summary>
    public abstract class Logger
    {
        public enum Level
        {
            Fine,
            Info,
            Warning,
            Error
        }

        private Level logLevel = Level.Info;

        public abstract void Log(string message, Level level);

        public void Fine(string message)
        {
            Log(message, Level.Fine);
        }

        public void Info(string message)
        {
            Log(message, Level.Info);
        }

        public void Warning(string message)
        {
            Log(message, Level.Warning);
        }

        public void Error(string message)
        {
            Log(message, Level.Error);
        }

        public void Error(Exception exception)
        {
            Log(exception + ": " + exception.Message, Level.Error);
        }

        public Level LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }

        protected bool ShouldLog(Level level)
        {
            return level >= LogLevel;
        }
    }
}
