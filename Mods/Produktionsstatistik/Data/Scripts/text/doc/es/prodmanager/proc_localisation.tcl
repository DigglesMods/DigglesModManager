proc localize {textString} {
	set g_lang_string {"neededMaterial" "Rohstoffe" "Waffen" "Manager" "settings" "sammleErfahrungin"
		 "infoStringManager"
		 "enable" "disable" "Equipment" "NewUserTask" "currentTaskList" "TransferLocations" "Remove_exchange_place"
		 "ItemsInWorld" "WennAllesFertig" "sofort"
		 "Niemand" "uebergibt" "an" "niemanden" "bei" "hebt" "nichts" "auf" "legt" "ab"}
	
	set g_lang_de_string {"benötigtes Material" "Rohstoffe" "Waffen" "Manager" "Einstellungen" "sammle Erfahrung in"
		"Hier kann man einem Zwerg die Anweisung geben, in welchem Gebiet er Erfahrungen sammeln soll. Dies geschieht, indem der Produktionsmanager den Zwerg derjenigen Produktionsstätte zuweist, für die es die meisten Erfahrungspunkte gibt." 
		"aktivieren" "deaktivieren" "Ausrüstung" "Neuer Task" "Aktuelle Aufgaben" "Übergabeorte" "Entferne_Übergabeort"
		"Items die frei herumliegen" "Wenn alles fertig" "sofort"
		 "Niemand" "übergibt" "an" "niemanden" "bei" "hebt" "nichts" "auf"  "legt" "ab"}
	
	set g_lang_en_string {"necessary material" "Resources" "Weapons" "Manager" "Settings" "gain experience in"
		"You can define a field of experience for each dwarf. The ProductionManager will try to assign the dwarf to that production site which gives the most experience gain."
		"enable" "disable" "Equipment" "New task" "Current tasks"  "Transfer locations" "Remove_transfer_location"
		"Items which lie around in the world" "When ready" "instant"
		 "Nobody" "hands over" "to" "nobody" "at" "picks" "nothing" "up"  "puts" "down"}
		
	set idx [lsearch $g_lang_string $textString]
	
	if {[locale] == "de"} {
		if {$idx >= 0} {
			return [lindex $g_lang_de_string $idx]
		} else {
			return "???"
		}
	} else {
		if {$idx >= 0} {
			return [lindex $g_lang_en_string $idx]
		} else {
			return "???"
		}
	}
}
