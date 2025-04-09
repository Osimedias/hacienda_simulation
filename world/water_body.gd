extends StaticBody3D

@export var y_range : Curve
@export var noise : FastNoiseLite
@onready var water_mesh: MeshInstance3D = $WaterMesh



func _ready() -> void:
	set_process(false)
	pass

func _process(_delta: float) -> void:
	
	var y_pos : float = y_range.sample(noise.get_noise_1d(randf()))
	
	position.y = clamp(position.y,0.0,y_pos)
	position.y = y_pos
	
	
	pass
