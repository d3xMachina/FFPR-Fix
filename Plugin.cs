using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using FFPR_Fix.Patches;
using HarmonyLib;

namespace FFPR_Fix;

[BepInPlugin("d3xMachina.ffpr_fix", "FFPR Fix", "1.1.1")]
public partial class Plugin : BasePlugin
{
    public static new ManualLogSource Log;
    public static new ModConfiguration Config;

    public override void Load()
    {
        Log = base.Log;

        Log.LogInfo("Loading...");

        Config = new ModConfiguration(base.Config);
        Config.Init();
        ModComponent.Inject();
        ApplyPatches();

        Log.LogInfo("Patches applied!");
    }

    private void ApplyPatches()
    {
        if (Config.uncapFPS.Value || Config.enableVsync.Value)
        {
            Harmony.CreateAndPatchAll(typeof(FrameratePatch));
        }

        if (Config.hideFieldMinimap.Value || Config.hideWorldMinimap.Value)
        {
            Harmony.CreateAndPatchAll(typeof(HideMinimapPatch));
        }

        if (Config.skipSplashscreens.Value || Config.skipPressAnyKey.Value)
        {
            Harmony.CreateAndPatchAll(typeof(SkipIntroPatch));
        }

        /* Crash because of a bug with BepInEx and Il2CppSystem.Nullable in methods
        if (playerMovespeed.Value > 0f)
        {
            Harmony.CreateAndPatchAll(typeof(PlayerMoveSpeedPatch));
        }
        */
    }
}
