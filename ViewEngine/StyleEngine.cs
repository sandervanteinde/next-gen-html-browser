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

        public StyleruleCollection GetCalculatedStylesForElement(DOMElement element)
        {
            var stylerules = GetDefinedStylesForElement(element).SelectMany(sb =>
            {
                return sb.Rules.GetProperties().Select(rule => (specificity: sb.Selector.Specificity, rule));
            });

            var resultRules = new Dictionary<string, (StyleRuleSpecificity specificity, (string name, AbstractStylePropertyValue value) rule)>();

            // Determine new rules
            foreach (var newRule in stylerules)
            {
                // Easy case
                var newRuleName = newRule.rule.name;
                if (!resultRules.ContainsKey(newRuleName))
                {
                    resultRules.Add(newRuleName, newRule);
                    continue;
                }

                // Compare importance
                var existingRule = resultRules[newRule.rule.name];
                if (newRule.rule.value != null && newRule.rule.value.Important)
                    resultRules[newRuleName] = newRule;
                else if (existingRule.rule.value != null && existingRule.rule.value.Important)
                    continue;
                else if (newRule.specificity >= existingRule.specificity)
                    resultRules[newRuleName] = newRule;
            }

            // Create new collection
            var result = new StyleruleCollection();
            foreach (var rule in resultRules)
            {
                result.SetProperty(rule.Value.rule.name, rule.Value.rule.value);
            }
            return result;
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
