#if !TORCH

namespace ClientPlugin.PluginTemplate.Shared.Config
{
    public class PluginConfig: IPluginConfig
    {
        // Enables the plugin
        public bool Enabled { get; set; } = true;

        // TODO: Implement your config properties and define their defaults
    }
}

#endif