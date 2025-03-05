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

- If you previously installed BepInEx and have a "mono" folder in the game directory, remove the "mono" and "BepInEx" folders.
- Download BepInEx Bleeding-Edge IL2CPP build from [here](https://builds.bepinex.dev/projects/bepinex_be/733/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.733%2B995f049.zip) and extract the content to the game directory (if playing FFVI, it's where "FINAL FANTASY VI.exe" is located). Replace the files if asked.
- Download the mod [here](https://github.com/d3xMachina/FFPR-Fix/releases/latest) and extract the content to the game directory. Replace the files if asked.
- On Steam Deck, add to the Steam launch options : export WINEDLLOVERRIDES="winhttp=n,b"; %command%
- Run the game once to generate the config file, change the config in "(GAME_PATH)\BepInEx\config\d3xMachina.ffpr_fix.cfg" and restart the game.

## License

FFPR Fix is available on Github under the MIT license.