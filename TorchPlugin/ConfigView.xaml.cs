using System.Windows;

namespace TorchPlugin
{
    // ReSharper disable once UnusedType.Global
    public partial class ConfigView
    {
        public ConfigView()
        {
            InitializeComponent();
            DataContext = Plugin.Config;
        }

        private void SomeButton_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO: Your event handler here
        }

        // TODO: Add any custom UI interaction code for the config view here (if any)
    }
}