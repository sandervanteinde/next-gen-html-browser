using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NextGen.HTMLParser;
using System.IO;
using NextGen.CSSParser;
using NextGen.ViewEngine;
using NextGen.HTMLParser.Elements;
using System.Drawing;

namespace NextGen.Browser
{
    public class BrowserPanel : Panel
    {
        private HTMLDocument document;
        private string documentPath;
        private StyleEngine styleEngine;
        internal void SetDocument(HTMLDocument document, string path)
        {
            this.document?.Dispose();
            this.document = document;
            documentPath = path;
            var engine = new StyleEngine();
            foreach (var style in FindStyleDocuments())
                engine.LoadDefinition(style);
            this.styleEngine = engine;
            Invalidate();
        }
        private IEnumerable<StyleDefinition> FindStyleDocuments()
        {
            foreach(var style in document.HeadElement.FindAll("style"))
            {
                yield return LoadStyleFromText(style.Content);
            }
            foreach(var style in document.HeadElement.FindAll("link")
                                    .Where(c => c.Attributes["rel"]?.Value == "stylesheet"))
            {
                var href = style.Attributes["href"];
                if (href == null)
                    continue;
                yield return LoadStyleFromUrl(href.Value);
            }
        }
        private StyleDefinition LoadStyleFromUrl(string url)
        {
            string str = Path.GetDirectoryName(documentPath);
            return LoadStyleFromText(File.ReadAllText(Path.Combine(str, url)));
        }
        private StyleDefinition LoadStyleFromText(string cssText)
        {
            return new AwesomeCssParser().ParseFromString(cssText);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (document == null) return;

            RenderElement(document.BodyElement, styleEngine, ClientRectangle, e.Graphics);
        }

        private void RenderElement(DOMElement element, StyleEngine styleEngine, Rectangle rect, Graphics g)
        {
            var elementStyles = styleEngine.GetCalculatedStylesForElement(element);

            // Search for background color
            if(elementStyles.BackgroundColor != null)
            {
                using(var b = new SolidBrush(elementStyles.BackgroundColor))
                {
                    g.FillRectangle(b, rect);
                }
            }

            // Search for text content
            if(element is TextElement te)
            {
                g.DrawString(te.Content, SystemFonts.DefaultFont, new SolidBrush(Color.Black), new Point(rect.Left, 0));
            }

            // Recurse
            foreach(var child in element.Children)
            {
                RenderElement(child, styleEngine, rect, g);
            }
        }
    }
}
