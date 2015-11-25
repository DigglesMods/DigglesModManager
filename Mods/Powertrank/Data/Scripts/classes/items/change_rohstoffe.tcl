$start
$before
def_class Pilzschnaps 
$put
def_class Bier_ food material 1 {} {
	call scripts/misc/autodef.tcl
	call scripts/classes/items/calls/resources.tcl
	class_defaultanim bier.standard

	method set_animation {animname} {
		if {$animname == "standard"} {
			set_anim this bier.standard 0 $ANIM_STILL
		} elseif {$animname == "drink"} {
			set_anim this bier.krug 0 $ANIM_STILL
		} else {
			log "Bier_ : set_animation : illegal Animation"
		}
	}

	method use {user} {
		tasklist_add $user "drinkpotion [get_ref this]"
	}

	method reaction {user} {
		foreach entry $stt_Bier__reaction {
			eval "add_attrib $user $entry"
		}
	}

	obj_init {
		call scripts/misc/autodef.tcl
		call scripts/classes/items/calls/resources.tcl
		set_anim this bier.standard 0 $ANIM_STILL
		set sttsection_tocall "Bier_"
		call scripts/misc/sparetimetunes.tcl
	}
}


$end