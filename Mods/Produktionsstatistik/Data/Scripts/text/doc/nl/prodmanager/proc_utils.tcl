# prints a link on the display
# param: - target callback handler
#				 - text visible text
proc hyperlink {target text} {
	layout print [layout autoxlink $target "$text"]
}

# print an icon as a link
# param: 
#				- taabsolut: absolut position from the left border
#				- target: code which will be executed on click
#				- id object ID. The name of this object is used to print a small number, e.g. "Holzkiepe_13" prints "13"
proc print_icon_link {taabsolut target id} {
	set class [get_objclass $id]
	set icon "data/gui/icons/$class.tga"
	set matchTuples [regexp -all -inline "\[0-9\]" [get_objname $id]]
					
	layout print "/(ta$taabsolut)"
	hyperlink "$target" "/(ii$icon)"
	layout print "/(ta$taabsolut)" $matchTuples
}

proc compare_by_age {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
	
	return [expr  [get_attrib $a GnomeAge] > [get_attrib $b GnomeAge]]
}

proc compare_by_name {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
    return [string compare [get_objname $a] [get_objname $b]]
}

proc compare_by_get_instore {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
	
	set view [get_view]
	set x [lindex $view 0]
	set y [lindex $view 1]
	
	#manhatten distance
	set distanceA [expr {abs([get_posx $a] - $x) + abs([get_posy $a] - $y)}]
	set distanceB [expr {abs([get_posx $b] - $x) + abs([get_posy $b] - $y)}]
	
	if {[get_instore $a] == [get_instore $b]} {
		return [expr {$distanceA > $distanceB}]
	} else {
		return [expr {[get_instore $a] > [get_instore $b]}]
	}
}

proc prodname {target pid} {
	if { [selection check $pid] || [is_contained $pid] } {
		return "[get_objname $pid]"
	} else {
		return "[layout autoxlink "$target $pid" "[get_objname $pid]"]"
	}
}

proc centerandselect {gid} {
	global infowin_prodmode modEnabled
	set view [get_view]
	set pos [get_pos $gid]
	set_view [vector_unpackx $pos] [vector_unpacky $pos] [vector_unpackz $view]
	selection clear
	selection include $gid
	layout reload
}

proc centerCamera {id} {
	global infowin_prodmode modEnabled
	set view [get_view]
	set pos [get_pos $id]
	set_view [vector_unpackx $pos] [vector_unpacky $pos] [vector_unpackz $view]
	layout reload
}

proc gnomename {gid} {
	if {$gid == -1} {return [lmsg {dead!}]}
	if { [selection check $gid] } {
		return "[get_objname $gid]"
	} else {
		return "[layout autoxlink "centerandselect $gid" "[get_objname $gid]"]"
	}
}
