using BepInEx.Configuration;

namespace FFPR_Fix;

public sealed class ModConfiguration
{
    private ConfigFile _config;
    public ConfigEntry<bool> uncapFPS;
    public ConfigEntry<bool> enableVsync;
    public ConfigEntry<bool> hideFieldMinimap;
    public ConfigEntry<bool> hideWorldMinimap;
    public ConfigEntry<bool> skipSplashscreens;
    public ConfigEntry<bool> skipPressAnyKey;
    public ConfigEntry<float> playerWalkspeed;
    public ConfigEntry<float> outBattleSpeedHackFactor;
    public ConfigEntry<float> battleSpeedHackFactor;
    public ConfigEntry<float> chocoboTurnFactor;
    public ConfigEntry<float> airshipTurnFactor;
    public ConfigEntry<bool> battleWaitPlayerCommand;
    public ConfigEntry<float> battleATBSpeed;
    public ConfigEntry<bool> runOnWorldMap;
    public ConfigEntry<bool> disableDiagonalMovements;
    public ConfigEntry<bool> useDecryptedSaveFiles;

    public ModConfiguration(ConfigFile config)
    {
        _config = config;
    }

    public void Init()
    {
        uncapFPS = _config.Bind(
             "Framerate",
             "Uncap",
             false,
             "Remove the 60 FPS limit. Use with SpecialK or Rivatuner to cap it to the intended FPS for best frame pacing."
        );

        enableVsync = _config.Bind(
             "Framerate",
             "Vsync",
             false,
             "Enable VSync. The framerate will match your monitor refresh rate."
        );

        hideFieldMinimap = _config.Bind(
             "UI",
             "HideFieldMinimap",
             false,
             "Hide the field minimap."
        );

        hideWorldMinimap = _config.Bind(
             "UI",
             "HideWorldMinimap",
             false,
             "Hide the world minimap."
        );

        skipSplashscreens = _config.Bind(
             "Skip intro",
             "SkipSplashscreens",
             true,
             "Skip the intro splashscreens."
        );

        skipPressAnyKey = _config.Bind(
             "Skip intro",
             "SkipPressAnyKey",
             false,
             "Skip the intro movie and the \"Press any key\" before the title screen."
        );

        outBattleSpeedHackFactor = _config.Bind(
             "Hack",
             "OutBattleSpeedHackFactor",
             1f,
             "Increase or decrease the game speed by X out-of-battle when T or PageUp (LT/L2 by default) is pressed."
        );

        battleSpeedHackFactor = _config.Bind(
             "Hack",
             "BattleSpeedHackFactor",
             1f,
             "Increase or decrease the game speed by X in battle when T or PageUp (LT/L2 by default) is pressed."
        );

        battleWaitPlayerCommand = _config.Bind(
             "Battle",
             "WaitPlayerCommand",
             false,
             "Pause the battle when it's your turn. You can resume until the next unit is ready by pressing P or Select. (FF4,5 and 6 only)"
        );

        battleATBSpeed = _config.Bind(
             "Battle",
             "ATBSpeed",
             1f,
             "Change the rate at which the ATB gauge fills. Best used with WaitPlayerCommand for a Turn based experience. (FF4,5 and 6 only)"
        );

        chocoboTurnFactor = _config.Bind(
             "Camera",
             "ChocoboTurnFactor",
             1f,
             "Increase or decrease the camera turn speed when on a chocobo by X. (FF3,5 and 6 only)"
        );

        airshipTurnFactor = _config.Bind(
             "Camera",
             "AirshipTurnFactor",
             1f,
             "Increase or decrease the camera turn speed when on an airship by X."
        );

        playerWalkspeed = _config.Bind(
             "Movement",
             "PlayerWalkspeed",
             1f,
             "Change the player movement speed on the field (SNES is 0.75). Set to 0.75 and enable DisableDiagonalMovements to avoid stutters at 60fps."
        );

        runOnWorldMap = _config.Bind(
             "Movement",
             "RunOnWorldMap",
             false,
             "Allow the player to run when on the world map."
        );

        disableDiagonalMovements = _config.Bind(
             "Movement",
             "DisableDiagonalMovements",
             false,
             "Restrict the movements to 4 directions instead of 8, like on the SNES."
        );

        useDecryptedSaveFiles = _config.Bind(
             "Advanced",
             "UseDecryptedSaveFiles",
             false,
             "USE AT YOUR OWN RISKS. BACKUP YOUR SAVES FIRST. The game won't encrypt the save files which will be in json format. It will only work with new saves."
        );
    }
}
