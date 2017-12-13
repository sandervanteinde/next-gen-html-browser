using NextGen.CSSParser.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles.PropertyParsers
{
    internal class BackgroundColorRule : AbstractPropertyValueParser<Color>
    {
        public override string PropertyName => "background-color";

        protected override void Execute(string propertyValue, StylePropertyValue<Color> prop)
        {
            prop.Value = ColorHelper.ParseColor(propertyValue);
        }
    }
}
