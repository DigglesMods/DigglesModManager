$start
$replace
exp_Transpor]
$with
exp_Transport]
$end

$start
$replace
			set_posbottom $invitem [vector_fix $npos]
			from_wall $item
$with
			set_posbottom $invitem [vector_fix $npos]
			from_wall $invitem
$end
