$if:CIV_DROPS

$start
$replace
					if {[im_in_campaign]} {
						set zone 0
						set UMK 0;set UTP 0; set UUM 0
						catch {set UMK [sm_get_event Uebergang_Met_Kris]}
						if {$UMK} {
							set zone 3
						} else {
							catch {set UTP [sm_get_event Titanic_Pumpe_aktiviert]}
							if {$UTP} {
								set zone 2
							} else {
								catch {set UUM [sm_get_event Uebergang_Urw_Met]}
								if {$UUM} {
									set zone 1
								}
							}
						}
					} else {
						global civ_state
						set zone [hmax 0 [hmin [expr {int($civ_state*10.0)}] 3]]
					}
$with
					global civ_state
					set zone [hmax 0 [hmin [expr {int($civ_state*10.0)}] 3]]
$end

$ifend


$start
$replace
				if {[im_in_campaign]} {
					set rndval 0.01
				} else {
					set rndval 0.04
				}
$with
				set rndval [expr {$print:DROP_CHANCE / 100.0}]
$end


