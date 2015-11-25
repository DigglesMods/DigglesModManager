# find a dwarf which is not working
# returns true if dwarf is free
proc check_free_gnome {gid} {
	if {[get_remaining_sparetime $gid] <= 0 && [state_get $gid] != "work_dispatch"} {
		return 1
	}
	return 0
}

proc get_inv_class_list {gid} {
	set result [list]
	foreach item [inv_list $gid] {
		lappend result [get_objclass $item]		
	}
	return $result
}

proc handle_pickup_tasks {} {
	global pickupGnomeStates pickupTaskGnomesList pickupTaskActionList
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
			
			set itemTransported 0
			set mid [lindex $materialIDs 0]
			
			if {[inv_find_obj $gnomeID $mid] != -1} {
				#item picked up
				#print "ProductionManager [get_objname $mid] picked up. Remaining: $materialIDs"
				set itemTransported 1
			}
			
			if {$itemTransported || ([is_contained $mid] && ![get_instore $mid])} {	
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

proc handle_experience_tasks {} {

		global experienceGainGnomeList experienceGainAttrib experienceProdEnabled

		set lengthList [llength $experienceGainGnomeList]
		set prodEnabled [list]
		
		if {$lengthList > 0} {
			#create list of best fitting production sites
			set prodList [obj_query 0 -type {production energy} -owner [get_local_player]]
			
			foreach prodID $prodList {
				if {![get_boxed $prodID] } {
					set slist [get_prod_slot_list $prodID]
					
					foreach class $slist {
						if { [get_prod_slot_invented $prodID $class] && [get_prod_slot_cnt $prodID $class] > 0} {
							#slot found which is invented and has to be produced
							set exp_incrs [call_method $prodID prod_item_exp_incr $class]
							lappend prodEnabled "$prodID $class $exp_incrs"
						}
					}
					set experienceProdEnabled $prodEnabled
				}
			}
			
			for {set i 0} { $i < $lengthList} {incr i} {
				set gnomeID [lindex $experienceGainGnomeList $i]
				set bestWorkplace 0
				set bestExp 0
				set bestClass ""
				set expGain [lindex $experienceGainAttrib $i]
				
				foreach prodSite $prodEnabled {
					set exp_incrs [lindex $prodSite 2]
					set result [prod_exp $exp_incrs $gnomeID $expGain]
					
					if {$result > $bestExp} {
						set bestExp $result
						set bestWorkplace [lindex $prodSite 0]
						set bestClass [lindex $prodSite 1]
					}
				}
				
				if {$bestWorkplace != 0 && [prod_gnome_get_preferred_workplace $gnomeID] != $bestWorkplace} {
					set_event $gnomeID evt_task_workprod_prefer -target $gnomeID -subject1 $bestWorkplace
				}
				#print "[get_objname $gnomeID]  [get_objname $bestWorkplace] $bestExp $bestClass"
			}	
		}
}

proc handle_equipment_tasks {} {
	global mode
	
	if {$mode == "dev"} {
		set proc_name "handle_equipment_tasks"
		callnc scripts/classes/work/prodman_proc_generic.tcl
		return
	}
	
	global equipmentTaskList
		
		set busyGnomes [list]
		set gnomeHasATask [list]
		set equipmentSecondTasks [list]
		
		for {set i 0} {$i <[llength $equipmentTaskList]} {incr i 1} {
			set task [lindex $equipmentTaskList $i]
			
			if {$task == 0} {
				lrem equipmentTaskList $i
				incr i -1
				continue
			}
			
			set type [lindex $task 0]
			
			if {$type == "autoprod"} {
				set gnome [lindex $task 1]
				if {($gnome == 0 || ![obj_valid $gnome]) || [get_objclass $gnome] != "Zwerg"} {
						# dest is not a valid gnome
						lrep equipmentTaskList $i 0
						continue
				}
				
				if {![get_prodautoschedule $gnome]} {
					lrep equipmentTaskList $i 0
					continue
				}
				
				lappend equipmentSecondTasks $task
			}
			
			if {$type == "transfer"} {
				set from [lindex $task 1]
				set item [lindex $task 2]
				set via [lindex $task 3]
				set dest [lindex $task 4]
				set timing [lindex $task 5]
				
				if {$from == $dest} {
					# source and dest are equal => remove
					lrem equipmentTaskList $i
					incr i -1
					continue
				}
				
				#indicate if the gnome has a task to do
				lappend gnomeHasATask $from
				lappend gnomeHasATask $dest
				
				if { [expr {[gettime] - [lindex $timing 1]}] > 20} {
						set timing "start [gettime] step"
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						continue
				}
				
				if {$item == 0 || ![obj_valid $item]} {
					# item not valid
					lrem equipmentTaskList $i
					incr i -1
					continue
				}
				
				if {($dest != 0 && ![obj_valid $dest]) || ([get_objclass $dest] != "Zwerg" && $via == 0)} {
						# dest is not a valid gnome if no via point specified
						lrem equipmentTaskList $i
						incr i -1
						continue
				}
				
				#handle put down task
				if {$from != 0} {
					if {[lsearch $busyGnomes $from] != -1} {
						continue
					}
					
					if {![obj_valid $from] || [get_objclass $from] != "Zwerg"} {
						# source is not a valid adult gnome
						lrem equipmentTaskList $i
						incr i -1
						continue
					}
					
					if {(![check_free_gnome $from] || ![get_prodautoschedule $from]) 
							&& [string first [lindex $timing 0] "instantprogress"] == -1} {
						# gnome not out of work
						# gnome disabled autoschedule
						continue
					}
					
					if {![obj_valid $via] ||  $via == 0} {
						# no valid handover location
						lrem equipmentTaskList $i
						incr i -1
						continue
					}
					
					if {[inv_find_obj $from $item] == -1} {
						#item layed down
						#convert task to pick up task
						lrep equipmentTaskList $i "transfer 0 $item 0 $dest \{forcestart [gettime] step\}"
						continue
					}
					
					if {[get_remaining_sparetime $from] < 5 && [get_remaining_sparetime $from] > 0 && [lindex $timing 0] != "instant"} {
						# force start of the task after sparetime finished
						set timing "forcestart [gettime] step"
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						continue
					}
					
					set eventTime [lindex $timing 1]
					set last_userevent_time [ref_get $from "last_userevent_time"]
					set occupation [call_method $from get_current_occupation]
					
					if {[lindex $timing 0] == "progress"} {
						lappend busyGnomes $from
						
						#calculate progress
						set destPos [get_pos $via]
						set startPos [get_pos $from]
						
						if {[lindex $timing 2] == "step"} {
							lrep timing 2 0
							lappend timing [vector_dist $destPos $startPos]
						} else {
							set distance [lindex $timing 3]
							set lastProgress [lindex $timing 2]
							set curProgress [expr {int(100 - [vector_dist $destPos $startPos] / $distance * 100)}]
							
							if {$curProgress > $lastProgress} {
								lrep timing 2 $curProgress
							}
						}
						
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						
						#check if a user event came in after start of our event
						if {($last_userevent_time - $eventTime) > 0.1} {
							set timing "start [gettime] step"
							lrep task 5 $timing
							lrep equipmentTaskList $i $task
							continue
						}
						
						#check gnome's tasklist if put down event is still where
						set gnomesTasklist [tasklist_list $from]
						if {[string first "beam_from_inv_to_pos \{$item" $gnomesTasklist] == -1} {
							#oh my goodness, someone deleted our task
							
							set timing "start [gettime] step"
							lrep task 5 $timing
							lrep equipmentTaskList $i $task
							continue
						}
					}
					
					if {(![check_free_gnome $from] || ![get_prodautoschedule $from]) && [lindex $timing 0] != "instant"} {
						# gnome not out of work
						# gnome disabled autoschedule
						continue
					}
					
					if {([lindex $timing 0] == "start" && [string equal $occupation "idle"]) || [lindex $timing 0] == "forcestart" || [lindex $timing 0] == "instant"} {
						#set itempos [get_place -center [get_pos $via] -rect -10 [hmin 1 [expr {11-[get_posz $via]}]] 10 [expr {13-[get_posz $via]}] -mindist 1.5 -random 2 -nearpos [get_pos $via] -materials false]
						set itempos [get_place -center [get_pos $via] -circle 10 -random 2 -materials false]
						if {[lindex $itempos 0]<0} {
							continue
						}
						
						set_event $from evt_task_putdown -target $from -pos1 $itempos -subject1 $item
						
						print "ProductionManager [get_objname $from] evt_task_putdown -target $from -pos1 $itempos -subject1 $item"
						set timing "progress [gettime] step"
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						lappend busyGnomes $from
					}
					######################################################
				} else {
					
					if {[lsearch $busyGnomes $dest] != -1 && [lindex $timing 0] != "instant"} {
						continue
					}
					
					
					
					if {[is_contained $item] && ![get_instore $item]} {
						lrem equipmentTaskList $i
						incr i -1
						continue
					}
					
					if {[inv_find_obj $dest $item] != -1} {
						#item picked up
						lrem equipmentTaskList $i
						incr i -1
						continue
					}
					
					
					if {[get_remaining_sparetime $dest] < 5 && [get_remaining_sparetime $dest] > 0 && [lindex $timing 0] != "instant"} {
						# force start of the task after sparetime finished
						set timing "forcestart [gettime] step"
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						continue
					}
					
					set eventTime [lindex $timing 1]
					set last_userevent_time [ref_get $dest "last_userevent_time"]
					set occupation [call_method $dest get_current_occupation]
					
					if {[lindex $timing 0] == "progress"} {
						lappend busyGnomes $dest
						
						#calculate progress
						set destPos [get_pos $item]
						set startPos [get_pos $dest]
						
						if {[lindex $timing 2] == "step"} {
							lrep timing 2 0
							lappend timing [vector_dist $destPos $startPos]
						} else {
							set distance [lindex $timing 3]
							set lastProgress [lindex $timing 2]
							set curProgress [expr {int(100 - [vector_dist $destPos $startPos] / $distance * 100)}]
							
							if {$curProgress > $lastProgress} {
								lrep timing 2 $curProgress
							}
						}
						
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						
						#check if a user event came in after state our event
						if {($last_userevent_time - $eventTime) > 0} {
							set timing "start [gettime] step"
							lrep task 5 $timing
							lrep equipmentTaskList $i $task
							continue
						}
					}
					
					if {(![check_free_gnome $dest] || ![get_prodautoschedule $dest]) && [lindex $timing 0] != "instant"} {
						# gnome not out of work
						# gnome disabled autoschedule
						continue
					}
					
					print "lbadfh" [lindex $timing 0]
					
					if {([lindex $timing 0] == "start" && [string equal $occupation "idle"]) || [lindex $timing 0] == "forcestart"|| [lindex $timing 0] == "instant"} {
						set_event $dest evt_autoprod_pickup -target $dest -subject1 $item -text1 [get_objclass $item]
						#prod_gnome_state $gnomeID pickup $mid
						print "ProductionManager evt_autoprod_pickup -target $dest -subject1 $item -text1 [get_objclass $item]"
					
						set timing "progress [gettime] step"
						lrep task 5 $timing
						lrep equipmentTaskList $i $task
						lappend busyGnomes $dest
					}
				}
			}
		}
		#print $equipmentTaskList
		
		foreach task $equipmentSecondTasks {
			set type [lindex $task 0]
			
			if {$type == "autoprod"} {
				set gnome [lindex $task 1]
				set goal [lindex $task 2]
				
				if {[lsearch $gnomeHasATask $gnome] != -1} {
					continue
				}
				
				set_prodautoschedule $gnome 0
			}
		}
}

#returns the value for the experience gain for expGain if it is in the list exp_incr
proc prod_exp {exp_incrs gnomeID expGain} {
	global mode
	set result ""
	
		#parameter: exp_incrs gnomeID expGain
		
		set result 0
		if {[obj_valid $gnomeID] && [get_objclass $gnomeID] == "Zwerg"} {
			foreach exp_incr $exp_incrs {
				set genre [lindex $exp_incr 0]
				
				if {$genre != $expGain} {
					continue
				}
				set factor [call_method $gnomeID get_clan_exp_factor $genre]
				set increase [expr { [lindex $exp_incr 1] * $factor}]
				
				if {[get_attrib $gnomeID $genre]<[expr $increase*100]} {
					set result $increase
				}
			}
		}
	
	return $result
}

