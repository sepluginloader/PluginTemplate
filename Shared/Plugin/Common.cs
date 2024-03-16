using System;
using System.IO;
using Shared.Config;
using Shared.Logging;
using Shared.Patches;

namespace Shared.Plugin
{
    public static class Common
    {
        public static ICommonPlugin Plugin { get; private set; }
        public static IPluginLogger Logger { get; private set; }
        public static IPluginConfig Config { get; private set; }

        public static string GameVersion;
        public static string DataDir;

        public static void SetPlugin(ICommonPlugin plugin, string gameVersion, string storageDir)
        {
            Plugin = plugin;
            Logger = plugin.Log;
            Config = plugin.Config;

            GameVersion = gameVersion;
            DataDir = Path.Combine(storageDir, "PluginTemplate");

            PatchHelpers.Configure();
        }
    }
}