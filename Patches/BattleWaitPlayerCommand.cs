using HarmonyLib;
using Last.Battle;
using Last.Defaine.Master;
using Last.Management;
using Last.UI;
using Last.UI.KeyInput;

namespace FFPR_Fix.Patches;

public class BattleWaitPlayerCommand
{
    [HarmonyPatch(typeof(BattleInfomationController), nameof(BattleInfomationController.IsWaiting))]
    [HarmonyPostfix]
    static void WaitForPlayerCommand(BattleInfomationController __instance, ref bool __result)
    {
        var battleType = SystemConfig.Instance()?.ATBBattleType;
        var state = __instance.stateMachine?.Current;

        if (battleType != ATBBattleType.Wait ||
            state != BattleInfomationController.State.CommandSelect)
        {
            return;
        }

        __result = ModComponent.Instance.CurrentBattleState.IsWaiting;
    }

    [HarmonyPatch(typeof(BattlePlugManager), nameof(BattlePlugManager.Start))]
    [HarmonyPostfix]
    static void InitStartBattle()
    {
        ModComponent.Instance.CurrentBattleState.Reset();
    }

    [HarmonyPatch(typeof(BattleProgressATB), nameof(BattleProgressATB.SetActData))]
    [HarmonyPostfix]
    static void NextTurn(BattleProgressATB __instance, BattleActData battleActData)
    {
        if (battleActData == null ||
            battleActData.AttackUnitData == null)
        {
            return;
        }

        // Ignore units who do nothing during their turn. The game doesn't even indicate in any way to the player they didn't act.
        var nonAct = false;
        if (battleActData.CurrentUseType == BattleActData.UseType.Ability)
        {
            nonAct = true;

            var abilities = battleActData.abilityList;
            if (abilities != null)
            {
                foreach (var ability in abilities)
                {
                    if (ability.TypeId != (int)AbilityType.NonAct)
                    {
                        nonAct = false;
                        break;
                    }
                }
            }
        }

        if (!nonAct)
        {
            ModComponent.Instance.CurrentBattleState.TurnPassed();
        }
    }

    [HarmonyPatch(typeof(BattleProgressATB), nameof(BattleProgressATB.UpdateAlways))]
    [HarmonyPostfix]
    static void Update(BattleProgressATB __instance)
    {
        ref var battleState = ref ModComponent.Instance.CurrentBattleState;

        // Don't pause when escaping
        var escaping = BattleUIManager.Instance?.IsEscape() ?? false;
        battleState.Escaping = escaping;

        // Pause when we pass a turn and an other ally is ready
        var playerUnitsReadyCount = __instance.battleATBOrderUiStack?.orderUiStack?.Count ?? 0;
        if (playerUnitsReadyCount != battleState.PlayerUnitsReadyCount)
        {
            battleState.TurnPassed();
            battleState.PlayerUnitsReadyCount = playerUnitsReadyCount;
        }
    }
}
