using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace FFPR_Fix
{
    [BepInPlugin("d3xMachina.ffpr_fix", "FFPR Fix", "1.0.0.0")]
    public class Plugin : BasePlugin
    {
        internal static new ManualLogSource Log;

        public static ConfigEntry<bool> uncapFPS;
        public static ConfigEntry<bool> hideFieldMinimap;
        public static ConfigEntry<bool> hideWorldMinimap;
        public static ConfigEntry<bool> skipSplashscreens;
        public static ConfigEntry<float> playerMovespeed;

        public override void Load()
        {
            Log = base.Log;

            Log.LogInfo("Loading...");

            InitConfig();
            ApplyPatches();

            Log.LogInfo("Patches applied!");
        }

        private void ApplyPatches()
        {
            if (uncapFPS.Value)
            {
                Harmony.CreateAndPatchAll(typeof(FrameratePatch));
            }

            if (hideFieldMinimap.Value || hideWorldMinimap.Value)
            {
                Harmony.CreateAndPatchAll(typeof(UIPatch));
            }

            if (skipSplashscreens.Value)
            {
                Harmony.CreateAndPatchAll(typeof(SkipIntroPatch));
            }

            /* Crash because of a bug with BepInEx and Il2CppSystem.Nullable in methods
            if (playerMovespeed.Value > 0f)
            {
                Harmony.CreateAndPatchAll(typeof(PlayerPatch));
            }
            */
        }

        private void InitConfig()
        {
            uncapFPS = Config.Bind(
                 "Framerate",
                 "Uncap",
                 false,
                 "Remove the 60 FPS limit. Use with SpecialK or Rivatuner to cap it to the intended FPS and force VSync."
            );

            hideFieldMinimap = Config.Bind(
                 "UI",
                 "HideFieldMinimap",
                 false,
                 "Hide the field minimap."
            );

            hideWorldMinimap = Config.Bind(
                 "UI",
                 "HideWorldMinimap",
                 false,
                 "Hide the world minimap."
            );

            skipSplashscreens = Config.Bind(
                 "Skip intro",
                 "SkipSplashscreens",
                 true,
                 "Skip the intro splashscreens."
            );

            /*
            playerMovespeed = Config.Bind(
                 "Player",
                 "PlayerMovespeed",
                 -1f,
                 "Change the player movement speed on the field (game default is 0.24, SNES is 0.32)."
            );
            */
        }
    }
}
