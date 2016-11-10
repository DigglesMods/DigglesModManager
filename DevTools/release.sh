#!/bin/bash

exit 1;
read -p "Release a new version of DigglesModManager? (y/n)" -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	read -p "Please put all necessary built binaries in the current folder. Continue (y/n)" -r
	if [[ $REPLY =~ ^[Yy]$ ]]

		
		echo "Preparing..."
		
		declare -a repoNames=("BetterStorage" "Barrow" "Rimless" "PowerPotion" "ProductionStatistics" 
						"CommandF9" "DwarfStatistics" "SwitcherGeneratorDungeon" "RareResources" 
						"DroppingResources" "ChangeGameSettings" "BugFix" "StoneDoor")
						
		echo "Current Mods are "
		echo $repoNames
		
		declare -a modDir=DigglesMods
		## create folder for repos
		mkdir -p $modDir
		cd $modDir

		## all repo names
		declare -a repoBase="https://github.com/DigglesMods"

		## clone every repo
		for repo in "${repoNames[@]}"
		do
			#skip if directory exists
			if [ ! -d "$repo" ]; then
				git clone $repoBase/$repo.git
			else
				echo "Directory already exists. Skipping $modDir/$repo..."
			fi
		done
		
		echo "Checked out necessary mods."
	fi
fi


