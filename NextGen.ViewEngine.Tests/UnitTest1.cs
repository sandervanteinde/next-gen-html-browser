using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextGen.CSSParser;
using NextGen.HTMLParser;

namespace NextGen.ViewEngine.Tests
{
    [TestClass]
    public class StyleEngineTest
    {
        private static string simpleCSS = @"
button {
    color:white;
    background-color:black;
    border-radius:6px;
}
button.large.larger {
    font-size: 2em;
}
button.large {
    font-size: 1.5em;
}
";
        private static string simpleHTML = @"<!DOCTYPE html><html><head><title>Test</title></head><body>
                    <header><p>This is the header</p></header>
                    <main><button class=""large larger"">This is the main</button></main>
                    <footer><p>This is the footer</p></footer>
                    </body></html>";

        [TestMethod]
        public void SimpleTestCase()
        {
            // Arrange
            var styles = new AwesomeCssParser().ParseFromString(simpleCSS);
            var styleEngine = new StyleEngine();
            styleEngine.LoadDefinition(styles);
            using (var reader = new HTMLDocument(simpleHTML))
            {
                reader.Parse();
                var buttonElement = reader.BodyElement.FindFirst("button");

                // Act
                var elementStyles = styleEngine.GetCalculatedStylesForElement(buttonElement);

                // Assert
            }
        }
    }
}
