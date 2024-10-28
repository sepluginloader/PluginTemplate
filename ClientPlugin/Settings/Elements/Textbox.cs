using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;

namespace ClientPlugin.Settings.Elements
{
    internal class TextboxAttribute : Attribute, IElement
    {
        public readonly string Description;

        public TextboxAttribute(string description = null)
        {
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            var textBox = new MyGuiControlTextbox(defaultText: (string)propertyGetter());
            textBox.TextChanged += (box)=>propertySetter(box.Text);

            return new List<MyGuiControlBase>()
            {
                new MyGuiControlLabel(text: name),
                textBox,
            };
        }
    }
}
