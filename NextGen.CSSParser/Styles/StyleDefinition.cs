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
    }
}
