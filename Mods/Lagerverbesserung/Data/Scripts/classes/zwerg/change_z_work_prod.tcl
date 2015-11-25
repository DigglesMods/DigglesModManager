$start
$replace 
tasklist_add this "state_disable this; walk_action \"-target \{[get_pos $next_item]\} \" {state_enable this}"
$with
tasklist_add this "state_disable this; walk_action \"-target \{[get_posbottom $next_item]\} \" {state_enable this}"
$end



$start
$replace 
	tasklist_add this "call_method $store retrieve_item $slotidx $item; beamto_inv $item"
$with
	set item_inv $item
	if {[get_objclass $item] == "Holzkiepe_"} {
		set item_inv [new Holzkiepe]
	} elseif {[get_objclass $item] == "Grosse_Holzkiepe_"} {
		set item_inv [new Grosse_Holzkiepe]
	}
	tasklist_add this "call_method $store retrieve_item $slotidx $item; beamto_inv $item_inv"
$end