using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    internal class HTMLReaderHelper
    {
        static HTMLReaderHelper()
        {
        }
        public static DOMElement GetDOMElementForType(string type)
        {
            return new DOMElement();
        }
    }
}
