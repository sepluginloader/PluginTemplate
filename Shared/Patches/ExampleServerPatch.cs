// TODO: Uncomment and understand this patch
// TODO: Enable and understand this patch.
#if false

#if TORCH || DEDICATED

using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Sandbox.Game.Multiplayer;
using Shared.Config;
using Shared.Plugin;
using Shared.Tools;
using SpaceEngineers.Game.EntityComponents.Blocks;

namespace Shared.Patches
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [HarmonyPatch(typeof(MyLcdSurfaceComponent))]
    public static class MyLcdSurfaceComponentPatch
    {
        private static IPluginConfig Config => Common.Config;

        [HarmonyPrefix]
        [HarmonyPatch("UpdateVisibility")]
        [EnsureCode("3e177a11")]
        private static bool UpdateVisibilityPrefix()
        {
            if (!Sync.IsDedicated)
                return true;

            return !true /*Config.FixTextPanel*/;
        }
    }
}

#endif

#endif
