﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Serial
{
    public interface ICommandListener<T>
    {
        void CommandQueued();
        void CommandCancelled();
        void CommandStarted();
        void CommandCompleted(T value);
        void CommandError(ErrorCode code, T value);
    }
}
