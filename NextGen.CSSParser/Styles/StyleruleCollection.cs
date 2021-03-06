﻿using NextGen.CSSParser.Styles.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.CSSParser.Styles
{
    public class StyleruleCollection
    {
        private static string[] Properties = new[] {
            "background-color",
            "color",
            "display",
            "position"
        };

        public StylePropertyValue<Color> BackgroundColor
        {
            get => GetTypedProperty<Color>("background-color");
            set
            {
                SetProperty("background-color", value);
            }
        }

        public StylePropertyValue<Color> Color
        {
            get => GetTypedProperty<Color>("color");
            set
            {
                SetProperty("color", value);
            }
        }

        public StylePropertyValue<BlockDisplayTypes> Display
        {
            get => GetTypedProperty<BlockDisplayTypes>("display");
            set
            {
                SetProperty("display", value);
            }
        }

        public StylePropertyValue<BoxPositionTypes> Position
        {
            get => GetTypedProperty<BoxPositionTypes>("position");
            set
            {
                SetProperty("position", value);
            }
        }











        private Dictionary<string, AbstractStylePropertyValue> _props = new Dictionary<string, AbstractStylePropertyValue>();

        public StylePropertyValue<T> GetTypedProperty<T>(string index)
        {
            return GetProperty(index) as StylePropertyValue<T>;
        }

        public AbstractStylePropertyValue GetProperty(string index)
        {
            if (_props.ContainsKey(index))
                return _props[index];
            return null;
        }

        public void SetProperty(string index, AbstractStylePropertyValue value)
        {
            _props[index] = value;
        }

        public IEnumerable<(string name, AbstractStylePropertyValue value)> GetProperties()
        {
            return Properties.Select(name => (name, GetProperty(name)));
        }
    }
}
