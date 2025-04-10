class_name State extends Node


@warning_ignore("unused_signal")
signal transitioned(state : State, next_state : StringName)

## Run starting code
func enter() -> void: pass
## Run when this node is queue_free
func exit() -> void: pass
## Update Logic of this state
func update(_delta : float) -> void: pass
## Update Logic of this state
func physics_update(_delta : float) -> void: pass
## Update inputs of this state
func input_update(_event : InputEvent) -> void: pass
