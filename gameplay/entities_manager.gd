extends Node








func spawn_entitie_on_world(entity : PackedScene,position : Vector3,parent : Node3D,entity_name : String) -> void:
	var new_entity = entity.instantiate() as Node3D
	new_entity.name = entity_name
	new_entity.global_position = position
	parent.add_child(new_entity)
