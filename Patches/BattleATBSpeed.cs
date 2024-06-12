using HarmonyLib;
using Last.Battle;

namespace FFPR_Fix.Patches;

public class BattleATBSpeed
{
    [HarmonyPatch(typeof(BattleController), nameof(BattleController.StartBattle), [ typeof(InstantiateManager), typeof(bool), typeof(int) ])]
    [HarmonyPostfix]
    static void InitStartBattle()
    {
        var battlePlugManager = BattlePlugManager.Instance();
        if (battlePlugManager == null)
        {
            return;
        }
        
        var atbSpeed = battlePlugManager.GetSpeed();
        var multiplier = Plugin.Config.BattleATBSpeed.Value;
        battlePlugManager.SetATBSpeed(atbSpeed * multiplier);
    }
}
