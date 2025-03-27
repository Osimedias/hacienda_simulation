
class_name Structure extends StaticBody3D

@export var spawn_entities_node : Marker3D
@export var reunion_marker : Marker3D

@export var action_event_key_list : Array[InputEventKey]

const RAY_LEN = 1000

var camera : Camera3D

var from : Vector3
var to : Vector3
var query : PhysicsRayQueryParameters3D
var space : PhysicsDirectSpaceState3D
var result : Dictionary

var mouse_position : Vector2

var click_on_valid_terrain : bool

var terrain_navigation : NavigationRegion3D

var this_structure_can_spawn_units : bool

func _ready() -> void:
	camera = get_viewport().get_camera_3d()
	terrain_navigation = get_parent_node_3d().get_node_or_null("TerrainNavigation")
	if spawn_entities_node == null:
		this_structure_can_spawn_units = false
	else:
		this_structure_can_spawn_units = true

func _physics_process(_delta: float) -> void:
	if click_on_valid_terrain == false:
		mouse_position = get_viewport().get_mouse_position()
		from = camera.project_ray_origin(mouse_position)
		to = from + camera.project_ray_normal(mouse_position) * RAY_LEN
		space = get_world_3d().direct_space_state
		query = PhysicsRayQueryParameters3D.create(from,to,collision_mask)
		result = space.intersect_ray(query)
		
		if result.size() > 0:
			transform.origin = result.position


func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		## que pedo no se ejecuta esta parte del juego xd
		if event.button_index == MOUSE_BUTTON_LEFT:
			click_on_valid_terrain = true
			if !terrain_navigation.is_baking():
				terrain_navigation.bake_navigation_mesh(true)
	
#TODO: Hacer esto mas dinamico, para no escribir tantos if else
func _unhandled_input(event: InputEvent) -> void:
	if this_structure_can_spawn_units:
		if event is InputEventKey:
			if event.is_pressed() and event.keycode == OS.find_keycode_from_string(action_event_key_list[0].as_text_keycode()):
				spawn_unit(1,2.0)


func spawn_unit(num : int,time : float) -> void:
	var timer = get_tree().create_timer(time)
	
	if timer.timeout:
		var new_unit = preload("res://objects/units/unit_base.tscn").instantiate()
		new_unit.global_position = spawn_entities_node.global_position
		new_unit.target = reunion_marker.global_position
		get_parent_node_3d().add_child(new_unit)
