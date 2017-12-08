using NextGen.CSSParser.Exceptions;
using NextGen.CSSParser.Styles;
using NextGen.CSSParser.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser
{
    public class AwesomeCssParser
    {
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
                styles.Add(Parse_CssBlock(tokenizer));
                tokenizer.SkipWhiteSpace();
            }

            return styles;
        }

        private StyleBlock Parse_CssBlock(ICssTokenizer tokenizer)
        {
            // Header
            var block = new StyleBlock();
            block.Selector = Parse_CssBlockSelector(tokenizer);

            // Lines
            tokenizer.Expect('{');
            while (!tokenizer.NextIs('}'))
            {
                block.AddRule(Parse_CssRule(tokenizer));
            }
            tokenizer.Expect('}');

            return block;
        }

        private StyleSelector Parse_CssBlockSelector(ICssTokenizer tokenizer)
        {
            // Read selector
            var selectorText = tokenizer.ReadTo('{');
            selectorText = selectorText.Trim();

            // Catch all case
            if (selectorText.Equals("*"))
            {
                return new StyleSelector { SelectAll = true };
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

            return selector;
        }

        private StyleRule Parse_CssRule(ICssTokenizer tokenizer)
        {
            // Read property name and value
            tokenizer.SkipWhiteSpace();
            var property = tokenizer.ReadTo(':');
            tokenizer.Expect(':');
            tokenizer.SkipWhiteSpace();
            var value = tokenizer.ReadTo(';').Trim();
            tokenizer.Expect(';');

            if (value.EndsWith("!important"))
            {
                return new StyleRule { Name = property, Value = value.Substring(0, value.Length - "!important".Length), Important = true };
            }
            else
            {
                return new StyleRule { Name = property, Value = value };
            }
        }
    }
}
