$start
$replace 
		set tttitems_Lager					{_Kisten_einlagern _Nahrung_einlagern _Pilze_einlagern _Rohmineralien_einlagern _Mineralien_einlagern}
		set tttpreinv_Lager					{_Kisten_einlagern _Nahrung_einlagern _Pilze_einlagern _Rohmineralien_einlagern _Mineralien_einlagern}
$with
		set tttitems_Lager					{_Kisten_einlagern _Nahrung_einlagern _Pilzhut_einlagern _Pilzstamm_einlagern _Rohmineralien_einlagern _Stein_einlagern _Kohle_einlagern _Eisen_einlagern _Gold_einlagern _Kristall_einlagern _Waffen_einlagern _Werkzeuge_einlagern _Traenke_einlagern _Ringe_einlagern}
		set tttpreinv_Lager					{_Kisten_einlagern _Nahrung_einlagern _Pilzhut_einlagern _Pilzstamm_einlagern _Rohmineralien_einlagern _Stein_einlagern _Kohle_einlagern _Eisen_einlagern _Gold_einlagern _Kristall_einlagern _Waffen_einlagern _Werkzeuge_einlagern _Traenke_einlagern _Ringe_einlagern}
$end


$start
$after
		set tttinfluence__Waffen_Werkzeug_und_Traenke_einlagern		{{exp_Transport 1.0}}
	}

$put

	"_Pilzhut_einlagern" {
		set tttmaterial__Pilzhut_einlagern	{}
		set tttinvent__Pilzhut_einlagern		{}
		set tttgain__Pilzhut_einlagern		{{exp_Transport 0.001}}
		set tttinfluence__Pilzhut_einlagern	{{exp_Transport 1.0}}
	}
	"_Pilzstamm_einlagern" {
		set tttmaterial__Pilzstamm_einlagern	{}
		set tttinvent__Pilzstamm_einlagern		{}
		set tttgain__Pilzstamm_einlagern		{{exp_Transport 0.001}}
		set tttinfluence__Pilzstamm_einlagern	{{exp_Transport 1.0}}
	}
	"_Rohmineralien_einlagern" {
		set tttmaterial__Rohmineralien_einlagern	{}
		set tttinvent__Rohmineralien_einlagern		{}
		set tttgain__Rohmineralien_einlagern		{{exp_Transport 0.001}}
		set tttinfluence__Rohmineralien_einlagern	{{exp_Transport 1.0}}
	}
	"_Stein_einlagern" {
		set tttmaterial__Stein_einlagern		{}
		set tttinvent__Stein_einlagern			{}
		set tttgain__Stein_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Stein_einlagern		{{exp_Transport 1.0}}
	}
	"_Kohle_einlagern" {
		set tttmaterial__Kohle_einlagern		{}
		set tttinvent__Kohle_einlagern			{}
		set tttgain__Kohle_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Kohle_einlagern		{{exp_Transport 1.0}}
	}
	"_Eisen_einlagern" {
		set tttmaterial__Eisen_einlagern		{}
		set tttinvent__Eisen_einlagern			{}
		set tttgain__Eisen_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Eisen_einlagern		{{exp_Transport 1.0}}
	}
	"_Gold_einlagern" {
		set tttmaterial__Gold_einlagern		{}
		set tttinvent__Gold_einlagern			{}
		set tttgain__Gold_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Gold_einlagern		{{exp_Transport 1.0}}
	}
	"_Kristall_einlagern" {
		set tttmaterial__Kristall_einlagern		{}
		set tttinvent__Kristall_einlagern			{}
		set tttgain__Kristall_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Kristall_einlagern		{{exp_Transport 1.0}}
	}
	"_Waffen_einlagern" {
		set tttmaterial__Waffen_einlagern		{}
		set tttinvent__Waffen_einlagern			{}
		set tttgain__Waffen_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Waffen_einlagern		{{exp_Transport 1.0}}
	}
	"_Werkzeuge_einlagern" {
		set tttmaterial__Werkzeuge_einlagern		{}
		set tttinvent__Werkzeuge_einlagern			{}
		set tttgain__Werkzeuge_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Werkzeuge_einlagern		{{exp_Transport 1.0}}
	}
	"_Traenke_einlagern" {
		set tttmaterial__Traenke_einlagern		{}
		set tttinvent__Traenke_einlagern			{}
		set tttgain__Traenke_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Traenke_einlagern		{{exp_Transport 1.0}}
	}
	"_Ringe_einlagern" {
		set tttmaterial__Ringe_einlagern		{}
		set tttinvent__Ringe_einlagern			{}
		set tttgain__Ringe_einlagern			{{exp_Transport 0.001}}
		set tttinfluence__Ringe_einlagern		{{exp_Transport 1.0}}
	}

$end


$start
$before
	"Holzkiepe" {
$put

	"Holzkiepe_" {
		set tttmaterial_Holzkiepe			{}
		set tttinvent_Holzkiepe				{}
		set tttgain_Holzkiepe				{}
		set tttinfluence_Holzkiepe			{}
	}
	"Grosse_Holzkiepe_" {
		set tttmaterial_Grosse_Holzkiepe				{}
		set tttinvent_Grosse_Holzkiepe		{}
		set tttgain_Grosse_Holzkiepe		{}
		set tttinfluence_Grosse_Holzkiepe	{}
	}

$end