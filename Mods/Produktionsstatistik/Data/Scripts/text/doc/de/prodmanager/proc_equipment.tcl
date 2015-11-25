proc set_equipment_item {inv item} {
	global prodMan infowin_prodmode
	set userInput [ref_get $prodMan equipmentUserInput]
	
	lrep userInput 0 "transfer"
	if {[obj_valid $item]} {
		set inventoryItem 0
		if {[is_contained $item] && $inv != 0 && [obj_valid $inv] && [get_objclass $inv] == "Zwerg"} {
			lrep userInput 1 $inv
			set inventoryItem 1
		} else {
			lrep userInput 1 0
		}
		lrep userInput 2 $item
	
		ref_set $prodMan equipmentUserInput $userInput
		
		if {$inventoryItem} {
			centerandselect $inv
		} elseif {$item != 0} {
			centerandselect $item
		}
	}
}

proc set_equipment_via {via} {
	global prodMan infowin_prodmode
	set userInput [ref_get $prodMan equipmentUserInput]
	
	if {[obj_valid $via]} {
		lrep userInput 3 $via
	
		ref_set $prodMan equipmentUserInput $userInput
		
		if {$via != 0} {
			centerandselect $via
		} else {
			layout reload
		}
	}
}

proc set_equipment_dest {dest} {
	global prodMan infowin_prodmode
	set userInput [ref_get $prodMan equipmentUserInput]
	
	if {[obj_valid $dest]} {
		lrep userInput 4 $dest
	
		ref_set $prodMan equipmentUserInput $userInput
		centerandselect $dest
	}
}

proc execute_equipment_task {} {
	global prodMan infowin_prodmode
	set userInput [ref_get $prodMan equipmentUserInput]
	
	call_method $prodMan add_equipment_task $userInput
	ref_set $prodMan equipmentUserInput {transfer 0 0 0 0 0}
	layout reload
}

proc remove_equipment_task {task} {
	global prodMan infowin_prodmode

	call_method $prodMan remove_equipment_task $task
	layout reload
}

proc force_equipment_task {task} {
	global prodMan infowin_prodmode

	call_method $prodMan force_equipment_task $task
	layout reload
}

proc delete_equipment_userinput {} {
	global prodMan infowin_prodmode
	
	ref_set $prodMan equipmentUserInput {transfer 0 0 0 0 0}
	layout reload
}

proc set_equipment_disable_autoprod {gnome} {
	global prodMan infowin_prodmode
	set userInput [ref_get $prodMan equipmentUserInput]
	
	if {[obj_valid $gnome]} {
		set userInput "autoprod $gnome off 0 0 0"
	
		ref_set $prodMan equipmentUserInput $userInput
	}
	layout reload
}

proc print_equipment_task {task} {
	if {[lindex $task 0] == "transfer"} {
		set from [lindex $task 1]
		set item [lindex $task 2]
		set via [lindex $task 3]
		set dest [lindex $task 4]
		set timing [lindex $task 5]
		set progress [lindex $timing 2]
		
		if {$progress == "step" || $progress == ""} {
			set progress ""
		} else {
			set progress "$progress %"
		}
		
		set who [localize "Niemand"]
		set itemname [localize "nichts"]
		set whom [localize "niemanden"]
		set ort ""
		
		set whoimage ""
		set whomimage ""
		
		if {$from != 0} {
			#hand over
			if {[obj_valid $from] && ([get_objclass $from] == "Zwerg" || [get_objclass $from] == "Baby")} {
				set who [get_objname $from]
				
				#small icon about occupation
				if {[get_remaining_sparetime $from] > 0} {
					set whoimage "/(iidata/gui/icons/S_Freizeit.tga)"
				} else {
					if {[call_method $from get_current_occupation] != "idle"} {
						set whoimage "/(iidata/gui/icons/S_Arbeiszeit.tga)"
					} else {
						set whoimage "/(iidata/gui/icons/S_Arbeiszeit2.tga)"
					}
				}
			}
			
			if {$item != 0 && [obj_valid $item]} {
				set itemname [get_objname $item]
			}
			
			if {$dest != 0 && [obj_valid $dest] && ([get_objclass $dest] == "Zwerg" || [get_objclass $dest] == "Baby")} {
				set whom [get_objname $dest]
				
				#small icon about occupation
				if {[get_remaining_sparetime $dest] > 0} {
					set whomimage "/(iidata/gui/icons/S_Freizeit.tga)"
				} else {
					if {[call_method $dest get_current_occupation] != "idle"} {
						set whomimage "/(iidata/gui/icons/S_Arbeiszeit.tga)"
					} else {
						set whomimage "/(iidata/gui/icons/S_Arbeiszeit2.tga)"
					}
				}
			}
			
			if {$via != 0 && [obj_valid $via]} {
				set ort "[localize bei] [get_objname $via]"
			}
			
			if {$via != 0 && $dest == 0} {
				layout print $who $whoimage [localize "legt"] $itemname $ort [localize "ab"]  $progress
			} else {
				layout print $who $whoimage [localize "uebergibt"] $itemname [localize "an"] $whom  $whomimage $ort  $progress
			}
		} else {
			if {[obj_valid $dest] && ([get_objclass $dest] == "Zwerg" || [get_objclass $dest] == "Baby")} {
				set who [get_objname $dest]
				
				#small icon about occupation
				if {[get_remaining_sparetime $dest] > 0} {
					set whoimage "/(iidata/gui/icons/S_Freizeit.tga)"
				} else {
					if {[call_method $dest get_current_occupation] != "idle"} {
						set whoimage "/(iidata/gui/icons/S_Arbeiszeit.tga)"
					} else {
						set whoimage "/(iidata/gui/icons/S_Arbeiszeit2.tga)"
					}
				}
			}
			
			if {$item != 0 && [obj_valid $item]} {
				set itemname [get_objname $item]
			}
			
			layout print $who  $whoimage [localize "hebt"] $itemname [localize "auf"] $progress
		}
	} elseif {[lindex $task 0] == "autoprod"} {
		set gnome [lindex $task 1]
		set intention [lindex $task 2]
		
		if {$gnome != 0 && [obj_valid $gnome] && [get_objclass $gnome] == "Zwerg" } {
			if {$intention == "off"} {
				layout print [localize "WennAllesFertig"] [get_objname $gnome] [localize "disable"] [lmsg "Automatic schedule"] 
			}
		}
	}
}

proc print_equipment_new_task {userInput} {
	layout print [localize "NewUserTask"] ":/p"
	print_equipment_task $userInput
	layout print "/p"
	hyperlink "execute_equipment_task" [lmsg ausfuehren]
	hyperlink "delete_equipment_userinput" [lmsg Undo]
	layout print "/p"
}

proc print_equipment {prodMan} {
	layout print "/(ls0)/(fn1)"
	#layout print "prodMan $prodMan /p"
	
	set userInput [ref_get $prodMan equipmentUserInput]
	set taskList [call_method $prodMan get_all_equipment_tasks]
	
	set allStuff {Steinschleuder PfeilUndBogen Buechse
		Schild Schild_1 Schild_2 Metallschild Schild_3 Kristallschild Schild_unq_1 Schild_unq_2 Drachenschuppe
		Keule Dolch_2 Axt_1 Schwert_1 Axt_unq_1 Schwert Axt_3 Schwert_4  Axt_unq_3
		Streitaxt Schwert_2 Axt_2 Axt_unq_2 Schwert_3 Lichtschwert Axt_4 Axt_unq_4
		Amulett_1 Amulett_2 Amulett_3
		Holzkiepe Grosse_Holzkiepe Reithamster Hoverboard Kristallstrahl Kettensaege Presslufthammer
		Kleiner_Heiltrank Heiltrank Grosser_Heiltrank Pilzschnaps Liebestrank Unverwundbarkeitstrank
		Unsichtbarkeitstrank Jungbrunnentrank Fruchtbarkeitstrank Wiederbelebung 
	}
		
	set weaponExp  {0 4 10
		0 0 1 3 4 6 7 8 10
		0 1 2 3 5 7 8 12 18
		0 2 3 7 8 12 15 20
		0 0 0
		0 0 0 0 0 0 0
		0 0 0 0 0 0
		0 0 0 0
	}
		
	set weaponType {Ballist. Ballist. Ballist.
		Verteid. Verteid. Verteid. Verteid. Verteid. Verteid. Verteid. Verteid. Verteid. 
		Schwertkampf Schwertkampf Schwertkampf Schwertkampf Schwertkampf Schwertkampf Schwertkampf Schwertkampf Schwertkampf 
		Zweihand Zweihand Zweihand Zweihand Zweihand Zweihand Zweihand Zweihand 
		0 0 0
		0 0 0 0 0 0 0
		0 0 0 0 0 0
		0 0 0 0
	}
	
	set inWorld [obj_query 0 -class $allStuff -flagneg {contained locked} -visibility playervisible  -alloc -1]
	set inStore [obj_query 0 -class $allStuff -flagpos {instore} -flagneg {locked} -visibility playervisible   -alloc -1]
	if {$inStore != 0} {
		set inWorld [lor $inWorld $inStore]
	}
	
	set exchangePlaces [obj_query 0 -class {Lager Feuerstelle} -flagpos {visible} -flagneg {boxed contained locked} -visibility playervisible -alloc -1]
	
	#find my gnomes
	set gnomeList [obj_query 0 -class Zwerg -owner [get_local_player]]
	set gnomeList [lsort -command compare_by_age $gnomeList]
	
	print_equipment_new_task $userInput
	
	layout print "/p"
	layout print [localize "currentTaskList"] ":/p"
	foreach task $taskList {
		if {$task != 0} {
			print_equipment_task $task
			hyperlink "remove_equipment_task \{$task\}" [lmsg Undo]
			hyperlink "force_equipment_task \{$task\}" [localize sofort]
			layout print "/p"
		}
	}
	
	layout print "/p[localize ItemsInWorld]:/p"
	set xLength -10
	set secondLine [list]
	set secondLinePos [list]
	set useSecondLine 0
	set addition 42
	
	if {[llength $inWorld] > 0} {
		foreach item $inWorld {
			set xLength [expr {$xLength + $addition}]
			print_icon_link $xLength "set_equipment_item 0 $item" $item
			
			set idx [lsearch $allStuff [get_objclass $item]]

			lappend secondLinePos $xLength
						
			if {[lindex $weaponType $idx] != 0} {
				set useSecondLine 1
				set addition 50
				lappend secondLine [string range "[lindex $weaponExp $idx] [lmsg [lindex $weaponType $idx]]" 0 7]
			} else {
				set addition 42
				lappend secondLine ""
			}
			
			if {$xLength > 450} {
				#print second line
				if {$useSecondLine} {
					layout print "/(fn0)/p"
					set idx 0
					foreach element $secondLine {
						set xLength [lindex $secondLinePos $idx]
						layout print "/(ta$xLength)" $element
						incr idx 1
					}
					layout print "/(fn1)"
				}
				
				set xLength -10
				set secondLine [list]
				set secondLinePos [list]
				set useSecondLine 0
				set addition 42
				layout print "/p"
			}
		}
	}
	#print second line
	if {$useSecondLine} {
			layout print "/(fn0)/p"
			set idx 0
			foreach element $secondLine {
					set xLength [lindex $secondLinePos $idx]
					layout print "/(ta$xLength)" $element
					incr idx 1
			}
			layout print "/(fn1)"
			set useSecondLine 0
	}
	
	#### printing transfer locations #####################
	layout print "/p [localize TransferLocations]:/p"
	set xLength -10
	
	if {[llength $exchangePlaces] > 0} {
		foreach via $exchangePlaces {
			set xLength [expr {$xLength + 42}]
			print_icon_link $xLength "set_equipment_via $via" $via
			
			if {$xLength > 450} {
				set xLength -10
				layout print "/p"
			}
		}
	}
	
	layout print "/p/(ta30)"
	hyperlink "set_equipment_via 0" [localize "Remove_exchange_place"]
	
	layout print "/p/p"
	print_equipment_new_task $userInput
	
	### printing inventories ##########################
	layout print "/p[lmsg Gnomes] / [lmsg Inventory]:/p"
	set gnomeIdx 0
	foreach gnome $gnomeList {
		if {$gnomeIdx > 6} {
			layout print "/p"
			print_equipment_new_task $userInput
			layout print "/p[lmsg Gnomes] / [lmsg Inventory]:/p"
			set gnomeIdx 0
		}
		
		hyperlink "set_equipment_dest $gnome" [get_objname $gnome]
		
		set xLength 40
		set secondLine [list]
		set secondLinePos [list]
		set useSecondLine 0
		set addition 42
		
		foreach id [inv_list $gnome] {
			if {[obj_valid $id]} {
				set class [get_objclass $id]
				set idx [lsearch $allStuff $class]
				
				if {$idx != -1} {
					set xLength [expr {$xLength + $addition}]
					print_icon_link $xLength "set_equipment_item $gnome $id" $id
					
					lappend secondLinePos $xLength
					
					if {[lindex $weaponType $idx] != 0} {
						set useSecondLine 1
						set addition 50
						lappend secondLine [string range "[lindex $weaponExp $idx] [lmsg [lindex $weaponType $idx]]" 0 7]
					} else {
						set addition 42
						lappend secondLine ""
					}
				}
			}
			
			if {$xLength > 450} {
				#print second line
				if {$useSecondLine} {
					layout print "/(fn0)/p"
					set idx 0
					foreach element $secondLine {
						set xLength [lindex $secondLinePos $idx]
						layout print "/(ta$xLength)" $element
						incr idx 1
					}
					layout print "/(fn1)"
				}
				
				set xLength 40
				set secondLine [list]
				set secondLinePos [list]
				set useSecondLine 0
				set addition 42
				layout print "/p"
			}
		}
		
		set xLength [expr {$xLength + $addition}]
		layout print "/(ta$xLength)"
		hyperlink "set_equipment_disable_autoprod $gnome" "/(iidata/gui/icons/Autoprod_On.tga)"
		
		if {$useSecondLine} {
			layout print "/(fn0)/p"
			set idx 0
			foreach element $secondLine {
				set xLength [lindex $secondLinePos $idx]
				layout print "/(ta$xLength)" $element
				incr idx 1
			}
			layout print "/(fn1)"
		}
		
		incr gnomeIdx 1
		
		layout print "/p"
	}
	
	layout print "/p"
}
