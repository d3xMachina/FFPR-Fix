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
    static void TimeFunctionFix(Action action, ref float waitTime, ref float acceleration, ref float waitTimeLowLimit, bool isAffectedTimeScale)
    {
        var rateFix = ModComponent.Instance.DefaultFrameRate / (1f / Time.unscaledDeltaTime);
        acceleration *= rateFix;

        // Fix fast input when the speedhack is enabled
        float timeScale = Time.timeScale;
        waitTime *= timeScale;
        waitTimeLowLimit *= timeScale;
    }
}
