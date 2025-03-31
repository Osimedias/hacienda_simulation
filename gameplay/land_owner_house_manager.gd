extends Node



@export var structure : Structure
@export_range(8,16) var housing : int = 8

var player : Player

func _ready() -> void:
	player = structure.get_parent_node_3d().get_node("Player") as Player


func _process(_delta: float) -> void:
	if structure.in_floor:
		player.current_houseing_space = housing
	pass
