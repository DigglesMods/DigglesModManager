$start
$after
def_class Eisen_ metal material 1 {} {}

$put

def_class Kohle_ stone material 1 {} {}
def_class Golderz_ metal material 1 {} {}

$end


$start
$after
		if {$item == "Eisen_"} {
			set item Eisen
		}

$put
		if {$item == "Kohle_"} {
			set item Kohle
		}
		if {$item == "Golderz_"} {
			set item Golderz
		}

$end


$start
$replace
		if {$item == "Eisen"} {
			lappend rlst "prod_itemtype_change_look Halbzeug_eisen roh"
		} elseif {$item == "Gold"} {
			lappend rlst "prod_itemtype_change_look Halbzeug_eisen gold"
		}
$with
		if {$item == "Eisen"} {
			lappend rlst "prod_itemtype_change_look Halbzeug_eisen roh"
		} elseif {$item == "Gold"} {
			lappend rlst "prod_itemtype_change_look Halbzeug_eisen gold"
		} elseif {$item == "Golderz"} {
			lappend rlst "prod_itemtype_change_look Halbzeug_eisen golderz"
		} elseif {$item == "Kohle"} {
			lappend rlst "prod_itemtype_change_look Halbzeug_eisen kohle"
		}
$end