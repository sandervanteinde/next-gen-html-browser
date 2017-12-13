using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles.PropertyParsers
{
    public class PropertyValueParser
    {
        private Dictionary<string, IPropertyValueRule> _parsers = new Dictionary<string, IPropertyValueRule>();

        internal PropertyValueParser()
        {
            RegisterParserRule(new BackgroundColorRule());
        }

        public void RegisterParserRule(IPropertyValueRule parser)
        {
            _parsers.Add(parser.PropertyName, parser);
        }

        public IPropertyValueRule GetParserForRule(string rule)
        {
            return _parsers[rule];
        }
    }
}
