using System.Windows;

namespace TorchPlugin
{
    // NOTE: The long name is to avoid the name collision with other
    // config classes while loaded into Torch next to other plugins.
    // Don't shorten it to just ConfigView.

    // ReSharper disable once UnusedType.Global
    public partial class PluginTemplateConfigView
    {
        public PluginTemplateConfigView()
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