using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qixle.iScanDuo.Controller.Protocol;

namespace Qixle.iScanDuo.Controller.Serial
{
    /// <summary>
    /// Listens to an iScan Duo 'query' or 'set' command's lifecycle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandListener<T>
    {
        /// <summary>
        /// Called when the command is queued.
        /// </summary>
        void CommandQueued();

        /// <summary>
        /// Called if the command is cancelled. This can occur if another command for the same parameter is queued.
        /// </summary>
        void CommandCancelled();

        /// <summary>
        /// Called when the command is started (began communication).
        /// </summary>
        void CommandStarted();

        /// <summary>
        /// Called when the command has completed. If the command was a query command, the returned value is given.
        /// </summary>
        /// <param name="value">Value of the completed command</param>
        void CommandCompleted(T value);

        /// <summary>
        /// Called when an error has occured during command communication.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="value">Value that was attempting to be set</param>
        void CommandError(ErrorCode code, T value);
    }
}
