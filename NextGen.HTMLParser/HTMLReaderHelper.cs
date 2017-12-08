using NextGen.HTMLParser.Elements;
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
        private static Dictionary<string, Type> domElements = new Dictionary<string, Type>();
        static HTMLReaderHelper()
        {
            var domType = typeof(DOMElement);
            foreach(var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attr = type.GetCustomAttribute<DOMElementForAttribute>();
                if (attr != null && domType.IsAssignableFrom(type))
                {
                        domElements.Add(attr.ElementType, type);
                }
            }
        }
        public static DOMElement GetDOMElementForType(string elementType)
        {
            Type type;
            if (domElements.TryGetValue(elementType, out type))
                return (DOMElement)Activator.CreateInstance(type);
            return new DOMElement(elementType);
        }
    }
}
