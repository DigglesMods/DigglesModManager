read -p "Show status of all mods? Are you sure? (y/n) " -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	## all repo names
	declare -a repoNames=("BetterStorage" "Barrow" "Rimless" "PowerPotion" "ProductionStatistics" 
					"CommandF9" "DwarfStatistics" "SwitcherGeneratorDungeon" "RareResources" 
					"DroppingResources" "ChangeGameSettings" "BugFix" "StoneDoor")

	## iterte over every repo
	for repo in "${repoNames[@]}"
	do
		if [ -d "$repo" ]; then
			cd $repo
			git status
			cd ..
		fi
	done
fi


