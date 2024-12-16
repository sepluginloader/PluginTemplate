using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;

namespace ClientPlugin.Settings.Elements
{
    internal class TextboxAttribute : Attribute, IElement
    {
        public readonly string Label;
        public readonly string Description;

        public TextboxAttribute(string label = null, string description = null)
        {
            Label = label;
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            var textBox = new MyGuiControlTextbox(defaultText: (string)propertyGetter());
            textBox.TextChanged += (box)=>propertySetter(box.Text);
            textBox.SetToolTip(Description);

            var label = Tools.GetLabelOrDefault(name, Label);
            return new List<MyGuiControlBase>()
            {
                new MyGuiControlLabel(text: label),
                textBox,
            };
        }
        public List<Type> SupportedTypes { get; } = new List<Type>()
        {
            typeof(string)
        };
    }
}
