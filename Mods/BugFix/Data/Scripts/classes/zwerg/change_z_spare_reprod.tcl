$start
$replace
	handle_event evt_zwerg_birth {
		bear_child [event_get this -num1] 1
	}
$with
	handle_event evt_zwerg_birth_forced {
		bear_child [event_get this -num1] 1
	}
$end


$start
$before

		if {!$forced&&[get_gnomeposition this]} {
$put

		if {!$reprod_pregnancy||![partner_info getpregnancy this]} {
			set reprod_pregnancy 0
			partner_info setpregnancy this 0
			return
		}

$end