#DevTools
You can find help for the bash-dev-scripts for DigglesModManager. There are text files and bash scripts in this directory.

##General
Most of the DevTools-Scripts are bash-scripts. You can either run them on unix-based distributions or on windows using a platform like cygwin, mingw etc. A convenient way
to install such an environment is installing git for windows. The git bash is capable of most basic unix-commands.

Run the given scripts with `./script.sh`. Some scripts may be executed from a predefined location, though.

##mod-names.txt
A comma-separated list of all mod-repositories existing on github. Has to be edited manually at the moment. This text file is used
by `clone-all-mods.sh`, `get-all-statuses.sh` and `release.sh` at the moment.

##packaged-files.txt
A comma-separated list of all files which have to be packaged inside a release.zip. This includes the binaries, .exe-files, dlls and so on.
This text file is used by release.sh mostly to build a release-package properly.

##clone-all-mods.sh
Clones all Mod-Repositories from github to your computer.

`./clone-all-mods.sh [PATH]`

Provide a path to clone the repositories to a given directory.

##get-all-statuses.sh
Checks the status of Mod-Repositories on your computer. (basically foreach (git status)) The source for the repo-names is `mod-names.txt`.

`./get-all-statuses.sh [PATH]`

Provide a path to check the repos in the given directory. In the given path has to be several directories with valid git-repositories.

###Example:
- you are here
  - DevTools
  - Mods
    - Barrow
    - Rimless
    - ..
    - StoneDoor

```bash
> DevTools/get-all-statuses.sh Mods/
Show status of all mods? Are you sure? (y/n) y
On branch master
Your branch is up-to-date with 'origin/master'.
nothing to commit, working directory clean
..
..
nothing to commit, working directory clean
```


