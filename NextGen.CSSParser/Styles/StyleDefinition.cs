using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser
{
    public class StyleDefinition
    {
        /// <summary>
        /// All blocks in this definition;
        /// </summary>
        public IEnumerable<StyleBlock> Blocks => _blocks.AsReadOnly();

        private List<StyleBlock> _blocks = new List<StyleBlock>();

        internal void Add(StyleBlock styleBlock)
        {
            _blocks.Add(styleBlock);
        }

        internal void AddRange(IEnumerable<StyleBlock> styleBlocks)
        {
            _blocks.AddRange(styleBlocks);
        }

        public static StyleDefinition operator +(StyleDefinition sd1, StyleDefinition sd2)
        {
            var result = new StyleDefinition();
            result.AddRange(sd1.Blocks);
            result.AddRange(sd2.Blocks);
            return result;
        }
    }
}