using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPlugin.Settings.Elements
{
    internal class ButtonAttribute : Attribute, IElement
    {
        public readonly string Description;

        public ButtonAttribute(string description = null)
        {
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            var button = new MyGuiControlButton(text: new StringBuilder(name), toolTip: Description);
            button.ButtonClicked += (_)=>((Action)propertyGetter())();

            return new List<MyGuiControlBase>() { button };
        }
    }
}
