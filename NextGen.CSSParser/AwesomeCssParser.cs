using NextGen.CSSParser.Exceptions;
using NextGen.CSSParser.Helpers;
using NextGen.CSSParser.Styles;
using NextGen.CSSParser.Styles.PropertyParsers;
using NextGen.CSSParser.Tokenization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser
{
    public class AwesomeCssParser
    {
        public PropertyValueParser PropertyValueParser { get; } = new PropertyValueParser();

        public StyleDefinition ParseFromString(string css)
        {
            return Parse(new StringTokenizer(css));
        }

        private StyleDefinition Parse(ICssTokenizer tokenizer)
        {
            // Setup intial state
            tokenizer.IgnoreWhiteSpace = true;
            var styles = new StyleDefinition();

            // Keep reading css blocks
            while (!tokenizer.Ended)
            {
                styles.AddRange(Parse_CssBlock(tokenizer));
                tokenizer.SkipWhiteSpace();
            }

            return styles;
        }

        private IEnumerable<StyleBlock> Parse_CssBlock(ICssTokenizer tokenizer)
        {
            // Header
            var selectors = Parse_CssBlockSelector(tokenizer);
            var rules = new StyleruleCollection();

            // Lines
            tokenizer.Expect('{');
            while (!tokenizer.NextIs('}'))
            {
                Parse_CssRule(tokenizer, rules);
            }
            tokenizer.Expect('}');

            // Return the list of blocks
            return selectors.Select(selector =>
            {
                return new StyleBlock { Selector = selector, Rules = rules };
            });
        }

        private IEnumerable<StyleSelector> Parse_CssBlockSelector(ICssTokenizer tokenizer)
        {
            // Read selector
            var selectorText = tokenizer.ReadTo('{');
            selectorText = selectorText.Trim();

            // Determine if we need to create multiple
            if (selectorText.Contains(","))
            {
                return selectorText.Split(',').SelectMany(s => Parse_CssBlockSelector(new StringTokenizer(s)));
            }

            // Catch all case
            if (selectorText.Equals("*"))
            {
                return new[] { new StyleSelector { SelectAll = true } };
            }

            // Split selector
            var selector = new StyleSelector();
            var t = new StringTokenizer(selectorText);
            if (!t.NextIsAny('.', '#'))
            {
                selector.TagName = t.ReadToAny('.', '#');
            }
            while (!t.Ended)
            {
                if (t.NextIs('.'))
                {
                    t.Expect('.');
                    selector.AddClass(t.ReadToAny('.', '#'));
                }
                else if (t.NextIs('#'))
                {
                    t.Expect('#');
                    if (selector.Id != null) throw new InvalidStyleException();
                    selector.Id = t.ReadToAny('.', '#');
                }
            }

            return new[] { selector };
        }

        private void Parse_CssRule(ICssTokenizer tokenizer, StyleruleCollection rules)
        {
            // Read property name and value
            tokenizer.SkipWhiteSpace();
            var property = tokenizer.ReadTo(':');
            tokenizer.Expect(':');
            tokenizer.SkipWhiteSpace();
            var value = tokenizer.ReadTo(';').Trim();
            tokenizer.Expect(';');

            // Get parser for rule
            var parser = PropertyValueParser.GetParserRule(property);
            if (parser == null) return;

            // Parse the value
            AbstractStylePropertyValue val = parser.ParseValue(value);

            // Set the rule
            rules.SetProperty(property, val);
        }
    }
}
