using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRageMath;

namespace ClientPlugin.Settings.Elements
{
    class CheckboxAttribute : Attribute, IElement
    {
        public readonly string Description;

        public CheckboxAttribute(string description = null)
        {
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {               
            return new List<MyGuiControlBase>()
            {
                new MyGuiControlLabel(text: name),
                new MyGuiControlCheckbox(toolTip: Description)
                {
                    IsChecked = (bool)propertyGetter(),
                    IsCheckedChanged = (x) => propertySetter(x.IsChecked),
                }
            };
        }
    }
}