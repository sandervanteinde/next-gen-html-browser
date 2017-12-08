using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    public class HTMLDocument : IDisposable
    {
        public DOMElement HTMLElement { get; private set; }
        public DOMElement HeadElement => HTMLElement.Children[0];
        public DOMElement BodyElement => HTMLElement.Children[1];
        public DOMElement DocumentHeader { get; private set; }
        private Stream stream;
        public HTMLDocument(string htmlAsString)
        {
            stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlAsString));
        }
        public HTMLDocument(FileStream file)
        {
            stream = file;
        }

        public void Parse()
        {
            var streamREader = new StreamReader(stream);
            var fileContents = streamREader.ReadToEnd();
            var reader = new HTMLReader(fileContents);
            DocumentHeader = reader.ReadHeader();
            HTMLElement = reader.ReadContent();
            if (HTMLElement.Children[0].Name != "head")
                throw new Exception("Invalid formatted html element. First child of html is not head!");
            if (HTMLElement.Children[1].Name != "body")
                throw new Exception("Invalid formatted html element. Second child of html is not body!");
        }

        public void Dispose()
        {
            stream?.Dispose();
        }
    }
}
