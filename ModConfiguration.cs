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
    public ConfigEntry<float> playerMovespeed;
    public ConfigEntry<float> outBattleSpeedHackFactor;
    public ConfigEntry<float> battleSpeedHackFactor;

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
             "Increase the game speed by X out-of-battle when T or L2 is pressed."
        );

        battleSpeedHackFactor = _config.Bind(
             "Hack",
             "BattleSpeedHackFactor",
             1f,
             "Increase the game speed by X in battle when T or L2 is pressed."
        );

        /*
        playerMovespeed = _config.Bind(
             "Player",
             "PlayerMovespeed",
             -1f,
             "Change the player movement speed on the field (game default is 0.24, SNES is 0.32)."
        );
        */
    }
}
