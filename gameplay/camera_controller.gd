extends Node3D

@export_flags_3d_render var cull_mask : int
@export var speed : float = 70.0
@export var zoom_speed : float = 45.0
@onready var camera_3d: Camera3D = $Head/Camera3D

@export var terrain : Terrain

var min_zoom : float = 0.0
var max_zoom : float = 65.0

var terrain_mesh : MeshInstance3D

func _ready() -> void:
	camera_3d.cull_mask = cull_mask
	if terrain:
		terrain_mesh = terrain.get_node("TerrainMesh")
	

func _process(_delta: float) -> void:
	
	position.x = clamp(position.x,terrain_mesh.get_aabb().position.x + 7,terrain_mesh.get_aabb().end.x - 7)
	position.y = clamp(position.y,0.0,65.0)
	position.z = clamp(position.z,terrain_mesh.get_aabb().position.z + 7,terrain_mesh.get_aabb().end.z - 7)
	
	
	var direction : Vector2 = Input.get_vector("move_left","move_right","move_forward","move_backward")
	
	transform.origin += Vector3(direction.x,0,direction.y)
	
	if Input.is_action_just_pressed("zoom_in"):
		$Head.transform.origin.y += 1
	if Input.is_action_just_pressed("zoom_out"):
		$Head.transform.origin.y -= 1
