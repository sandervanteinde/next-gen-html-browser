using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Helpers
{
    public static class ColorHelper
    {
        public static Dictionary<string, Color> NamedColors = new Dictionary<string, Color> {
            {"aliceblue",Color.FromArgb(240,248,255)},
{"antiquewhite",Color.FromArgb(250,235,215)},
{"aqua",Color.FromArgb(0,255,255)},
{"aquamarine",Color.FromArgb(127,255,212)},
{"azure",Color.FromArgb(240,255,255)},
{"beige",Color.FromArgb(245,245,220)},
{"bisque",Color.FromArgb(255,228,196)},
{"black",Color.FromArgb(0,0,0)},
{"blanchedalmond",Color.FromArgb(255,235,205)},
{"blue",Color.FromArgb(0,0,255)},
{"blueviolet",Color.FromArgb(138,43,226)},
{"brown",Color.FromArgb(165,42,42)},
{"burlywood",Color.FromArgb(222,184,135)},
{"cadetblue",Color.FromArgb(95,158,160)},
{"chartreuse",Color.FromArgb(127,255,0)},
{"chocolate",Color.FromArgb(210,105,30)},
{"coral",Color.FromArgb(255,127,80)},
{"cornflowerblue",Color.FromArgb(100,149,237)},
{"cornsilk",Color.FromArgb(255,248,220)},
{"crimson",Color.FromArgb(220,20,60)},
{"cyan",Color.FromArgb(0,255,255)},
{"darkblue",Color.FromArgb(0,0,139)},
{"darkcyan",Color.FromArgb(0,139,139)},
{"darkgoldenrod",Color.FromArgb(184,134,11)},
{"darkgray",Color.FromArgb(169,169,169)},
{"darkgrey",Color.FromArgb(169,169,169)},
{"darkgreen",Color.FromArgb(0,100,0)},
{"darkkhaki",Color.FromArgb(189,183,107)},
{"darkmagenta",Color.FromArgb(139,0,139)},
{"darkolivegreen",Color.FromArgb(85,107,47)},
{"darkorange",Color.FromArgb(255,140,0)},
{"darkorchid",Color.FromArgb(153,50,204)},
{"darkred",Color.FromArgb(139,0,0)},
{"darksalmon",Color.FromArgb(233,150,122)},
{"darkseagreen",Color.FromArgb(143,188,143)},
{"darkslateblue",Color.FromArgb(72,61,139)},
{"darkslategray",Color.FromArgb(47,79,79)},
{"darkslategrey",Color.FromArgb(47,79,79)},
{"darkturquoise",Color.FromArgb(0,206,209)},
{"darkviolet",Color.FromArgb(148,0,211)},
{"deeppink",Color.FromArgb(255,20,147)},
{"deepskyblue",Color.FromArgb(0,191,255)},
{"dimgray",Color.FromArgb(105,105,105)},
{"dimgrey",Color.FromArgb(105,105,105)},
{"dodgerblue",Color.FromArgb(30,144,255)},
{"firebrick",Color.FromArgb(178,34,34)},
{"floralwhite",Color.FromArgb(255,250,240)},
{"forestgreen",Color.FromArgb(34,139,34)},
{"fuchsia",Color.FromArgb(255,0,255)},
{"gainsboro",Color.FromArgb(220,220,220)},
{"ghostwhite",Color.FromArgb(248,248,255)},
{"gold",Color.FromArgb(255,215,0)},
{"goldenrod",Color.FromArgb(218,165,32)},
{"gray",Color.FromArgb(128,128,128)},
{"grey",Color.FromArgb(128,128,128)},
{"green",Color.FromArgb(0,128,0)},
{"greenyellow",Color.FromArgb(173,255,47)},
{"honeydew",Color.FromArgb(240,255,240)},
{"hotpink",Color.FromArgb(255,105,180)},
{"indianred",Color.FromArgb(205,92,92)},
{"indigo",Color.FromArgb(75,0,130)},
{"ivory",Color.FromArgb(255,255,240)},
{"khaki",Color.FromArgb(240,230,140)},
{"lavender",Color.FromArgb(230,230,250)},
{"lavenderblush",Color.FromArgb(255,240,245)},
{"lawngreen",Color.FromArgb(124,252,0)},
{"lemonchiffon",Color.FromArgb(255,250,205)},
{"lightblue",Color.FromArgb(173,216,230)},
{"lightcoral",Color.FromArgb(240,128,128)},
{"lightcyan",Color.FromArgb(224,255,255)},
{"lightgoldenrodyellow",Color.FromArgb(250,250,210)},
{"lightgray",Color.FromArgb(211,211,211)},
{"lightgrey",Color.FromArgb(211,211,211)},
{"lightgreen",Color.FromArgb(144,238,144)},
{"lightpink",Color.FromArgb(255,182,193)},
{"lightsalmon",Color.FromArgb(255,160,122)},
{"lightseagreen",Color.FromArgb(32,178,170)},
{"lightskyblue",Color.FromArgb(135,206,250)},
{"lightslategray",Color.FromArgb(119,136,153)},
{"lightslategrey",Color.FromArgb(119,136,153)},
{"lightsteelblue",Color.FromArgb(176,196,222)},
{"lightyellow",Color.FromArgb(255,255,224)},
{"lime",Color.FromArgb(0,255,0)},
{"limegreen",Color.FromArgb(50,205,50)},
{"linen",Color.FromArgb(250,240,230)},
{"magenta",Color.FromArgb(255,0,255)},
{"maroon",Color.FromArgb(128,0,0)},
{"mediumaquamarine",Color.FromArgb(102,205,170)},
{"mediumblue",Color.FromArgb(0,0,205)},
{"mediumorchid",Color.FromArgb(186,85,211)},
{"mediumpurple",Color.FromArgb(147,112,219)},
{"mediumseagreen",Color.FromArgb(60,179,113)},
{"mediumslateblue",Color.FromArgb(123,104,238)},
{"mediumspringgreen",Color.FromArgb(0,250,154)},
{"mediumturquoise",Color.FromArgb(72,209,204)},
{"mediumvioletred",Color.FromArgb(199,21,133)},
{"midnightblue",Color.FromArgb(25,25,112)},
{"mintcream",Color.FromArgb(245,255,250)},
{"mistyrose",Color.FromArgb(255,228,225)},
{"moccasin",Color.FromArgb(255,228,181)},
{"navajowhite",Color.FromArgb(255,222,173)},
{"navy",Color.FromArgb(0,0,128)},
{"oldlace",Color.FromArgb(253,245,230)},
{"olive",Color.FromArgb(128,128,0)},
{"olivedrab",Color.FromArgb(107,142,35)},
{"orange",Color.FromArgb(255,165,0)},
{"orangered",Color.FromArgb(255,69,0)},
{"orchid",Color.FromArgb(218,112,214)},
{"palegoldenrod",Color.FromArgb(238,232,170)},
{"palegreen",Color.FromArgb(152,251,152)},
{"paleturquoise",Color.FromArgb(175,238,238)},
{"palevioletred",Color.FromArgb(219,112,147)},
{"papayawhip",Color.FromArgb(255,239,213)},
{"peachpuff",Color.FromArgb(255,218,185)},
{"peru",Color.FromArgb(205,133,63)},
{"pink",Color.FromArgb(255,192,203)},
{"plum",Color.FromArgb(221,160,221)},
{"powderblue",Color.FromArgb(176,224,230)},
{"purple",Color.FromArgb(128,0,128)},
{"rebeccapurple",Color.FromArgb(102,51,153)},
{"red",Color.FromArgb(255,0,0)},
{"rosybrown",Color.FromArgb(188,143,143)},
{"royalblue",Color.FromArgb(65,105,225)},
{"saddlebrown",Color.FromArgb(139,69,19)},
{"salmon",Color.FromArgb(250,128,114)},
{"sandybrown",Color.FromArgb(244,164,96)},
{"seagreen",Color.FromArgb(46,139,87)},
{"seashell",Color.FromArgb(255,245,238)},
{"sienna",Color.FromArgb(160,82,45)},
{"silver",Color.FromArgb(192,192,192)},
{"skyblue",Color.FromArgb(135,206,235)},
{"slateblue",Color.FromArgb(106,90,205)},
{"slategray",Color.FromArgb(112,128,144)},
{"slategrey",Color.FromArgb(112,128,144)},
{"snow",Color.FromArgb(255,250,250)},
{"springgreen",Color.FromArgb(0,255,127)},
{"steelblue",Color.FromArgb(70,130,180)},
{"tan",Color.FromArgb(210,180,140)},
{"teal",Color.FromArgb(0,128,128)},
{"thistle",Color.FromArgb(216,191,216)},
{"tomato",Color.FromArgb(255,99,71)},
{"turquoise",Color.FromArgb(64,224,208)},
{"violet",Color.FromArgb(238,130,238)},
{"wheat",Color.FromArgb(245,222,179)},
{"white",Color.FromArgb(255,255,255)},
{"whitesmoke",Color.FromArgb(245,245,245)},
{"yellow",Color.FromArgb(255,255,0)},
{"yellowgreen",Color.FromArgb(154,205,50)}
        };

        public static Color ParseColor(string colorString)
        {
            // TODO: Support rgb()
            // TODO: Support rgba()
            // TODO: Support hsl()
            // TODO: Support 3 letter colors (ccc => #cccccc)

            // Named color
            colorString = colorString.ToLowerInvariant();
            if (NamedColors.ContainsKey(colorString))
                return NamedColors[colorString];

            // Strip # from start
            if (colorString.StartsWith("#"))
                colorString = colorString.Substring(1);

            // Parse Color as hex value (yes, this is really how it works)
            colorString = new string(colorString.ToCharArray().Select(c => "0123456789abcdef".Contains(c) ? c : '0').ToArray());
            colorString = colorString.PadRight((int)Math.Ceiling(colorString.Length / 3.0f) * 3, '0');
            var s = colorString.Length / 3;
            var redString = colorString.Substring(0, s).Substring(0, 2);
            var greenString = colorString.Substring(s, s).Substring(0, 2);
            var blueString = colorString.Substring(s * 2).Substring(0, 2);

            return Color.FromArgb(
                Convert.ToInt32(redString, 16),
                Convert.ToInt32(greenString, 16),
                Convert.ToInt32(blueString, 16)
                );
        }
    }
}
