using NextGen.CSSParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.HTMLParser;
using NextGen.CSSParser.Styles;
using NextGen.HTMLParser.Elements;

namespace NextGen.ViewEngine
{
    public class StyleEngine
    {
        private List<StyleDefinition> _definitions = new List<StyleDefinition>();

        public void LoadDefinition(StyleDefinition definition)
        {
            _definitions.Add(definition);
        }

        public IEnumerable<StyleBlock> GetDefinedStylesForElement(DOMElement element)
        {
            // Find style blocks that match the selector
            var styleblocks = _definitions.SelectMany(d => d.Blocks).Where(b => DoSelectorsMatch(element, b.Selector));
            return styleblocks;
        }

        public IEnumerable<StyleRule> GetCalculatedStylesForElement(DOMElement element)
        {
            var stylerules = GetDefinedStylesForElement(element)
                .SelectMany(sb =>
                {
                    return sb.Rules.Select(rule => (rule, specificity: sb.Specificity));
                });

            var resultRules = new Dictionary<string, (StyleRule rule, StyleRuleSpecificity specificity)>();

            foreach (var newRule in stylerules)
            {
                // Easy case
                var newRuleName = newRule.rule.Name;
                if (!resultRules.ContainsKey(newRuleName))
                {
                    resultRules.Add(newRuleName, newRule);
                    continue;
                }

                // Compare importance
                var existingRule = resultRules[newRule.rule.Name];
                if (newRule.rule.Important)
                    resultRules[newRuleName] = newRule;
                else if (existingRule.rule.Important)
                    continue;
                else if (newRule.specificity >= existingRule.specificity)
                    resultRules[newRuleName] = newRule;
            }

            return resultRules.Values.Select(v => v.rule);
        }

        private bool DoSelectorsMatch(DOMElement element, StyleSelector selector)
        {
            if (selector.TagName != null && element.Name != selector.TagName)
                return false;

            if (selector.Id != null && element.Id != selector.Id)
                return false;

            if (!selector.Classes.All(c => element.ClassList.Contains(c)))
                return false;

            return true;
        }
    }
}
