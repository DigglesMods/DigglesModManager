# DevTools
You can find help for the bash-dev-scripts for DigglesModManager. There are text files and bash scripts in this directory.

## General
Most of the DevTools-Scripts are bash-scripts. You can either run them on unix-based distributions or on windows using a platform like cygwin, mingw etc. A convenient way
to install such an environment is installing git for windows. The git bash is capable of most basic unix-commands.

Run the given scripts with `./script.sh`. Some scripts may be executed from a predefined location, though.

## mod-names.txt
A comma-separated list of all mod-repositories existing on github. At the moment it has to be edited manually. This text file is used
by `clone-all-mods.sh`, `get-all-statuses.sh` and `build-release.sh` at the moment.

The file must only contain the comma-separated names (no comments, no line breaks).

Before a release, make sure that all listed mods are at their latest version (the release script clones the current default branch of each mod).

## packaged-files.txt
A comma-separated list of all files which have to be packaged inside a `DigglesModManager-VERSION.zip`. Since version 1.4 this is only the self-contained `DigglesModManager.exe` (no dlls) plus LICENSE and README.md.
This text file is used by `build-release.sh` to build a release-package properly.

## clone-all-mods.sh
Clones all Mod-Repositories from github to your computer. The source for the repo-names is `mod-names.txt`.

`./clone-all-mods.sh [PATH]`

Provide a path to clone the repositories to a given directory.

## get-all-statuses.sh
Checks the status of Mod-Repositories on your computer (basically foreach \[git status\]). The shell has to be inside of the DigglesMods-directory.

```bash
cd DigglesMods
../DevTools/get-all-statuses.sh
```

### Example:
- root
  - DevTools
  - DigglesMods <-- you are here
    - Barrow
    - Rimless
    - ..
    - StoneDoor

```bash
> ../DevTools/get-all-statuses.sh
Show status of all mods? Are you sure? (y/n) y
On branch master
Your branch is up-to-date with 'origin/master'.
nothing to commit, working directory clean
..
..
nothing to commit, working directory clean
```

## Release
Releases are created by the GitHub workflow `.github/workflows/release.yml`, like the releases of the mods:
pushing a tag `vX.Y.Z` builds the release package and publishes it as GitHub release. The description of the
release is the section of the version in `CHANGELOG.md`, followed by the list of the included mods and their versions.

### Release steps
1. Make sure all mods of `mod-names.txt` are released in the version that should be part of the package (the package
   contains the current default branch of each mod).
2. Set the version in `Properties/AssemblyInfo.cs`: `[assembly: AssemblyVersion("1.4.0.*")]` (the `*` is replaced by
   the compiler).
3. Update `CHANGELOG.md`: the section `## [v1.4.0] - YYYY-MM-DD` contains all changes of this version.
4. Optional: test the package with "Run workflow" of the workflow "Release" on GitHub (Actions). This only builds the
   package and uploads it as artifact, nothing is published.
5. Commit and push the changes, then create and push the tag:
   ```bash
   git tag v1.4.0
   git push origin v1.4.0
   ```
   The workflow fails if the tag does not match the version in `Properties/AssemblyInfo.cs` or if `CHANGELOG.md` has
   no section for the version.
6. Add a new section for the next version to `CHANGELOG.md` when the next changes are made.

## build-release.sh
Builds the release package `bin/release-package/DigglesModManager-<version>.zip` (used by the release workflow):
the self-contained `DigglesModManager.exe` (`dotnet publish -p:PublishProfile=SelfContained`), the files of
`packaged-files.txt` and the directory `Mods` with all mods of `mod-names.txt` (cloned from GitHub, without `.git` and
`.github`). It can also be run locally with Git Bash and the .NET 10 SDK: `DevTools/build-release.sh`

## changelog-section.sh
Prints the section of a version from `CHANGELOG.md`: `DevTools/changelog-section.sh v1.4.0`
