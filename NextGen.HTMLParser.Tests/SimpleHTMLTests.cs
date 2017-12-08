using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextGen.HTMLParser.Tests
{
    [TestClass]
    public class SimpleHTMLTests
    {
        private const string simpleHTML = @"
<!DOCTYPE html>
<html>
  <head>
    <title>Test</title>
  </head>
  <body>
    <header>
       <p>This is the header</p>
    </header>
    <main>
      <p>This is the main</p>
    </main>
    <footer>
      <p>This is the footer</p>
    </footer>
   </body>
</html>";
        [TestMethod]
        public void TestSimpleHTMLFile()
        {
            var reader = new HTMLReader(simpleHTML);
            var doctype =reader.ReadHeader();
            var html = reader.ReadContent();
            Assert.AreEqual("!DOCTYPE", doctype.Name);
            Assert.AreEqual("html", html.Name);
            Assert.AreEqual(2, html.Children.Count);
            Assert.AreEqual("head", html.Children[0].Name);
            Assert.AreEqual("body", html.Children[1].Name);
            Assert.AreEqual("title", html.Children[0].Children[0].Name);
            Assert.AreEqual("header", html.Children[1].Children[0].Name);
            Assert.AreEqual("main", html.Children[1].Children[1].Name);
            Assert.AreEqual("footer", html.Children[1].Children[2].Name);
        }
        [TestMethod]
        public void TestFindMethod()
        {
            using (var reader = new HTMLDocument(simpleHTML))
            {
                reader.Parse();
                var p = reader.BodyElement.FindFirst("p");
                Assert.AreEqual("This is the header", p.Content);
            }
        }
        [TestMethod]
        public void TestFindAllMethod()
        {
            using (var reader = new HTMLDocument(simpleHTML))
            {
                reader.Parse();
                var p = reader.BodyElement.FindAll("p").ToArray();
                Assert.AreEqual(3, p.Length);
                Assert.AreEqual("This is the header", p[0].Content);
                Assert.AreEqual("This is the main", p[1].Content);
                Assert.AreEqual("This is the footer", p[2].Content);
            }
        }
    }
}
