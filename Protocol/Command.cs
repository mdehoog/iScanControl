using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qixle.iScanDuo.Controller.Protocol
{
    /// <summary>
    /// Abstract ICommand implementation.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public abstract class Command<T> : ICommand
    {
        private readonly string name;
        private readonly string id;
        private readonly string valuePrefix;
        private readonly bool savable;
        private readonly bool queryable;
        private readonly T defaultValue;
        private bool armed;

        public Command(string name, string id, string valuePrefix, bool savable, T defaultValue)
            : this(name, id, valuePrefix, savable, true, defaultValue)
        {
        }

        public Command(string name, string id, string valuePrefix, bool savable, bool queryable, T defaultValue)
        {
            if (valuePrefix != null && valuePrefix.EndsWith(" "))
                valuePrefix = valuePrefix.Substring(0, valuePrefix.Length - 1);

            this.name = name;
            this.id = id;
            this.valuePrefix = valuePrefix;
            this.savable = savable;
            this.queryable = queryable;
            this.defaultValue = defaultValue;
        }

        public abstract string ValueToString(T value);
        public abstract T StringToValue(string str);

        public string Name
        {
            get { return name; }
        }

        public string Id
        {
            get { return id; }
        }

        public bool HasValuePrefix
        {
            get { return valuePrefix != null && valuePrefix.Length > 0; }
        }

        public string ValuePrefix
        {
            get { return valuePrefix; }
        }

        public bool IsSavable
        {
            get { return savable; }
        }

        public bool IsQueryable
        {
            get { return queryable; }
        }

        public bool IsArmed
        {
            get { return armed; }
            set { armed = value; }
        }

        public T DefaultValue
        {
            get { return defaultValue; }
        }

        public string DefaultValueAsString
        {
            get { return ValueToString(DefaultValue); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
