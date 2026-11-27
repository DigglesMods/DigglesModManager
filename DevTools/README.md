# DevTools
You can find help for the bash-dev-scripts for DigglesModManager. There are text files and bash scripts in this directory.

## General
Most of the DevTools-Scripts are bash-scripts. You can either run them on unix-based distributions or on windows using a platform like cygwin, mingw etc. A convenient way
to install such an environment is installing git for windows. The git bash is capable of most basic unix-commands.

Run the given scripts with `./script.sh`. Some scripts may be executed from a predefined location, though.

## mod-names.txt
A comma-separated list of all mod-repositories existing on github. At the moment it has to be edited manually. This text file is used
by `clone-all-mods.sh`, `get-all-statuses.sh` and `release.sh` at the moment.

The file must only contain the comma-separated names (no comments, no line breaks).

**TODO:** Add `RevisedPlanner` as soon as its conflicts with the BugFix mod are resolved. Before a release, make sure that
all listed mods are at their latest version (the release script clones the current default branch of each mod).

## packaged-files.txt
A comma-separated list of all files which have to be packaged inside a `DigglesModManager-VERSION.zip`. Since version 1.4 this is only the self-contained `DigglesModManager.exe` (no dlls) plus LICENSE and README.md.
This text file is used by `release.sh` to build a release-package properly.

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

## release.sh
This script automates the release-process to make it less error-prone.

### Usage

1. Install the .NET 10 SDK. The script asks to build the release exe with `dotnet publish DigglesModManager.csproj -p:PublishProfile=SelfContained` (output: `bin/publish/self-contained`).
2. Make sure there is a file `github.token` in the same directory as `release.sh`. It must contain a valid github-token for your account. This token is used to make the new release-package public on github. (https://help.github.com/articles/creating-an-access-token-for-command-line-use/)
2. Use a linux-bash.
3. Navigate to project-root of DigglesModManager. (cd XY)
4. Execute release.sh. (Usually with `DevTools/release.sh`)
5. Press y,Enter if asked for. Provide a valid next version, if asked for. e.g. Release 1.0 -> Next version 1.1, Release 2.9 -> Next version 2.10

### Troubleshooting

 - To reset the working files, completely delete the bin/publish folder and publish again (#1.)
 - The file `github.token` must not contain any other letters or line breaks except the one line with the github-token.
 - Any files which are read by `release.sh` must not contain only one line with comma-separated and trimmed strings.
 - You can revert the relese by visiting https://github.com/DigglesMods/DigglesModManager/releases, clicking the wrong release and click 'Delete'

### Dependencies
The release-script uses 7zip and github-release to automate all necessary release steps. 7zip is used to pack several files to a zip-archive. github-release(.exe) is used to automatically provide a new release on github using the commandline.

#### 7zip
Used command:
`DevTools/7zip/7za.exe a $ZIPFILE $NECESSARY_FILES... Mods`

#### github-release
Used command:
`DevTools/github-release/github-release.exe DigglesMods/DigglesModManager DigglesModManager-v$VERSION master "$DESCRIPTION" $ZIPFILE`

### Release-Steps

What is the release-script doing?

1. Gather all Mods.
  - If the mods are checked out using git, make sure the .git-directories are deleted.
2. Gather all necessary binaries.
3. Zip a folder with the following files and name it DigglesModManager-VERSION.zip
  - Mods
    - ..
  - DigglesModManager.exe
  - LICENSE, README.md
4. Adjust the version in Properties/AssemblyInfo.cs. The version consists of three numbers: `[assembly: AssemblyVersion("major.minor.patch.*")]` The `*` is a .NET-constant which functions as a placeholder for the revision. It is generated automatically by the compiler. Version 1.4.0 would look like this: `[assembly: AssemblyVersion("1.4.0.*")]`
5. Commit version change to version-control:
  ```bash
 git add Properties/AssemblyInfo.cs
 git commit -m "release version x.y and stepped up to x.(y+1)" #REPLACE VERSION
 git pull
 git push
  ```
6. Release zip-package in github.
  - https://github.com/DigglesMods/DigglesModManager/releases/new
  - Describe the release-changes
  - Give it a title like 'DigglesModManager v1.0'
  - Don't forget to provide the zip-file!



