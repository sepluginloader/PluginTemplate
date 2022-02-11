using System;
using Shared.Config;
using Torch;
using Torch.Views;

namespace TorchPlugin
{
    [Serializable]
    public class PluginConfig : ViewModel, IPluginConfig
    {
        private bool enabled = true;
        // TODO: Implement your config fields

        [Display(GroupName = "General", Name = "Enable plugin", Order = 1, Description = "Enables/disables the plugin")]
        public bool Enabled
        {
            get => enabled;
            set => SetValue(ref enabled, value);
        }

        // TODO: Encapsulate them as properties
    }
}