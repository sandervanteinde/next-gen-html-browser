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

        public BrowserPanel()
        {
            DoubleBuffered = true;
        }

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
            foreach (var style in document.HeadElement.FindAll("link")
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
            var viewmodel = compositingEngine.CreateViewModel(document.BodyElement, styleEngine);
            viewmodel.X.OnNext(ClientRectangle.Left);
            viewmodel.Y.OnNext(ClientRectangle.Top);
            viewmodel.Width.OnNext(ClientRectangle.Width);
            Height = (int) viewmodel.Children.Value[0].Height.Value;

            // Render the element
            e.Graphics.Clear(Color.White);
            RenderElement(viewmodel, e.Graphics);
        }

        private void RenderElement(Box b, Graphics g)
        {
            // Render properties
            using (var brush = new SolidBrush(b.BackgroundColor.Value))
            {
                g.FillRectangle(brush, b.Rect.Value);
            }

            // Render content if present
            if (!string.IsNullOrEmpty(b.Text.Value))
                using (var brush = new SolidBrush(b.TextColor.Value))
                    g.DrawString(b.Text.Value, SystemFonts.DefaultFont, brush, b.Rect.Value, StringFormat.GenericTypographic);

            // Render debug markers
            if (!string.IsNullOrEmpty(b.Text.Value))
            {
                g.FillRectangle(new SolidBrush(Color.Yellow),
                    new RectangleF(b.TextEndOffset.Value, new Size(10, 10)));
            }

            // Recurse
            foreach (var child in b.Children.Value)
            {
                RenderElement(child, g);
            }
        }
    }
}
