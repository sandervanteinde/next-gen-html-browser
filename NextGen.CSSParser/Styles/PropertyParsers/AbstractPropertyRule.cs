using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles.PropertyParsers
{
    internal abstract class AbstractPropertyRule<T> : IPropertyValueRule
    {
        public abstract string PropertyName { get; }

        public AbstractStylePropertyValue ParseValue(string propertyValue)
        {
            // Split important
            bool important = propertyValue.EndsWith("!important");
            if (important)
                propertyValue = propertyValue.Substring(0, propertyValue.Length - "!important".Length).Trim();

            // Create the instance that will be returned
            var val = new StylePropertyValue<T> { Important = important };

            // Special cases
            if (propertyValue == "inherit")
                val.ValueType = AbstractStylePropertyValue.ValueTypes.Inherit;
            else if (propertyValue == "initial")
                val.ValueType = AbstractStylePropertyValue.ValueTypes.Initial;
            else if (propertyValue == "unset")
                val.ValueType = AbstractStylePropertyValue.ValueTypes.Unset;
            else
            {
                val.ValueType = AbstractStylePropertyValue.ValueTypes.Value;
                Execute(propertyValue, val);
            }

            // Return the parsed rule
            return val;
        }

        protected abstract void Execute(string propertyValue, StylePropertyValue<T> prop);
    }
}
