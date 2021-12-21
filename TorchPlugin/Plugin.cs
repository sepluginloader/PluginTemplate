using NLog;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Session;

namespace TorchPlugin
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Plugin : TorchPluginBase
    {
        public const string PluginName = "PluginTemplate";

        public static Plugin Instance;

        private Logger _logger;
        private Logger Log => _logger ?? (_logger = LogManager.GetLogger(PluginName));

        private TorchSessionManager sessionManager;
        private bool Initialized => sessionManager != null;

        // ReSharper disable once UnusedMember.Local
        private readonly TorchCommands commands = new TorchCommands();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            Instance = this;

            Log.Info($"{PluginName}: Init");

            sessionManager = torch.Managers.GetManager<TorchSessionManager>();
            sessionManager.SessionStateChanged += SessionStateChanged;
        }

        private void SessionStateChanged(ITorchSession session, TorchSessionState newstate)
        {
            switch (newstate)
            {
                case TorchSessionState.Loading:
                    Log.Debug($"{PluginName}: Loading");
                    break;

                case TorchSessionState.Loaded:
                    Log.Debug($"{PluginName}: Loaded");
                    // TODO: Put your one time initialization here
                    break;

                case TorchSessionState.Unloading:
                    Log.Debug($"{PluginName}: Unloading");
                    // TODO: Make sure to save any persistent modifications here
                    break;

                case TorchSessionState.Unloaded:
                    Log.Debug($"{PluginName}: Unloaded");
                    break;
            }
        }

        public override void Update()
        {
            // TODO: Generic update processing if needed
        }

        public override void Dispose()
        {
            Instance = null;

            if (!Initialized)
                return;

            Log.Debug($"{PluginName}: Disposing");

            sessionManager.SessionStateChanged -= SessionStateChanged;
            sessionManager = null;

            Log.Debug($"{PluginName}: Disposed");

            base.Dispose();
        }
    }
}