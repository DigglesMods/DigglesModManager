
layout clear

layout print "/(fn2)"
layout print "/(ac)[lmsg Zwerge]"
layout print "/p"

layout print "/(fn1,ls2,ml5,mr5,al)"

set playerid [get_local_player]

set gnomelist [obj_query 0 -type {gnome baby} -owner $playerid]

if { $gnomelist == 0 } {return}

if {! [info exists infowin_gnomemode] } {
	set infowin_gnomemode "standard"
}

if {! [info exists infowin_gnomeOrderMode] } {
	set infowin_gnomeOrderMode "name"
}


proc centerandselect {gid} {
	global infowin_gnomemode infowin_gnomeOrderMode
	
	set view [get_view]
	set pos [get_pos $gid]
	set_view [vector_unpackx $pos] [vector_unpacky $pos] [vector_unpackz $view]
	selection clear
	selection include $gid
	layout reload
}

proc gnomename {gid} {
	if {$gid == -1} {return [lmsg {dead!}]}
	if { [selection check $gid] } {
		return "[get_objname $gid]"
	} else {
		return "[layout autoxlink "centerandselect $gid" "[get_objname $gid]"]"
	}
}

proc gnomeinfo_order_by {mode} {
	global infowin_gnomemode infowin_gnomeOrderMode
	set infowin_gnomeOrderMode $mode
	layout reload
}

proc gnomeinfo_switchto {mode} {
	global infowin_gnomemode infowin_gnomeOrderMode
	set infowin_gnomemode $mode
	layout reload
}

proc gnomeinfo_switch_sorting {mode title} {
	return [layout autoxlink "gnomeinfo_order_by $mode" [lmsg $title]]
}

proc gnomeinfo_tablehead_switchhead {mode title} {
	return [layout autoxlink "gnomeinfo_switchto $mode" [lmsg $title]]
	
	global infowin_gnomemode
	if {$mode == $infowin_gnomemode} {
		return [lmsg $title]
	} else {
		return [layout autoxlink "gnomeinfo_switchto $mode" [lmsg $title]]
	}
}

proc gnomeinfo_tablehead {} {
	global infowin_gnomemode
//	layout print "[layout autoxlink "gnomeinfo_collapseall" { /(iidata/gui/buttons/minus.tga) }] /(tx  ) [layout autoxlink "gnomeinfo_expandall" { /(iidata/gui/buttons/plus.tga) }]"
	layout print "/(fn1)"
	layout print "/(tx   )[gnomeinfo_tablehead_switchhead standard Standard]"
	layout print "/(tx   )[gnomeinfo_tablehead_switchhead production Produktion]"
	layout print "/(tx   )[gnomeinfo_tablehead_switchhead fight Fight]"
	layout print "/(tx   )[gnomeinfo_tablehead_switchhead inventory Inventory]"
	layout print "/(tx   )[gnomeinfo_tablehead_switchhead family Family]"
	layout print "/p"
//	layout print "Mode: $infowin_gnomemode"
	layout print "/p"
}

proc gnomeinfo_tablehead_standard {} {
	layout print "/(fn0)"
	set x 10
	set xi 55
	
	layout print "/(tx   )[gnomeinfo_switch_sorting name Name]"; set x [expr $x + 90]
	# layout print "/(ta$x)[lmsg objref]"; set x [expr $x + 50]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Hitpoints Ges.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Nutrition Ern.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Alertness Aufm.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting atr_Mood Stimm.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting GnomeAge Age]"; set x [expr $x + $xi]
	
	layout print "/p"
}

proc gnomeinfo_tablehead_family {} {
	layout print "/(fn0)"
	set x 10
	set xi 100
	layout print "/(ta$x)[lmsg Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[lmsg Partner]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Mutter]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Vater]"; set x [expr $x + $xi]
	layout print "/(ta$x)[lmsg Kinder]"; set x [expr $x + $xi]
	layout print "/p"
}

proc gnomeinfo_tablehead_production {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	
	layout print "/(ta$x)[gnomeinfo_switch_sorting name Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[lmsg Summe]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Nahrung  Nahrung]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Holz Holz]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Stein Steine]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Metall Metalle]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Transport Transp.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Energie Alchemie]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Service Service]"; set x [expr $x + $xi]
	layout print "/p"
}

proc gnomeinfo_tablehead_fight {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	
	layout print "/(ta$x)[gnomeinfo_switch_sorting name Name]"; set x [expr $x + 90]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_Kampf Kampf]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Sword Schwertkampf]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Twohanded Zweihand]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Defense Verteid.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Ballistic Ballist.]"; set x [expr $x + $xi]
	layout print "/(ta$x)[gnomeinfo_switch_sorting exp_F_Kungfu Kungfu]"; set x [expr $x + $xi]
	layout print "/p"
}

proc gnomeinfo_tablehead_inventory {} {
	layout print "/(fn0)"
	set x 10
	set xi 60
	layout print "/(ta$x)[lmsg Name]"; set x [expr $x + 90]
	layout print "/p"
}

proc gnomeinfo_standard {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 55
	layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(fn0)"
	
	# layout print "/(ta$x)$gid";set x [expr $x + 50]
	layout print "/(ta$x)/(ccHealthbar $gid atr_Hitpoints)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccHealthbar $gid atr_Nutrition)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccHealthbar $gid atr_Alertness)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccHealthbar $gid atr_Mood)"; set x [expr $x + $xi]
	set age [expr  int(([gettime] - [get_attrib $gid GnomeAge]) / 1800.0) ]
	//layout print "/(ta$x)[lmsg Age]:$age";
	set x [expr $x + 10]
	layout print "/(ta$x)$age";

	if {![get_prodautoschedule $gid]} {
		set imgname "/gui/icons/S_Autoprodoff.tga"
	} else {
		if {[get_remaining_sparetime $gid] > 0} {
			set imgname "/gui/icons/S_Freizeit.tga"
		} else {
			if {[state_get $gid] == "work_dispatch"} {
				set imgname "/gui/icons/S_Arbeiszeit.tga"
			} else {
				set imgname "/gui/icons/S_Arbeiszeit2.tga"
			}
		}
	}
	set x [expr $x + 20]
	layout print "/(ta$x)/(iidata$imgname)  "
	if {[get_objtype $gid] == "gnome"} {
		set pggpw [prod_gnome_get_preferred_workplace $gid]
		if {$pggpw} {
			set prodname [get_objname $pggpw]
			set imgname "/gui/icons/S_Gelock.tga"
			set x [expr $x + 20]
			layout print "/(ta$x)/(iidata$imgname)  "
			set x [expr $x + 20]
			layout print "/(ta$x)$prodname"
		}
	}
}

proc gnomeinfo_family {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 100
	//layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(ta$x)/(ccEditName $gid)"; set x [expr $x + 90]
	layout print "/(fn0)"
	set partner [partner_info getpartner $gid]
	if {$partner == 0} {
		set pname ""
	} else {
		set pname [gnomename $partner]
	}
	set mother [partner_info getmother $gid]
	if {$mother == 0} {
		set mname ""
	} else {
		set mname [gnomename $mother]
	}
	set father [partner_info getfather $gid]
	if {$father == 0} {
		set fname ""
	} else {
		set fname [gnomename $father]
	}
	layout print "/(ta$x)$pname"; set x [expr $x + $xi]
	layout print "/(ta$x)$mname"; set x [expr $x + $xi]
	layout print "/(ta$x)$fname"; set x [expr $x + $xi]
	set children [partner_info getchildren $gid]
	layout print "/(ta$x)$children"; set x [expr $x + $xi]
	if { [partner_info getpregnancy $gid] } {
		layout print "/(tx  )[lmsg schwanger]"; set x [expr $x + $xi]
	}
}

proc gnomeinfo_inventory2 {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 60
	layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(fn0)"
}

proc gnomeinfo_production {gid} {
	layout print "/(fn1)"
	set x 10
	set xi 60
	layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(fn0)"
	layout print "/(ta$x)/(ccAttribrangebar $gid)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Nahrung)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Holz)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Stein)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Metall)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Transport)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Energie)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_Service)"; set x [expr $x + $xi]
}

proc gnomeinfo_fight {gid} {
	set x 10
	set xi 60
	layout print "/(fn1)"
	layout print "/(ta$x)[gnomename $gid]"; set x [expr $x + 90]
	layout print "/(fn0)"
	layout print "/(ta$x)/(ccAttribbar $gid exp_Kampf)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_F_Sword)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_F_Twohanded)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_F_Defense)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_F_Ballistic)"; set x [expr $x + $xi]
	layout print "/(ta$x)/(ccAttribbar $gid exp_F_Kungfu)"; set x [expr $x + $xi]
}

proc gnomeinfo_inventory {gid} {
	layout print "/(fn1)"
	layout print "/(ta10)[gnomename $gid]"
	layout print "/(fn0)/(ta80)/(bo-10)"
	set olst [inv_list $gid]
	set x 90
	set cnt 0
	set clst [list]
	set colorlst [list]
	set cclst [list]
	set weaponlist [list]

	foreach oref $olst {
		set class [get_objclass $oref]
        set widtrue [get_weapon_id $oref true]
		set widfalse [get_weapon_id $oref false]
		set weaponID 0
		if {$widtrue > $widfalse} {set weaponID $widtrue} else {set weaponID $widfalse}
		if {![check_weapon_exp $gid $weaponID]} {
			set color "Red"
		} else {
			set color "nocolor"
		}
		
		lappend weaponlist "$widtrue $widfalse $weaponID"
		lappend clst $class
		lappend colorlst $color
	}
	
	foreach cref $clst {
		set color [lindex $colorlst $cnt]
		if {$color == "nocolor"} {set color ""}
		set imgname "/gui/icons/${cref}.tga"
		set imgname "/texture/classicons/$color${cref}.tga"
//		set imgname "/gui/icons/grillpilz.tga"
//		layout print "/(tx  )$imgname"
		layout print "/(ta$x)/(iidata$imgname)  "
		# layout print "/(ta$x) [lindex $weaponlist $cnt]"
		
		set x [expr $x + 31]
		incr cnt
	}
	set sz [inv_getsize $gid]
//	set cnt 0
//	set x 80
//	set x [expr $x + 31]
	layout print "/(fn0)/(ta80)/(bo0)"
	for {} {$cnt < $sz} {incr cnt} {
//		layout print "/(ta$x)/(iidata/gui/icons/xoverlay_switchon.tga)  "
//		layout print "/(ta$x)/(iidata/gui/inventoryslotsmall.tga)  "
		layout print "/(ta$x)/(tx__)  "
//		layout print "/(ta$x)/(iidata/gui/hand.tga)  "
		set x [expr $x + 31]
	}
	layout print "/(fn0)/(bo0)"
}

gnomeinfo_tablehead

if {$infowin_gnomemode == "standard" } {
	gnomeinfo_tablehead_standard
}
if {$infowin_gnomemode == "fight" } {
	gnomeinfo_tablehead_fight
}
if {$infowin_gnomemode == "production" } {
	gnomeinfo_tablehead_production
}
if {$infowin_gnomemode == "inventory" } {
	gnomeinfo_tablehead_inventory
}
if {$infowin_gnomemode == "family" } {
	gnomeinfo_tablehead_family
}

proc compare_by_name {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
    return [string compare [get_objname $a] [get_objname $b]]
}

proc compare_by_attribute {a b} {
	if {$a == -1} {return -1}
	if {$b == -1} {return 1}
	
	global infowin_gnomeOrderMode
	return [expr  [get_attrib $a $infowin_gnomeOrderMode] < [get_attrib $b $infowin_gnomeOrderMode]]
}

 if {$infowin_gnomeOrderMode == "name" || $infowin_gnomeOrderMode == "" } {
	set gnomelist [lsort -command compare_by_name $gnomelist]
} else {
	set gnomelist [lsort -command compare_by_attribute $gnomelist]
}


log "$gnomelist"

foreach gid $gnomelist {

	set collapsed 0

	if {$infowin_gnomemode == "standard" } {
		gnomeinfo_standard $gid
	}
	if {$infowin_gnomemode == "production" } {
		gnomeinfo_production $gid
	}
	if {$infowin_gnomemode == "fight" } {
		gnomeinfo_fight $gid
	}
	if {$infowin_gnomemode == "inventory" } {
		gnomeinfo_inventory $gid
	}
	if {$infowin_gnomemode == "family" } {
		gnomeinfo_family $gid
	}

	layout print "/p"
}

layout print "/p"

