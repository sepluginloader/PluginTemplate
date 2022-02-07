#if !TORCH

namespace Shared.Config
{
    public class PluginConfig: IPluginConfig
    {
        public bool Enabled { get; set; } = true;

        // TODO: Implement your config properties and define their defaults
    }
}

#endif