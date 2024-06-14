using HarmonyLib;
using Il2CppSystems.Files;
using System;
using System.IO;
using UnhollowerBaseLib;
using UnityEngine;

namespace FFPR_Fix.Patches;

public class BackupSaveFiles
{
    [HarmonyPatch(typeof(FileOperationUtility), nameof(FileOperationUtility.FileWrite))]
    [HarmonyPrefix]
    static bool BackupFile(ref FileOperationUtility.ResultCode __result, string fileName, string filePath, string extension, Il2CppStructArray<byte> target)
    {
        var fullPath = filePath + fileName + extension;
        var backupPath = fullPath + ".bak";

        try
        {
            var spaceAvailable = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
            var spaceNeeded = (int)Mathf.Ceil(target.Count * sizeof(byte) / 1024 / 1024);
            if (spaceAvailable < spaceNeeded)
            {
                __result = FileOperationUtility.ResultCode.Storage;
                return false;
            }

            if (File.Exists(fullPath))
            {
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
                File.Move(fullPath, backupPath);
            }
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Cannot backup file {fullPath} to {backupPath}: {e}");
            __result = FileOperationUtility.ResultCode.Unknown;
            return false;
        }

        return true;
    }
}
