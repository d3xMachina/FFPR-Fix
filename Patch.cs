using HarmonyLib;
using Last.Entity.Field;
using Last.Map;
using Last.UI;
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
        static void TimeFunctionFix(Il2CppSystem.Action action, float waitTime, ref float acceleration, float waitTimeLowLimit, bool isAffectedTimeScale)
        {
            var rate = DefaultFrameRate / (1.0f / Time.unscaledDeltaTime);
            acceleration *= rate;
        }
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
                __instance.stateMachine.Change(SplashController.State.Title);
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
