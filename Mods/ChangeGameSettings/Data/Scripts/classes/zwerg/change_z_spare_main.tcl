$start
$replace
return [hmin [expr {10.0+$civ*20.0}] 22.0]
$with
return [hmin [expr {(54.0 - 2.0*$print:POP_SOLL.0)+$civ*(5*$print:POP_SOLL.0 - 90)}] $print:POP_SOLL.0]
$end