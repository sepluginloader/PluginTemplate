using Sandbox;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Utils;

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
            var label = new MyGuiControlLabel();

            void ValueUpdate(MyGuiControlSlider element)
            {
                switch (Type)
                {
                    case SliderType.Integer:
                        int intValue = Convert.ToInt32(element.Value);
                        propertySetter(intValue);
                        label.Text = intValue.ToString();
                        break;

                    case SliderType.Float:
                        propertySetter(element.Value);
                        label.Text = MyValueFormatter.GetFormatedFloat(element.Value, element.LabelDecimalPlaces);
                        break;
                }
            }

            bool SpecifyValue(MyGuiControlSlider element)
            {
                MyGuiScreenDialogAmount screen = new MyGuiScreenDialogAmount(
                    Min,
                    Max,
                    MyCommonTexts.DialogAmount_SetValueCaption,
                    defaultAmount: Convert.ToSingle(propertyGetter()),
                    parseAsInteger: Type == SliderType.Integer,
                    backgroundTransition: MySandboxGame.Config.UIBkOpacity,
                    guiTransition: MySandboxGame.Config.UIOpacity);

                screen.OnConfirmed += (value) => element.Value = value;

                // Much needed visual change requires reflection due to private types
                typeof(MyGuiScreenBase)
                    .GetField("m_canHideOthers", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(screen, true);

                MyGuiSandbox.AddScreen(screen);
                return true;
            }

            var slider = new MyGuiControlSlider(
                toolTip: Description,
                defaultValue: Convert.ToSingle(propertyGetter()),
                minValue: Min,
                maxValue: Max,
                intValue: Type == SliderType.Integer)
            {
                MinimumStepOverride = Step,
            };

            slider.ValueChanged += ValueUpdate;
            slider.SliderSetValueManual = SpecifyValue;

            ValueUpdate(slider);

            return new List<MyGuiControlBase>()
            {
                new MyGuiControlLabel(text: name),
                label,
                slider,
            };
        }

        public List<Type> SupportedTypes { get; } = new List<Type>()
        {
            typeof(float),
            typeof(int),
        };
    }
}
