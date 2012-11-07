using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Serial
{
    public interface ICommunicatorListener
    {
        void CommunicationBegun();
        void CommunicationComplete();
    }
}
