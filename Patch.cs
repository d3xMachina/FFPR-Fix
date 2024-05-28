using HarmonyLib;
using Last.Entity.Field;
using Last.Map;
using Last.UI;
using UnityEngine;

namespace FFPR_Fix
{
    public class FrameratePatch
    {
        [HarmonyPatch(typeof(SceneBoot), nameof(SceneBoot.Start))]
        [HarmonyPostfix]
        static void UncapFramerate()
        {
            Plugin.Log.LogInfo("Uncap framerate.");
            Application.targetFrameRate = 0;
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
