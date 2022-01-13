using System;
using System.Reflection;
using HarmonyLib;
using VRage.Plugins;
using VRage.Utils;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin
    {
        public const string Name = "PluginTemplate";

        private static readonly PluginLogger Log = new PluginLogger(Name);

        private static bool initialized;

        private static Harmony Harmony => new Harmony(Name);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Log.WriteLine(MyLogSeverity.Debug, "Patching");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.WriteLine(MyLogSeverity.Info, "Patches applied");
        }

        public void Dispose()
        {
            try
            {
                // TODO: Put anything that needs to be disposed/closed when the game closes here
                // Do NOT use harmony.UnpatchAll()
            }
            catch (Exception ex)
            {
                Log.WriteException(MyLogSeverity.Critical, ex);
            }
        }

        public void Update()
        {
            if (initialized)
                return;

            Initialize();

            initialized = true;

            try
            {
                // TODO: Put anything that needs to be called on update here
            }
            catch (Exception ex)
            {
                Log.WriteException(MyLogSeverity.Critical, ex);
            }
        }

        private void Initialize()
        {
            Log.WriteLine(MyLogSeverity.Info, "Initializing");

            try
            {
                // TODO: Put your one time initialization here
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Log.WriteException(MyLogSeverity.Critical, ex);
            }

            Log.WriteLine(MyLogSeverity.Info, "Initialized");
        }
    }
}