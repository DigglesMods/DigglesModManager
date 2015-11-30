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

$start
$replace
set age [expr  int(([gettime] - [get_attrib $gid GnomeAge]) / 1800.0) ]
$with
set age [expr  int(([gettime] - [call_method $gid getbirthtime]) / 1800.0) ]
$end
