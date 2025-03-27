extends MeshInstance3D


@onready var sub_viewport: SubViewport = $"../SubViewport"


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	mesh.surface_get_material(0).set_shader_parameter("height_map_mask",sub_viewport.get_texture())
	pass # Replace with function body.
