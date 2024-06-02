using HarmonyLib;
using Il2CppSystem;
using Last.Entity.Field;

namespace FFPR_Fix.Patches;

public class PlayerMoveSpeedPatch
{
    [HarmonyPatch(typeof(FieldEntityConstants), nameof(FieldEntityConstants.GetFieldSpriteMoveSecondsPerCell))]
    [HarmonyPostfix]
    static void SpriteMovespeed(ref float __result, Nullable<FieldEntityConstants.FieldSpriteSpeedID> speedId, float entityMoveMagnification)
    {
        if (Plugin.Config.playerMovespeed.Value > 0f &&
            (!speedId.HasValue || speedId.Value == FieldEntityConstants.FieldSpriteSpeedID.FieldPlayer))
        {
            Plugin.Log.LogInfo("Get sprite movespeed.");
            __result = Plugin.Config.playerMovespeed.Value / entityMoveMagnification;
        }
    }
        
    [HarmonyPatch(typeof(FieldPlayer), nameof(FieldPlayer.GetMoveSpeed))]
    [HarmonyPostfix]
    static void PlayerMovespeed(FieldPlayer __instance, ref float __result)
    {
        if (Plugin.Config.playerMovespeed.Value > 0f)
        {
            Plugin.Log.LogInfo("Get player movespeed.");
            __result = Plugin.Config.playerMovespeed.Value / __instance.entityMoveMagnification;
        }
    }

    // For movespeed multipliers, check class FieldPlayerConstants
}