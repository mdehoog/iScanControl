using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Protocol
{
    public class StringCommand : Command<string>
    {
        public StringCommand(string name, string id)
            : base(name, id, null, false, null)
        {
        }

        public StringCommand(string name, string id, string valuePrefix, bool savable)
            : base(name, id, valuePrefix, savable, null)
        {
        }

        public override string ValueToString(string value)
        {
            return value;
        }

        public override string StringToValue(string str)
        {
            //input label values return with NULL characters instead of spaces
            return str.Replace("\0", " ");
        }
    }
}
