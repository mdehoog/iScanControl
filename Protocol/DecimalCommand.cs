using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Qixle.iScanDuo.Controller.Protocol
{
    /// <summary>
    /// Command used to send decimal values (including integers) to the Duo.
    /// </summary>
    public class DecimalCommand : Command<decimal>
    {
        private readonly int decimalPlaces;
        private readonly static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        public DecimalCommand(string name, string id, string valuePrefix, bool savable, decimal defaultValue, int decimalPlaces)
            : base(name, id, valuePrefix, savable, defaultValue)
        {
            this.decimalPlaces = decimalPlaces;
        }

        public DecimalCommand(string name, string id, string valuePrefix, bool savable, int decimalPlaces)
            : this(name, id, valuePrefix, savable, 0, decimalPlaces)
        {
        }

        public DecimalCommand(string name, string id, decimal defaultValue, int decimalPlaces)
            : this(name, id, null, true, defaultValue, decimalPlaces)
        {
        }

        public DecimalCommand(string name, string id, string valuePrefix, int decimalPlaces)
            : this(name, id, valuePrefix, true, 0, decimalPlaces)
        {
        }

        public DecimalCommand(string name, string id, int decimalPlaces)
            : this(name, id, null, decimalPlaces)
        {
        }

        public override string ValueToString(decimal value)
        {
            return value.ToString("F" + decimalPlaces, culture);
        }

        public override decimal StringToValue(string str)
        {
            decimal value;
            NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint;
            if (!decimal.TryParse(str, style, culture, out value))
            {
                Context.Logger.Warning("Could not parse decimal: " + str);
            }
            return value;
        }

        public int DecimalPlaces
        {
            get { return decimalPlaces; }
        }
    }
}
