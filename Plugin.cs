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
        bool mountRotatePatch = false;

        if (Config.UncapFPS.Value || Config.EnableVsync.Value)
        {
            ApplyPatch(typeof(FrameratePatch));
            mountRotatePatch = true;
        }

        if (Config.ChocoboTurnFactor.Value != 1f || Config.AirshipTurnFactor.Value != 1f)
        {
            mountRotatePatch = true;
        }

        if (mountRotatePatch)
        {
            ApplyPatch(typeof(AirshipRotatePatch));
            ApplyPatch(typeof(ChocoboRotatePatch), GameVersion.FF3 | GameVersion.FF5 | GameVersion.FF6);
        }

        if (Config.HideFieldMinimap.Value || Config.HideWorldMinimap.Value)
        {
            ApplyPatch(typeof(HideMinimapPatch));
        }

        if (Config.SkipSplashscreens.Value || Config.SkipPressAnyKey.Value)
        {
            ApplyPatch(typeof(SkipIntroPatch));
        }

        if (Config.BattleWaitPlayerCommand.Value)
        {
            ApplyPatch(typeof(BattleWaitPlayerCommand), GameVersion.FF4 | GameVersion.FF5 | GameVersion.FF6);
        }

        if (Config.BattleATBSpeed.Value > 0f && Config.BattleATBSpeed.Value != 1f)
        {
            ApplyPatch(typeof(BattleATBSpeed), GameVersion.FF4 | GameVersion.FF5 | GameVersion.FF6);
        }

        if (Config.PlayerWalkspeed.Value > 0f && Config.PlayerWalkspeed.Value != 1f)
        {
            ApplyPatch(typeof(PlayerMoveSpeedPatch));
        }

        if (Config.RunOnWorldMap.Value)
        {
            ApplyPatch(typeof(RunOnWorldMap));
        }

        if (Config.DisableDiagonalMovements.Value)
        {
            ApplyPatch(typeof(DisableDiagonalMovements));
        }

        if (Config.UseDecryptedSaveFiles.Value)
        {
            ApplyPatch(typeof(UseDecryptedSaveFiles));
        }
        else if (Config.BackupSaveFiles.Value) // already implemented in UseDecryptedSaveFiles
        {
            ApplyPatch(typeof(BackupSaveFiles));
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
