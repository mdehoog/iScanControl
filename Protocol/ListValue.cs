using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profiler.Protocol
{
    public class ListValue
    {
        private readonly string name;
        private readonly int value;

        public ListValue(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
        }

        public int Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
