using System;
using System.IO;
using System.Xml.Serialization;
using NLog;
using Torch;
using Torch.Views;

namespace TorchPlugin
{
    [Serializable]
    public class PluginConfig : ViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();


        private static readonly string ConfigFileName = $"{Plugin.PluginName}.cfg";
        private static string ConfigFilePath => Path.Combine(Plugin.Instance.StoragePath, ConfigFileName);


        private static PluginConfig _instance;
        public static PluginConfig Instance => _instance ?? (_instance = new PluginConfig());

        private static XmlSerializer ConfigSerializer => new XmlSerializer(typeof(PluginConfig));

        #region Properties

        private bool enabled = true;

        [Display(Description = "Enables/disables the plugin", Name = "Enable Plugin", Order = 1)]
        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }

        // TODO: Define your properties here

        #endregion

        #region Persistence

        protected override void OnPropertyChanged(string propName = "")
        {
            Save();
        }

        public void Save()
        {
            var path = ConfigFilePath;
            try
            {
                using (var streamWriter = new StreamWriter(path))
                {
                    ConfigSerializer.Serialize(streamWriter, _instance);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{Plugin.PluginName}: Failed to save configuration file: {path}");
            }
        }

        public void Load()
        {
            var path = ConfigFilePath;
            try
            {
                if (!File.Exists(path))
                {
                    Log.Warn($"{Plugin.PluginName}: Missing configuration file, saving defaults: {path}");
                    Save();
                    return;
                }

                using (var streamReader = new StreamReader(path))
                {
                    if (!(ConfigSerializer.Deserialize(streamReader) is PluginConfig config))
                    {
                        Log.Error($"{Plugin.PluginName}: Failed to deserialize configuration file: {path}");
                        return;
                    }

                    _instance = config;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{Plugin.PluginName}: Failed to load configuration file: {path}");
            }
        }

        #endregion
    }
}