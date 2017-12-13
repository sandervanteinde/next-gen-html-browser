using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles.PropertyParsers
{
    internal abstract class AbstractPropertyValueParser<T> : IPropertyValueRule
    {
        public abstract string PropertyName { get; }

        public AbstractStylePropertyValue CreateNewValueInstance()
        {
            return (AbstractStylePropertyValue)Activator.CreateInstance(typeof(StylePropertyValue<T>));
        }

        public void ParseValue(string propertyValue, AbstractStylePropertyValue propInstance)
        {
            Execute(propertyValue, (StylePropertyValue<T>)propInstance);
        }

        protected abstract void Execute(string propertyValue, StylePropertyValue<T> prop);
    }
}
