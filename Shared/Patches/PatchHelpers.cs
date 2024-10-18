// #define LIST_ALL_TYPES

using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Shared.Logging;
using Shared.Plugin;
using Shared.Tools;

namespace Shared.Patches
{
    // ReSharper disable once UnusedType.Global
    public static class PatchHelpers
    {
        public static bool HarmonyPatchAll(IPluginLogger log, Harmony harmony)
        {
#if DEBUG && LIST_ALL_TYPES
            log.Info("All types:");
            foreach (var typ in AccessTools.AllTypes())
            {
                log.Info(typ.FullName);
            }
#endif

            if (Common.Plugin.Config.DetectCodeChanges && 
                Environment.GetEnvironmentVariable("SE_PLUGIN_DISABLE_METHOD_VERIFICATION") == null &&
                !WineDetector.IsRunningInWineOrProton())
            {
                log.Debug("Scanning for conflicting code changes");
                try
                {
                    var codeChanges = EnsureCode.Verify().ToList();
                    if (codeChanges.Count != 0)
                    {
                        log.Critical("Detected conflicting code changes:");
                        foreach (var codeChange in codeChanges)
                            log.Info(codeChange.ToString());
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log.Critical(ex, "Failed to scan for conflicting code changes");
                    return false;
                }
            }
            else
            {
                log.Warning("Conflicting code change detection is disabled in plugin configuration");
            }

            log.Debug("Applying Harmony patches");
            try
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                log.Critical(ex, "Failed to apply Harmony patches");
                return false;
            }

            return true;
        }

        // Called after loading configuration, but before patching
        public static void Configure()
        {
            // Call the Configure public static method on each of your patch
            // classes requiring one-time configuration (no world is loaded yet)
            // For example:
            //MyPatch.Configure();
        }

        // Called on every update
        public static void PatchUpdates()
        {
            // Call the Update public static method on each of your
            // patch classes which needs periodic updates, it is
            // called on each game update (tick).
            // For example:
            //MyPatch.Update();
        }
    }
}