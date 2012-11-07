using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Serial;

namespace Profiler.Protocol
{
    /// <summary>
    /// Command used to enable the hidden test pattern functionality. Value is "RED GREEN BLUE SIZE", with spaces between values.
    /// </summary>
    public class CustomTestPatternCommand : Command<int[]>
    {
        public CustomTestPatternCommand()
            : base("Custom Test Pattern", "AF", null, false, false, new int[] { 100, 100, 100, 100 })
        {
        }

        public override string ValueToString(int[] value)
        {
            if (value == null || value.Length != 4)
            {
                value = DefaultValue;
            }
            foreach (int i in value)
            {
                if (i < 0 || i > 100)
                {
                    value = DefaultValue;
                    break;
                }
            }
            return value[0] + " " + value[1] + " " + value[2] + " " + value[3];
        }

        public override int[] StringToValue(string str)
        {
            return new int[] { 0, 0, 0, 0 };
        }

        /// <summary>
        /// ICommandListener for this Command that does nothing (this command cannot be queried).
        /// </summary>
        public class NullListener : ICommandListener<int[]>
        {
            public void CommandQueued()
            {
            }

            public void CommandCancelled()
            {
            }

            public void CommandStarted()
            {
            }

            public void CommandCompleted(int[] value)
            {
            }

            public void CommandError(ErrorCode code, int[] value)
            {
            }
        }
    }
}
