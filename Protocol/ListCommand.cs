using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Protocol
{
    public class ListCommand : Command<ListValue>
    {
        private readonly ListValues listValues;

        public ListCommand(string name, string id, bool savable, ListValues listValues)
            : this(name, id, savable, true, listValues)
        {
        }

        public ListCommand(string name, string id, bool savable, bool queryable, ListValues listValues)
            : base(name, id, null, savable, queryable, listValues.DefaultValue)
        {
            this.listValues = listValues;
        }

        public override string ValueToString(ListValue value)
        {
            if (value == null)
                return null;
            return value.Value.ToString();
        }

        public override ListValue StringToValue(string str)
        {
            return ListValues.StringToValue(str);
        }

        public ListValues ListValues
        {
            get { return listValues; }
        }
    }
}
