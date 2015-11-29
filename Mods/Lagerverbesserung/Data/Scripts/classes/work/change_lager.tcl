$start
$replace 
set slot_size 3
$with
set slot_size $print:ITEMS_PER_SLOT
$end



$if:ADD_STORE_ROW

$start
$replace 
set max_slots 24
$with
set max_slots 30
$end

$start
$replace 
;// 4 Zeilen * 6 Spalten
$with
;// 5 Zeilen * 6 Spalten
$end

$start
$replace 
set sloty {-0.4 -1.15 -1.9 -2.6}
$with
set sloty {-0.4 -1.15 -1.9 -2.6 -3.3}
$end

$start
$replace 
set slotlist  [list 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0]
$with
set slotlist  [list 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0]
$end

$start
$replace 
set slottypes [list 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0]
$with
set slottypes [list 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0]
$end

$start
$after
			if {$i == 3} {return putjumphighest}

$put

			if {$i == 4} {return putjumphighest}

$end

$ifend



$start
$after
def_class _Mineralien_einlagern 					service material 1 {} {}

$put

def_class _Pilzhut_einlagern		service material 1 {} {}
def_class _Pilzstamm_einlagern		service material 1 {} {}
def_class _Stein_einlagern		service material 1 {} {}
def_class _Kohle_einlagern		service material 1 {} {}
def_class _Eisen_einlagern		service material 1 {} {}
def_class _Gold_einlagern		service material 1 {} {}
def_class _Kristall_einlagern		service material 1 {} {}
def_class _Waffen_einlagern	service material 1 {} {}
def_class _Werkzeuge_einlagern	service material 1 {} {}
def_class _Traenke_einlagern	service material 1 {} {}

$end

$start
$after
global store_food 
$put
store_mushroomcap store_mushroomtrunk store_stone store_coal store_iron store_gold store_crystal store_weapons store_tools store_potions 
$end

$start
$replace
		if {[get_prod_slot_cnt this _Pilze_einlagern] != 0} {
				set store_mushrooms 1
		}  else {
				set store_mushrooms 0
		}

		if {[get_prod_slot_cnt this _Rohmineralien_einlagern] != 0} {
				set store_rawminerals 1
		}  else {
				set store_rawminerals 0
		}

		if {[get_prod_slot_cnt this _Mineralien_einlagern] != 0} {
				set store_minerals 1
		}  else {
				set store_minerals 0
		}

$with
		set store_mushrooms 0
		set store_minerals 0
		
		if {[get_prod_slot_cnt this _Pilzhut_einlagern] != 0} {
				set store_mushroomcap 1
		}  else {
				set store_mushroomcap 0
		}
		if {[get_prod_slot_cnt this _Pilzstamm_einlagern] != 0} {
				set store_mushroomtrunk 1
		}  else {
				set store_mushroomtrunk 0
		}
		if {[get_prod_slot_cnt this _Rohmineralien_einlagern] != 0} {
				set store_rawminerals 1
		}  else {
				set store_rawminerals 0
		}
		if {[get_prod_slot_cnt this _Stein_einlagern] != 0} {
				set store_stone 1
		}  else {
				set store_stone 0
		}
		if {[get_prod_slot_cnt this _Kohle_einlagern] != 0} {
				set store_coal 1
		}  else {
				set store_coal 0
		}
		if {[get_prod_slot_cnt this _Eisen_einlagern] != 0} {
				set store_iron 1
		}  else {
				set store_iron 0
		}
		if {[get_prod_slot_cnt this _Gold_einlagern] != 0} {
				set store_gold 1
		}  else {
				set store_gold 0
		}
		if {[get_prod_slot_cnt this _Kristall_einlagern] != 0} {
				set store_crystal 1
		}  else {
				set store_crystal 0
		}
		if {[get_prod_slot_cnt this _Waffen_einlagern] != 0} {
				set store_weapons 1
		}  else {
				set store_weapons 0
		}
		if {[get_prod_slot_cnt this _Werkzeuge_einlagern] != 0} {
				set store_tools 1
		}  else {
				set store_tools 0
		}
		if {[get_prod_slot_cnt this _Traenke_einlagern] != 0} {
				set store_potions 1
		}  else {
				set store_potions 0
		}
$end

$start
$after
		set store_minerals     0	  ;// Stein Eisen Gold Kristall Kohle

$put
		set store_mushroomcap    0      ;// Pilzhut
		set store_mushroomtrunk  0      ;// Pilzstamm
		set store_stone		   0	  ;// Stein
		set store_coal    0      ;// Kohle
		set store_iron  0      ;// Eisen
		set store_gold     0	  ;// Gold
		set store_crystal		   0	  ;// Kristall
		set store_weapons		   0	  ;// Waffen
		set store_tools		   0	  ;// Werkzeuge
		set store_potions		   0	  ;// Traenke

$end

$start
$replace
		set_prod_slot_cnt this _Pilze_einlagern	 		0
		set_prod_slot_cnt this _Mineralien_einlagern 	0
		set_prod_slot_cnt this _Rohmineralien_einlagern 0
$with
		set_prod_slot_cnt this _Pilzhut_einlagern	 		0
		set_prod_slot_cnt this _Pilzstamm_einlagern 	0
		set_prod_slot_cnt this _Rohmineralien_einlagern 0
		set_prod_slot_cnt this _Stein_einlagern	 		0
		set_prod_slot_cnt this _Kohle_einlagern 	0
		set_prod_slot_cnt this _Eisen_einlagern 0
		set_prod_slot_cnt this _Gold_einlagern	 		0
		set_prod_slot_cnt this _Kristall_einlagern 	0
		set_prod_slot_cnt this _Waffen_einlagern 	0
		set_prod_slot_cnt this _Werkzeuge_einlagern 	0
		set_prod_slot_cnt this _Traenke_einlagern 	0
$end

$start
$after
			if {$store_minerals} {
				lappend classes Eisen Gold Kristall Stein Kohle
			}

$put
			if {$store_mushroomcap} {
				lappend classes Pilzhut
			}
			if {$store_mushroomtrunk} {
				lappend classes Pilzstamm
			}
			if {$store_rawminerals} {
				lappend classes Eisenerz Golderz Kristallerz
			}
			if {$store_stone} {
				lappend classes Stein
			}
			if {$store_coal} {
				lappend classes Kohle
			}
			if {$store_iron} {
				lappend classes Eisen
			}
			if {$store_gold} {
				lappend classes Gold
			}
			if {$store_crystal} {
				lappend classes Kristall
			}
			if {$store_weapons} {
				lappend classes Steinschleuder PfeilUndBogen Buechse Schild Schild_1 Schild_2 Metallschild Schild_3 Kristallschild Schild_unq_1 Schild_unq_2 Drachenschuppe Keule Dolch_2 Axt_1 Schwert_1 Axt_unq_1 Schwert Axt_3 Schwert_4  Axt_unq_3 Streitaxt Schwert_2 Axt_2 Axt_unq_2 Schwert_3 Lichtschwert Axt_4 Axt_unq_4 Amulett_1 Amulett_2 Amulett_3
			}
			if {$store_tools} {
				lappend classes Reithamster Kettensaege Presslufthammer Hoverboard Kristallstrahl Holzkiepe Grosse_Holzkiepe
			}
			if {$store_potions} {
				lappend classes Heiltrank Kleiner_Heiltrank Grosser_Heiltrank Pilzschnaps Liebestrank Unverwundbarkeitstrank Unsichtbarkeitstrank Jungbrunnentrank Fruchtbarkeitstrank Wiederbelebung
			}

$end



$start
$replace
set classes "Grillpilz Grillhamster Pilzbrot Raupensuppe Raupenschleimkuchen Gourmetsuppe Hamstershake Bier Pilzstamm Pilzhut Eisenerz Golderz Kristallerz Eisen Gold Kristall Stein Kohle"
$with
set classes [get_classes_to_store]
$end


$start
$after
    handle_event evt_timer_search {
		global items_list old_items_list storable_items_list store_range
//    	log "lager.tcl: Event evt_timer_search"

$put
 
		set classes [get_classes_to_store]
		if {[llength $classes] > 0} {
			set items [obj_query this "-flagneg {instore} -type {tool transport} -class \{$classes\} -owner \{[get_owner this] -1\} -alloc -1"]
			foreach item $items {
				if {[get_storable $item] == 0} {
					set_storable $item 1
				}
			}
		}

$end





$start
$replace
			inv_add this $item
$with
			if {[get_objtype $item] == "transport"} {
				//Container ausleeren, sonst gehen Items verloren
				foreach invitem [inv_list $item] {
					inv_rem $item $invitem
					set_visibility $invitem 1
					set_hoverable $invitem 1
					set_posbottom $invitem [vector_fix [get_pos $item]]
				}
				
				if {[get_objclass $item] == "Holzkiepe"} {
					del $item
					set item [new Holzkiepe_]
				} elseif {[get_objclass $item] == "Grosse_Holzkiepe"} {
					del $item
					set item [new Grosse_Holzkiepe_]
				}
			}

			inv_add this $item
$end


$start
$after
log "item $item [get_objname $item] retrieved from lager"

$put
			if {[get_objclass $item] == "Holzkiepe_"} {
				set_visibility $item 0
				del $item
			} elseif {[get_objclass $item] == "Grosse_Holzkiepe_"} {
				set_visibility $item 0
				del $item
			}

$end


$start
$replace
			set_pos $item [vector_add [get_pos this] "[expr {[lindex $slotx [expr {$slotidx % 6}]] + [random -0.15 0.15]}] [lindex $sloty [expr {$slotidx / 6}]] 0"]
$with
			if {[get_objclass $item] == "Holzkiepe_" || [get_objclass $item] == "Grosse_Holzkiepe_"} {
				set_pos $item [vector_add [get_pos this] "[expr {[lindex $slotx [expr {$slotidx % 6}]] + [random -0.15 0.15]}] [expr {[lindex $sloty [expr {$slotidx / 6}]] - 0.4}] -0.2"]
			} else {
				set_pos $item [vector_add [get_pos this] "[expr {[lindex $slotx [expr {$slotidx % 6}]] + [random -0.15 0.15]}] [lindex $sloty [expr {$slotidx / 6}]] 0"]
			}
			
$end



