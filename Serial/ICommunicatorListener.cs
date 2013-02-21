using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Serial
{
    public interface ICommunicatorListener
    {
        void CommunicationBegun();
        void CommunicationComplete();
    }
}
