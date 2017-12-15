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

        public static (RectangleF rect, PointF end) MeasureStringAdvanced(this Graphics g, string text, Font f, float width, float offset)
        {
            if (width == 0) return (new RectangleF(), new PointF());

            // FIXME: This can be done a lot more efficient

            // Determine the number of characters on the first line
            float i = 0;
            var charsOnFirstLine = g.MeasureCharacterRangesAdvanced(text, f, new RectangleF(0, 0, width - offset, Int64.MaxValue)).TakeWhile(r => { var result = r.X >= i; i = r.X; return result; }).ToArray();

            if(charsOnFirstLine.Count() == text.Length)
            {
                return (new RectangleF(0, 0, width, charsOnFirstLine[0].Height), new PointF(0, charsOnFirstLine.Max(region => region.Right)));
            }

            // Get offset for next line
            var offsetNextLine = charsOnFirstLine[0].Height;

            // Determine range for text
            var text2 = text.Substring(charsOnFirstLine.Length).Trim();
            var format = new StringFormat(StringFormat.GenericTypographic);
            format.SetMeasurableCharacterRanges(new[] { new CharacterRange(text2.Length - 1, 1)});
            var region2 = g.MeasureCharacterRanges(text2, f, new RectangleF(0, offsetNextLine, width, Int64.MaxValue), format)[0].GetBounds(g);

            return (new RectangleF(0, 0, width, region2.Bottom), new PointF(region2.Top, region2.Right));
        }

        public static IEnumerable<RectangleF> MeasureCharacterRangesAdvanced(this Graphics g, string text, Font f, RectangleF r)
        {
            var format = new StringFormat(StringFormat.GenericTypographic);

            foreach(var range in Enumerable.Range(0, text.Length).Chunk(32).SelectMany(numberrange => {
                var characterRanges = numberrange.Select(i => new CharacterRange(i, 1)).ToArray();
                format.SetMeasurableCharacterRanges(characterRanges);
                return g.MeasureCharacterRanges(text, f, r, format).Select(cr => cr.GetBounds(g));
            }))
            {
                yield return range;
            }
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            if (chunksize <= 0) throw new ArgumentException("Chunk size must be greater than zero.", nameof(chunksize));

            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }
    }
}
