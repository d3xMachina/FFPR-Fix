using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using FFPR_Fix.Patches;
using HarmonyLib;
using System;

namespace FFPR_Fix;

[BepInPlugin("d3xMachina.ffpr_fix", "FFPR Fix", "1.2.1")]
public partial class Plugin : BasePlugin
{
    public static new ManualLogSource Log;
    public static new ModConfiguration Config;

    public override void Load()
    {
        Log = base.Log;

        Log.LogInfo($"Game detected: {GameDetection.Version}");
        Log.LogInfo("Loading...");

        Config = new ModConfiguration(base.Config);
        Config.Init();
        if (ModComponent.Inject())
        {
            ApplyPatches();
        }
    }

    private void ApplyPatches()
    {
        var version = GameDetection.Version;
        bool mountRotatePatch = false;

        if (Config.uncapFPS.Value || Config.enableVsync.Value)
        {
            ApplyPatch(typeof(FrameratePatch));
            mountRotatePatch = true;
        }

        if (Config.chocoboTurnFactor.Value != 1f || Config.airshipTurnFactor.Value != 1f)
        {
            mountRotatePatch = true;
        }

        if (mountRotatePatch)
        {
            ApplyPatch(typeof(AirshipRotatePatch));
            ApplyPatch(typeof(ChocoboRotatePatch), GameVersion.FF3 | GameVersion.FF5 | GameVersion.FF6);
        }

        if (Config.hideFieldMinimap.Value || Config.hideWorldMinimap.Value)
        {
            ApplyPatch(typeof(HideMinimapPatch));
        }

        if (Config.skipSplashscreens.Value || Config.skipPressAnyKey.Value)
        {
            ApplyPatch(typeof(SkipIntroPatch));
        }

        if (Config.battleWaitPlayerCommand.Value)
        {
            ApplyPatch(typeof(BattleWaitPlayerCommand), GameVersion.FF4 | GameVersion.FF5 | GameVersion.FF6);
        }

        if (Config.playerWalkspeed.Value > 0f && Config.playerWalkspeed.Value != 1f)
        {
            ApplyPatch(typeof(PlayerMoveSpeedPatch));
        }

        if (Config.runOnWorldMap.Value)
        {
            ApplyPatch(typeof(RunOnWorldMap));
        }

        if (Config.disableDiagonalMovements.Value)
        {
            ApplyPatch(typeof(DisableDiagonalMovements));
        }

        Log.LogInfo("Patches applied!");
    }

    private void ApplyPatch(Type type, GameVersion versionsFlag = GameVersion.Any)
    {
        if ((GameDetection.Version & versionsFlag) != GameDetection.Version)
        {
            return;
        }

        Log.LogInfo($"Patching {type.Name}...");

        Harmony.CreateAndPatchAll(type);
    }
}
