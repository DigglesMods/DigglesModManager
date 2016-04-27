if {! [info exists infowin_generatestate]} {
	set generationstate 0
}

proc getgeneratestate {storymgr} {
	call_method $storymgr get_generate_enable
}

proc setgeneratestate {storymgr state} {
	call_method $storymgr set_generate_enable $state
}

proc generateinfo_change {storymgr} {
	global infowin_generatestate
	set state $infowin_generatestate
	print "change"
	if {$state} {
		setgeneratestate $storymgr 0
	} else {
		setgeneratestate $storymgr 1
	}	
	layout reload
	
}

proc generateinfo_switchstate {storymgr} {
	global infowin_generatestate
	set infowin_generatestate [getgeneratestate $storymgr]
	return [layout autoxlink "generateinfo_change $storymgr" [lmsg Enable_$infowin_generatestate]]
}

proc infowin_generatemanagershow {storymgr} {
	layout print "/p/(fn3)"
	layout print "/(ac)[lmsg Game_Manager]"
	layout print "/p/p"
	layout print "/(fn1,ls2,ml5,mr5,al)"
	set x 35
	layout print "/(ta$x)[lmsg {Generation_Temp}]"
	layout print "/(tx )[generateinfo_switchstate $storymgr]"
	layout print "/p"
}

proc get_storymgr {} {
	return [lindex [obj_query 0 -class StoryMgr -limit 1] 0]
}

proc info_gamemanager_start {} {
	if {[is_storymgr]} {
		set storymgr [get_storymgr]
		if {$storymgr != 0} {
			infowin_generatemanagershow $storymgr
		}
	}
}

