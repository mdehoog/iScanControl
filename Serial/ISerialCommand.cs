using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qixle.iScanDuo.Controller.Protocol;
using System.IO.Ports;

namespace Qixle.iScanDuo.Controller.Serial
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
