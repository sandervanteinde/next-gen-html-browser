using NextGen.CSSParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Tokenization
{
    internal class StringTokenizer : ICssTokenizer
    {
        public bool IgnoreWhiteSpace { get; set; } = false;

        public bool Ended
        {
            get
            {
                return _pointer == _text.Length;
            }
        }

        private char CurrentChar => _text[_pointer];

        private readonly string _text;

        private int _pointer = 0;

        public StringTokenizer(string text)
        {
            _text = text;
        }

        public void SkipAllWhitespaceIfAllowed()
        {
            if (!IgnoreWhiteSpace) return;
            this.SkipWhiteSpace();
        }

        public void Expect(char v)
        {
            if (!NextIs(v))
                throw new InvalidStyleException();
            _pointer++;
        }

        public bool NextIs(char v)
        {
            SkipAllWhitespaceIfAllowed();
            return !Ended && v.Equals(CurrentChar);
        }

        public void SkipWhiteSpace()
        {
            while (_pointer < _text.Length && " \r\n\t".ToCharArray().Contains(CurrentChar))
            {
                _pointer++;
            }
        }

        public string ReadTo(char v)
        {
            var index = _text.IndexOf(v, _pointer);
            var result = _text.Substring(_pointer, index - _pointer);
            _pointer = index;
            return result;
        }

        public string ReadToAny(params char[] chars)
        {
            var index = _text.IndexOfAny(chars, _pointer);

            if (index == -1)
            {
                var result = _text.Substring(_pointer);
                _pointer = _text.Length;
                return result;
            }
            else
            {
                var result = _text.Substring(_pointer, index - _pointer);
                _pointer = index;
                return result;
            }
        }

        public bool NextIsAny(params char[] v)
        {
            SkipAllWhitespaceIfAllowed();
            return !Ended && v.Contains(CurrentChar);
        }
    }
}
