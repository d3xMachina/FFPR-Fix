using HarmonyLib;
using Il2CppSystems.Files;
using System;
using System.IO;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace FFPR_Fix.Patches;

public class UseDecryptedSaveFiles
{
    [HarmonyPatch(typeof(FileOperationUtility), nameof(FileOperationUtility.ConvertSha256))]
    [HarmonyPrefix]
    static bool DoNotEncodeFileName(ref string __result, string targetName)
    {
        __result = targetName;
        return false;
    }

    [HarmonyPatch(typeof(FileOperationUtility), nameof(FileOperationUtility.FileWrite))]
    [HarmonyPrefix]
    static bool WriteUnencrypted(ref FileOperationUtility.ResultCode __result, string fileName, string filePath, string extension, Il2CppStructArray<byte> target)
    {
        var fullPath = filePath + fileName + extension;

        try
        {
            var spaceAvailable = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
            var spaceNeeded = (int)Mathf.Ceil(target.Count * sizeof(byte) / 1024 / 1024);
            if (spaceAvailable < spaceNeeded)
            {
                __result = FileOperationUtility.ResultCode.Storage;
            }
            else
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                // write to a temp file before to be safe in case of error
                var tempPath = fullPath + ".tmp";
                File.WriteAllBytes(tempPath, target);
                if (File.Exists(fullPath))
                {
                    var backupPath = fullPath + ".bak";
                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }
                    File.Move(fullPath, backupPath);
                }
                File.Move(tempPath, fullPath);

                __result = FileOperationUtility.ResultCode.Success;
            }
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Cannot write file {fullPath}: {e}");
            __result = FileOperationUtility.ResultCode.Unknown;
        }

        return false;
    }

    [HarmonyPatch(typeof(FileOperationUtility), nameof(FileOperationUtility.FileRead), [typeof(string), typeof(string), typeof(string), typeof(int)])]
    [HarmonyPrefix]
    static bool ReadUnencrypted(ref string __result, string readResult)
    {
        __result = readResult;
        return false;
    }
}
