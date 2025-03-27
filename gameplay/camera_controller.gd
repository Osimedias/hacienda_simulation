extends Node3D

@export_flags_3d_render var cull_mask : int
@export var speed : float = 70.0
@export var zoom_speed : float = 45.0
@onready var camera_3d: Camera3D = $Head/Camera3D


var min_zoom : float = 0.0
var max_zoom : float = 65.0

func _ready() -> void:
	camera_3d.cull_mask = cull_mask

func _process(_delta: float) -> void:
	
	var direction : Vector2 = Input.get_vector("move_left","move_right","move_forward","move_backward")
	
	transform.origin += Vector3(direction.x,0,direction.y)
	
	if Input.is_action_just_pressed("zoom_in"):
		$Head.transform.origin.y += 1
	if Input.is_action_just_pressed("zoom_out"):
		$Head.transform.origin.y -= 1


func _on_up_mouse_entered() -> void:
	transform.origin.z += 1


func _on_down_mouse_entered() -> void:
	transform.origin.z -= 1


func _on_left_mouse_entered() -> void:
	transform.origin.x += 1


func _on_right_mouse_entered() -> void:
	transform.origin.x -= 1


func _on_up_mouse_exited() -> void:
	transform.origin.z = 0


func _on_down_mouse_exited() -> void:
	transform.origin.z = 0


func _on_left_mouse_exited() -> void:
	transform.origin.x = 0


func _on_right_mouse_exited() -> void:
	transform.origin.x = 0
