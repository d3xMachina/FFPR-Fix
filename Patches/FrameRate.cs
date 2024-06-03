using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Common;
using UnityEngine;

namespace FFPR_Fix.Patches;

public class FrameratePatch
{
    [HarmonyPatch(typeof(SceneBoot), nameof(SceneBoot.Start))]
    [HarmonyPostfix]
    static void UncapFramerate()
    {
        if (Application.targetFrameRate != 0)
        {
            ModComponent.Instance.DefaultFrameRate = Application.targetFrameRate;

            if (Plugin.Config.enableVsync.Value)
            {
                Plugin.Log.LogInfo("VSync enabled.");
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                Plugin.Log.LogInfo("Uncap framerate.");
                QualitySettings.vSyncCount = 0;
            }

            Application.targetFrameRate = 0;
        }
    }

    // Mainly used by the game menus to accumulate speed when a button keep being pressed down
    // As this is always called on frame updates, we adjust the accumulation speed to match the new framerate
    [HarmonyPatch(typeof(TimeFunction), nameof(TimeFunction.Function))]
    [HarmonyPrefix]
    static bool TimeFunctionFix(TimeFunction __instance, Action action, float waitTime, float acceleration, float waitTimeLowLimit, bool isAffectedTimeScale)
    {
        var deltaTime = Time.unscaledDeltaTime;
        __instance.checkTime += deltaTime;

        var rateFix = ModComponent.Instance.DefaultFrameRate / (1f / deltaTime);
        var waitTimeRemaining = waitTime - (__instance.Acceleration * rateFix);
        if (waitTime != 0f && waitTimeRemaining <= waitTimeLowLimit)
        {
            waitTimeRemaining = waitTimeLowLimit;
        }

        if (waitTimeRemaining <= __instance.checkTime)
        {
            action?.Invoke();
            __instance.checkTime = 0f;
        }

        __instance.Acceleration += acceleration;
        return false;
    }
}
