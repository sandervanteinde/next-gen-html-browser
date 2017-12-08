using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser.Elements
{
    [DOMElementFor("link")]
    public class LinkElement : DOMElement
    {
        public LinkElement()
            : base("link")
        {
            RequireEndTag = false;
        }
    }
}
