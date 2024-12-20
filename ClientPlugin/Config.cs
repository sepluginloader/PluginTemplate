using ClientPlugin.Settings;
using ClientPlugin.Settings.Elements;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Input;
using VRageMath;


namespace ClientPlugin
{
    public enum ExampleEnum
    {
        FirstAlpha,
        SecondBeta,
        ThirdGamma,
        AndTheDelta,
        Epsilon
    }

    public class Config : INotifyPropertyChanged
    {
        #region Options

        // TODO: Define your configuration options and their default values
        private bool toggle = true;
        private int integer = 2;
        private float number = 0.1f;
        private string text = "Default Text";
        private ExampleEnum dropdown = ExampleEnum.FirstAlpha;
        private Color color = Color.Cyan;
        private Color colorWithAlpha = new Color(0.8f, 0.6f, 0.2f, 0.5f);
        private Binding keybind = new Binding(MyKeys.None);

        #endregion

        #region User interface

        // TODO: Settings dialog title
        public readonly string Title = "Config Demo";

        // TODO: Settings dialog controls, one property for each configuration option

        [Checkbox(description: "Checkbox Tooltip")]
        public bool Toggle
        {
            get => toggle;
            set => SetField(ref toggle, value);
        }

        [Slider(-1f, 10f, 1f, SliderAttribute.SliderType.Integer, description: "Integer Slider Tooltip")]
        public int Integer
        {
            get => integer;
            set => SetField(ref integer, value);
        }

        [Slider(-5f, 4.5f, 0.5f, SliderAttribute.SliderType.Float, description: "Float Slider Tooltip")]
        public float Number
        {
            get => number;
            set => SetField(ref number, value);
        }

        [Textbox(description: "Textbox Tooltip")]
        public string Text
        {
            get => text;
            set => SetField(ref text, value);
        }

        [Dropdown(description: "Dropdown Tooltip")]
        public ExampleEnum Dropdown
        {
            get => dropdown;
            set => SetField(ref dropdown, value);
        }

        [Color(description: "RGB color")]
        public Color Color
        {
            get => color;
            set => SetField(ref color, value);
        }

        [Color(hasAlpha: true, description: "RGBA color")]
        public Color ColorWithAlpha
        {
            get => colorWithAlpha;
            set => SetField(ref colorWithAlpha, value);
        }

        [Keybind(description: "Keybind Tooltip - Unbind by right clicking the button")]
        public Binding Keybind
        {
            get => keybind;
            set => SetField(ref keybind, value);
        }

        [Button(description: "Button Tooltip")]
        public void Button()
        {
            MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(
                MyMessageBoxStyleEnum.Info,
                buttonType: MyMessageBoxButtonsType.OK,
                messageText: new StringBuilder("You clicked me!"),
                messageCaption: new StringBuilder("Custom Button Function"),
                size: new Vector2(0.6f, 0.5f)
            ));
        }

        #endregion

        #region Property change notification bilerplate

        public static readonly Config Default = new Config();
        public static readonly Config Current = ConfigStorage.Load();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}