using System.Diagnostics;

namespace FFPR_Fix;

public enum GameVersion
{
    Unknown = 0,
    FF1 = 1 << 0,
    FF2 = 1 << 1,
    FF3 = 1 << 2,
    FF4 = 1 << 3,
    FF5 = 1 << 4,
    FF6 = 1 << 5,
    Any = int.MaxValue
}

public class GameDetection
{
    private static GameVersion _version = GameVersion.Unknown;
    public static GameVersion Version
    { 
        get
        {
            if (_version ==  GameVersion.Unknown)
            {
                _version = GetGameVersion();
            }
            return _version;
        }
        private set
        {
            _version = value;
        }
    }
    
    private static GameVersion GetGameVersion()
    {
        switch (GetProductName())
        {
            case "FINAL FANTASY":
                return GameVersion.FF1;
            case "FINAL FANTASY II":
                return GameVersion.FF2;
            case "FINAL FANTASY III":
                return GameVersion.FF3;
            case "FINAL FANTASY IV":
                return GameVersion.FF4;
            case "FINAL FANTASY V":
                return GameVersion.FF5;
            case "FINAL FANTASY VI":
                return GameVersion.FF6;
        }

        switch (GetModuleFileName())
        {
            case "FINAL FANTASY":
                return GameVersion.FF1;
            case "FINAL FANTASY II":
                return GameVersion.FF2;
            case "FINAL FANTASY III":
                return GameVersion.FF3;
            case "FINAL FANTASY IV":
                return GameVersion.FF4;
            case "FINAL FANTASY V":
                return GameVersion.FF5;
            case "FINAL FANTASY VI":
                return GameVersion.FF6;
        }

        return GameVersion.Unknown;
    }

    private static string GetProductName()
    {
        var fileInfo = FileVersionInfo.GetVersionInfo(BepInEx.Paths.ExecutablePath);
        var productName = TrimAfterNullCharacter(fileInfo.ProductName);

        Plugin.Log.LogDebug($"Product name: {productName}");

        return productName;
    }

    private static string GetModuleFileName()
    {
        Plugin.Log.LogDebug($"Module name: {BepInEx.Paths.ProcessName}");

        return BepInEx.Paths.ProcessName;
    }

    private static string TrimAfterNullCharacter(string input)
    {
        int nullCharIndex = input.IndexOf('\0');

        if (nullCharIndex == -1)
        {
            return input;
        }

        return input.Substring(0, nullCharIndex);
    }
}
