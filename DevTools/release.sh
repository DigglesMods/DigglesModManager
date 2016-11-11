#!/bin/bash

#this part modifies the standard output to write everything to a log-file (release.log)
LOG_LOCATION=release.log
rm -f release.log
exec > >(tee -i $LOG_LOCATION)
exec 2>&1

#this function converts an array to a space-divided string
function concat_array { local IFS="$1"; shift; echo "$*"; }

assemblyInfoFile=Properties/AssemblyInfo.cs
#retrieve the current versino from AssemblyInfo.cs
version=$(sed -nre 's/^\[assembly: AssemblyVersion\(\"(.*)\.\*\"\)\]/\1/p' $assemblyInfoFile)
			
#the location of the release.sh-script itself
scriptLocation=$(pwd)/$(dirname "$0")

organization=DigglesMods
#the github-repo to publish the release in
releaseRepository=aehrraid/evolution #$organization/DigglesModManager

#all currently available mods (read from mod-names.txt)
IFS=$'\n' read -d '' -r -a repoNames < $scriptLocation/mod-names.txt

#read all necessary files from packaged-files.txt
IFS=$'\n' read -d '' -r -a necessaryFiles < $scriptLocation/packaged-files.txt

read -p "Release version $version of DigglesModManager? (y/n) " -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	workDir=$(pwd)/bin/Release
	
	echo "Please build the project with Release-Profile before continuing. There have to be several necessary files in $workDir. These include:"
	for file in "${necessaryFiles[@]}"
	do
		echo "$workDir/$file"
	done
	
	echo "Current files in target folder are: "
	ls -l $workDir
	echo ""
	
	read -p "Continue? (y/n) " -r
	if [[ $REPLY =~ ^[Yy]$ ]]
	then
		echo "Validating..."
		
		cd $workDir
		
		for file in "${necessaryFiles[@]}"
		do
			if ! [[ -f "$file" ]]; then
				echo "Could not find $file! Aborting.."
				exit 1
			fi
		done
		
		echo "Preparing..."
		
		echo "Current mods are"
		for repo in "${repoNames[@]}"
		do
			echo "$repo"
		done
								
		declare -a modDir=Mods
		## create folder for repos
		mkdir -p $modDir #ALL FILES AT THIS PATH ARE GOING TO BE DELETED IN THE END
		cd $modDir

		## all repo names
		declare -a repoBase="https://github.com/$organization"

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
		
		echo "Checked out all necessary mods."
		echo "Removing .git-directories..."
		
		for repo in "${repoNames[@]}"
		do
			#skip if directory exists
			cd $repo
			rm -rf ".git"
			cd ..
		done
		
		cd ..
		
		declare -a releaseFileName="DigglesModManager-$version.zip"
		if [[ -f "$releaseFileName" ]]; then
			echo "Removing existing Release-package..."
			rm $releaseFileName
		fi
		
		7z a $releaseFileName $(concat_array " " "${necessaryFiles[@]}") Mods

		echo "Packed zip."
		
		rm -rf $modDir #CARE!
		#TODO remove $modDir even if script crashes
		echo "Removed mod directories."
		
		cd ../..
				
		read -p "Adjust version number (current is $version)? (y/n) " -r
		if [[ $REPLY =~ ^[Yy]$ ]]
		then
			proposal=$(echo $version | grep -o "[0-9]*$")
			proposal=$(echo $version | grep -o "^[0-9]*").$(($proposal+1))
			read -p "New version [x.y] Version '$proposal' is proposed: " -r
			if [ -z "$REPLY" ]; then
				newVersion=$proposal
			else
				newVersion=$REPLY
			fi
			echo "Adjusting version to $newVersion in $assemblyInfoFile..."
			
			sed -i "/\[assembly: AssemblyVersion/ c\[assembly: AssemblyVersion(\"$newVersion.*\")]" $assemblyInfoFile
			
			read -p "Commit and push changes to github? (y/n) " -r
			if [[ $REPLY =~ ^[Yy]$ ]]
			then
				git add Properties/AssemblyInfo.cs
				git status
				git commit -m "release version $version and stepped up to $newVersion"
				git pull
				git push
			fi
		fi
		
		read -p "Release new version as zip to github? (y/n) " -r
		if [[ $REPLY =~ ^[Yy]$ ]]
		then
			echo "Please provide a valid github-token. Locate the token at $scriptLocation/github.token"
			echo "Generate a token using this manual https://help.github.com/articles/creating-an-access-token-for-command-line-use/"
			read -p "Continue? (y/n) " -r
			if [[ $REPLY =~ ^[Yy]$ ]]
			then
				token=$(cat DevTools/github.token)
				export GITHUB_TOKEN=$token
				
				IFS= read -r -p "Please enter a release-description: "
				description=$REPLY
				if [ -z "$description" ]; then
					echo "WARNING: Releasing with empty description. Please adjust on github.com!"
				fi

				$scriptLocation/github-release $releaseRepository DigglesModManager-v$version master "$description" $workDir/$releaseFileName
				echo "Finished."
			fi
		fi
	fi
fi


