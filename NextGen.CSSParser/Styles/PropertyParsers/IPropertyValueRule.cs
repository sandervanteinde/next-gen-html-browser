using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles.PropertyParsers
{
    public interface IPropertyValueRule
    {
        string PropertyName { get; }

        AbstractStylePropertyValue CreateNewValueInstance();

        void ParseValue(string propertyValue, AbstractStylePropertyValue propInstance);
    }
}
