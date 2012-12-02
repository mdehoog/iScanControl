using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Serial
{
    public class IgnoreCommandListener<T> : ICommandListener<T>
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

        public void CommandCompleted(T value)
        {
        }

        public void CommandError(ErrorCode code, T value)
        {
        }
    }
}
