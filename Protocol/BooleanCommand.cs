using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    /// <summary>
    /// Command used to send boolean values (on/off) to the Duo.
    /// </summary>
    public class BooleanCommand : Command<bool>
    {
        public BooleanCommand(string name, string id, bool savable, bool defaultValue)
            : base(name, id, null, savable, defaultValue)
        {
        }

        public override string ValueToString(bool value)
        {
            return value ? "1" : "0";
        }

        public override bool StringToValue(string str)
        {
            return str != null && str.Equals("1", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
