using HarmonyLib;
using Last.Map;

namespace FFPR_Fix.Patches;

public class HideMinimapPatch
{
    [HarmonyPatch(typeof(MapUIManager), nameof(MapUIManager.UpdateController))]
    [HarmonyPostfix]
    static void HideMinimap(MapUIManager __instance)
    {
        if (Plugin.Config.HideFieldMinimap.Value)
        {
            __instance.SetActiveMinimap(false);
        }

        if (Plugin.Config.HideWorldMinimap.Value)
        {
            __instance.SetActiveOverlayGps(false);
        }
    }
}