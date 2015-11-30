$start
$replace
//	layout print "init to standard/p"
$with
}

if {! [info exists infowin_gnomeOrderMode] } {
	set infowin_gnomeOrderMode "name"
$end


$start
$before

proc gnomeinfo_switchto {mode} {
$put

proc gnomeinfo_order_by {mode} {
	global infowin_gnomeOrderMode
	set infowin_gnomeOrderMode $mode
	layout reload
}


$end


$start
$before

proc gnomeinfo_tablehead_switchhead {mode title} {
$put

proc gnomeinfo_switch_sorting {mode title} {
	return [layout autoxlink "gnomeinfo_order_by $mode" [lmsg $title]]
}


$end


$start
$replace
proc gnomeinfo_tablehead_standard {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	layout print "/(ta$x)[lmsg Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[lmsg Ges.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Ern.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Aufm.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Stimm.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Age]"; set x [expr $x + $xi]
	layout print "/p"
}
$with
proc gnomeinfo_tablehead_standard {} {
	layout print "/(fn0)"
	set x 10
	set xi 55
	layout print "/(ta$x)[gnomeinfo_switch_sorting name Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Hitpoints Ges.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Nutrition Ern.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Alertness Aufm.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Mood Stimm.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting GnomeAge Age]"; set x [expr $x + $xi]
	layout print "/p"
}
$end


$start
$replace
proc gnomeinfo_tablehead_production {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	layout print "/(ta$x)[lmsg Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[lmsg Summe]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Nahrung]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Holz]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Steine]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Metalle]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Transp.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Alchemie]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Service]"; set x [expr $x + $xi]
	layout print "/p"
}
$with
proc gnomeinfo_tablehead_production {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	layout print "/(ta$x)[gnomeinfo_switch_sorting name Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[lmsg Summe]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Nahrung  Nahrung]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Holz Holz]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Stein Steine]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Metall Metalle]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Transport Transp.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Energie Alchemie]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Service Service]"; set x [expr $x + $xi]
	layout print "/p"
}
$end


$start
$replace
proc gnomeinfo_tablehead_fight {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	layout print "/(ta$x)[lmsg Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[lmsg Kampf]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Schwertkampf]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Zweihand]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Verteid.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Ballist.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Kungfu]"; set x [expr $x + $xi]
	layout print "/p"
}
$with
proc gnomeinfo_tablehead_fight {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	layout print "/(ta$x)[gnomeinfo_switch_sorting name Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Kampf Kampf]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Sword Schwertkampf]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Twohanded Zweihand]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Defense Verteid.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Ballistic Ballist.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Kungfu Kungfu]"; set x [expr $x + $xi]
	layout print "/p"
}
$end


$start
$replace
proc gnomeinfo_standard {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 60
$with
proc gnomeinfo_standard {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 55
$end

$start
$replace
set x [expr $x + 30]
$with
set x [expr $x + 20]
$end


$start
$after
	set cclst [list]

$put
	set weaponlist [list]

$end


$start
$replace
		lappend cclst "$class $color"
	}
	set cclst [lsort $cclst]
	foreach item $cclst {
		lappend clst [lindex $item 0]
		lappend colorlst [lindex $item 1]
$with
		lappend weaponlist "$widtrue $widfalse $weaponID"
		lappend clst $class
		lappend colorlst $color
$end


$start
$replace
set gnomelist [lsort -integer $gnomelist]
$with
proc compare_by_name {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
    return [string compare [get_objname $a] [get_objname $b]]
}

proc compare_by_attribute {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
	
	global infowin_gnomeOrderMode
	return [expr  [get_attrib $a $infowin_gnomeOrderMode] < [get_attrib $b $infowin_gnomeOrderMode]]
}

 if {$infowin_gnomeOrderMode == "name" || $infowin_gnomeOrderMode == "" } {
	set gnomelist [lsort -command compare_by_name $gnomelist]
} else {
	set gnomelist [lsort -command compare_by_attribute $gnomelist]
}
$end






$if:!mod:BugFix

$start
$replace
proc gnomeinfo_inventory {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 60
	layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(fn0)"
}
$with
proc gnomeinfo_inventory2 {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 60
	layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(fn0)"
}
$end

$start
$replace
weapoiID
$with
weaponID
$end

$ifend






$if:mod:BugFix

$start
$before

proc compare_by_name {a b} {
$put
proc compare_by_age {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
	
	return [expr  [call_method $a getbirthtime] < [call_method $b getbirthtime]]
}


$end


$start
$replace
 if {$infowin_gnomeOrderMode == "name" || $infowin_gnomeOrderMode == "" } {
	set gnomelist [lsort -command compare_by_name $gnomelist]
} else {
	set gnomelist [lsort -command compare_by_attribute $gnomelist]
}
$with
 if {$infowin_gnomeOrderMode == "name" || $infowin_gnomeOrderMode == "" } {
	set gnomelist [lsort -command compare_by_name $gnomelist]
} elseif {$infowin_gnomeOrderMode == "age"} {
	set gnomelist [lsort -command compare_by_age $gnomelist]
} else {
	set gnomelist [lsort -command compare_by_attribute $gnomelist]
}
$end


$start
$replace
layout print "/(ta$x)[gnomeinfo_switch_sorting GnomeAge Age]"; set x [expr $x + $xi]
$with
layout print "/(ta$x)[gnomeinfo_switch_sorting age Age]"; set x [expr $x + $xi]
$end

$ifend
