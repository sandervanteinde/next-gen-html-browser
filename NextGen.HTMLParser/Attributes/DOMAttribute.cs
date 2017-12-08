using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    public class DOMAttribute
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value.ToLower();
        }
        public string Value { get; set; }
    }
}
