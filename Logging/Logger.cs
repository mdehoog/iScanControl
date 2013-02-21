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
        /// <summary>
        /// Log level enumeration
        /// </summary>
        public enum Level
        {
            Fine,
            Info,
            Warning,
            Error
        }

        private Level logLevel = Level.Info;

        /// <summary>
        /// Log this message at the given level. Logger implementations override this. Should call ShouldLog(Level) to check if this message's level should be logged.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="level">Log message level</param>
        public abstract void Log(string message, Level level);

        /// <summary>
        /// Log a message at 'Fine' level
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Fine(string message)
        {
            Log(message, Level.Fine);
        }

        /// <summary>
        /// Log a message at 'Info' level
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Info(string message)
        {
            Log(message, Level.Info);
        }

        /// <summary>
        /// Log a message at 'Warning' level
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Warning(string message)
        {
            Log(message, Level.Warning);
        }

        /// <summary>
        /// Log a message at 'Error' level
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Error(string message)
        {
            Log(message, Level.Error);
        }

        /// <summary>
        /// Log an exception at 'Error' level
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void Error(Exception exception)
        {
            Log(exception + ": " + exception.Message, Level.Error);
        }

        /// <summary>
        /// The current log level
        /// </summary>
        public Level LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }

        /// <summary>
        /// Should a message at the given log level be logged?
        /// </summary>
        /// <param name="level">Level to check</param>
        /// <returns>True if the given level is greater that this logger's current log level</returns>
        protected bool ShouldLog(Level level)
        {
            return level >= LogLevel;
        }
    }
}
