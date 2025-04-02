@tool
extends Node3D

@onready var ray_cast_3d: RayCast3D = $RayCast3D

func _ready() -> void:
	_physics_process(false)
# no se si funcione
func _physics_process(delta: float) -> void:
	if ray_cast_3d.is_colliding():
		global_position.y = ray_cast_3d.get_collision_normal().y
