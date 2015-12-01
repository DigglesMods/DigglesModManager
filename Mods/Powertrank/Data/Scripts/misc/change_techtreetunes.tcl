$start
$before
"Pilzschnaps"
$put
"Bier_" {
		set tttmaterial_Bier_				{Pilzstamm Raupe Raupe Raupe Raupe Raupe Raupe}
		set tttinvent_Bier_					{{exp_Nahrung 0.3} {exp_Energie 0.15}}
		set tttgain_Bier_					{{exp_Nahrung 0.003} {exp_Service 0.002}}
		set tttinfluence_Bier_				{{exp_Nahrung 0.3}}
	}
	
$end

$start
$after
set tttitems_Brauerei				{
$put
Bier_ 
$end