def_class ProductionManager none none 0 {} {
	def_event evt_timer
	
	set ticks 0
	member ticks
	
	#define if we are in development mode or in production mode
	#if "dev" mode is enabled we will read all method definitions from the file productionmanager_methods_generic.tcl
	# please set this variable in prodman_members_generic.tcl
	set mode "prod"
	member mode
	
	#increase the version number with each release to be downward compatible
	set versionNumber 3
	member versionNumber 
	
	handle_event evt_timer {
		if {[ref_checkvar this versionNumber] == 0} {
			#always set to zero. So we can reinitialize member variables
			ref_set this versionNumber 0
		}
		
		if {$mode == "dev" || $versionNumber < 2} {
			# check if all member variables are defined, if not create them.
			# This is needed so we can update earlier versions of the class ProductionManager
			# Now we do not have to start a new game
			callnc scripts/classes/work/prodman_members_generic.tcl
			set versionNumber 2
		}
		
		# same procedure as explained above.
		# we call generic methods and change the methods at runtime :-)
		# each member variable is reachable in the file productionmanager_methods_generic
		if {$mode == "dev"} {
			set method_name "evt_timer"
			
			callnc scripts/classes/work/prodman_methods_generic.tcl
			return
		}
		
		set ticks [expr {$ticks + 1}]
		
		handle_pickup_tasks
		
		if {[expr {$ticks % 5}] == 0} {
			handle_experience_tasks
		}
		
		handle_equipment_tasks
	}
	
	method add_pickup_task {gnomeID materialIDs} {
		set gidx [lsearch $pickupTaskGnomesList $gnomeID]
		if {$gidx != -1} {
			# delete old task list
			lrem pickupTaskGnomesList $gidx
			lrem pickupTaskActionList $gidx
			lrem pickupGnomeStates $gidx
		}
		
		if {$gnomeID > 0 && [obj_valid $gnomeID] && [check_free_gnome $gnomeID] == 1} {
			set state "muetze [gettime]"
			print "add_pickup_task $gnomeID $materialIDs"
			
			while {1} {
				set mid [lindex $materialIDs 0]
				if {$mid != 0 && [obj_valid $mid]} {
					break
				} 
				
				if {[llength $materialIDs] <= 0} {
					break
				}
				
				lrem materialIDs 0
			}
			
			if {[llength $materialIDs] > 0} {
				set_event $gnomeID evt_change_muetze -target $gnomeID -text1 transport
				
				print "ProductionManager evt_change_muetze -target $gnomeID -text1 transport"
			
				lappend pickupGnomeStates $state
				lappend pickupTaskGnomesList $gnomeID
				lappend pickupTaskActionList $materialIDs
			}
		}
		
		print "add_pickup_task $gnomeID $materialIDs"
	}
	
	method get_free_gnomes {playerID} {
		set gnomeList [obj_query 0 -type gnome -owner $playerID]
		set freeGnomes [list]
		
		foreach gid $gnomeList {
			if {[check_free_gnome $gid] == 1} {
				lappend freeGnomes $gid
			}
		}
		
		return $freeGnomes
	}
	
	method load_pick_up_task {} {
		return $pickupUserInput
	}
	
	method save_pick_up_task {arg} {
		if {[llength $arg] < 2} {
			set pickupUserInput {0 0}
		} else {
			set pickupUserInput $arg
		}
	}
	
	method set_experience_gain {gnomeID attrib} {

		#parameter: gnomeID attrib
		set idx [lsearch $experienceGainGnomeList $gnomeID]
		
		if {($attrib == "" || $attrib == "None")} {
			if {$idx >= 0} {
				lrem experienceGainAttrib $idx
				lrem experienceGainGnomeList $idx
			}
		} elseif {$idx >= 0} {
			lrep experienceGainAttrib $idx $attrib
		} else {
			lappend experienceGainGnomeList $gnomeID
			lappend experienceGainAttrib $attrib
		}
	}
	
	method get_experience_gain {gnomeID} {
		set result ""
		
		#parameter: gnomeID
		set idx [lsearch $experienceGainGnomeList $gnomeID]
		if {$idx >= 0} {
			set result [lindex $experienceGainAttrib $idx]
		} else {
			set result 0
		}
		
		return $result
	}
	
	method add_equipment_task {task} {
		if {$mode == "dev"} {
			set method_name "add_equipment_task"
			callnc scripts/classes/work/prodman_methods_generic.tcl
		}
		
		#parameter: task
		global equipmentTaskList
		
		set timing "start [gettime] step"
		lrep task 5 $timing
		lappend equipmentTaskList $task
	}
	
	method force_equipment_task {task} {
		if {$mode == "dev"} {
			set method_name "force_equipment_task"
			callnc scripts/classes/work/prodman_methods_generic.tcl
			return
		}
		
		global equipmentTaskList
		
		for {set i 0} {$i < [llength $equipmentTaskList]} {incr i 1} {
			set li [lindex $equipmentTaskList $i]
			if {[lindex $task 0] == [lindex $li 0]} {
				if {[lindex $task 2] == [lindex $li 2] && [lindex $task 4] == [lindex $li 4]} {
					set timing [lindex $li 5] 
					lrep timing 0 "instant"
					lrep li 5 $timing
					lrep equipmentTaskList $i $li
					break
				}
			}
		}
	}
	
	method remove_equipment_task {task} {
		if {$mode == "dev"} {
			set method_name "remove_equipment_task"
			callnc scripts/classes/work/prodman_methods_generic.tcl
			return
		}
		
		global equipmentTaskList
		
		for {set i 0} {$i < [llength $equipmentTaskList]} {incr i 1} {
			set li [lindex $equipmentTaskList $i]
			if {[lindex $task 0] == [lindex $li 0]} {
				if {[lindex $task 2] == [lindex $li 2] && [lindex $task 4] == [lindex $li 4]} {
					lrem equipmentTaskList $i
					break
				}
			}
		}
	}
	
	method get_all_equipment_tasks {} {
		set result ""
		
		if {$mode == "dev"} {
			set method_name "get_all_equipment_tasks"
			callnc scripts/classes/work/prodman_methods_generic.tcl
			return $result
		} else {
			global equipmentTaskList
			set result $equipmentTaskList
			return $result
		}
	}
	
	obj_init {
		#all the variables will be stored in the game save file
		#add a variable and modify prodman_members_generic.tcl !
		set pickupUserInput {0 0}
		set pickupGnomeStates [list]
		set pickupTaskGnomesList [list]
		set pickupTaskActionList [list]
		set experienceGainGnomeList [list]
		set experienceGainAttrib [list]
		set experienceProdEnabled 0
		set equipmentUserInput {0 0 0 0 0 0}
		set equipmentTaskList 0
		
		call scripts/classes/work/prodman_proc.tcl
	}
}
