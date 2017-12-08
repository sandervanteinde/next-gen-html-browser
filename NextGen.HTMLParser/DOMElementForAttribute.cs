using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    internal class DOMElementForAttribute : Attribute
    {
        public string ElementType { get; private set; }
        public DOMElementForAttribute(string elementType)
        {
            ElementType = elementType;
        }
    }
}
