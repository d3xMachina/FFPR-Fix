using HarmonyLib;
using Last.Entity.Field;

namespace FFPR_Fix.Patches;

public class PlayerMoveSpeedPatch
{
    [HarmonyPatch(typeof(FieldPlayer), nameof(FieldPlayer.ChangeEntityMoveMagnificationByCurrentMoveState))]
    [HarmonyPostfix]
    static void ChangeWalkSpeed(FieldPlayer __instance)
    {
        var moveSecondsPerCell = __instance.GetMoveSpeed() * __instance.EntityMoveMagnification;

        if (__instance.moveState == FieldPlayerConstants.MoveState.Walk ||
            __instance.moveState == FieldPlayerConstants.MoveState.Dush)
        {
            __instance.EntityMoveMagnification *= moveSecondsPerCell / Plugin.Config.playerWalkspeed.Value;
        }
    }
    
    // For movespeed multipliers, check class FieldPlayerConstants
}