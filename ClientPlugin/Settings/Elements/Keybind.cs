using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRage;
using VRageMath;

namespace ClientPlugin.Settings.Elements
{
    internal class KeybindAttribute : Attribute, IElement
    {
        public readonly string Description;

        public Func<MyKeys> PropertyGetter;
        public Action<MyKeys> PropertySetter;

        public KeybindAttribute(string description = null)
        {
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            PropertyGetter = ()=>(MyKeys)propertyGetter();
            PropertySetter = (MyKeys key)=>propertySetter(key);

            var label = new MyGuiControlLabel(text: name);

            var control = new MyControl(
                MyStringId.GetOrCompute($"{name.Replace(" ", "")}Keybind"),
                MyStringId.GetOrCompute(name),
                MyGuiControlTypeEnum.General,
                null,
                PropertyGetter(),
                MyStringId.GetOrCompute(Description),
                null,
                MyStringId.GetOrCompute(Description));

            StringBuilder output = null;
            control.AppendBoundButtonNames(ref output, MyGuiInputDeviceEnum.Keyboard);
            MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));

            var button = new MyGuiControlButton(text: output, onButtonClick: OnRebindClick)
            {
                VisualStyle = MyGuiControlButtonStyleEnum.ControlSetting,
                UserData = new ControlButtonData(control, MyGuiInputDeviceEnum.Keyboard),
            };

            return new List<MyGuiControlBase>() { label, button };
        }

        private class ControlButtonData
        {
            public readonly MyControl Control;
            public readonly MyGuiInputDeviceEnum Device;

            public ControlButtonData(MyControl control, MyGuiInputDeviceEnum device)
            {
                Control = control;
                Device = device;
            }
        }

        private void OnRebindClick(MyGuiControlButton button)
        {
            var userData = (ControlButtonData)button.UserData;
            var messageText = MyCommonTexts.AssignControlKeyboard;
            if (userData.Device == MyGuiInputDeviceEnum.Mouse)
                messageText = MyCommonTexts.AssignControlMouse;

            // KEEN!!! MyGuiScreenOptionsControls.MyGuiControlAssignKeyMessageBox is PRIVATE!
            var screenClass = typeof(MyGuiScreenOptionsControls).GetNestedType(
                "MyGuiControlAssignKeyMessageBox",
                BindingFlags.NonPublic);

            var editBindingDialog = (MyGuiScreenBase)Activator.CreateInstance(
                screenClass,
                BindingFlags.CreateInstance,
                null,
                new object[]
                {
                    userData.Device,
                    userData.Control,
                    messageText
                },
                null);

            editBindingDialog.Closed += (s, isUnloading) => StoreControl(button);
            MyGuiSandbox.AddScreen(editBindingDialog);
        }

        private void StoreControl(MyGuiControlButton button)
        {
            StringBuilder output = null;
            var userData = (ControlButtonData)button.UserData;
            userData.Control.AppendBoundButtonNames(ref output, userData.Device);

            PropertySetter(userData.Control.GetKeyboardControl());

            MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
            button.Text = output.ToString();
            output.Clear();
        }
    }
}
