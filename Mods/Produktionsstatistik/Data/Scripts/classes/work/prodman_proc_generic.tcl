#this file contains generic definitions of the procedures of class ProductionManager
#this file is supposed to be used for development

if {[info exists proc_name]} {
switch $proc_name {
	"compare_by_distance_from_point" {
		global pickupDwarfPoint
		
		if {$a == -1} {
			print "ProductionManager ERROR sorting"
			return -1
		}
		if {$b == -1} {
			print "ProductionManager ERROR sorting"
			return 1
		}
		
		set distanceA [expr {abs([get_posx $a] - [get_posx $pickupDwarfPoint]) + abs([get_posy $a] - [get_posy $pickupDwarfPoint])}]
		set distanceB [expr {abs([get_posx $b] - [get_posx $pickupDwarfPoint]) + abs([get_posy $b] - [get_posy $pickupDwarfPoint])}]
		
		return [expr {$distanceA > $distanceB}]
	}
	
	 "handle_pickup_tasks" {
	global pickupGnomeStates pickupTaskGnomesList pickupTaskActionList pickupDwarfPoint
	global ticks
	
		#check if the have pick up tasks
		if {[llength $pickupGnomeStates] != [llength $pickupTaskActionList] || [llength $pickupTaskGnomesList] != [llength $pickupTaskActionList]} {
			#something went wrong so delete everything
			print "ProductionManager ERROR: problem solved with deleting pickup tasks"
			set pickupGnomeStates [list]
			set pickupTaskGnomesList [list]
			set pickupTaskActionList [list]
		}
		
		set removeIndex -1
		
		for {set idx 0} {[llength $pickupTaskGnomesList] > $idx} {incr idx} {
			set state [lindex $pickupGnomeStates $idx]
			set gnomeID [lindex $pickupTaskGnomesList $idx]
			set materialIDs [lindex $pickupTaskActionList $idx]
			
			if {$gnomeID == 0 || ![obj_valid $gnomeID] || [get_objclass $gnomeID] != "Zwerg"} {
				# dwarf does not exist
				set removeIndex $idx
				continue
			}
			
			if {[llength $materialIDs] <= 0} {
				#task list is empty
				set removeIndex $idx
				continue
			}
			
			if {[inv_cnt $gnomeID] >= [inv_getsize $gnomeID]} {
				#inventory full
				set removeIndex $idx
				continue
			}
			
			set eventTime [lindex $state 1]
			set last_userevent_time [ref_get $gnomeID "last_userevent_time"]
			set last_eventtype [ref_get $gnomeID "last_eventtype"]
			if {[ref_checkvar $gnomeID "current_workitem"]} {
				set current_workitem [ref_get $gnomeID "current_workitem"]
			} else {
				set current_workitem -1
			}
			
			#check if a user event came in after state our event
			if {[expr {$last_userevent_time - $eventTime}] > 0} {
				# canceled by user
				set removeIndex $idx
				continue
			}
			
			set occupation [call_method $gnomeID get_current_occupation]
			
			if {[string first "muetze" $state 0] != -1 && $occupation == "work"} {
				set wantedHat [call_method $gnomeID get_nameofmuetze "transport"]
				set gnomeHat [get_objname [call_method $gnomeID get_muetze_ref]]
				
				if {![string equal -length 25 $wantedHat $gnomeHat] && [expr {[gettime] - $eventTime}]< 5 } {
					#wait one more cycle until the dwarf has changed his hat
					lrep pickupGnomeStates $idx $state
					continue
				} else {
					set state "transport $eventTime"
				}
			} else {
				set state "transport $eventTime"
			}
			
			
			set pickupDwarfPoint $gnomeID
			
			set itemTransported 0
			set mid [lindex $materialIDs 0]
			lrem materialIDs 0
			
			set materialIDs [lsort -command compare_by_distance_from_point $materialIDs]
			set materialIDs "$mid $materialIDs"
			
			if {[inv_find_obj $gnomeID $mid] != -1} {
				#item picked up
				#print "ProductionManager [get_objname $mid] picked up. Remaining: $materialIDs"
				set itemTransported 1
			}
			
			#FIXED: checking class type to remove invalid items in the pick up list, e.g. misc_lights
			set classType [get_class_type [get_objclass $mid]]
			
			if {$itemTransported || ([is_contained $mid] && ![get_instore $mid]) || [string equal $classType "dummy"] || [string equal $classType "info"]} {	
				lrem materialIDs 0
				
				if {[llength $materialIDs] <= 0} {
					set removeIndex $idx
					continue
				}
				
				#get next item
				set mid [lindex $materialIDs 0]
			}
			
			if {$mid != 0 && [obj_valid $mid]} {
				if  {$itemTransported || $current_workitem != $mid || [string equal $occupation "idle"]} {
					set_event $gnomeID evt_autoprod_pickup -target $gnomeID -subject1 $mid -text1 [get_objclass $mid]
					prod_gnome_state $gnomeID pickup $mid
					print "ProductionManager evt_autoprod_pickup -target $gnomeID -subject1 $mid -text1 [get_objclass $mid]"
				}
			}
			
			if {$mid == 0} {
				lrem materialIDs 0
			}
			
			lrep pickupGnomeStates $idx $state
			lrep pickupTaskActionList $idx $materialIDs 
		}
		
		if {$removeIndex != -1} {
			lrem pickupGnomeStates $removeIndex
			lrem pickupTaskGnomesList $removeIndex
			lrem pickupTaskActionList $removeIndex
		}
	}
}
}
