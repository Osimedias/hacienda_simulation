class_name StateMainBuilding extends State

## This node is the firts of the 'StateMachine' node

@export var structure : Structure

func enter() -> void: 
	if structure.in_floor and structure.this_structure_can_spawn_units:
		transitioned.emit(self,"StateSpawnPeasents")
	pass
## Run when this node is queue_free
func exit() -> void: 
	pass
## Update Logic of this state
func update(_delta : float) -> void:
	if structure.in_floor and structure.this_structure_can_spawn_units:
		transitioned.emit(self,"StateSpawnPeasents")
