#this file contains generic definitions of the procedures of class ProductionManager
#this file is supposed to be used for development

if {[info exists proc_name]} {
switch $proc_name {
	  "handle_experience_tasks" {
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
				
				#FIXED invalid gnomes. E.g. gnome died
				if {[obj_valid $gnomeID] == 0} {
					lrem experienceGainGnomeList $i
					lrem experienceGainAttrib $i
					set lengthList [llength $experienceGainGnomeList]
					incr i -1
					continue
				}
				
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
}
}
