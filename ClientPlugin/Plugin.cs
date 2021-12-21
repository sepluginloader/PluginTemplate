using System.Reflection;
using HarmonyLib;
using NLog;
using VRage.Plugins;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin
    {
        public const string Name = "PluginTemplate";

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static bool initialized;

        private static Harmony Harmony => new Harmony(Name);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Log.Debug($"{Name}: Patching");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Info($"{Name}: Patches applied");
        }

        public void Dispose()
        {
            // Do NOT use harmony.UnpatchAll()
        }

        public void Update()
        {
            if (initialized)
                return;

            Initialize();

            initialized = true;
        }

        private void Initialize()
        {
            Log.Debug($"{Name}: Initializing");

            // TODO: Put your one time initialization here

            Log.Info($"{Name}: Initialized");
        }
    }
}