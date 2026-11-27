[![Build Application](https://github.com/DigglesMods/DigglesModManager/workflows/Build%20Application/badge.svg)](https://github.com/DigglesMods/DigglesModManager/actions?query=workflow%3A%22Build+Application%22)
[![Software License](https://img.shields.io/badge/license-GPL3-brightgreen.svg?style=flat-square)](LICENSE)

# DigglesModManager
This tool helps you to mod the game "Diggles - The Myth of Fenris" (ger: Wiggles).

You want to create a mod? Here some documentation: https://github.com/DigglesMods/DigglesModManager/wiki/Mod-Developing

## Installation (Windows)
1. Extract the release zip into the game directory (where `Diggles.exe` or `Wiggles.exe` is located), so that
   `DigglesModManager.exe` and the `Mods` directory are next to the game executable.
2. Start `DigglesModManager.exe`, choose your mods and click "Let's Mod".
3. Start the game as usual.

The `DigglesModManager.exe` of the release is self-contained: no .NET runtime has to be installed.
If the game is installed in `C:\Program Files`, the mod manager has to be started as administrator.

## Usage on Linux (Wine, Lutris, Steam/Proton)
The mod manager runs with Wine/Proton, too. As it only changes the game files, it has to run in the same
Wine prefix as the game.

1. Put `DigglesModManager.exe` and the `Mods` directory into the game directory (next to `Wiggles.exe`/`Diggles.exe`).
2. Start the mod manager instead of the game once and click "Let's Mod":
   - **Lutris:** Open the game configuration ("Configure" → "Game options") and set the executable to
     `DigglesModManager.exe` temporarily, or use "Run EXE inside Wine prefix" and select `DigglesModManager.exe`.
   - **Steam/Proton** (game added as non-Steam game): change the target of the shortcut from `Wiggles.exe`
     to `DigglesModManager.exe`, start it, apply the mods and change the target back.
   - **Plain Wine:** `cd` into the game directory and run `wine DigglesModManager.exe`.
3. Start the game as usual.

## Mod dependencies (for mod developers)
Since version 1.4 a mod can declare other mods it depends on in its `config.json`. The id is the directory name of
the other mod (like in `$if:mod:`):

```json
"dependencies": [
    { "id": "BugFix" },
    { "id": "DigglesHDmod", "required": false }
]
```

- **required** (default): the other mod is activated together with this mod, and deactivating it also deactivates this
  mod (after a confirmation). If it is not installed, this mod cannot be activated.
- **optional** (`"required": false`): nothing is activated, but if the other mod is active, it is applied first.

In both cases the mod manager keeps this mod below the mods it depends on in the list of active mods (this is the order
in which the mods are applied). Older versions of the mod manager ignore the field.

## Building
The mod manager is a Windows Forms application for .NET 10. The patch logic is located in the project
`DigglesModManager.Core` (without any UI dependency).

```
dotnet build DigglesModManager.sln
# self-contained single exe (used for releases): bin/publish/self-contained
dotnet publish DigglesModManager.csproj -p:PublishProfile=SelfContained
# small single exe which needs the installed .NET Desktop Runtime 10: bin/publish/framework-dependent
dotnet publish DigglesModManager.csproj -p:PublishProfile=FrameworkDependent
```

See [DevTools/README.md](DevTools/README.md) for the release process.
