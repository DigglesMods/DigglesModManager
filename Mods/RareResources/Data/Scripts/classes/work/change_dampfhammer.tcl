$start
$before

def_class Dampfhammer metal production 2 {} {
$put
def_class Eisenerz_ metal material 1 {} {}


$end


$start
$replace
		set rlst [list]

        if {[lsearch $BOXED_CLASSES [get_class_type $item]] != -1} {
$with
		set rlst [list]

		if {$item == "Eisenerz_"} {
			set item Eisenerz
		}
		
        if {[lsearch $BOXED_CLASSES [get_class_type $item]] != -1} {
$end


$start
$after
        foreach material $materiallist {

$put

			if {$item == "Kristallerz"} {
				set material KohleHaemmern
			}

$end


$start
$before

// Kohle

$put
// Kristallerzherstellung durch Kohle hämmern

	method prod_actions_KohleHaemmern {itemtype exp_infl} {
		set rlst [list]

		lappend rlst "prod_walk_and_hide_itemtype Kohle"           ;// Kohle holen

		lappend rlst "prod_goworkdummy 1"                        ;// plazieren
		lappend rlst "prod_turnback"
		lappend rlst "prod_beam_itemtype_to_dummypos Kohle 13"

		lappend rlst "prod_changetool Hammer"                         ;// rumhämmern
		lappend rlst "prod_anim hammerstart"
		lappend rlst "prod_anim_loop_expinfl hammerloopmetall 1 5 $exp_infl"
		lappend rlst "prod_anim hammerend"
		lappend rlst "prod_changetool 0"

		lappend rlst "prod_go_near_workdummy 1 1 0 0"
		lappend rlst "prod_turnback"
		lappend rlst "prod_anim kickmachine"                            ;// Dampfhammer an
		if {[random 1.0] > $exp_infl} {
			lappend rlst "prod_anim scratchhead"
			lappend rlst "prod_turnright"
			lappend rlst "prod_anim dontknow"
			lappend rlst "prod_turnback"
			lappend rlst "prod_anim kickmachine"
		}
		lappend rlst "prod_machineanim dampfhammer.ani start"
		lappend rlst "prod_call_method dust 1"

		lappend rlst "prod_goworkdummy 1"
		lappend rlst "prod_turnback"
		lappend rlst "prod_anim_loop_expinfl workmetall 1 5 $exp_infl"
		if {[random 1.0] > $exp_infl} {
			lappend rlst "prod_anim kontrol"
			lappend rlst "prod_anim scratchhead"
			lappend rlst "prod_changetool Hammer"                         ;// rumhämmern
			lappend rlst "prod_anim hammerstart"
			lappend rlst "prod_anim_loop_expinfl hammerloopmetall 1 5 $exp_infl"
			lappend rlst "prod_anim hammerend"
			lappend rlst "prod_changetool 0"
		}

		lappend rlst "prod_go_near_workdummy 1 1 0 0"
		lappend rlst "prod_turnback"
		lappend rlst "prod_anim kickmachine"                            ;// Dampfhammer aus
		lappend rlst "prod_call_method dust 0"
		lappend rlst "prod_machineanim dampfhammer.standard stop"

		lappend rlst "prod_goworkdummy 1"
		lappend rlst "prod_turnback"

		lappend rlst "prod_anim puta"                           ;// Werkstück abkühlen
		lappend rlst "prod_hide_itemtype Kohle"
		lappend rlst "prod_anim putb"
		lappend rlst "prod_consume_from_workplace Kohle"
		lappend rlst "prod_turnleft"

		return $rlst
	}



$end