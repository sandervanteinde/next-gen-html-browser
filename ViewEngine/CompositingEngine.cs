using NextGen.CSSParser.Styles;
using NextGen.CSSParser.Styles.DataTypes;
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
        public Box CreateViewModel(DOMElement element, StyleEngine engine)
        {
            // Setup the root CB
            var result = new Box();
            result.Styles.OnNext(new StyleruleCollection());
            result.Children.OnNext(new[] {
                CreateBox(element, engine, result, result)
            });
            return result;
        }

        public Box CreateBox(DOMElement element, StyleEngine engine, Box parent, Box root)
        {
            // The element that we will determine the properties of
            var result = new Box();
            result.HtmlElement = element;
            result.Parent.OnNext(parent);

            // Set styles for this element
            result.Styles.OnNext(engine.GetCalculatedStylesForElement(element));

            // Setup base properties
            SetupAncestorChain(result);
            SetupDisplay(result);
            SetupWidth(result);

            // Setup styles
            SetupTextColor(result);
            SetupBackgroundColor(result);
            SetupTextContent(result);


            // Set the children
            result.Children.OnNext(element.Children.Select(c => CreateBox(c, engine, result, root)).ToArray());

            // Return this element
            return result;
        }

        void SetupTextColor(Box element)
        {
            // Function to calculate the color
            Action<StyleruleCollection, Color> calcTextColor = (elementStyles, parentTextColor) =>
            {
                if (elementStyles.Color == null || elementStyles.Color.ValueType == ValueTypes.Inherit)
                {
                    element.TextColor.OnNext(parentTextColor);
                }
                else if (elementStyles.Color.ValueType == ValueTypes.Value)
                {
                    element.TextColor.OnNext(elementStyles.Color.Value);
                }
                else
                    throw new NotImplementedException();
            };

            // Depends on element.Styles
            element.Styles.Subscribe(s => calcTextColor(s, element.Parent.Value.TextColor.Value));

            // Depends on element.Parent.TextColor
            IDisposable parentTextColorSubscription = null;
            element.Parent.Subscribe(p =>
            {
                parentTextColorSubscription?.Dispose();
                parentTextColorSubscription = p.TextColor.Subscribe(tc => calcTextColor(element.Styles.Value, tc));
            });
        }

        void SetupAncestorChain(Box element)
        {
            IEnumerable<Box> calcAncestorChain(IEnumerable<Box> parentAncestors, Box e)
            {
                yield return e;
                foreach (var ancestor in parentAncestors)
                    yield return ancestor;
            }

            // Depends on element.Parent.AncestorChain
            IDisposable parentAncestorChainSubscription = null;
            element.Parent.Subscribe(p =>
            {
                parentAncestorChainSubscription?.Dispose();
                parentAncestorChainSubscription = p.AncestorChain.Subscribe(pa => element.AncestorChain.OnNext(calcAncestorChain(pa, element)));
            });
        }

        void SetupBackgroundColor(Box element)
        {
            element.Styles.Subscribe(c =>
            {
                element.BackgroundColor.OnNext(c.BackgroundColor == null ? Color.Transparent : c.BackgroundColor.Value);
            });
        }

        void SetupDisplay(Box element)
        {
            Action<StyleruleCollection> calcDisplay = (styles) =>
            {
                // TODO: Determine what to do if display is not defined
                if (styles.Display != null)
                    element.Display.OnNext(styles.Display);
                else
                    element.Display.OnNext(BlockDisplayTypes.Block);
            };

            element.Styles.Subscribe(calcDisplay);
        }

        void SetupWidth(Box element)
        {
            Action<BlockDisplayTypes, float> calcWidth = (display, parentWidth) => {
                if(display == BlockDisplayTypes.Block)
                {
                    element.Width.OnNext(parentWidth);
                } else
                {
                    throw new NotImplementedException();
                }
            };

            // Depends on element.Display
            element.Display.Subscribe(d => calcWidth(d, element.Parent.Value.Width.Value));

            // Depends on element.Parent.Width
            IDisposable parentWidthSubscription = null;
            element.Parent.Subscribe(p =>
            {
                parentWidthSubscription?.Dispose();
                parentWidthSubscription = p.Width.Subscribe(w => calcWidth(element.Display.Value, w));
            });


            element.Height.OnNext(100);
        }

        void SetupTextContent(Box element)
        {
            if(element.HtmlElement is TextElement te)
                element.Text.OnNext(te.Content);
        }

        void SetupStyles(Box b, StyleruleCollection styles)
        {
            //// Determine SIZE and POSITION
            //{
            //    var position = result.Styles.Position;

            //    // Default values
            //    if (position == null)
            //        position = BoxPositionTypes.Static;

            //    result.Position = position;

            //    // Determine the containing block
            //    Rectangle containingBlock;
            //    switch (position.Value)
            //    {
            //        case BoxPositionTypes.Relative:
            //        case BoxPositionTypes.Sticky:
            //            containingBlock = parent.SizeBox;
            //            break;
            //        case BoxPositionTypes.Fixed:
            //            containingBlock = root.SizeBox;
            //            break;
            //        case BoxPositionTypes.Absolute:
            //            containingBlock = parent.AncestorChain.FirstOrDefault(b => b.Position != BoxPositionTypes.Static)?.SizeBox ?? root.SizeBox;
            //            break;
            //        default:
            //            throw new NotImplementedException();
            //    }
            //}
        }
    }
}
