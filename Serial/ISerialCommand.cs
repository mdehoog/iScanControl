using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profiler.Protocol;
using System.IO.Ports;

namespace Profiler.Serial
{
    public interface ISerialCommand : IComparable<ISerialCommand>
    {
        ICommand ICommand { get; }
        long CommandId { get; }
        void NotifyQueued();
        void NotifyCancelled();
        void Run(ICommunicator communicator);
    }
}
