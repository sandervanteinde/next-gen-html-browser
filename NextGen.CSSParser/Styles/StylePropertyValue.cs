using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles
{
    public class StylePropertyValue<T> : AbstractStylePropertyValue
    {
        public T Value { get; internal set; }

        public static implicit operator T(StylePropertyValue<T> obj)
        {
            return obj.Value;
        }

        public static implicit operator StylePropertyValue<T>(T obj)
        {
            return new StylePropertyValue<T> {
                Value = obj,
                ValueType = ValueTypes.Value
            };
        }
    }
}
