using NextGen.HTMLParser.Attributes;
using NextGen.HTMLParser.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NextGen.HTMLParser
{
    public class HTMLReader 
    {
        private string contents;
        private int position;
        private Regex nodeRegex = new Regex(@"<(!?\w+)(( ?\w+(=['""].*['""])?)*)>", RegexOptions.Compiled);
        private Regex attributeRegex = new Regex(@"(\w+)=([""'])(.*)\2", RegexOptions.Compiled);
        private Stack<DOMElement> domTree = new Stack<DOMElement>();
        public AttributeParser AttributeParser { get; set; } = new AttributeParser();
        public HTMLReader(string contents)
        {
            this.contents = contents
                .Trim(' ', '\n', '\r');
        }
        public DOMElement ReadHeader()
        {
            var element = ReadElement();
            if (element.Name != "!DOCTYPE")
                throw new Exception("HTML Document does not have the proper element");
            return element;
        }

        public DOMElement ReadContent()
        {
            DOMElement html = ReadElement();
            if (html.Name != "html")
                throw new Exception($"Invalid start tag of html document. Expected html, got {html.Name}");
            domTree.Push(html);
            while(domTree.Count > 0)
            {
                var newElement = ReadElement();
                if (newElement == null) continue; //end tag
                var parent = domTree.Peek();
                parent.Children.Add(newElement);
                newElement.Parent = parent;
                domTree.Push(newElement);
            }
            return html;
        }

        private IEnumerable<DOMAttribute> ParseAttributes(string attributeString)
        {
            return AttributeParser.Parse(attributeString);
            /*List<DOMAttribute> attributes = new List<DOMAttribute>(attrParser.Parse(attributeString));
            return
            foreach(string kvp in attributeString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (kvp.IndexOf('=') == -1)
                    yield return new DOMAttribute { Name = kvp };
                else
                {
                    var match = attributeRegex.Match(kvp);
                    if (!match.Success)
                        throw new Exception("Attribute is fucked");
                    yield return new DOMAttribute { Name = match.Groups[1].Value, Value = match.Groups[3].Value };
                }
            }*/
        }
        private DOMElement ParseNode(string node, int startIndex, int endIndex)
        {
            if (node.StartsWith("</"))
            {
                string name = node.Substring(2, node.Length - 3);
                var topElement = domTree.Pop();
                while (topElement.Name != name)
                {
                    if(topElement.RequireEndTag)
                        throw new Exception($"Wrong end tag. Expected tag {topElement.Name}, but got tag {name}!");
                    topElement = domTree.Pop();
                }
                topElement.EndBodyIndex = startIndex - 1;
                topElement.Content = contents.Substring(topElement.BodyIndex, topElement.EndBodyIndex - topElement.BodyIndex + 1).Trim(' ', '\n', '\r');
                return null;
            }
            var result = nodeRegex.Match(node);
            if (!result.Success)
                throw new Exception("This node is fucked");
            var element = HTMLReaderHelper.GetDOMElementForType(result.Groups[1].Value);
            element.BodyIndex = position;
            string attributes = result.Groups[2].Value.Trim(' ');
            if (attributes.Length > 0)
                element.Attributes.AddRange(ParseAttributes(attributes));
            return element;
        }
        private DOMElement ReadElement()
        {
            int startTag = contents.IndexOf('<', position);

            var leftOverText = contents.Substring(position, startTag - position).Trim();
            if(!string.IsNullOrEmpty(leftOverText))
            {
                // TODO: Handle input tags on top of stack
                domTree.Peek().Children.Add(new TextElement { Content = leftOverText });
            }

            int endTag = contents.IndexOf('>', position);
            string tag = contents.Substring(startTag, endTag - startTag + 1);
            position = endTag + 1;
            return ParseNode(tag, startTag, endTag);
            
        }
    }
}
