$start
$replace
return [expr  [get_attrib $a GnomeAge] > [get_attrib $b GnomeAge]]
$with
return [expr  [call_method $a getbirthtime] > [call_method $b getbirthtime]
$end