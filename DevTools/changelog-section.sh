#!/bin/bash
# Prints the section of the given version from CHANGELOG.md (without the heading), like the release workflow of the mods.
# usage: DevTools/changelog-section.sh v1.4.0
set -euo pipefail

if [ $# -ne 1 ]; then
	echo "usage: $0 <version, e.g. v1.4.0>" >&2
	exit 1
fi
version=$1
changelog="$(cd "$(dirname "$0")/.." && pwd)/CHANGELOG.md"

# remove everything up to the heading of the version and everything from the next heading on
tr -d '\r' < "$changelog" | sed "0,/^## \[${version//./\\.}\]/d" | sed '/^## \[/,$d'
