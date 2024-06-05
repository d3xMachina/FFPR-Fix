using HarmonyLib;
using Last.Entity.Field;
using Last.Map;
using UnityEngine;

namespace FFPR_Fix.Patches;

public class RunOnWorldMap
{
    // Activate the dash after movement inputs are processed
    [HarmonyPatch(typeof(FieldPlayerKeyController), nameof(FieldPlayerKeyController.OnTouchPadCallback))]
    [HarmonyPrefix]
    static void UpdateDashState(FieldPlayerKeyController __instance, Vector2 axis)
    {
        if (axis == Vector2.zero ||
            __instance.mapHandle == null ||
            !__instance.mapHandle.CheckCurrentWorldMap())
        {
            return;
        }

        var dash = __instance.pressDashKey;
        if (TryGetAutoDash(out var autoDash))
        {
            if (autoDash)
            {
                dash = !__instance.pressDashKey;
            }
        }

        var moveState = dash ? FieldPlayerConstants.MoveState.Dush : FieldPlayerConstants.MoveState.Walk;
        __instance.fieldPlayer?.ChangeMoveState(moveState);
    }

    [HarmonyPatch(typeof(FieldPlayerKeyController), nameof(FieldPlayerKeyController.OnKeyDown))]
    [HarmonyPatch(typeof(FieldPlayerKeyController), nameof(FieldPlayerKeyController.OnKey))]
    [HarmonyPostfix]
    static void OnKeyDown(FieldPlayerKeyController __instance, FieldInputKey key)
    {
        if (key == FieldInputKey.Dush)
        {
            __instance.pressDashKey = true;
        }
    }

    [HarmonyPatch(typeof(FieldPlayerKeyController), nameof(FieldPlayerKeyController.OnKeyUp))]
    [HarmonyPostfix]
    static void OnKeyUp(FieldPlayerKeyController __instance, FieldInputKey key)
    {
        if (key == FieldInputKey.Dush)
        {
            __instance.pressDashKey = false;
        }
    }

    // Check the auto dash input key to trigger auto dash
    [HarmonyPatch(typeof(FieldMap), nameof(FieldMap.UpdatePlayerStatePlay))]
    [HarmonyPostfix]
    static void TriggerAutoDash(FieldMap __instance)
    {
        var mapUIManager = MapUIManager.Instance;
        var inputDevice = __instance.inputDevice;
        var configClient = Last.Management.ConfigClient.Instance();

        if (mapUIManager == null ||
            inputDevice == null ||
            configClient == null ||
            !mapUIManager.CheckCurrentWorldMap() ||
            !inputDevice.GetKeyDown(__instance.keyAutoDash) ||
            !TryGetAutoDash(out var autoDash))
        {
            return;
        }

        configClient.SetIsAutoDash(autoDash ? 0 : 1);
        mapUIManager.AutoDashOperationSwitch(!autoDash);
    }

    [HarmonyPatch(typeof(FieldMap), nameof(FieldMap.UpdatePlayerStatePlay))]
    [HarmonyPrefix]
    static void AddAutoDashKeybind(FieldMap __instance)
    {
        if (__instance.keyAutoDash != null)
        {
            return;
        }

        var player = __instance.fieldController?.player;
        if (player == null)
        {
            return;
        }

        var moveState = player.moveState;
        if (moveState == FieldPlayerConstants.MoveState.Walk ||
            moveState == FieldPlayerConstants.MoveState.Dush)
        {
            __instance.keyAutoDash = Il2CppSystem.Input.Key.StickL;
        }
    }

    static bool TryGetAutoDash(out bool autoDash)
    {
        var userDataManager = Last.Management.UserDataManager.Instance();
        if (userDataManager != null)
        {
            var config = userDataManager.Config;
            if (config != null)
            {
                autoDash = config.IsAutoDash != 0;
                return true;
            }
        }
        autoDash = false;
        return false;
    }
}
