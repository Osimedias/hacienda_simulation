
class_name Structure extends StaticBody3D

@export var spawn_entities_node : Marker3D
@export var reunion_marker : Marker3D

@export var action_event_key_list : Array[InputEventKey]
@export var null_worker_model : Node3D
@export var amount_of_workers_for_this_structure : int = 0

var click_on_valid_terrain : bool
var terrain_navigation : NavigationRegion3D

var this_structure_can_spawn_units : bool
var in_floor : bool = false

var has_a_worker : bool = false

func _ready() -> void:
	terrain_navigation = get_parent_node_3d().get_node_or_null("TerrainNavigation")
	if spawn_entities_node == null:
		this_structure_can_spawn_units = false
	else:
		this_structure_can_spawn_units = true

func _physics_process(_delta: float) -> void:
	if click_on_valid_terrain == false:
		transform.origin = RaycastSystem.get_mouse_world_position(collision_mask)
	if has_a_worker:
		null_worker_model.hide()
	else:
		null_worker_model.show()


func _input(event: InputEvent) -> void:
	
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			click_on_valid_terrain = true
			in_floor = true
			print("is in floor: ", in_floor)
			if !terrain_navigation.is_baking():
				terrain_navigation.bake_navigation_mesh(true)
	
#TODO: Hacer esto mas dinamico, para no escribir tantos if else
func _unhandled_input(event: InputEvent) -> void:
	
	if in_floor and this_structure_can_spawn_units:
		if action_event_key_list.size() > 0:
			if event is InputEventKey:
				if event.is_pressed() and event.keycode == OS.find_keycode_from_string(action_event_key_list[0].as_text_keycode()):
					spawn_unit(2.0)


func spawn_unit(time : float) -> void:
	var timer = get_tree().create_timer(time)
	
	if timer.timeout:
		var new_unit = preload("res://objects/units/unit_base.tscn").instantiate()
		new_unit.global_position = spawn_entities_node.global_position
		new_unit.target = reunion_marker.global_position
		get_parent_node_3d().add_child(new_unit)
