using NextGen.CSSParser.Styles;
using NextGen.CSSParser.Styles.DataTypes;
using NextGen.HTMLParser.Elements;
using NextGen.ViewEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewEngine.Compositing;
using static NextGen.CSSParser.Styles.AbstractStylePropertyValue;

namespace ViewEngine
{
    public class CompositingEngine
    {
        private static Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        private Graphics Scratchpad = Graphics.FromImage(new Bitmap(10000, 10000));

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
            SetupHeight(result);
            SetupChildFormattingContext(result);
            SetupChildLocations(result);

            // Setup computed properties
            SetupSize(result);
            SetupLocation(result);
            SetupRect(result);

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
            Action<BlockDisplayTypes, float> calcWidth = (display, parentWidth) =>
            {
                if (display == BlockDisplayTypes.Block)
                {
                    element.Width.OnNext(parentWidth);
                }
                else if (display == BlockDisplayTypes.Inline)
                {
                    // TODO: Implement property
                    // This should depend on the size of all children
                    element.Width.OnNext(parentWidth);
                }
                else
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
        }

        void SetupSize(Box element)
        {
            Action<float, float> calcSize = (width, height) =>
             {
                 element.Size.OnNext(new SizeF(width, height));
             };

            element.Width.Subscribe(w => calcSize(w, element.Height.Value));
            element.Height.Subscribe(h => calcSize(element.Width.Value, h));
        }

        void SetupLocation(Box element)
        {
            Action<float, float> calcLocation = (x, y) =>
            {
                element.Location.OnNext(new PointF(x, y));
            };

            element.X.Subscribe(x => calcLocation(x, element.Y.Value));
            element.Y.Subscribe(y => calcLocation(element.X.Value, y));
        }

        void SetupRect(Box element)
        {
            Action<PointF, SizeF> calcRect = (point, size) =>
            {
                element.Rect.OnNext(new RectangleF(point, size));
            };

            element.Location.Subscribe(l => calcRect(l, element.Size.Value));
            element.Size.Subscribe(s => calcRect(element.Location.Value, s));
        }

        void SetupChildFormattingContext(Box element)
        {
            Action<BlockDisplayTypes, Box[]> calcChildFormattingContext = (display, children) =>
            {
                var context = display == BlockDisplayTypes.Inline
                        && children.All(c => c.ChildFormattingContext.Value == BoxChildFormattingContext.Inline)
                    ? BoxChildFormattingContext.Inline
                    : BoxChildFormattingContext.Block;
                element.ChildFormattingContext.OnNext(context);
            };

            // Depends on element.Display
            element.Display.Subscribe(display =>
                calcChildFormattingContext(
                    display,
                    element.Children.Value
                )
            );

            // Depends on element.Children.BlockFormattingContext
            IEnumerable<IDisposable> childrenBlockFormattingContextSubscriptions = new IDisposable[0];
            element.Children.Subscribe(children =>
            {
                foreach (var s in childrenBlockFormattingContextSubscriptions)
                    s.Dispose();
                childrenBlockFormattingContextSubscriptions = children.Select(c =>
                    c.ChildFormattingContext.Subscribe(_ =>
                        calcChildFormattingContext(
                            element.Display.Value,
                            element.Children.Value
                        )
                    )
                ).ToArray();
            });
        }

        void SetupHeight(Box element)
        {
            Action<Box[], string, float, PointF> calcHeight = (children, text, parentWidth, startOffset) =>
            {
                // No children
                if (children.Length == 0)
                {
                    // No text content
                    if (string.IsNullOrEmpty(text))
                    {
                        element.Height.OnNext(0);
                        return;
                    }

                    // Text content
                    if (!string.IsNullOrEmpty(text))
                    {
                        // Determine size of text
                        // var textSize = Scratchpad.MeasureString(text, SystemFonts.DefaultFont, (int)parentWidth);
                        // element.Height.OnNext(textSize.Height);
                        var s = Scratchpad.MeasureStringAdvanced(text, SystemFonts.DefaultFont, parentWidth, startOffset.X);
                        element.Height.OnNext(s.rect.Height);
                        element.TextEndOffset.OnNext(s.end);
                        return;
                    }

                    throw new NotImplementedException();
                }

                // Children
                if (children.Length > 0)
                {
                    // Determine max bottom of all child rects and derive the height
                    var maxbottom = children.Select(c => c.Rect.Value.Bottom).Max();
                    var height = maxbottom - element.Rect.Value.Top;
                    element.Height.OnNext(height);
                    return;
                }
            };

            // Depends on element.Children
            element.Children.Subscribe(children =>
                calcHeight(
                    children,
                    element.Text.Value,
                    element.Parent.Value.Width.Value,
                    element.TextStartOffset.Value
                    )
            );

            // Depends on element.Text
            element.Text.Subscribe(text =>
                calcHeight(
                    element.Children.Value,
                    text,
                    element.Parent.Value.Width.Value,
                    element.TextStartOffset.Value
                    )
            );

            // Depends on element.TextStartOffset
            element.TextStartOffset.Subscribe(offset =>
                calcHeight(
                    element.Children.Value,
                    element.Text.Value,
                    element.Parent.Value.Width.Value,
                    offset
                    )
            );

            // Depends on element.Parent.Width
            IDisposable parentWidthSubscription = null;
            element.Parent.Subscribe(p =>
            {
                parentWidthSubscription?.Dispose();
                parentWidthSubscription = p.Width.Subscribe(w =>
                calcHeight(
                    element.Children.Value,
                    element.Text.Value,
                    w,
                    element.TextStartOffset.Value
                    )
                );
            });

            // Depends on element.Children.Rect
            IEnumerable<IDisposable> childrenRectSubscriptions = new IDisposable[0];
            element.Children.Subscribe(children =>
            {
                foreach (var s in childrenRectSubscriptions)
                    s.Dispose();
                childrenRectSubscriptions = children.Select(c =>
                    c.Rect.Subscribe(_ =>
                        calcHeight(
                            element.Children.Value,
                            element.Text.Value,
                            element.Width.Value,
                            element.TextStartOffset.Value
                        )
                    )
                );
            });





            // TODO


            // Depends on element.Styles

            // Depends on element.Display
        }

        void SetupChildLocations(Box element)
        {
            Action<BoxChildFormattingContext, Box[]> calcFormattingContext = (context, children) =>
            {
                // Easiest case: no children
                if (children.Length == 0)
                    return;

                if (context == BoxChildFormattingContext.Block)
                {
                    var offset = element.Location.Value;
                    foreach (var child in children)
                    {
                        child.X.OnNext(offset.X);
                        child.Y.OnNext(offset.Y);
                        offset = new PointF(
                            offset.X,
                            offset.Y + child.Size.Value.Height
                            );
                    }
                    return;
                }

                if (context == BoxChildFormattingContext.Inline)
                {
                    // TODO
                }
            };


            // Depends on element.Children
            element.Children.Subscribe(children =>
                calcFormattingContext(element.ChildFormattingContext.Value, children)
            );

            // Depends on element.Children.Size
            IEnumerable<IDisposable> childrenRectSubscriptions = new IDisposable[0];
            element.Children.Subscribe(children =>
            {
                foreach (var s in childrenRectSubscriptions)
                    s.Dispose();
                childrenRectSubscriptions = children.Select(c =>
                    c.Size.Subscribe(_ =>
                        calcFormattingContext(
                            element.ChildFormattingContext.Value,
                            element.Children.Value
                        )
                    )
                ).ToArray();
            });

            // Depends on element.ChildFormattingContext
            element.ChildFormattingContext.Subscribe(context =>
                calcFormattingContext(context, element.Children.Value)
            );
        }

        void SetupTextContent(Box element)
        {
            if (element.HtmlElement is TextElement te)
                element.Text.OnNext(WhitespaceRegex.Replace(te.Content, " "));
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
