using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    /// <summary>
    /// Represents a command that can be sent to the Duo to set or query data.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Command name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Command id (2 character string, sent to the Duo)
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Should this command prefix its values with a prefix string?
        /// </summary>
        bool HasValuePrefix { get; }

        /// <summary>
        /// Prefix string to prefix command values sent to the Duo
        /// </summary>
        string ValuePrefix { get; }

        /// <summary>
        /// Is this command savable? Some commands cannot be saved, such as the firmware update command, and remote control messages.
        /// </summary>
        bool IsSavable { get; }

        /// <summary>
        /// Is this command queryable? Some commands don't have values stored, such as remote control messages.
        /// </summary>
        bool IsQueryable { get; }

        /// <summary>
        /// Is this command armed for saving?
        /// </summary>
        bool IsArmed { get; set; }

        /// <summary>
        /// Default value of this command (as a string)
        /// </summary>
        string DefaultValueAsString { get; }
    }
}
