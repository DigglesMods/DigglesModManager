#this file contains generic definitions of the method of class ProductionManager
#this file is supposed to be used for development

if {[info exists method_name]} {
switch $method_name {
	"evt_timer" {
		set ticks [expr {$ticks + 1}]
		
		handle_pickup_tasks
		
		if {[expr {$ticks % 5}] == 0} {
			handle_experience_tasks
		}
		
		handle_equipment_tasks
	}
	
	"add_equipment_task" {
		#parameter: task
		global equipmentTaskList
		
		set timing "start [gettime] step"
		lrep task 5 $timing
		lappend equipmentTaskList $task
	}
	
	"remove_equipment_task" {
		#parameter: task
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
	
	 "force_equipment_task" {
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
	
	"get_all_equipment_tasks" {
		global equipmentTaskList
		set result $equipmentTaskList
	}
}
}
