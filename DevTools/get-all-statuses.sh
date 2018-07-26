#!/bin/bash
read -p "Show status of all mods? Are you sure? (y/n) " -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
	#all currently available mods (read from mod-names.txt)
	IFS=$',' read -d '' -r -a repoNames < $SCRIPT_DIR/mod-names.txt

	## iterte over every repo
	for repo in "${repoNames[@]}"
	do
		if [ -d "$repo" ]; then
			echo "Checking '$repo'"
			cd $repo
			git status
			cd ..
		fi
	done
fi
