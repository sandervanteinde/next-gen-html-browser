using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser.Attributes
{
    public class AttributeParser
    {
        private enum ParseState
        {
            FindAttribute,
            AttributeFound,
            Done,
            ReadAttributeValue,
            CheckHasAttributeValue,
            AttributeFinished
        }
        public IEnumerable<DOMAttribute> Parse(string str)
        {
            ParseState state = ParseState.FindAttribute;
            int index = 0;
            var attr = new DOMAttribute();
            while(state != ParseState.Done)
            {
                switch (state)
                {
                    case ParseState.FindAttribute:
                        FindFirstNonSpaceNonEnter(str, ref index);
                        if (index >= str.Length)
                            state = ParseState.Done;
                        else
                            state = ParseState.AttributeFound;
                        break;
                    case ParseState.AttributeFound:
                        attr.Name = ReadAttribute(str, ref index);
                        if (index >= str.Length)
                            state = ParseState.AttributeFinished;
                        else if (str[index] == '=')
                        {
                            index++;
                            state = ParseState.ReadAttributeValue;
                        }
                        else
                            state = ParseState.CheckHasAttributeValue;
                        break;
                    case ParseState.CheckHasAttributeValue:
                        FindFirstNonSpaceNonEnter(str, ref index);
                        if (str[index] == '=')
                        {
                            index++;
                            state = ParseState.CheckHasAttributeValue;
                        }
                        else
                            state = ParseState.AttributeFinished;
                        break;
                    case ParseState.ReadAttributeValue:
                        attr.Value = ReadAttributeValue(str, ref index);
                        state = ParseState.AttributeFinished;
                        break;
                    case ParseState.AttributeFinished:
                        yield return attr;
                        attr = new DOMAttribute();
                        state = ParseState.FindAttribute;
                        break;

                }
            }
        }
        private string ReadAttributeValue(string str, ref int index)
        {
            int firstQuoteAttr = str.IndexOfAny(new[] { '\'', '"' }, index);
            char c = str[index];
            int endQuote = firstQuoteAttr;
            do
            {
                endQuote = str.IndexOf(c, endQuote + 1);
            }
            while (str[endQuote - 1] == '\\');
            string value = str.Substring(firstQuoteAttr + 1, endQuote - firstQuoteAttr - 1);
            index = endQuote + 1;
            return value;
        }
        private string ReadAttribute(string str, ref int index)
        {
            int endIndex = str.IndexOfAny(new[] { ' ', '=' }, index);
            if (endIndex == -1)
                endIndex = str.Length;
            string attrName = str.Substring(index, endIndex - index);
            index = endIndex;
            return attrName;
        }
        private void FindFirstNonSpaceNonEnter(string str, ref int index)
        {
            while(index < str.Length)
            {
                char c = str[index];
                if (
                    (c != ' ') && 
                    (c != '\n') &&
                    (c != '\t') &&
                    (c != '\r')
                )
                    return; 
                index++;
            }
        }
    }
}
