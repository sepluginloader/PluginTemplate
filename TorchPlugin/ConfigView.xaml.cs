using System.Windows;
using System.Windows.Controls;

namespace TorchPlugin
{
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once RedundantExtendsListEntry
    public partial class ConfigView : UserControl
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