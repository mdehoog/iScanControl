using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    public class ListValues : List<ListValue>
    {
        private int defaultValueIndex = -1;

        public ListValues(int defaultValue, params ListValue[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                ListValue value = values[i];
                if (value == null)
                    continue;

                if (value.Value == defaultValue)
                    defaultValueIndex = Count;

                Add(value);
            }
        }

        public ListValue DefaultValue
        {
            get { return defaultValueIndex >= 0 && defaultValueIndex < Count ? this[defaultValueIndex] : null; }
        }

        public ListValue StringToValue(string s)
        {
            int i;
            if (int.TryParse(s, out i))
            {
                return ValueToValue(i);
            }
            return null;
        }

        public ListValue ValueToValue(int v)
        {
            foreach (ListValue value in this)
            {
                if (value.Value == v)
                    return value;
            }
            return null;
        }
    }
}
