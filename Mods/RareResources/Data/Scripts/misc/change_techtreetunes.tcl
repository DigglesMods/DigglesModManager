$start
$after
set tttitems_Dampfhammer			{
$put
Eisenerz_ Kristallerz 
$end


$start
$after
set tttitems_Hochofen				{
$put
Kohle_ Golderz_ 
$end


$start
$after
set tttitems_Schmelze				{
$put
Kohle Golderz 
$end


$start
$after
//# IF FULL
		set tttitems_Steinmetz				{
$put
Eisenerz 
$end


$start
$before

	"Gold" {
$put
	"Eisenerz_" {# im Dampfhammer
		set tttmaterial_Eisenerz_         {Stein Stein Stein Stein Stein Stein}
		set tttinvent_Eisenerz_            {{exp_Stein 0.3} {exp_Metall 0.25}}
		set tttgain_Eisenerz_            {{exp_Metall 0.05} {exp_Stein 0.06}}
		set tttinfluence_Eisenerz_         {{exp_Metall 0.5}}
	}
	"Kohle_" {# im Hochofen
		set tttmaterial_Kohle_            {Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm}
		set tttinvent_Kohle_               {{exp_Holz 0.15} {exp_Stein 0.1}}
		set tttgain_Kohle_               {{exp_Stein 0.05} {exp_Holz 0.1} {exp_Energie 0.016}}
		set tttinfluence_Kohle_            {{exp_Stein 0.6}}
	}
	"Golderz_" {# im Hochofen
		set tttmaterial_Golderz_            {Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz}
		set tttinvent_Golderz_            {{exp_Metall 0.4} {exp_Stein 0.25} {exp_Energie 0.2}}
		set tttgain_Golderz_               {{exp_Stein 0.07} {exp_Metall 0.12} {exp_Energie 0.028}}
		set tttinfluence_Golderz_         {{exp_Stein 0.6}}
	}
	"Kristallerz" {# im Dampfhammer
		set tttmaterial_Kristallerz         {Kohle Kohle Kohle Kohle Kohle Kohle}
		set tttinvent_Kristallerz         {{exp_Stein 0.7}}
		set tttgain_Kristallerz            {{exp_Stein 0.3}}
		set tttinfluence_Kristallerz      {{exp_Stein 0.9}}
	}
	"Eisenerz" {# im Steinmetz
		set tttmaterial_Eisenerz         {Stein Stein Stein Stein Stein Stein Stein Stein Stein}
		set tttinvent_Eisenerz            {{exp_Stein 0.15} {exp_Metall 0.2}}
		set tttgain_Eisenerz            {{exp_Metall 0.05} {exp_Stein 0.06}}
		set tttinfluence_Eisenerz         {{exp_Metall 0.5}}
	}
	"Kohle" {# in der Schmelze
		set tttmaterial_Kohle           {Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm Pilzstamm}
		set tttinvent_Kohle              {{exp_Holz 0.1} {exp_Stein 0.15}}
		set tttgain_Kohle              {{exp_Stein 0.05} {exp_Holz 0.1} {exp_Energie 0.016}}
		set tttinfluence_Kohle            {{exp_Stein 0.6}}
	}
	"Golderz" {# in der Schmelze
		set tttmaterial_Golderz           {Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz Eisenerz}
		set tttinvent_Golderz            {{exp_Metall 0.25} {exp_Stein 0.15} {exp_Energie 0.2}}
		set tttgain_Golderz               {{exp_Stein 0.07} {exp_Metall 0.12} {exp_Energie 0.028}}
		set tttinfluence_Golderz         {{exp_Stein 0.6}}
	}

$end