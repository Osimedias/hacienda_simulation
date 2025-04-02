@tool
extends Node3D

@export var speed : float = 1.0:
	set(value):
		speed = value



func _process(delta: float) -> void:
	rotate_x(speed*delta)
	pass
