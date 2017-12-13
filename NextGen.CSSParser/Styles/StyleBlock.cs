using NextGen.CSSParser.Styles;
using System.Collections.Generic;

namespace NextGen.CSSParser
{
    public class StyleBlock
    {
        /// <summary>
        /// The selector for this style block
        /// </summary>
        public StyleSelector Selector { get; internal set; }

        /// <summary>
        /// The rules in this style block.
        /// </summary>
        public StyleruleCollection Rules { get; internal set; }
    }
}