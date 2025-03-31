class_name StateMachine extends Node


@export var initial_state : State
## Actor is the root node
@export var actor : Node
## The current state to execute
var current_state : State
## All states nodes
var states : Dictionary = {}

func _ready() -> void:
	# Get the childrens of the StateMachine and add the State nodes type to the list
	for child in get_children():
		if child is State:
			states[child.name.to_lower()] = child
			child.transitioned.connect(on_child_transitioned)
		else:
			push_warning("State machine contains child which is not 'State'")
	if initial_state:
		initial_state.enter()
		current_state = initial_state

func _process(delta: float) -> void:
	if current_state:
		current_state.update(delta)


func _physics_process(delta: float) -> void:
	if current_state:
		current_state.physics_update(delta)


func _input(event: InputEvent) -> void:
	if current_state:
		current_state.input_update(event)

func on_child_transitioned(state : State, new_state_name : StringName) -> void:
	if state != current_state:
		return
	var next_state : State = states.get(new_state_name.to_lower())
	
	if !next_state:
		return
	
	if current_state:
		current_state.exit()
	
	next_state.enter()
	current_state = next_state
	print("Current state: ", current_state.name.to_lower())
