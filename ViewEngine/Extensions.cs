using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewEngine
{
    public static class Extensions
    {
        public static Rectangle Add(this Rectangle rect, Point p)
        {
            return new Rectangle(
                new Point(
                    rect.Location.X + p.X,
                    rect.Location.Y + p.Y),
                rect.Size);
        }
    }
}
