proc set_gnome_experience_task {gnomeID expGain} {
	#referencing to global variable in order to save them
	global prodMan infowin_prodmode modEnabled
	print "set_gnome_experience_task $gnomeID $expGain"
	
	call_method $prodMan set_experience_gain $gnomeID $expGain
	
	layout reload
}

proc delete_gnome_prefered_workplace {gnomeID} {
	set curWorkplace [prod_gnome_get_preferred_workplace $gnomeID]
	set_event $gnomeID evt_task_workprod_prefer -target $gnomeID -subject1 $curWorkplace
	set_gnome_experience_task $gnomeID "None"
}

proc print_manager {prodMan} {
	set gnomelist [obj_query 0 -type {gnome} -owner [get_local_player]]
	set gnomelist [lsort -command compare_by_age $gnomelist]
	
	#layout print "$prodMan/p"
	layout print "/(fn0)/(ta10)[lmsg Name]/(ta80)[localize sammleErfahrungin]/(ta220)[lmsg "Assigned to"]/p"
	
	#two lists. First list: attribut variables, second list: displayed attribute names
	set expGainList {None exp_Nahrung exp_Holz exp_Stein exp_Metall exp_Transport exp_Energie exp_Service}
	set expGainNames {Nichts Nahrung Holz Steine Metalle Transp. Alchemie Service}
	
	foreach gnomeID $gnomelist {
		set x 10
		set xi 55
		layout print "/(fn1)/(ta$x)[gnomename $gnomeID]"; set x [expr $x + 90]
		layout print "/(fn0)"
		
		#layout print "/(ta$x)$gnomeID";set x [expr $x + 40]
		
		set attrib [call_method $prodMan get_experience_gain $gnomeID]
		set idx 0
		
		if {$attrib != 0 && $attrib != ""} {
			set idx [lsearch $expGainList $attrib]
		}
		
		if {$idx < 0} {
			set idx 0
		}
		
		set attribName  [lindex $expGainNames $idx]
		
		incr idx
		if {$idx >= [llength $expGainList]} {
			set idx 0
		}
		set nextAttrib [lindex $expGainList $idx]
		
		layout print "/(ta$x)/(ccAttribbar $gnomeID $attrib)"; set x [expr $x + 55]
		layout print "/(ta$x)[layout autoxlink "set_gnome_experience_task $gnomeID $nextAttrib" "[lmsg $attribName]"]"; set x [expr $x + 60]
		
		set prodID [prod_gnome_get_preferred_workplace $gnomeID]
		if {[obj_valid $prodID] && $prodID > 0} {
			layout print "/(ta$x)[prodname centerandselect $prodID]"
			layout print "/(tx   )"
			hyperlink "delete_gnome_prefered_workplace $gnomeID" [lmsg Undo]
		}
		
		layout print "/p"
	}
	
	layout print "/p"
	layout print [localize infoStringManager]
	layout print "/p"
	
	#some debug information
	#if {[ref_checkvar $prodMan experienceProdEnabled] != 0 } {
	#	set debug [ref_get $prodMan experienceProdEnabled]
	#	foreach s $debug {
	#		layout print "$s/p"
	#	}
	#}
}
