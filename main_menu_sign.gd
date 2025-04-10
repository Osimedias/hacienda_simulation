extends Node3D



@export var player_name : StringName


# Used for checking if the mouse is inside the Area3D.
var is_mouse_inside = false
# The last processed input touch/mouse event. To calculate relative movement.
var last_event_pos2D = null
# The time of the last event in seconds since engine start.
var last_event_time: float = -1.0

func _ready() -> void:
	set_process_unhandled_input(false)

func _unhandled_input(event: InputEvent) -> void:
	for mouse_event in [InputEventMouseButton, InputEventMouseMotion, InputEventScreenDrag, InputEventScreenTouch]:
		if is_instance_of(event,mouse_event):
			return
	$Letters/Text.push_input(event)
	pass

func _on_area_3d_area_entered(_area: Area3D) -> void:
	is_mouse_inside = true
	print("entered")
	pass # Replace with function body.


func _on_area_3d_area_exited(_area: Area3D) -> void:
	is_mouse_inside = false
	print("exited")
	pass # Replace with function body.


func _on_area_3d_input_event(_camera: Node, event: InputEvent, event_position: Vector3, _normal: Vector3, _shape_idx: int) -> void:
	var event_pos3d = event_position
	var quad_mesh_size = $Letters.mesh.size
	
	var now : float = Time.get_ticks_msec() / 1000.0
	event_pos3d = $Letters.global_transform.affine_inverse() * event_pos3d
	
	var event_pos2d : Vector2 = Vector2()
	
	if is_mouse_inside:
		event_pos2d = Vector2(event_pos3d.x,-event_pos3d.y)
		event_pos2d.x = event_pos2d.x / quad_mesh_size.x
		event_pos2d.y = event_pos2d.y / quad_mesh_size.y
		event_pos2d.x += 0.5
		event_pos2d.y += 0.5
		
		event_pos2d.x *= $Letters/Text.size.x
		event_pos2d.y *= $Letters/Text.size.y
	elif last_event_pos2D != null:
		event_pos2d = last_event_pos2D
	
	event.position = event_pos2d
	if event is InputEventMouse:
		event.global_position = event_pos2d
	
	if event is InputEventMouseMotion or event is InputEventScreenDrag:
		if last_event_pos2D == null:
			event.relative = Vector2(0,0)
		else:
			event.relative = event_pos2d - last_event_pos2D
			event.velocity = event.relative / (now - last_event_time)
	
	last_event_pos2D = event_pos2d
	last_event_time = now
	
	$Letters/Text.push_input(event)
