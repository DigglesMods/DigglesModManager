# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).
The section of a version is used as description of its GitHub release (see [DevTools/README.md](DevTools/README.md)).

## [v1.4.0] - unreleased
### Added
- Mods can declare dependencies on other mods in their config.json: required mods are activated automatically and are
  always applied first (#41).
- Mods can declare incompatibilities with other mods: either they cannot be active at the same time, or the user is warned.
- Usage on Linux with Wine, Lutris or Steam/Proton is described in the README (#48).

### Changed
- Updated to .NET 10. The DigglesModManager.exe is self-contained: no .NET runtime has to be installed, and the
  Newtonsoft.Json.dll is not needed anymore (it can be deleted when updating).
- The arrow buttons use images instead of special characters, which were missing under Wine (#48).
- The restore file (restore.dm) stores paths relative to the game directory, so a moved or copied game directory can
  still be restored. Restore files of older versions are still supported.
- The active mods keep their order after a restart. This is the order in which the mods are applied.
- A warning is written to diggles-mod-manager-errors.log if a text that a mod wants to change is not found.
- The game directory is also found if the mod manager is started from another working directory.

### Fixed
- Umlauts and other special characters in game files were destroyed by the unreleased .NET 6 version.
- Mod config files in ANSI encoding are read correctly.
- Directories created by mods are removed when the mods are deactivated.
- A CHANGELOG.md in the root directory of a mod is not copied into the game directory anymore.

### Included mods
- RevisedPlanner was added to the release package.

## [v1.3.1] - 2021-01-12
### Fixed
- Mod settings were not read correctly (thanks to Voidheartd for the report).
- The new version was not shown.

## [v1.3.0] - 2021-01-11
### Changed
- The user interface was changed: the lists show icons, mods can show messages to inform the user (e.g. about
  incompatibilities), and a description box shows the selected mod.
- Mods that change textures work correctly with the English and Polish game version.
- Newtonsoft.Json updated.
- Czech language supported.

### Fixed
- A wrong warning about an $ifend.

### Included mods
- ElfPatch and Recycling added; Czech and Russian support for many mods.

## [v1.2.1] - 2020-05-12
### Added
- Mod settings with drop-down menus (see ChangeCampaignClan).
- Mod configs can define optional data directories, which are used only with certain settings or installed mods.
- Files that are added or replaced by mods can contain mod variables.
- Polish and Russian support.
- Installation directories with a Diggles.exe are supported.
- Errors and warnings of the "Let's Mod" process are logged in diggles-mod-manager-errors.log.

### Changed
- The min and max values of integer mod settings are considered.
- Improved stability of the "Let's Mod" process and a more accurate progress bar.

### Fixed
- The mod settings window of mods with more than 7 settings.

### Included mods
- MushroomSettings added; optional HD mod: https://github.com/DigglesMods/DigglesHDmod/releases

## [v1.1.0] - 2019-03-12
### Added
- Support for the Polish game version and Polish translations (thanks to Saguro and shrike82).

### Included mods
- ChangeCampaignClan added; bugfixes of PowerPotion and ChangeGameSettings.

## [v1.0.0] - 2018-07-26
### Changed
- New design with support for different languages and links to the wiki with the mod descriptions.
- Compatible with Windows XP.
- Support for the Polish game version.
- Almost all file formats changed to JSON (mods in the old format are not supported anymore).

### Fixed
- Mod settings are used correctly in all cases.

Older versions: see https://github.com/DigglesMods/DigglesModManager/releases
