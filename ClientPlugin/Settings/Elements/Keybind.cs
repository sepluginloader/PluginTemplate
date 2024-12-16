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
        public readonly string Label;
        public readonly string Description;

        public Func<MyKeys> PropertyGetter;
        public Action<MyKeys> PropertySetter;

        public KeybindAttribute(string label = null, string description = null)
        {
            Label = label;
            Description = description;
        }

        public List<MyGuiControlBase> GetElements(string name, Func<object> propertyGetter, Action<object> propertySetter)
        {
            PropertyGetter = ()=>(MyKeys)propertyGetter();
            PropertySetter = (MyKeys key)=>propertySetter(key);

            var label = new MyGuiControlLabel(text: Tools.GetLabelOrDefault(name, Label));

            var control = new MyControl(
                MyStringId.GetOrCompute($"{name.Replace(" ", "")}Keybind"),
                MyStringId.GetOrCompute(name),
                MyGuiControlTypeEnum.General,
                null,
                PropertyGetter());

            StringBuilder output = null;
            control.AppendBoundButtonNames(ref output, MyGuiInputDeviceEnum.Keyboard);
            MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));

            var button = new MyGuiControlButton(
                text: output,
                onButtonClick: OnRebindClick,
                onSecondaryButtonClick: OnUnbindClick,
                toolTip: Description)
            {
                VisualStyle = MyGuiControlButtonStyleEnum.ControlSetting,
                UserData = new ControlButtonData(control, MyGuiInputDeviceEnum.Keyboard),
            };

            return new List<MyGuiControlBase>() { label, button };
        }

        public List<Type> SupportedTypes { get; } = new List<Type>()
        {
            typeof(MyKeys)
        };

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

        private void OnUnbindClick(MyGuiControlButton button)
        {
            void Callback(MyGuiScreenMessageBox.ResultEnum result)
            {
                if (result == MyGuiScreenMessageBox.ResultEnum.NO)
                    return;

                var userData = (ControlButtonData)button.UserData;
                userData.Control.SetControl(userData.Device, MyKeys.None);

                StoreControl(button);
            }

            MyGuiScreenBase alert = MyGuiSandbox.CreateMessageBox(
                MyMessageBoxStyleEnum.Info,
                buttonType: MyMessageBoxButtonsType.YES_NO,
                messageText: new StringBuilder("Are you sure?"),
                messageCaption: new StringBuilder("UNBIND CONTROL"),
                yesButtonText: MyStringId.GetOrCompute("Confirm"),
                noButtonText: MyStringId.GetOrCompute("Cancel"),
                callback: Callback
            );

            MyGuiSandbox.AddScreen(alert);
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
