using HarmonyLib;
using Last.Map;
using UnityEngine;

namespace FFPR_Fix.Patches;

public class DisableDiagonalMovements
{
    [HarmonyPatch(typeof(FieldPlayerKeyController), nameof(FieldPlayerKeyController.OnTouchPadCallback))]
    [HarmonyPrefix]
    static void Pad8WayTo4Way(FieldPlayerController __instance, ref Vector2 axis)
    {
        ref var lastAxis = ref ModComponent.Instance.LastPadAxis;
        ref var lastAxisSanitized = ref ModComponent.Instance.LastPadAxisSanitized;
        ref var padNoInput = ref ModComponent.Instance.ProcessPadNoInput;

        if (padNoInput) // function is never called when no pad input so check from ObserveInput
        {
            lastAxis = Vector2.zero;
            lastAxisSanitized = Vector2.zero;
            padNoInput = false;
        }

        if (axis == lastAxis)
        {
            axis = lastAxisSanitized;
            return;
        }

        var prevAxis = axis;

        // 2 directions pressed, in case of irresolvable conflict we ignore the input, other priorize the most recent input
        if (axis.y != 0 && axis.x != 0)
        {
            // Irresolvable conflict as the previous input doesn't overlap
            if (lastAxisSanitized.x != axis.x && lastAxisSanitized.y != axis.y)
            {
                axis.x = 0;
                axis.y = 0;
            }
            // Up-Right and Up-Left
            else if (axis.y > 0 && axis.x > 0 ||
                     axis.y > 0 && axis.x < 0)
            {
                if (lastAxisSanitized.y > 0)
                {
                    axis.y = 0;
                }
                else
                {
                    axis.x = 0;
                }
            }
            // Down-Right and Down-Left
            else
            {
                if (lastAxisSanitized.y < 0)
                {
                    axis.y = 0;
                }
                else
                {
                    axis.x = 0;
                }
            }
        }

        lastAxis = prevAxis;
        lastAxisSanitized = axis;
    }

    [HarmonyPatch(typeof(InputDevice), nameof(InputDevice.ObserveInput))]
    [HarmonyPostfix]
    static void CheckPadNoInput(InputDevice __instance)
    {
        if (__instance.InputAxis == Vector2.zero)
        {
            ModComponent.Instance.ProcessPadNoInput = true;
        }
    }
}
