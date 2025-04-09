extends Node3D



@export var player_name : StringName


@onready var _name: Label = $Letters/Text/V/Name



func _ready() -> void:
	_name.text = player_name
