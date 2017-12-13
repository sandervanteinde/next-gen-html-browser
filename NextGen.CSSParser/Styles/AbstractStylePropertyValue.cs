using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles
{
    public class AbstractStylePropertyValue
    {
        public enum ValueTypes
        {
            Inherit,
            Initial,
            Unset,
            Value
        }

        public bool Important { get; internal set; }

        public ValueTypes ValueType { get; internal set; }
    }
}
