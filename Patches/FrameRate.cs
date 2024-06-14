using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Common;
using UnityEngine;

namespace FFPR_Fix.Patches;

public class FrameratePatch
{
    [HarmonyPatch(typeof(SceneBoot), nameof(SceneBoot.Start))]
    [HarmonyPostfix]
    static void UncapFrameRate()
    {
        ModComponent.Instance.FixGetFrameRate = false; // we want to read the unity target framerate

        if (Application.targetFrameRate != 0)
        {
            ModComponent.Instance.DefaultFrameRate = Application.targetFrameRate;
            ModComponent.Instance.LastFrameRate = ModComponent.Instance.DefaultFrameRate;

            if (Plugin.Config.EnableVsync.Value)
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

        ModComponent.Instance.FixGetFrameRate = true;
    }

    // Return the current framerate instead of 0 because the game devs thought it was a good
    // idea to use the target framerate in the revive animation calculation instead of a delta time 
    // since the last frame. This would lead to a division by zero and a crash with the framerate uncapped.
    [HarmonyPatch(typeof(Application), nameof(Application.targetFrameRate), MethodType.Getter)]
    [HarmonyPrefix]
    static bool GetFramerate(ref int __result)
    {
        if (!ModComponent.Instance.FixGetFrameRate)
        {
            return true;
        }

        var deltaTime = Time.unscaledDeltaTime;
        if (deltaTime > 0)
        {
            var frameRate = (int)Mathf.Round(1f / deltaTime);
            if (frameRate > 0)
            {
                ModComponent.Instance.LastFrameRate = frameRate;
            }
        }

        __result = ModComponent.Instance.LastFrameRate;

        return false;
    }

    // Mainly used by the game menus to accumulate speed when a button keep being pressed down
    // As this is always called on frame updates, we adjust the accumulation speed to match the new framerate
    [HarmonyPatch(typeof(TimeFunction), nameof(TimeFunction.Function))]
    [HarmonyPrefix]
    static bool TimeFunctionFix(TimeFunction __instance, Action action, float waitTime, float acceleration, float waitTimeLowLimit, bool isAffectedTimeScale)
    {
        var deltaTime = Time.unscaledDeltaTime;
        if (deltaTime > 0)
        {
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
        }

        __instance.Acceleration += acceleration;
        return false;
    }
}
