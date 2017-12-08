using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextGen.HTMLParser.Tests
{
    [TestClass]
    public class HtmlWithAttributesTests
    {
        private const string attributeHtml = @"
<!DOCTYPE html>
<html>
    <head>
    </head>
    <body>
        <input type='text' value='hello'></input>
        <img src='http://www.google.nl/logo.png' alt='Googlelogo!'></img>
    </body>
</html>";
        [TestMethod]
        public void TestParser()
        {
            var doc = new HTMLDocument(attributeHtml);
            doc.Parse();
        }
        [TestMethod]
        public void TestAttributes()
        {
            var doc = new HTMLDocument(attributeHtml);
            doc.Parse();

            var inputEl = doc.BodyElement.FindFirst("input");
            var typeAttr = inputEl.Attributes["type"];
            Assert.IsNotNull(typeAttr);
            Assert.AreEqual("text", typeAttr.Value);

            var value = inputEl.Attributes["value"];
            Assert.IsNotNull(value);
            Assert.AreEqual("hello", value.Value);
        }
    }
}
