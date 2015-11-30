$start
$replace
if {$material == "Pilzstamm"  ||  $material == "Kohle"} {
$with
if {$item != "Kohle" && ($material == "Pilzstamm"  ||  $material == "Kohle")} {
$end



$start
$before

			if {$material != "Pilzstamm"  &&  $material != "Kohle"} {
$put

			if {$item == "Kohle"} {
				set material PilzstammHaemmern
			}

$end



$start
$before

// Eisenerz
$put

// Pilzstamm hämmern für Kohlegewinnung

	method prod_actions_PilzstammHaemmern {itemtype exp_infl} {
		set rlst [list]

		lappend rlst "prod_walk_and_hide_itemtype Pilzstamm"

		lappend rlst "prod_goworkdummy 2"
		lappend rlst "prod_turnback"
		lappend rlst "prod_anim benda"
		lappend rlst "prod_beam_itemtype_near_dummypos Pilzstamm 4 -1.0 0 0"
		lappend rlst "prod_anim bendb"
		lappend rlst "prod_waittime 1"
		lappend rlst "prod_anim wait"
		lappend rlst "prod_anim_loop_expinfl workfloormetall 1 5 $exp_infl"
		lappend rlst "prod_consume_from_workplace Pilzstamm"
		return $rlst
	}


$end