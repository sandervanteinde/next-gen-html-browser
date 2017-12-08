using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    public class DOMAttributeCollection : List<DOMAttribute>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var attr in this)
            {
                sb.Append(attr.Name);
                if (attr.Value != null)
                    sb.Append(' ').Append(attr.Value);
            }
            return sb.ToString();
        }
        public DOMAttribute this[string name]
        {
            get { return this.FirstOrDefault(a => a.Name == name); }
        }
    }
}
