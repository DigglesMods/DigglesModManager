$start
$before

proc compare_by_name {a b} {
$put
proc compare_by_age {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
	
	return [expr  [call_method $a getbirthtime] > [call_method $b getbirthtime]]
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
} else {
	if {$infowin_gnomeOrderMode == "age"} {
		set gnomelist [lsort -command compare_by_age $gnomelist]
	} else {
		set gnomelist [lsort -command compare_by_attribute $gnomelist]
	}
}
$end


$start
$replace
set age [expr  int(([gettime] - [get_attrib $gid GnomeAge]) / 1800.0) ]
$with
set age [expr  int(([gettime] - [call_method $gid getbirthtime]) / 1800.0) ]
$end

$start
$replace
layout print "/(ta$x)[gnomeinfo_switch_sorting GnomeAge Age]"; set x [expr $x + $xi]
$with
layout print "/(ta$x)[gnomeinfo_switch_sorting age Age]"; set x [expr $x + $xi]
$end