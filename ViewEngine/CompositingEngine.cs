using NextGen.CSSParser.Styles;
using NextGen.HTMLParser.Elements;
using NextGen.ViewEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewEngine.Compositing;
using static NextGen.CSSParser.Styles.AbstractStylePropertyValue;

namespace ViewEngine
{
    public class CompositingEngine
    {
        public Box CreateViewModel(DOMElement element, StyleEngine engine, Rectangle initialCB)
        {
            // Setup the root CB
            var result = new Box()
            {
                Rect = initialCB,
                Styles = new StyleruleCollection()
            };
            result.Children = new[] { CreateBox(element, engine, result) };
            return result;
        }

        public Box CreateBox(DOMElement element, StyleEngine engine, Box parent)
        {
            // The element that we will determine the properties of
            var result = new Box();

            // Determine styles for this element
            result.Styles = engine.GetCalculatedStylesForElement(element);

            // Determine COLOR
            {
                if (result.Styles.Color == null || result.Styles.Color.ValueType == ValueTypes.Inherit)
                {
                    result.Styles.Color = parent.Styles.Color;
                }
                else if (result.Styles.Color.ValueType == ValueTypes.Value)
                {
                    // Do nothing, the color is already set
                }
                else
                {
                    throw new Exception("Unhandled situation");
                }
            }

            // Determine if this element has text content
            if (element is TextElement te)
            {
                result.Text = te.Content;
            }

            // Determine rectangle for this element
            result.Rect = parent.Rect;

            // Determine children for this element
            result.Children = element.Children.Select(c => CreateBox(c, engine, result)).ToArray();

            // Return this element
            return result;
        }
    }
}
