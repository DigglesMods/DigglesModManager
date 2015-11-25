proc saveRes {} {
	global infowin_prodResourceTask prodMan
	call_method $prodMan save_pick_up_task $infowin_prodResourceTask
}

proc addPickupTask_Material {mid} {
	global infowin_prodResourceTask
	
	#search for the item if it is already in the list
	set idx [lsearch $infowin_prodResourceTask $mid]
	if {$idx < 0} {
		lappend infowin_prodResourceTask $mid
		print "adding [get_objname $mid]"
	}
	
	saveRes
	centerandselect $mid
}

proc removePickupTask_Material {mid} {
	global infowin_prodmode infowin_prodResourceTask modEnabled
	set idx [lsearch $infowin_prodResourceTask $mid]
	if {$idx == 1} {
		lrep infowin_prodResourceTask $idx 0
	} 
	if {$idx > 1} {
		lrem infowin_prodResourceTask $idx
	}
	
	saveRes
	layout reload
}

# print material hyperlinks, returns true if sth was printed
proc gui_printPickupString {} {
	global infowin_prodResourceTask
	
	set result 0
	
	for {set idx 1} {[llength $infowin_prodResourceTask] > $idx} {incr idx} {
		set mid [lindex $infowin_prodResourceTask $idx]
		
		if {$mid == 0 || ![obj_valid $mid] || ([is_contained $mid] && ![get_instore $mid])} {
			lrem infowin_prodResourceTask $idx
			incr idx -1
			continue
		} 
		
		if {$mid != 0 && [obj_valid $mid] && [string first [get_objtype $mid] "materialtooltransport"] != -1} {
			set result 1
			hyperlink "removePickupTask_Material $mid" [get_objname $mid]
		}
	}
	
	if {[llength $infowin_prodResourceTask] < 2} {
		lappend infowin_prodResourceTask 0
	}
	
	saveRes
	return $result
}

proc addPickupTask_Gnome {gid} {
	global infowin_prodResourceTask
	lrep infowin_prodResourceTask 0 $gid
	
	saveRes
	centerandselect $gid
}

proc removePickupTask_Gnome {gid} {
	global infowin_prodmode infowin_prodResourceTask modEnabled
	lrep infowin_prodResourceTask 0 0
	
	saveRes
	layout reload
}

proc executePickupTask {} {
	global infowin_prodmode infowin_prodResourceTask prodMan modEnabled
	
	set gnomeID [lindex $infowin_prodResourceTask 0]
	set materialIDs [lrange $infowin_prodResourceTask 1 end]
	
	call_method $prodMan add_pickup_task $gnomeID $materialIDs
	layout reload
}
