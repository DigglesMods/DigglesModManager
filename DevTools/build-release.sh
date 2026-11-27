#!/bin/bash
# Builds the release package bin/release-package/DigglesModManager-<version>.zip:
# - the self-contained DigglesModManager.exe (dotnet publish) and the files of packaged-files.txt
# - the directory "Mods" with the current state (default branch) of all mods of mod-names.txt
# Additionally bin/release-package/included-mods.md lists the versions of the included mods (used in the release notes).
#
# Used by the GitHub workflow .github/workflows/release.yml, but it can also be run locally (Git Bash, .NET 10 SDK).
# usage: DevTools/build-release.sh
set -euo pipefail

devTools="$(cd "$(dirname "$0")" && pwd)"
root="$(cd "$devTools/.." && pwd)"
organization=DigglesMods

version=$(tr -d '\r' < "$root/Properties/AssemblyInfo.cs" | sed -nre 's/^\[assembly: AssemblyVersion\("(.*)\.\*"\)\]/\1/p')
if [ -z "$version" ]; then
	echo "Could not read the version from Properties/AssemblyInfo.cs" >&2
	exit 1
fi
echo "Building DigglesModManager $version"

out="$root/bin/release-package"
package="$out/package"
publishDir="$root/bin/publish/self-contained"
rm -rf "$out" "$publishDir"
mkdir -p "$package/Mods"

# build the exe
dotnet publish "$root/DigglesModManager.csproj" -p:PublishProfile=SelfContained

# files of the mod manager
IFS=',' read -r -a packagedFiles <<< "$(tr -d '\r\n' < "$devTools/packaged-files.txt")"
for file in "${packagedFiles[@]}"; do
	if [ ! -f "$publishDir/$file" ]; then
		echo "Could not find $publishDir/$file" >&2
		exit 1
	fi
	cp "$publishDir/$file" "$package/"
done

# mods (current state of the default branch of each mod)
IFS=',' read -r -a modNames <<< "$(tr -d '\r\n' < "$devTools/mod-names.txt")"
echo "### Included mod versions" > "$out/included-mods.md"
for mod in "${modNames[@]}"; do
	git clone --quiet --depth 1 "https://github.com/$organization/$mod.git" "$package/Mods/$mod"
	# .github would be copied into the game directory by the mod manager
	rm -rf "$package/Mods/$mod/.git" "$package/Mods/$mod/.github"
	modVersion=$(grep -a -o -m 1 '"version" *: *"[^"]*"' "$package/Mods/$mod/config.json" | sed -e 's/.*"\([^"]*\)"$/\1/' || true)
	echo "- $mod ${modVersion:+v$modVersion}" >> "$out/included-mods.md"
done

# zip (7-Zip of the GitHub runner or of the DevTools)
zipFile="$out/DigglesModManager-$version.zip"
if command -v 7z > /dev/null; then
	sevenZip=7z
else
	sevenZip="$devTools/7zip/7za.exe"
fi
(cd "$package" && "$sevenZip" a -tzip -bso0 "$zipFile" ./*)

echo "Created $zipFile"
cat "$out/included-mods.md"
