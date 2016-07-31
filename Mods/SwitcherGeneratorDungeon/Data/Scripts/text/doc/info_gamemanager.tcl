#cave generation on/off
if {! [info exists infowin_generatestate]} {
	set infowin_generatestate 0
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


#Urwald cave generation on/off
if {! [info exists infowin_generateUrwald]} {
	set infowin_generateUrwald 0
}

proc get_generate_Urwald {storymgr} {
	call_method $storymgr get_generate_Urwald_enable
}

proc set_generate_Urwald {storymgr state} {
	call_method $storymgr set_generate_Urwald_enable $state
}

proc generateUrwald_change {storymgr} {
	global infowin_generateUrwald
	set state $infowin_generateUrwald
	print "Urwald change"
	if {$state} {
		set_generate_Urwald $storymgr 0
	} else {
		set_generate_Urwald $storymgr 1
	}	
	layout reload
}

proc generateUrwald_switchstate {storymgr} {
	global infowin_generateUrwald
	set infowin_generateUrwald [get_generate_Urwald $storymgr]
	return [layout autoxlink "generateUrwald_change $storymgr" [lmsg Enable_$infowin_generateUrwald]]
}


#Metall cave generation on/off
if {! [info exists infowin_generateMetall]} {
	set infowin_generateMetall 0
}

proc get_generate_Metall {storymgr} {
	call_method $storymgr get_generate_Metall_enable
}

proc set_generate_Metall {storymgr state} {
	call_method $storymgr set_generate_Metall_enable $state
}

proc generateMetall_change {storymgr} {
	global infowin_generateMetall
	set state $infowin_generateMetall
	print "Metall change"
	if {$state} {
		set_generate_Metall $storymgr 0
	} else {
		set_generate_Metall $storymgr 1
	}	
	layout reload
}

proc generateMetall_switchstate {storymgr} {
	global infowin_generateMetall
	set infowin_generateMetall [get_generate_Metall $storymgr]
	return [layout autoxlink "generateMetall_change $storymgr" [lmsg Enable_$infowin_generateMetall]]
}


#Kristall cave generation on/off
if {! [info exists infowin_generateKristall]} {
	set infowin_generateKristall 0
}

proc get_generate_Kristall {storymgr} {
	call_method $storymgr get_generate_Kristall_enable
}

proc set_generate_Kristall {storymgr state} {
	call_method $storymgr set_generate_Kristall_enable $state
}

proc generateKristall_change {storymgr} {
	global infowin_generateKristall
	set state $infowin_generateKristall
	print "Kristall change"
	if {$state} {
		set_generate_Kristall $storymgr 0
	} else {
		set_generate_Kristall $storymgr 1
	}	
	layout reload
}

proc generateKristall_switchstate {storymgr} {
	global infowin_generateKristall
	set infowin_generateKristall [get_generate_Kristall $storymgr]
	return [layout autoxlink "generateKristall_change $storymgr" [lmsg Enable_$infowin_generateKristall]]
}


#Lava cave generation on/off
if {! [info exists infowin_generateLava]} {
	set infowin_generateLava 0
}

proc get_generate_Lava {storymgr} {
	call_method $storymgr get_generate_Lava_enable
}

proc set_generate_Lava {storymgr state} {
	call_method $storymgr set_generate_Lava_enable $state
}

proc generateLava_change {storymgr} {
	global infowin_generateLava
	set state $infowin_generateLava
	print "Lava change"
	if {$state} {
		set_generate_Lava $storymgr 0
	} else {
		set_generate_Lava $storymgr 1
	}	
	layout reload
}

proc generateLava_switchstate {storymgr} {
	global infowin_generateLava
	set infowin_generateLava [get_generate_Lava $storymgr]
	return [layout autoxlink "generateLava_change $storymgr" [lmsg Enable_$infowin_generateLava]]
}


proc infowin_generatemanagershow {storymgr} {
	layout print "/p/(fn3)"
	layout print "/(ac)[lmsg Game_Manager]"
	layout print "/p/p"
	layout print "/(fn1,ls2,ml5,mr5,al)"
	set x 35
	layout print "/(ta$x)[lmsg Generation_Temp]"
	layout print "/(tx )[generateinfo_switchstate $storymgr]"
	layout print "/p"
	#Urwald switch
	layout print "/(ta$x)[lmsg Generation_Urwald]"
	layout print "/(tx )[generateUrwald_switchstate $storymgr]"
	layout print "/p"
	#Metall switch
	layout print "/(ta$x)[lmsg Generation_Metall]"
	layout print "/(tx )[generateMetall_switchstate $storymgr]"
	layout print "/p"
	#Kristall switch
	layout print "/(ta$x)[lmsg Generation_Kristall]"
	layout print "/(tx )[generateKristall_switchstate $storymgr]"
	layout print "/p"
	#Lava switch
	layout print "/(ta$x)[lmsg Generation_Lava]"
	layout print "/(tx )[generateLava_switchstate $storymgr]"
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

