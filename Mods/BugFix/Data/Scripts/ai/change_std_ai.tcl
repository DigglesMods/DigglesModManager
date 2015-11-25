$start
$replace
set_event $obj evt_task_walk -target $obj -pos1 \{[get_safe_pos]\}"
$with
set_event $obj evt_task_walk -target $obj -pos1 "[get_safe_pos]"
$end
