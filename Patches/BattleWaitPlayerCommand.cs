using HarmonyLib;
using Last.UI.KeyInput;

namespace FFPR_Fix.Patches;

public class BattleWaitPlayerCommand
{
    [HarmonyPatch(typeof(BattleInfomationController), nameof(BattleInfomationController.IsWaiting))]
    [HarmonyPostfix]
    static void WaitForPlayerCommand(BattleInfomationController __instance, ref bool __result)
    {
        if (__result)
        {
            return;
        }
        
        if (__instance.stateMachine.Current == BattleInfomationController.State.CommandSelect)
        {
            __result = true;
        }
    }
}
