extends Control

@export var world_node : Node3D

func _on_barracks_pressed() -> void:
	var barracks = preload("res://objects/structures/barracks.tscn").instantiate()
	barracks.name = "barracks"+str(barracks.get_rid().get_id())
	world_node.add_child(barracks)


func _on_tower_pressed() -> void:
	var tower = preload("res://objects/structures/tower_big.tscn").instantiate()
	tower.name = "tower"+str(tower.get_rid().get_id())
	world_node.add_child(tower)
