#this file contains generic definitions of the methods of class ProductionManager
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
}
}
