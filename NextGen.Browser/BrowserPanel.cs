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
using ViewEngine;
using ViewEngine.Compositing;

namespace NextGen.Browser
{
    public class BrowserPanel : Panel
    {
        private HTMLDocument document;
        private string documentPath;
        private StyleEngine styleEngine = new StyleEngine();
        private CompositingEngine compositingEngine = new CompositingEngine();

        internal void SetDocument(HTMLDocument document, string path)
        {
            this.document?.Dispose();
            this.document = document;
            documentPath = path;
            foreach (var style in FindStyleDocuments())
                styleEngine.LoadDefinition(style);
            Invalidate();
        }
        private IEnumerable<StyleDefinition> FindStyleDocuments()
        {
            // First style sheet is the useragent
            yield return LoadStyleFromText(File.ReadAllText("./useragent.css"));

            // Load all inline styles
            foreach (var style in document.HeadElement.FindAll("style"))
            {
                yield return LoadStyleFromText(style.Content);
            }

            // Load all linked stylesheets
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

            // Build the viewmodel
            var viewmodel = compositingEngine.CreateViewModel(document.BodyElement, styleEngine, ClientRectangle);

            // Render the element
            RenderElement(viewmodel, ClientRectangle, e.Graphics);
        }

        private void RenderElement(Box b, Rectangle rect, Graphics g)
        {
            // Determine some vars
            var currentRect = b.Rect.Add(rect.Location);

            // Render properties
            if(b.Styles.BackgroundColor != null)
            {
                using(var brush = new SolidBrush(b.Styles.BackgroundColor))
                {
                    g.FillRectangle(brush, currentRect);
                }
            }

            // Render content if present
            if(b.Text != null)
            {
                using (var brush = new SolidBrush(b.Styles.Color))
                {
                    g.DrawString(b.Text, SystemFonts.DefaultFont, brush, b.Rect);
                }
            }

            // Recurse
            foreach(var child in b.Children)
            {
                RenderElement(child, currentRect, g);
            }
        }
    }
}
