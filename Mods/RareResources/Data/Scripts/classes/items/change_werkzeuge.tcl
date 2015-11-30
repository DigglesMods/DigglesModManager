$start
$replace
		set look [string map {roh eisen half halber_metallbarren stab metallstab rad metallzahnrad blech metallblech gold gold golderz golderz} $look]
		if {$look == "golderz"} {
			set_anim this "golderz_0[expr [irandom 3] +1].standard" 0 $ANIM_STILL
		} else {
			set_anim this $look.standard 0 $ANIM_STILL
		}
$with
		set look [string map {roh eisen half halber_metallbarren stab metallstab rad metallzahnrad blech metallblech gold gold golderz golderz kohle kohle} $look]
		if {$look == "golderz"} {
			set_anim this "golderz_0[expr [irandom 3] +1].standard" 0 $ANIM_STILL
		} elseif {$look == "kohle"} {
			set_anim this "kohle_0[irandom 1 4].standard" 0 $ANIM_STILL
		} else {
			set_anim this $look.standard 0 $ANIM_STILL
		}
$end