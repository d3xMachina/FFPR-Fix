# FFPR Fix

A BepInEx plugin containing a bunch of tweaks for Final Fantasy Pixel Remaster games.

## Features

- Uncap framerate (and fix input issues from higher framerate).
- Skip the intro splashscreens and the following "Press any key".
- Hide the field and world minimaps separately.
- Change the player walk speed (to match the SNES for example).
- Change the camera turn speed on a chocobo and on an airship.
- Pause the battle when it's your turn.
- Change the ATB gauge fill rate.
- Speedup the game (speedhack).
- Allow running on the world map.
- Disable diagonal movements like on the SNES.
- Automatic saves backup.
- Use of decrypted saves. (AT YOUR OWN RISKS)

## Installation

- Install BepInEx Bleeding-Edge IL2CPP build from [here](https://builds.bepinex.dev/projects/bepinex_be/577/BepInEx_UnityIL2CPP_x64_ec79ad0_6.0.0-be.577.zip).
- Drop the BepInEx folder from the mod archive in the game directory. (if playing FFVI, it's where "FINAL FANTASY VI.exe" is located)
- On Steam Deck, add to the Steam launch options : export WINEDLLOVERRIDES="winhttp=n,b"; %command%
- Run the game once to generate the config file, change the config in "(GAME_PATH)\BepInEx\config\d3xMachina.ffpr_fix.cfg" and restart the game.

## License

FFPR Fix is available on Github under the MIT license.