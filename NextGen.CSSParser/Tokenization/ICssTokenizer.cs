using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Tokenization
{
    internal interface ICssTokenizer
    {
        /// <summary>
        /// Whether to ignore whitespaces
        /// </summary>
        bool IgnoreWhiteSpace { get; set; }

        /// <summary>
        /// Whether the tokenizer has ended
        /// </summary>
        bool Ended { get; }

        /// <summary>
        /// Ensures the next character is the given one, else throws an exception
        /// </summary>
        /// <param name="v"></param>
        void Expect(char v);

        /// <summary>
        /// Checks if next character is the given one.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        bool NextIs(char v);

        /// <summary>
        /// Skip all following whitespace.
        /// </summary>
        void SkipWhiteSpace();

        /// <summary>
        /// Read until the given character (or the end)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        string ReadTo(char v);

        /// <summary>
        /// Read until any of the given characters (or the end)
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        string ReadToAny(params char[] chars);

        /// <summary>
        /// Checks if the next character is any of the given
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        bool NextIsAny(params char[] v);
    }
}
