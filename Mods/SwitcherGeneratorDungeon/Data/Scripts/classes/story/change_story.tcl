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

	
$end
