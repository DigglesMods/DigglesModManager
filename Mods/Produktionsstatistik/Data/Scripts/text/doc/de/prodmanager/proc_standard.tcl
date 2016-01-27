proc get_inv_class_list {gid} {
	set result ""
	foreach item [inv_list $gid] {
		lappend result [get_objclass $item]		
	}
	return $result
}

proc standard_create_printable_string {input} {
			# sort by name
			set input [lsort $input]
			
			set printableList ""
			set lastItem ""
			set count 0
			foreach item $input {
				if {$item == $lastItem} {
					incr count 1
				}
					
				if {$item != $lastItem} {
					if {$count > 0 && [string first "Halbzeug" $lastItem 0] == -1} {
						set printableList [concat $printableList "$count x [lmsg $lastItem]/(tx )"]
					}
					set count 1
					set lastItem $item
				}
			}
				
			if {$count > 0 && [string first "Halbzeug" $lastItem 0] == -1} {
				set printableList [concat $printableList "$count x [lmsg $lastItem]"]
			}
}

proc prodinfo_stats {gid} {
	set slist [get_prod_slot_list $gid]
	
	set prodlist [list]
	set invlist [list]

	foreach class $slist {
		if {! [get_prod_slot_invented $gid $class] } {
			if { [get_prod_slot_inventable $gid $class] } {
				lappend invlist $class
			}
		} else {
			lappend prodlist $class
		}
	}
	
	layout print "/(ls-5,ta20)[prodname centerandselect $gid]/p"
	layout print "/(ls0,ta80)"
	
	set x 50
	set show 1
	set boxed [get_boxed $gid]
	if {$boxed} {
		set icon "data/gui/icons/unpack.tga"
		layout print "/(ta$x)/(ii$icon)"
		set x [expr $x + 38]
		set show 0
	}
	
	if {[get_objtype $gid] == "energy" && $show} {
		if {[get_prod_enabled $gid]} {
			set icon "data/gui/icons/energyon.tga"
		} else {
			set icon "data/gui/icons/energyoff.tga"
		}

		layout print "/(ta$x)/(ii$icon)"
		set x [expr $x + 38]
		set show 0
	}
	
	
	if {([llength $prodlist] != 0) && $show} {
		foreach class $prodlist {
			set count [get_prod_slot_cnt $gid $class]
			set icon "data/gui/icons/$class.tga"

			if { ![get_prod_slot_buildable $gid $class] } {
				set icon "data/gui/icons/Red$class.tga"
			}

			layout print "/(ta$x)/(ii$icon)"
			if { $count > 0 } {
				if { $count == 10 } {
						layout print "/(ta[expr $x + 25])o"
						layout print "/(ta[expr $x + 30])o"
				} else {
					layout print "/(ta[expr $x + 25])$count"
				}
			}
			set x [expr $x + 38]
		}
		
		if {[get_attrib $gid atr_Hitpoints] < 1} {
			set icon "data/gui/icons/repair.tga"
			layout print "/(ta$x)/(ii$icon)"
		}
		
		#prints the inventory of the production facility
		set inventory_list [inv_list $gid]
		
		if {$inventory_list != ""} {
			layout print "/p/(tx    ) [lmsg Inventory]:"
			set sortedInventory [list]
			foreach class $inventory_list {
				#get class names
				lappend sortedInventory [get_objclass $class]
			}
			
			set printableList [standard_create_printable_string $sortedInventory]
			layout print "/(tx   )$printableList"
		}
		
		if {[get_prod_materialneed $gid]} {
			#what do we need
			set itemneedlist ""
			foreach slot [call_method $gid prod_items] {
				
				if {[get_owner_attrib [get_local_player] Bp$slot] > 0} {
					set i [get_prod_slot_cnt $gid $slot]
					
					if {[string first [get_objclass $gid] "FarmBar"] != -1 && $i > 0} {
						set i 1
					}
					
					set materiallist [call_method $gid prod_item_materials $slot]
					while {$i > 0} {
						set itemneedlist [concat $itemneedlist $materiallist]
						incr i -1
					}
				}
			}
			
			if {$itemneedlist != ""} {
				set itemhavelist [get_inv_class_list $gid]

				// von den vorhandenen die benötigten abziehen
				foreach item $itemhavelist {
					set idx [lsearch $itemneedlist $item]
					if {$idx >= 0} {
						lrem itemneedlist $idx
					}
				}
			}
			
			if {$itemneedlist != ""} {
				
				set itemneedlist [lsort  $itemneedlist]
				
				set printableList [standard_create_printable_string $itemneedlist]
				
				layout print "/(ls0)/p/(tx    )[localize neededMaterial]"
				layout print "/(tx   )$printableList/p"
			}
		} 
	}
	
	layout print "/p"
}
