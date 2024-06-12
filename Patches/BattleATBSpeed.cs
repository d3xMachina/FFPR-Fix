using HarmonyLib;
using Last.Battle;

namespace FFPR_Fix.Patches;

public class BattleATBSpeed
{
    [HarmonyPatch(typeof(BattleUtility), nameof(BattleUtility.GetBattleSpeedByConfigValue))]
    [HarmonyPostfix]
    static void SetATBSpeed(ref float __result)
    {
        __result *= Plugin.Config.BattleATBSpeed.Value;
    }
}
