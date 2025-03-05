using HarmonyLib;
using Last.Map;
using UnityEngine;

namespace FFPR_Fix.Patches;

public abstract class MountRotate
{
    public struct MountRotateData
    {
        public float accelerator;
        public float left;
        public float right;
    }

    // Calculate and adjust the rotation accelerator to match the framerate for the chocobo and the airship
    protected static MountRotateData HandleMountRotation(FieldPlayerController controller, MountRotateData rotateData, float inputX, float customRate)
    {
        // Names of constants deducted from FieldPlayerAirshipConstants class
        const float DeltaRotateValue = 0.2f;
        const float RotateDecelerateScale = 0.5f;
        const float LimitRotateAccelerator = 2f;

        var rateFix = ModComponent.Instance.DefaultFrameRate * Time.unscaledDeltaTime;
        rateFix *= customRate;

        var rotateScale = 1f;
        if (inputX < 0)
        {
            rotateScale = rotateData.left;
        }
        else if (inputX > 0)
        {
            rotateScale = rotateData.right;
        }

        var rotateAccelerator = rotateData.accelerator - inputX * DeltaRotateValue * rotateScale * rateFix;
        var sign = Mathf.Sign(rotateAccelerator);
        rotateAccelerator = Mathf.Min(rotateAccelerator * sign, rotateScale * LimitRotateAccelerator * rateFix) * sign;

        if (rotateAccelerator != 0f)
        {
            var mapHandle = controller.mapHandle;
            if (mapHandle != null)
            {
                var newRotation = mapHandle.GetZAxisRotateBirdCamera() + rotateAccelerator;
                mapHandle.SetZAxisRotateBirdCamera(newRotation);
            }

            rotateAccelerator -= DeltaRotateValue * RotateDecelerateScale * rateFix * sign;
            rotateAccelerator = Mathf.Max(0, rotateAccelerator * sign) * sign;
        }

        return new MountRotateData
        {
            left = rotateData.left,
            right = rotateData.right,
            accelerator = rotateAccelerator,
        };
    }
}

public class ChocoboRotatePatch : MountRotate
{
    [HarmonyPatch(typeof(FieldPlayerKeyGroundBirdController), nameof(FieldPlayerKeyGroundBirdController.UpdateRotateAccelerator))]
    [HarmonyPrefix]
    static void MountSpeedFixPre(FieldPlayerKeyGroundBirdController __instance, out MountRotateData __state)
    {
        var rotateData = new MountRotateData
        {
            accelerator = __instance.rotateAccelerator,
            left = __instance.leftFastRotateScale,
            right = __instance.rightFastRotateScale
        };

        // Backup the rotate values
        __state = HandleMountRotation(__instance, rotateData, __instance.inputAxis.x, Plugin.Config.ChocoboTurnFactor.Value);

        // Skip the rotation code in the game method as we handled it
        __instance.rotateAccelerator = 0f;
        __instance.leftFastRotateScale = 0f;
        __instance.rightFastRotateScale = 0f;
    }

    [HarmonyPatch(typeof(FieldPlayerKeyGroundBirdController), nameof(FieldPlayerKeyGroundBirdController.UpdateRotateAccelerator))]
    [HarmonyPostfix]
    static void MountSpeedFixPost(FieldPlayerKeyGroundBirdController __instance, MountRotateData __state)
    {
        // Restore the values
        __instance.rotateAccelerator = __state.accelerator;
        __instance.leftFastRotateScale = __state.left;
        __instance.rightFastRotateScale = __state.right;
    }
}

public class AirshipRotatePatch : MountRotate
{
    [HarmonyPatch(typeof(FieldPlayerKeyAirshipController), nameof(FieldPlayerKeyAirshipController.UpdateRotateAccelerator))]
    [HarmonyPrefix]
    static void MountSpeedFixPre(FieldPlayerKeyAirshipController __instance, out MountRotateData __state)
    {
        var rotateData = new MountRotateData
        {
            accelerator = __instance.rotateAccelerator,
            left = __instance.leftFastRotateScale,
            right = __instance.rightFastRotateScale
        };

        // Backup the rotate values
        __state = HandleMountRotation(__instance, rotateData, __instance.inputAxis.x, Plugin.Config.AirshipTurnFactor.Value);

        // Skip the rotation code in the game method as we handled it
        __instance.rotateAccelerator = 0f;
        __instance.leftFastRotateScale = 0f;
        __instance.rightFastRotateScale = 0f;
    }

    [HarmonyPatch(typeof(FieldPlayerKeyAirshipController), nameof(FieldPlayerKeyAirshipController.UpdateRotateAccelerator))]
    [HarmonyPostfix]
    static void MountSpeedFixPost(FieldPlayerKeyAirshipController __instance, MountRotateData __state)
    {
        // Restore the values
        __instance.rotateAccelerator = __state.accelerator;
        __instance.leftFastRotateScale = __state.left;
        __instance.rightFastRotateScale = __state.right;
    }
}
