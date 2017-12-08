using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.HTMLParser.Elements
{
    [DOMElementFor("input")]
    public class InputElement : DOMElement
    {
        public InputElementType Type {
            get
            {
                var attr = Attributes["type"];
                InputElementType type;
                if (attr == null || !Enum.TryParse(attr.Value, true, out type))
                    return InputElementType.Text;
                return type;
            }
        }
        public InputElement()
            : base("input")
        {
            RequireEndTag = false;
        }
    }
    public enum InputElementType
    {
        Text = 0,

        Button,
        CheckBox,
        Color,
        Date,
        DateTimeLocal,
        Email,
        File,
        Hidden,
        Image,
        Month,
        Number,
        Password,
        Radio,
        Range,
        Reset,
        Search,
        Submit,
        Tel,
        Time,
        Url,
        Week
    }
}
