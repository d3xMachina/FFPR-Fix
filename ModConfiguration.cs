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
             "Increase or decrease the game speed by X out-of-battle when T or L2 is pressed."
        );

        battleSpeedHackFactor = _config.Bind(
             "Hack",
             "BattleSpeedHackFactor",
             1f,
             "Increase or decrease the game speed by X in battle when T or L2 is pressed."
        );

        battleWaitPlayerCommand = _config.Bind(
             "Hack",
             "BattleWaitPlayerCommand",
             false,
             "Prevent the enemies from attacking during your turn."
        );

        chocoboTurnFactor = _config.Bind(
             "Camera",
             "ChocoboTurnFactor",
             1f,
             "Increase or decrease the camera turn speed when on a chocobo by X."
        );

        airshipTurnFactor = _config.Bind(
             "Camera",
             "AirshipTurnFactor",
             1f,
             "Increase or decrease the camera turn speed when on an airship by X."
        );

        playerWalkspeed = _config.Bind(
             "Player",
             "PlayerWalkspeed",
             1f,
             "Change the player movement speed on the field (SNES is 0.75). Set to 0.75 to avoid stutters at 60fps."
        );
    }
}
