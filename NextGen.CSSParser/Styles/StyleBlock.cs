using NextGen.CSSParser.Styles;
using System.Collections.Generic;

namespace NextGen.CSSParser
{
    public class StyleBlock
    {
        /// <summary>
        /// The selector for this style block
        /// </summary>
        public StyleSelector Selector { get; internal set; }

        /// <summary>
        /// The rules in this style block.
        /// </summary>
        public IEnumerable<StyleRule> Rules => _rules.AsReadOnly();

        private List<StyleRule> _rules = new List<StyleRule>();

        internal void AddRule(StyleRule rule)
        {
            _rules.Add(rule);
        }
    }
}