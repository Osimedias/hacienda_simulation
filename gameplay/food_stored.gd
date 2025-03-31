extends Node

@export var wheat : int = 60
@export var corn : int = 60
@export var tuna : int = 60
@export var meat : int = 60
@export var bread : int = 60





func _process(delta: float) -> void:
	## I can simplify this but for now nop
	if wheat == 0 and corn == 0 and tuna == 0 and meat == 0 and bread == 0:
		## Seend a signal to player to do something not implemented
		pass
