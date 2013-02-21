using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Qixle.iScanDuo.Controller.Serial
{
    public class WaitDecimalCommandListener : ICommandListener<decimal>
    {
        private decimal? value;
        private ErrorCode? error;

        public decimal? Get(out ErrorCode? e)
        {
            lock (this)
            {
                if (!value.HasValue && !error.HasValue)
                {
                    Monitor.Wait(this);
                }
                e = error;
                return value;
            }
        }

        private void Set(decimal? value, ErrorCode? error)
        {
            lock (this)
            {
                this.value = value;
                this.error = error;
                Monitor.Pulse(this);
            }
        }

        public void CommandQueued()
        {
        }

        public void CommandCancelled()
        {
            Set(null, ErrorCode.NoError);
        }

        public void CommandStarted()
        {
        }

        public void CommandCompleted(decimal value)
        {
            Set(value, null);
        }

        public void CommandError(ErrorCode code, decimal value)
        {
            Set(null, code);
        }
    }
}
