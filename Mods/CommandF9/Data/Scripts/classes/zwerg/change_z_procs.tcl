$start
$replace
proc beamto_world_all {} {
	foreach item [inv_list this] {
		// zusätzliches inv_find_obj, weil beamto_world u.U. mehrere Items ablegt (bei Kiepen mit Inhalt z.B.)
		if {[inv_find_obj this $item] >= 0} {
			beamto_world $item [get_roty this]
		}
	}
}
$with
proc beamto_world_all {} {
	set myTrue 1
	set myFalse 0
	
	//weapons
	set ballistItem -1
	set shieldItem -1
	set onehandItem -1
	set twohandItem -1
	set weaponRankingBallist {Buechse PfeilUndBogen Steinschleuder}
	set weaponRankingShield {Trollschild_3 Drachenschuppe Schild_unq_2 Schild_unq_1 Trollschild_2 Kristallschild Schild_3 Metallschild Schild_2 Schild_1 Trollschild_1 Schild}
	set weaponRankingOneHand {Axt_unq_3 Schwert_4 Krumsaebel Axt_3 Streitkolben Schwert Dolch_1 Axt_unq_1 Schwert_1 Axt_1 Dolch_2 Keule}
	set weaponRankingTwoHand {Hellebarde Axt_unq_4 Zauberstab Axt_4 Lichtschwert Schwert_3 Axt_unq_2 Lanze_2 Axt_2 Schwert_2 Streitaxt Lanze_1}
	
	set searchlockclasses [list]
	//add tools
	if {$my$print:TOOLS} {
		lappend searchlockclasses Kettensaege Presslufthammer Kristallstrahl
	}
	//add movement items
	if {$my$print:MOVEMENT} {
		lappend searchlockclasses Reithamster Hoverboard
	}
	//add transport items
	if {$my$print:TRANSPORT} {
		lappend searchlockclasses Holzkiepe Grosse_Holzkiepe Schubkarren
	}
	//add weapons
	if {$my$print:WEAPONS} {
		foreach item $weaponRankingBallist {lappend searchlockclasses $item}
		foreach item $weaponRankingShield {lappend searchlockclasses $item}
		foreach item $weaponRankingOneHand {lappend searchlockclasses $item}
		foreach item $weaponRankingTwoHand {lappend searchlockclasses $item}
	}
	
	set lockclasses [list]
	set rememberItems [list]

	//search and remember items
	foreach item [inv_list this] {
		set objclass [get_objclass $item]
		
		if {[lsearch $searchlockclasses $objclass] > -1} {
			if {[lsearch $lockclasses $objclass] == -1} {
				
				set canAppend 1
				
				//find lower items
				if {$my$print:LOWER_ITEMS} {
					if {[lsearch {Reithamster} $objclass] > -1} {
						//if item is a Reithamster and in inventory is a Hoverboard
						if {[inv_find this Hoverboard]  > -1} {
							set canAppend 0
						}
					} elseif {[lsearch {Holzkiepe} $objclass] > -1} {
						//if item is a Holzkiepe and in inventory is a Grosse_Holzkiepe
						if {[inv_find this Grosse_Holzkiepe]  > -1} {
							set canAppend 0
						}
					} elseif {$my$print:WEAPONS} {
						if {[lsearch $weaponRankingBallist $objclass]  > -1} {
							set canAppend 0
							set ballistItem [get_better_weapon $weaponRankingBallist $ballistItem $item]
						} elseif {[lsearch $weaponRankingShield $objclass]  > -1} {
							set canAppend 0
							set shieldItem [get_better_weapon $weaponRankingShield $shieldItem $item]
						} elseif {[lsearch $weaponRankingOneHand $objclass]  > -1} {
							set canAppend 0
							set onehandItem [get_better_weapon $weaponRankingOneHand $onehandItem $item]
						} elseif {[lsearch $weaponRankingTwoHand $objclass]  > -1} {
							set canAppend 0
							set twohandItem [get_better_weapon $weaponRankingTwoHand $twohandItem $item]
						}
					}
				}
				
				if {$canAppend} {
					//remember item
					if {$my$print:MULTIPLE_ITEMS} {
						lappend lockclasses $objclass
					}
					lappend rememberItems $item
				}
			}
		}
	}
	
	//remember weapons
	if {$ballistItem > -1} {lappend rememberItems $ballistItem}
	if {$shieldItem > -1} {lappend rememberItems $shieldItem}
	if {$onehandItem > -1} {lappend rememberItems $onehandItem}
	if {$twohandItem > -1} {lappend rememberItems $twohandItem}
	
	//drop all items
	foreach item [inv_list this] {
		if {[inv_find_obj this $item] >= 0} {
			beamto_world $item [get_roty this]
		}
	}
	
	//get all remembered items
	foreach item $rememberItems {
		take_item $item
	}
	
}

proc is_weapon_usable {weapon} {
        set widtrue [get_weapon_id $weapon true]
		set widfalse [get_weapon_id $weapon false]
		set weaponID 0
		if {$widtrue > $widfalse} {set weaponID $widtrue} else {set weaponID $widfalse}
		return [check_weapon_exp this $weaponID]
}

proc get_better_weapon {weaponRanking weap1 weap2} {
	if {$weap1 == -1} {
		set weap1 $weap2
	} elseif {$weap2 == -1} {
		set weap2 $weap1
	}
	
	//check dwarf experience
	set weap1_exp [is_weapon_usable $weap1]
	set weap2_exp [is_weapon_usable $weap2]
	
	if {$weap1_exp && $weap2_exp} {
		//both weapons can be used
		//check witch one is better
		set objclass1 [get_objclass $weap1]
		set objclass2 [get_objclass $weap2]
		
		foreach objclass $weaponRanking {
			if {$objclass == $objclass1} {
				return $weap1
			}
			if {$objclass == $objclass2} {
				return $weap2
			}
		}
	} else {
		//only one can be used
		if {$weap1_exp} {
			return $weap1
		}
		if {$weap2_exp} {
			return $weap2
		}
	}
	// no one can be used
	return -1
}
$end






