using NextGen.CSSParser.Styles.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles.PropertyParsers
{
    internal class DisplayRule : AbstractPropertyRule<BlockDisplayTypes>
    {
        public override string PropertyName => "display";

        protected override void Execute(string propertyValue, StylePropertyValue<BlockDisplayTypes> prop)
        {
            switch(propertyValue)
            {
                case "block":
                    prop.Value = BlockDisplayTypes.Block;
                    break;
                case "inline":
                    prop.Value = BlockDisplayTypes.Inline;
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
