using NextGen.CSSParser.Styles;
using NextGen.CSSParser.Styles.DataTypes;
using NextGen.HTMLParser.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ViewEngine.Compositing
{
    public class Box
    {
        public DOMElement HtmlElement { get; internal set; }




        public readonly BehaviorSubject<StyleruleCollection> Styles = new BehaviorSubject<StyleruleCollection>(null);

        public readonly BehaviorSubject<float> Width = new BehaviorSubject<float>(0);

        public readonly BehaviorSubject<float> Height = new BehaviorSubject<float>(0);

        public readonly BehaviorSubject<float> X = new BehaviorSubject<float>(0);

        public readonly BehaviorSubject<float> Y = new BehaviorSubject<float>(0);

        public readonly BehaviorSubject<Box> Parent = new BehaviorSubject<Box>(null);

        public readonly BehaviorSubject<Box[]> Children = new BehaviorSubject<Box[]>(new Box[0]);

        public readonly BehaviorSubject<Color> BackgroundColor = new BehaviorSubject<Color>(Color.Transparent);

        public readonly BehaviorSubject<Color> TextColor = new BehaviorSubject<Color>(Color.Transparent);

        public readonly BehaviorSubject<string> Text = new BehaviorSubject<string>(null);

        public readonly BehaviorSubject<IEnumerable<Box>> AncestorChain = new BehaviorSubject<IEnumerable<Box>>(new Box[0]);

        public readonly BehaviorSubject<BlockDisplayTypes> Display = new BehaviorSubject<BlockDisplayTypes>(BlockDisplayTypes.Block);

        public readonly BehaviorSubject<SizeF> Size = new BehaviorSubject<SizeF>(new SizeF());

        public readonly BehaviorSubject<PointF> Location = new BehaviorSubject<PointF>(new PointF());

        public readonly BehaviorSubject<RectangleF> Rect = new BehaviorSubject<RectangleF>(new RectangleF());

        public readonly BehaviorSubject<BoxChildFormattingContext> ChildFormattingContext = new BehaviorSubject<BoxChildFormattingContext>(BoxChildFormattingContext.Block);

        public readonly BehaviorSubject<PointF> TextStartOffset = new BehaviorSubject<PointF>(PointF.Empty);

        public readonly BehaviorSubject<PointF> TextEndOffset = new BehaviorSubject<PointF>(PointF.Empty);
    }
}
