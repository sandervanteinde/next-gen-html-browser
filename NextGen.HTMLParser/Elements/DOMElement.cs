using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    public class DOMElement
    {
        public string Name { get; set; }
        public DOMElement Parent { get; internal set; }
        public List<DOMElement> Children { get; set; } = new List<DOMElement>();
        public string Content { get; internal set; }
        public DOMAttributeCollection Attributes { get; } = new DOMAttributeCollection();
        internal int BodyIndex { get; set; }
        internal int EndBodyIndex { get; set; }
        public override string ToString()
        {
            var attributeString = Attributes.ToString();
            if (attributeString.Length == 0)
                return $"<{Name}>";
            return $"<{Name} {attributeString}>";
        }
        public DOMElement FindFirst(string elementName)
        {
            if (Name == elementName)
                return this;
            foreach(var el in Children)
            {
                var childEl = el.FindFirst(elementName);
                if (childEl != null)
                    return childEl;
            }
            return null;
        }
        public IEnumerable<DOMElement> FindAll(string elementName)
        {

            if (Name == elementName)
                yield return this;
            foreach (var el in Children)
                foreach (var results in el.FindAll(elementName))
                    yield return results;
        }
    }
}
