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
    public enum DropDownEnum
    {
        Alpha,
        Beta,
        Gamma,
        Delta,
        Epsilon
    }

    public class Config : INotifyPropertyChanged
    {
        public static readonly Config Default = new Config();
        public static readonly Config Current = Storage.Load();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // ----- Build your UI -----
        public readonly string Title = "Config Demo";

        private MyKeys _keybind = MyKeys.None;
        [Keybind("Keybind Tooltip")]
        public MyKeys Keybind
        {
            get => _keybind;
            set => SetField(ref _keybind, value);
        }

        public string _text = "Default Text";
        [Textbox("Textbox Tooltip")]
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public DropDownEnum _dropdown = DropDownEnum.Alpha;
        [Dropdown(description: "Dropdown Tooltip")]
        public DropDownEnum Dropdown
        {
            get => _dropdown;
            set => SetField(ref _dropdown, value);
        }

        public float _number = 0.1f;
        [Slider(-5f, 4.5f, 0.5f, SliderAttribute.SliderType.Float, description: "Float Slider Tooltip")]
        public float Number
        {
            get => _number;
            set => SetField(ref _number, value);
        }

        public int _integer = 2;
        [Slider(-1f, 10f, 1f, SliderAttribute.SliderType.Integer, description: "Integer Slider Tooltip")]
        public int Integer
        {
            get => _integer;
            set => SetField(ref _integer, value);
        }

        public bool _toggle = true;
        [Checkbox(description: "Checkbox Tooltip")]
        public bool Toggle
        {
            get => _toggle;
            set => SetField(ref _toggle, value);
        }

        [Button("Button Tooltip")]
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
    }
}