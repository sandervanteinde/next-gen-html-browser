using NextGen.CSSParser.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewEngine.Compositing
{
    public class Box
    {
        public Rectangle Rect { get; internal set; }

        public StyleruleCollection Styles { get; internal set; }

        public IEnumerable<Box> Children { get; internal set; }

        public string Text { get; internal set; }
    }
}
