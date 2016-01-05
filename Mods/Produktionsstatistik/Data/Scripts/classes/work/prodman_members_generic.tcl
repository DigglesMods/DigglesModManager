# check for variable in ProductionManager
if {[ref_checkvar this ticks] == 0} {
	# create the member variable if it does not exist
	#version 1
	ref_set this ticks 0
}

#version 1
ref_set this mode "prod"

if {[ref_checkvar this pickupGnomeStates] == 0} {
	#version 1
	ref_set this pickupGnomeStates [list]
}

if {[ref_checkvar this pickupUserInput] == 0} {
	#version 1
	ref_set this pickupUserInput {0 0}
}

if {[ref_checkvar this pickupTaskGnomesList] == 0} {
	#version 1
	ref_set this pickupTaskGnomesList [list]
}

if {[ref_checkvar this pickupTaskActionList] == 0} {
	#version 1
	ref_set this pickupTaskActionList [list]
}

if {[ref_checkvar this experienceGainGnomeList] == 0 } {
	#version 2
	ref_set this experienceGainGnomeList [list]
}

if {[ref_checkvar this experienceGainAttrib] == 0 } {
	#version 2
	ref_set this experienceGainAttrib [list]
}

if {[ref_checkvar this experienceProdEnabled] == 0 } {
	#version 2
	ref_set this experienceProdEnabled 0
}

if {[ref_checkvar this equipmentUserInput] == 0 } {
	#version 3
	ref_set this equipmentUserInput {0 0 0 0 0 0}
}

if {[ref_checkvar this equipmentTaskList] == 0 } {
	#version 3
	ref_set this equipmentTaskList 0
}

if {[ref_checkvar this pickupDwarfPoint] == 0 } {
	#version 4
	ref_set this pickupDwarfPoint 0
}



