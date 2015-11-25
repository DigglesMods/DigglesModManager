$start
$after
			method is_weapon {} {}
			method destroy {} { del this }
			obj_init \"
				set_selectable 	this 1
				set_hoverable 	this 1

$put
				set_storable 	this 1

$end

