using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextGen.HTMLParser.Elements;

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
        <input type='tel' value='06-12345678'>
        <img src='http://www.google.nl/logo.png' alt='Google logo!'>
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
            Assert.AreEqual("tel", typeAttr.Value);
            Assert.AreEqual(InputElementType.Tel, ((InputElement)inputEl).Type);

            var value = inputEl.Attributes["value"];
            Assert.IsNotNull(value);
            Assert.AreEqual("06-12345678", value.Value);

            var imgEl = doc.BodyElement.FindFirst("img");
            var srcAttr = imgEl.Attributes["src"];
            Assert.IsNotNull(srcAttr);
            Assert.AreEqual("http://www.google.nl/logo.png", srcAttr.Value);

            var altAttr = imgEl.Attributes["alt"];
            Assert.IsNotNull(altAttr);
            Assert.AreEqual("Google logo!", altAttr.Value);
        }
    }
}
