#!/bin/bash
read -p "Cloning all mods into 'DigglesMods' Are you sure? (y/n) " -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	echo "Cloning.."
	declare -a dir=DigglesMods
	## create folder for repos
    mkdir -p $dir
	cd $dir

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
		else
			echo "Directory already exists. Skipping $dir/$repo..."
		fi
	done
fi


