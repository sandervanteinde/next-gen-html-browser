using System.Collections.Generic;

namespace NextGen.CSSParser
{
    public class StyleSelector
    {
        /// <summary>
        /// Whether this is the catch-all selector (*)
        /// </summary>
        public bool SelectAll { get; internal set; }

        /// <summary>
        /// The tagname (if any, else null)
        /// </summary>
        public string TagName { get; internal set; }

        /// <summary>
        /// The id (if any, else null)
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// The list of classes (if any, else empty)
        /// </summary>
        public IEnumerable<string> Classes => _classes.AsReadOnly();

        private List<string> _classes = new List<string>();

        internal void AddClass(string s)
        {
            _classes.Add(s);
        }
    }
}