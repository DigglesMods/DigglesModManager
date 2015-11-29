$start
$before

	method init {} {
$put
method getbirthtime {} {
	global birthtime
	return $birthtime
}


$end

$start
$replace
		set_attrib this GnomeAge [gettime]
$with
		set birthtime [gettime]
		set_attrib this GnomeAge $birthtime
$end


$start
$after
proc grow_up {} {
			global 
$put
birthtime 
$end

$start
$replace
[gettime]-[get_attrib this GnomeAge]
$with
[gettime]-$birthtime
$end