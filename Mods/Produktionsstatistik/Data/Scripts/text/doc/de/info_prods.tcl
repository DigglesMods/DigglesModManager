layout clear

layout print "/(fn2,ac)"
layout print [lmsg Produktion]
layout print "/p"

#default layout
layout print "/(fn1,ls0,ml5,mr5,al)"

#always set to zero. If we find a ProductionManager then we set it to 1
set modEnabled 0

#find the Production Manager. It should be a singleton class.
#if we do not find it, create a new one
set prodMan [obj_query 0 -class ProductionManager]

if {[llength $prodMan] > 0} {
	set prodMan [lindex $prodMan 0]
} 

if {$prodMan != 0 && [obj_valid $prodMan]} {
	// layout print "ProductionManager found: $prodMan"
	set modEnabled 1
}

if {! [info exists infowin_prodmode] } {
	set infowin_prodmode "standard"
} elseif {$modEnabled == 0} {
	if {[string first $infowin_prodmode "resourcemanagerequipment"] != -1} {
		#set to standard because a game load does not reset this variable
		set infowin_prodmode "standard"
	}
}

set infowin_prodResourceTask {0 0}
if {$modEnabled} {
	set infowin_prodResourceTask [call_method $prodMan load_pick_up_task]
	print "loaded infowin_prodResourceTask  $infowin_prodResourceTask"
}

# load localisation scripts
call "scripts/text/doc/[locale]/prodmanager/proc_localisation.tcl"

# load some misc procedures
call "scripts/text/doc/[locale]/prodmanager/proc_utils.tcl"

# load procedures for the submenu "resources"
call "scripts/text/doc/[locale]/prodmanager/proc_resources.tcl"

# load procedures for the submenu "standard"
call "scripts/text/doc/[locale]/prodmanager/proc_standard.tcl"
	
if {$modEnabled} {
	# load procedures for the submenu "manager"
	call "scripts/text/doc/[locale]/prodmanager/proc_manager.tcl"
	
	# load procedures for the submenu "equipment"
	call "scripts/text/doc/[locale]/prodmanager/proc_equipment.tcl"
}

# #####################
proc prodinfo_switchto {mode} {
	global infowin_prodmode modEnabled
	
	set infowin_prodmode $mode
	layout reload
}

proc prodinfo_enableMod {} {
	global infowin_prodmode modEnabled prodMan
	set infowin_prodmode "settings"
	
	set prodMan [new ProductionManager]
	print "Created new ProductionManager: $prodMan"
	#generate a timer event, repeat it forever in an interval of one second
	timer_event $prodMan evt_timer -repeat -1 -interval 1
	
	set modEnabled 1
	layout reload
}

proc prodinfo_disableMod {} {
	global infowin_prodmode modEnabled prodMan
	set infowin_prodmode "settings"
	
	foreach obj [obj_query 0 -class ProductionManager] {
		if {$obj > 0} {
			del $obj
		}
	}
	
	set prodMan 0
	set modEnabled 0
	layout reload
}

# switchhead
layout print "/(fn1)"

layout print "/(tx   )[layout autoxlink "prodinfo_switchto standard" [lmsg Standard]]"
if {$modEnabled} {
	layout print "/(tx   )[layout autoxlink "prodinfo_switchto resource" [localize Rohstoffe]]"
	layout print "/(tx   )[layout autoxlink "prodinfo_switchto manager" [localize Manager]]"
	layout print "/(tx   )[layout autoxlink "prodinfo_switchto equipment" [localize Equipment]]"
}
layout print "/(tx   )[layout autoxlink "prodinfo_switchto settings" [localize settings]]"
layout print "/p"
layout print "/p"

if {$infowin_prodmode == "resource"} {
	#Ressource finder
	layout print "/(ls-1)"
	
	#find free gnomes
	set freeGnomes [call_method $prodMan get_free_gnomes [get_local_player]]
	
	#print dwarf list and an pick up execution link
	if {[llength $freeGnomes] > 0} {

		layout print "/(tx   ) [lmsg BitteeinenZwerganwaehlen]:"
		
		#count elements for a beautiful user interface
		set elementCounter 0 
		foreach gid $freeGnomes {
			hyperlink "addPickupTask_Gnome $gid" "[get_objname $gid]"
			layout print "/(tx )"
			
			incr elementCounter 1
			if {$elementCounter == 4 || $elementCounter == 11} {
				layout print "/p"
			}
		}
		
		if {$elementCounter < 4} {
			layout print "/p"
		}
		if {$elementCounter < 11} {
			layout print "/p"
		}
		
		layout print "/p/(tx   )"
		set gnomeID [lindex $infowin_prodResourceTask 0]
		set materialIDs [lrange $infowin_prodResourceTask 1 end]
		
		if {[obj_valid $gnomeID] == 0 || [get_objclass $gnomeID] != "Zwerg"} {
			set infowin_prodResourceTask {0 0}
			set gnomeID 0
		}
		
		set ret 0
		if {$gnomeID != 0} {
			hyperlink "removePickupTask_Gnome $gnomeID" [get_objname $gnomeID]
			layout print "[lmsg pickup]"
			
			set ret [gui_printPickupString $infowin_prodResourceTask]
			#layout print $ret
			if { $ret > 0} {
				hyperlink "executePickupTask" "[lmsg ausfuehren]"
			}
		} else {
			layout print "[lmsg Zwerg] [lmsg pickup]"
			set ret [gui_printPickupString $infowin_prodResourceTask]
		}
		
		if {$ret < 3} {
			layout print "/p"
		}
		if {$ret < 7} {
			layout print "/p"
		}
		if {$ret < 11} {
			layout print "/p"
		}
		layout print "/p"
	}
	
			
	layout print "/(fn0)"
	
	# print a resource list
	set allResources {Pilzstamm Pilzhut Stein Kohle Eisenerz Eisen Kristallerz Kristall Golderz Gold Grillhamster Grillpilz Pilzbrot Raupensuppe Raupenschleimkuchen Hamstershake}
	foreach resource $allResources {
		# boxed, visible, locked, storable, male, female, contained, hoverable, selectable, build, instore
		set resList [obj_query 0 -class $resource -flagneg {contained locked} -visibility playervisible  -alloc -1]
		set resList [lor $resList [obj_query 0 -class $resource -flagpos {instore} -flagneg {locked} -visibility playervisible  -alloc -1]]
		
		if {[llength $resList] <= 0 || $resList == 0} {
			continue
		}
		
		foreach item $infowin_prodResourceTask {
			set idx [lsearch $resList $item]
			if {$idx != -1} {
				lrem resList $idx
			}
		}
		
		set resList [lsort -command compare_by_get_instore $resList]
		
		#set icon "data/gui/icons/$resource.tga"
		#layout print "/(ii$icon)"
		layout print "[lmsg $resource]"
		
		set xLength 40
		set xSize 470
		
		foreach item $resList {
			if {$item == 0} {
				continue;
			}
			
			if {$xLength >= $xSize} {
				layout print "/p"
				set xLength -2
				set xSize 480
			}
			
			set objName "[get_objname $item]"
			set matchTuples [regexp -all -inline "\[0-9\]" $objName]
			#set xLength [expr {$xLength + 2 + [string length $objName]}]
			set xLength [expr {$xLength + 42}]
			
			set icon "data/gui/icons/$resource.tga"
			layout print "/(ta$xLength)"
			hyperlink "addPickupTask_Material $item"  "/(ii$icon)" 
			layout print "/(ta$xLength)" $matchTuples
		}
		layout print "/p"
	}
	
	saveRes
}

if {$infowin_prodmode == "standard"} {
	set prodlist [obj_query 0 -type {production energy} -owner [get_local_player]]
	set prodlist [lsort -command compare_by_name $prodlist]
	set prodlist_2 ""
	
	for {set i 0} {$i < [llength $prodlist]} {incr i 1} {
		set prodID [lindex $prodlist $i]
		
		if {[get_prod_total_task_cnt $prodID] == 0} {
			lappend prodlist_2 $prodID
			lrem prodlist $i
			incr i -1
		}
	}
	
	layout print "/(fn0)"
	
	foreach prodID $prodlist {
		prodinfo_stats $prodID
	}
	
	foreach prodID $prodlist_2 {
		prodinfo_stats $prodID
	}
}

if {$infowin_prodmode == "manager"} {
	print_manager $prodMan
}

if {$infowin_prodmode == "equipment"} {
	print_equipment $prodMan
}

if {$infowin_prodmode == "settings"} {
	if {$modEnabled} {
		hyperlink prodinfo_disableMod [localize disable]
	} else {
		hyperlink prodinfo_enableMod [localize enable]
	}
	
	#
	layout print "$prodMan/p"
	#set game_expsum [gamestats attribsum 0 expsum]
	#set game_built [gamestats numbuiltprodclasses 0]
	#set civ_state [ expr { ( $game_expsum + $game_built ) * 0.01 } ]
	#
	#layout print "expsum $game_expsum;  Built: $game_built;  CivState: $civ_state /p"
	#
	#set schatzbucher [obj_query 0 -class Schatzbuch -owner [get_local_player]]
	#
	#foreach book $schatzbucher {
	#	hyperlink "centerandselect $book" [call_method $book get_erfahrungsbezeichnung ]
	#	layout print "/p"
	#}
}

