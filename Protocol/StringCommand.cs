using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    public class StringCommand : Command<string>
    {
        public StringCommand(string name, string id)
            : base(name, id, null, false, null)
        {
        }

        public override string ValueToString(string value)
        {
            return value;
        }

        public override string StringToValue(string str)
        {
            return str;
        }
    }
}
