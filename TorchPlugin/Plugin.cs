#define USE_HARMONY

using System;
using System.Reflection;
using HarmonyLib;
using Shared.Logging;
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
        public static readonly IPluginLogger Log = new TorchPluginLogger(PluginName);
        public static Plugin Instance;
        public static long Tick;

#if USE_HARMONY
        private static readonly Harmony Harmony = new Harmony(PluginName);
#endif

        private TorchSessionManager sessionManager;
        private bool Initialized => sessionManager != null;
        private bool failed;

        // ReSharper disable once UnusedMember.Local
        private readonly TorchCommands commands = new TorchCommands();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            Instance = this;

            Log.Info("Init");

#if USE_HARMONY
            Log.Debug("Patching");
            try
            {
                Harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Log.Critical(ex, "Patching failed");
                failed = true;
                return;
            }
#endif

            sessionManager = torch.Managers.GetManager<TorchSessionManager>();
            sessionManager.SessionStateChanged += SessionStateChanged;
        }

        private void SessionStateChanged(ITorchSession session, TorchSessionState newstate)
        {
            switch (newstate)
            {
                case TorchSessionState.Loading:
                    Log.Debug("Loading");
                    break;

                case TorchSessionState.Loaded:
                    Log.Debug("Loaded");
                    OnLoaded();
                    break;

                case TorchSessionState.Unloading:
                    Log.Debug("Unloading");
                    OnUnloading();
                    break;

                case TorchSessionState.Unloaded:
                    Log.Debug("Unloaded");
                    break;
            }
        }

        public override void Dispose()
        {
            Instance = null;

            if (Initialized)
            {
                Log.Debug("Disposing");

                sessionManager.SessionStateChanged -= SessionStateChanged;
                sessionManager = null;

                Log.Debug("Disposed");
            }

            base.Dispose();
        }

        private void OnLoaded()
        {
            try
            {
                // TODO: Put your one time initialization here
            }
            catch (Exception e)
            {
                Log.Error(e, "OnLoaded failed");
                failed = true;
            }
        }

        private void OnUnloading()
        {
            try
            {
                // TODO: Make sure to save any persistent modifications here
            }
            catch (Exception e)
            {
                Log.Error(e, "OnUnloading failed");
                failed = true;
            }
        }

        public override void Update()
        {
            try
            {
                if (!failed)
                {
                    CustomUpdate();
                    Tick++;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Update failed");
                failed = true;
            }

            Tick++;
        }

        private void CustomUpdate()
        {
            throw new NotImplementedException();
        }
    }
}