$start
$replace
// item passt nicht in die Tasche --> fertig"
$with
// item passt nicht in die Tasche --> fertig
$end

$start
$replace
if {$richtung == "right"} {set negx 0; set posx 10} else {set negx -10; set posx 0"}
$with
if {$richtung == "right"} {set negx 0; set posx 10} else {set negx -10; set posx 0}
$end