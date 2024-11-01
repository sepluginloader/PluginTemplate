using ClientPlugin.Settings;
using ClientPlugin.Settings.Elements;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using System.Xml.Serialization;
using VRage.Input;
using VRageMath;


namespace ClientPlugin
{
    public enum DropDownEnum
    {
        Alpha,
        Beta,
        Gamma,
        Delta,
        Epsilon
    }

    public class Config
    {
        public static readonly Config Default = new Config();
        public static readonly Config Current = Storage.Load();

        // Build your UI
        public readonly string Title = "Config";

        [Keybind("Test Keybind Tooltip")]
        public MyKeys TestKeybind { get; set; } = MyKeys.None;

        [Textbox("Test Text Input Tooltip")]
        public string TextInput { get; set; } = "Default Text";

        [Dropdown(description: "Test Dropdown Tooltip")]
        public DropDownEnum DropdownTest { get; set; } = DropDownEnum.Alpha;

        [Slider(-5f, 4.5f, 0.5f, SliderAttribute.SliderType.Float, description: "Test Float Slider Tooltip")]
        public float SliderFloatTest { get; set; } = 0.1f;

        [Slider(-1f, 10f, 1f, SliderAttribute.SliderType.Integer, description: "Test Int Slider Tooltip")]
        public int SliderIntTest { get; set; } = 2;

        [XmlIgnore]
        [Button("Test Button Tooltip")]
        public Action TestButton { get; set; } = () =>
        {
            MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(
                MyMessageBoxStyleEnum.Info,
                buttonType: MyMessageBoxButtonsType.OK,
                messageText: new StringBuilder("You clicked me!"),
                messageCaption: new StringBuilder("Custom Button Function"),
                size: new Vector2(0.6f, 0.7f)
            ));
        };
    }
}