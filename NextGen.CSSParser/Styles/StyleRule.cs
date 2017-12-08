namespace NextGen.CSSParser
{
    public class StyleRule
    {
        public string Name { get; internal set; }
        public string Value { get; internal set; }
        public bool Important { get; internal set; }
    }
}