using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles
{
    public struct StyleRuleSpecificity
    {
        internal int StyleAttr;
        internal int IdAttr;
        internal int ClassAttr;
        internal int Element;

        public static bool operator ==(StyleRuleSpecificity srs1, StyleRuleSpecificity srs2)
        {
            return srs1.StyleAttr == srs2.StyleAttr
                && srs1.IdAttr == srs2.IdAttr
                && srs1.ClassAttr == srs2.ClassAttr
                && srs1.Element == srs2.Element;
        }

        public static bool operator !=(StyleRuleSpecificity srs1, StyleRuleSpecificity srs2)
        {
            return !(srs1 == srs2);
        }

        public static bool operator >(StyleRuleSpecificity srs1, StyleRuleSpecificity srs2)
        {
            return srs1.StyleAttr > srs2.StyleAttr
                || (srs1.StyleAttr == srs2.StyleAttr && srs1.IdAttr > srs2.IdAttr)
                || (srs1.StyleAttr == srs2.StyleAttr && srs1.IdAttr == srs2.IdAttr && srs1.ClassAttr > srs2.ClassAttr)
                || (srs1.StyleAttr == srs2.StyleAttr && srs1.IdAttr == srs2.IdAttr && srs1.ClassAttr == srs2.ClassAttr && srs1.Element > srs2.Element);
        }

        public static bool operator <(StyleRuleSpecificity srs1, StyleRuleSpecificity srs2)
        {
            return !(srs1 > srs2) && srs1 != srs2;
        }

        public static bool operator >=(StyleRuleSpecificity srs1, StyleRuleSpecificity srs2)
        {
            return srs1 > srs2 || srs1 == srs2;
        }

        public static bool operator <=(StyleRuleSpecificity srs1, StyleRuleSpecificity srs2)
        {
            return srs1 < srs2 && srs1 == srs2;
        }

        public override bool Equals(object obj)
        {
            return obj is StyleRuleSpecificity && this == (StyleRuleSpecificity)obj;
        }

        public override int GetHashCode()
        {
            var hashCode = -2033893809;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + StyleAttr.GetHashCode();
            hashCode = hashCode * -1521134295 + IdAttr.GetHashCode();
            hashCode = hashCode * -1521134295 + ClassAttr.GetHashCode();
            hashCode = hashCode * -1521134295 + Element.GetHashCode();
            return hashCode;
        }
    }
}
