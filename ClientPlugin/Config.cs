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
        public readonly string Title = "Config";

        private MyKeys _testKeybind = MyKeys.None;
        [Keybind("Test Keybind Tooltip")]
        public MyKeys TestKeybind
        {
            get => _testKeybind;
            set => SetField(ref _testKeybind, value);
        }

        public string _textInput = "Default Text";
        [Textbox("Test Text Input Tooltip")]
        public string TextInput
        {
            get => _textInput;
            set => SetField(ref _textInput, value);
        }

        public DropDownEnum _dropdownTest = DropDownEnum.Alpha;
        [Dropdown(description: "Test Dropdown Tooltip")]
        public DropDownEnum DropdownTest
        {
            get => _dropdownTest;
            set => SetField(ref _dropdownTest, value);
        }

        public float _sliderFloatTest = 0.1f;
        [Slider(-5f, 4.5f, 0.5f, SliderAttribute.SliderType.Float, description: "Test Float Slider Tooltip")]
        public float SliderFloatTest
        {
            get => _sliderFloatTest;
            set => SetField(ref _sliderFloatTest, value);
        }

        public int _sliderIntTest = 2;
        [Slider(-1f, 10f, 1f, SliderAttribute.SliderType.Integer, description: "Test Int Slider Tooltip")]
        public int SliderIntTest
        {
            get => _sliderIntTest;
            set => SetField(ref _sliderIntTest, value);
        }

        [Button("Test Button Tooltip")]
        public void TestButton()
        {
            MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(
                MyMessageBoxStyleEnum.Info,
                buttonType: MyMessageBoxButtonsType.OK,
                messageText: new StringBuilder("You clicked me!"),
                messageCaption: new StringBuilder("Custom Button Function"),
                size: new Vector2(0.6f, 0.7f)
            ));
        }
    }
}