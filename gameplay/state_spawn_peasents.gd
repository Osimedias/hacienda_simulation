class_name StateSpawnPeasents extends State

@export var amount_of_peasents : int = 8
@export var peasant : PackedScene
@export var structure : Structure
@export var spawn_entitiy_loc : Marker3D
@export var reunion_loc : Marker3D

var player : Player

var spawn_point : Vector3

func enter() -> void:
	player = structure.get_parent_node_3d().get_node("Player") as Player
	
	spawn_point = spawn_entitiy_loc.global_position
	
## Run when this node is queue_free
func exit() -> void:
	
	pass

## Update Logic of this state
func update(_delta : float) -> void:
	
	if player.current_houseing_space > 1 or player.current_worker_population != player.current_houseing_space:
		await get_tree().create_timer(2.5).timeout
		EntitiesManager.spawn_entitie_on_world(peasant,spawn_point,structure.get_parent_node_3d(),peasant.resource_name)
		if player.current_worker_population != amount_of_peasents:
			player.current_worker_population += 1
			transitioned.emit(self,"StateMainBuilding")
	else:
		pass
## Update Logic of this state
func physics_update(_delta : float) -> void:
	
	pass
## Update inputs of this state
func input_update(_event : InputEvent) -> void:
	
	pass
