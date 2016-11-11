#!/bin/bash

if [ -z "$1" ]; then
	declare -a dir="$(pwd)/DigglesMods"
else
	declare -a dir=$1
fi

if ! [[ -d "$dir" ]]; then
	if ! [[ -f "$dir" ]]; then
		read -p "Starting clone process. Create directory $dir? (y/n) " -r
		if [[ $REPLY =~ ^[Yy]$ ]]; then
			mkdir -p $dir
		fi
	fi
fi
	

if [ -d "$dir" ] ; then
	cd "$dir" #change to the directory provided (relative to the script)
	read -p "Cloning all mods into '$(pwd)' Are you sure? (y/n) " -r
	if [[ $REPLY =~ ^[Yy]$ ]]; then
		echo "Cloning.."

		## all repo names
		declare -a repoBase="https://github.com/DigglesMods"
		declare -a repoNames=("BetterStorage" "Barrow" "Rimless" "PowerPotion" "ProductionStatistics" 
						"CommandF9" "DwarfStatistics" "SwitcherGeneratorDungeon" "RareResources" 
						"DroppingResources" "ChangeGameSettings" "BugFix" "StoneDoor")

		## clone every repo
		for repo in "${repoNames[@]}"
		do
			#skip if directory exists
			if [ ! -d "$repo" ]; then
				git clone $repoBase/$repo.git
			#else
			#	echo "Directory already exists. Pulling $repoBase/$repo.git in $repo instead..."
			#	cd $repo
			#	git pull 
			#	cd ..
			fi
		done
	fi
else
	echo "The chosen path is not a directory! Aborted."
fi


