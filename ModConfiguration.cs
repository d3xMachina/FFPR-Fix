using BepInEx.Configuration;

namespace FFPR_Fix;

public sealed class ModConfiguration
{
    private ConfigFile _config;
    public ConfigEntry<bool> UncapFPS;
    public ConfigEntry<bool> EnableVsync;
    public ConfigEntry<bool> HideFieldMinimap;
    public ConfigEntry<bool> HideWorldMinimap;
    public ConfigEntry<bool> SkipSplashscreens;
    public ConfigEntry<bool> SkipPressAnyKey;
    public ConfigEntry<float> PlayerWalkspeed;
    public ConfigEntry<float> OutBattleSpeedHackFactor;
    public ConfigEntry<float> BattleSpeedHackFactor;
    public ConfigEntry<float> ChocoboTurnFactor;
    public ConfigEntry<float> AirshipTurnFactor;
    public ConfigEntry<bool> BattleWaitPlayerCommand;
    public ConfigEntry<float> BattleATBSpeed;
    public ConfigEntry<bool> RunOnWorldMap;
    public ConfigEntry<bool> DisableDiagonalMovements;
    public ConfigEntry<bool> BackupSaveFiles;
    public ConfigEntry<bool> UseDecryptedSaveFiles;

    public ModConfiguration(ConfigFile config)
    {
        _config = config;
    }

    public void Init()
    {
        UncapFPS = _config.Bind(
             "Framerate",
             "Uncap",
             false,
             "Remove the 60 FPS limit. Use with SpecialK or Rivatuner to cap it to the intended FPS for best frame pacing."
        );

        EnableVsync = _config.Bind(
             "Framerate",
             "Vsync",
             false,
             "Enable VSync. The framerate will match your monitor refresh rate."
        );

        HideFieldMinimap = _config.Bind(
             "UI",
             "HideFieldMinimap",
             false,
             "Hide the field minimap."
        );

        HideWorldMinimap = _config.Bind(
             "UI",
             "HideWorldMinimap",
             false,
             "Hide the world minimap."
        );

        SkipSplashscreens = _config.Bind(
             "Skip intro",
             "SkipSplashscreens",
             true,
             "Skip the intro splashscreens."
        );

        SkipPressAnyKey = _config.Bind(
             "Skip intro",
             "SkipPressAnyKey",
             false,
             "Skip the intro movie and the \"Press any key\" before the title screen."
        );

        OutBattleSpeedHackFactor = _config.Bind(
             "Hack",
             "OutBattleSpeedHackFactor",
             1f,
             "Increase or decrease the game speed by X out-of-battle when T or PageUp (LT/L2 by default) is pressed."
        );

        BattleSpeedHackFactor = _config.Bind(
             "Hack",
             "BattleSpeedHackFactor",
             1f,
             "Increase or decrease the game speed by X in battle when T or PageUp (LT/L2 by default) is pressed."
        );

        BattleWaitPlayerCommand = _config.Bind(
             "Battle",
             "WaitPlayerCommand",
             false,
             "Pause the battle when it's your turn. You can resume until the next unit is ready by pressing P or Select. (FF4,5 and 6 only)"
        );

        BattleATBSpeed = _config.Bind(
             "Battle",
             "ATBSpeed",
             1f,
             "Change the rate at which the ATB gauge fills. Best used with WaitPlayerCommand for a Turn based experience. (FF4,5 and 6 only)"
        );

        ChocoboTurnFactor = _config.Bind(
             "Camera",
             "ChocoboTurnFactor",
             1f,
             "Increase or decrease the camera turn speed when on a chocobo by X. (FF3,5 and 6 only)"
        );

        AirshipTurnFactor = _config.Bind(
             "Camera",
             "AirshipTurnFactor",
             1f,
             "Increase or decrease the camera turn speed when on an airship by X."
        );

        PlayerWalkspeed = _config.Bind(
             "Movement",
             "PlayerWalkspeed",
             1f,
             "Change the player movement speed on the field (SNES is 0.75). Set to 0.75 and enable DisableDiagonalMovements to avoid stutters at 60fps."
        );

        RunOnWorldMap = _config.Bind(
             "Movement",
             "RunOnWorldMap",
             false,
             "Allow the player to run when on the world map."
        );

        DisableDiagonalMovements = _config.Bind(
             "Movement",
             "DisableDiagonalMovements",
             false,
             "Restrict the movements to 4 directions instead of 8, like on the SNES."
        );

        BackupSaveFiles = _config.Bind(
             "Save",
             "BackupSaveFiles",
             false,
             "Make a backup of the existing save when the game is saved. Saves are located in \"%USERPROFILE%\\Documents\\My Games\\FINAL FANTASY [GAME_VERSION] PR\\Steam\\[YOUR_ID]\"." +
             " Backups have the .bak extension. Useful to recover from corrupted save files."
        );

        UseDecryptedSaveFiles = _config.Bind(
             "Save",
             "UseDecryptedSaveFiles",
             false,
             "USE AT YOUR OWN RISKS. BACKUP YOUR SAVES FIRST. The game won't encrypt the save files which will be in json format. It will only work with new saves."
        );
    }
}
