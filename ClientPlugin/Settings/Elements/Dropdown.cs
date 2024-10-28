using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClientPlugin.Settings.Elements
{
    internal class DropdownAttribute : Attribute, IElement
    {
        public readonly int VisibleRows;
        public readonly string Description;
        
        private static string UnCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        public DropdownAttribute(int visibleRows = 20, string description = null)
        {
            VisibleRows = visibleRows;
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            object selectedEnum = propertyGetter();
            Type choiceEnum = selectedEnum.GetType();

            var dropdown = new MyGuiControlCombobox(toolTip: Description);
            string[] elements = Enum.GetNames(choiceEnum);

            for (int i = 0; i < elements.Length; i++)
            {
                dropdown.AddItem(i, UnCamelCase(elements[i]));
            }

            void OnItemSelect()
            {
                long key = dropdown.GetSelectedKey();
                string value = elements[key];

                object enumValue = Enum.Parse(choiceEnum, value);
                propertySetter(enumValue);
            }

            dropdown.ItemSelected += OnItemSelect;
            dropdown.SelectItemByIndex(Convert.ToInt32(selectedEnum));

            return new List<MyGuiControlBase>()
            {
                new MyGuiControlLabel(text: name),
                dropdown,
            };
        }
    }
}
