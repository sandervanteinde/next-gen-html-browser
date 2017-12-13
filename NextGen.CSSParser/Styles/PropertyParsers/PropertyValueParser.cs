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
            RegisterParserRule(new ColorRule());
        }

        public void RegisterParserRule(IPropertyValueRule parser)
        {
            _parsers.Add(parser.PropertyName, parser);
        }

        public IPropertyValueRule GetParserRule(string rule)
        {
            if (!_parsers.TryGetValue(rule, out IPropertyValueRule parserRule))
                throw new Exception();
            return parserRule;
        }
    }
}
