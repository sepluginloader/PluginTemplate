using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPlugin.Settings.Elements
{
    internal class SliderAttribute : Attribute, IElement
    {
        public enum SliderType
        {
            Integer,
            Float,
        }

        public readonly float Min;
        public readonly float Max;
        public readonly float Step;
        public readonly SliderType Type;
        public readonly string Description;

        public SliderAttribute(float min, float max, float step = 1f, SliderType type = SliderType.Float, string description = null)
        {
            Min = min;
            Max = max;
            Step = step;
            Type = type;
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            void ValueUpdate(MyGuiControlSlider element)
            {
                switch (Type)
                {
                    case SliderType.Integer:
                        propertySetter(Convert.ToInt32(element.Value));
                        break;
                    case SliderType.Float:
                        propertySetter(element.Value);
                        break;
                }

            }

            var slider = new MyGuiControlSlider(
                toolTip: Description,
                defaultValue: Convert.ToSingle(propertyGetter()),
                minValue: Min,
                maxValue: Max,
                showLabel: false,
                intValue: Type == SliderType.Integer)
            {
                MinimumStepOverride = Step,
            };
            slider.ValueChanged += ValueUpdate;

            return new List<MyGuiControlBase>()
            {
                new MyGuiControlLabel(text: name),
                slider,
            };
        }
    }
}
