using HarmonyLib;
using Last.UI;
using Last.UI.KeyInput;

namespace FFPR_Fix.Patches;

public class SkipIntroPatch
{
    [HarmonyPatch(typeof(SplashController), nameof(SplashController.Initialize))]
    [HarmonyPostfix]
    static void SkipSplashscreens(ref SplashController __instance)
    {
        if (Plugin.Config.SkipSplashscreens.Value)
        {
            Plugin.Log.LogInfo("Skip splashscreen.");
            __instance.stateMachine?.Change(SplashController.State.Title);
        }
    }

    [HarmonyPatch(typeof(TitleWindowController), nameof(TitleWindowController.UpdateNone))]
    [HarmonyPostfix]
    static void SkipPressAnyKey(TitleWindowController __instance)
    {
        if (Plugin.Config.SkipPressAnyKey.Value)
        {
            if (Last.Scene.SceneTitle.PreloadIsFinished())
            {
                Plugin.Log.LogInfo("Skip \"press any key\".");
                __instance.stateMachine?.Change(TitleWindowController.State.Select);
            }
        }
    }
}
