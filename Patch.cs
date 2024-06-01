﻿using HarmonyLib;
using Last.Entity.Field;
using Last.Map;
using Last.UI;
using Last.UI.KeyInput;
using UnityEngine;

namespace FFPR_Fix
{
    public class FrameratePatch
    {
        static int DefaultFrameRate = 60;

        [HarmonyPatch(typeof(SceneBoot), nameof(SceneBoot.Start))]
        [HarmonyPostfix]
        static void UncapFramerate()
        {
            Plugin.Log.LogInfo("Uncap framerate.");
            if (Application.targetFrameRate != 0)
            {
                DefaultFrameRate = Application.targetFrameRate;
                Application.targetFrameRate = 0;
            }
        }

        // Mainly used by the game menus to accumulate speed when a button keep being pressed down
        // As this is always called on frame updates, we adjust the accumulation speed to match the new framerate
        [HarmonyPatch(typeof(Il2CppSystem.Common.TimeFunction), nameof(Il2CppSystem.Common.TimeFunction.Function))]
        [HarmonyPrefix]
        static void TimeFunctionFix(Il2CppSystem.Action action, ref float waitTime, ref float acceleration, ref float waitTimeLowLimit, bool isAffectedTimeScale)
        {
            var rate = DefaultFrameRate / (1f / Time.unscaledDeltaTime);
            acceleration *= rate;

            // Fix fast input when the speedhack is enabled
            float timeScale = Time.timeScale;
            waitTime *= timeScale;
            waitTimeLowLimit *= timeScale;
        }
    }

    public class SkipIntroPatch
    {
        [HarmonyPatch(typeof(SplashController), nameof(SplashController.Initialize))]
        [HarmonyPostfix]
        static void SkipSplashscreens(ref SplashController __instance)
        {
            if (Plugin.skipSplashscreens.Value)
            {
                Plugin.Log.LogInfo("Skip splashscreen.");
                __instance.stateMachine?.Change(SplashController.State.Title);
            }
        }

        [HarmonyPatch(typeof(TitleWindowController), nameof(TitleWindowController.UpdateNone))]
        [HarmonyPostfix]
        static void SkipPressAnyKey(TitleWindowController __instance)
        {
            if (Plugin.skipPressAnyKey.Value)
            {
                if (Last.Scene.SceneTitle.PreloadIsFinished())
                {
                    __instance.stateMachine?.Change(TitleWindowController.State.Select);
                }
            }
        }
    }

    public class UIPatch
    {
        [HarmonyPatch(typeof(MapUIManager), nameof(MapUIManager.UpdateController))]
        [HarmonyPostfix]
        static void HideMinimap(ref MapUIManager __instance)
        {
            if (Plugin.hideFieldMinimap.Value)
            {
                __instance.SetActiveMinimap(false);
            }

            if (Plugin.hideWorldMinimap.Value)
            {
                __instance.SetActiveOverlayGps(false);
            }
        }
    }

    public class PlayerPatch
    {
        [HarmonyPatch(typeof(FieldEntityConstants), nameof(FieldEntityConstants.GetFieldSpriteMoveSecondsPerCell))]
        [HarmonyPostfix]
        static void SpriteMovespeed(ref float __result, Il2CppSystem.Nullable<FieldEntityConstants.FieldSpriteSpeedID> speedId, float entityMoveMagnification)
        {
            if (Plugin.playerMovespeed.Value > 0f &&
                (!speedId.HasValue || speedId.Value == FieldEntityConstants.FieldSpriteSpeedID.FieldPlayer))
            {
                Plugin.Log.LogInfo("Get sprite movespeed.");
                __result = Plugin.playerMovespeed.Value / entityMoveMagnification;
            }
        }
        
        [HarmonyPatch(typeof(FieldPlayer), nameof(FieldPlayer.GetMoveSpeed))]
        [HarmonyPostfix]
        static void PlayerMovespeed(ref FieldPlayer __instance, ref float __result)
        {
            if (Plugin.playerMovespeed.Value > 0f)
            {
                Plugin.Log.LogInfo("Get player movespeed.");
                __result = Plugin.playerMovespeed.Value / __instance.entityMoveMagnification;
            }
        }

        // For movespeed multipliers, check class FieldPlayerConstants
    }
}
