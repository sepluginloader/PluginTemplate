#if !TORCH

namespace ClientPlugin.PluginTemplate.Shared.Config
{
    public class PluginConfig: IPluginConfig
    {
        // Enables/disables the plugin
        public bool Enabled { get; set; }

        // TODO: Implement all config properties
    }
}

#endif