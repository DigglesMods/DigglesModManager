$start
$before
        proc GameOver {} {
$put

		proc get_generate {} {
			global Generate_enabled
			return $Generate_enabled
		}
		proc set_generate {state} {
			global Generate_enabled
			set Generate_enabled $state
		}
		
		proc get_generate_Urwald {} {
			global generate_Urwald
			return $generate_Urwald
		}
		proc set_generate_Urwald {state} {
			global generate_Urwald
			set generate_Urwald $state
		}
		
		proc get_generate_Metall {} {
			global generate_Metall
			return $generate_Metall
		}
		proc set_generate_Metall {state} {
			global generate_Metall
			set generate_Metall $state
		}
		
		proc get_generate_Kristall {} {
			global generate_Kristall
			return $generate_Kristall
		}
		proc set_generate_Kristall {state} {
			global generate_Kristall
			set generate_Kristall $state
		}
		proc get_generate_Lava {} {
			global generate_Lava
			return $generate_Lava
		}
		proc set_generate_Lava {state} {
			global generate_Lava
			set generate_Lava $state
		}


$end

$start
$before
method generate_next {x y} {
$put
	method get_generate_enable {}  {
		return [get_generate]
	}
	method set_generate_enable {state} {
		set_generate $state
	}
	
	method get_generate_Urwald_enable {}  {
		return [get_generate_Urwald]
	}
	method set_generate_Urwald_enable {state} {
		set_generate_Urwald $state
	}
	
	method get_generate_Metall_enable {}  {
		return [get_generate_Metall]
	}
	method set_generate_Metall_enable {state} {
		set_generate_Metall $state
	}
	
	method get_generate_Kristall_enable {}  {
		return [get_generate_Kristall]
	}
	method set_generate_Kristall_enable {state} {
		set_generate_Kristall $state
	}
	
	method get_generate_Lava_enable {}  {
		return [get_generate_Lava]
	}
	method set_generate_Lava_enable {state} {
		set_generate_Lava $state
	}

	
$end


$start
$after
global zone midx temp_buffer failcount maxfailures gencount resolution act_sort_y act_sort_x
$put
 generate_Urwald generate_Metall generate_Kristall generate_Lava
$end

$start
$replace
			lg_tp_addtemplatesets 	" $zone.Std "
			lg_tp_objfilter 		{	Troll Zwerg	}


			switch $zone {
				"Urwald"	{lg_set_templategroupvalue $zone.Std.Hol 0.287;log "UUUU"}
				"Metall"    {lg_set_templategroupvalue $zone.Std.Hol 0.185;log "MMM"}
				"Kristall"  {lg_set_templategroupvalue $zone.Std.Hol 0.077;log "KKKK"}
				"Lava"      {lg_set_templategroupvalue $zone.Std.Hol 0.105;log "LLLL"}
			}
$with
			set generate_sum [expr {$generate_Urwald + $generate_Metall + $generate_Kristall + $generate_Lava}]
			if { $generate_Urwald == 1 } { 
				lg_tp_addtemplatesets " Urwald.Std " 
				lg_set_templategroupvalue Urwald.Std.Hol [expr {0.287 / $generate_sum}]
			}
			if { $generate_Metall == 1 } { 
				lg_tp_addtemplatesets " Metall.Std "
				lg_set_templategroupvalue Metall.Std.Hol [expr {0.185 / $generate_sum}]
			}
			if { $generate_Kristall == 1 } {
				lg_tp_addtemplatesets " Kristall.Std "
				lg_set_templategroupvalue Kristall.Std.Hol [expr {0.077 / $generate_sum}]
			}
			if { $generate_Lava == 1 } { 
				lg_tp_addtemplatesets " Lava.Std "
				lg_set_templategroupvalue Lava.Std.Hol [expr {0.105 / $generate_sum}]
			}
			lg_tp_objfilter { Troll Zwerg }
$end

$start
$before
		set zone "Urwald"
$put
		set generate_Urwald 1
		set generate_Metall 0
		set generate_Kristall 0
		set generate_Lava 0

$end