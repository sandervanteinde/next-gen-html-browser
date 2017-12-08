using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser.Elements
{
    [DOMElementFor("img")]
    public class ImageElement : DOMElement
    {
        public ImageElement()
            :base("img")
        {
            RequireEndTag = false;
        }
    }
}
