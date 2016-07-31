$start
$replace
sm_force_zone Kristall
$with
sm_force_zone Kristall

set storymgr [lindex [obj_query 0 -class StoryMgr -limit 1] 0]
if {$storymgr != 0} {
	call_method $storymgr set_generate_Urwald_enable 0
	call_method $storymgr set_generate_Metall_enable 0
	call_method $storymgr set_generate_Kristall_enable 1
	call_method $storymgr set_generate_Lava_enable 0
}
$end