extends Node

static var resolutions : PackedVector2Array = [
	Vector2i(3840,2160),
	Vector2i(2569,1440),
	Vector2i(1920,1080),
	Vector2i(1366,768),
	Vector2i(1536,864),
	Vector2i(1280,720),
	Vector2i(1440,900),
	Vector2i(1600,900),
	Vector2i(1024,600),
	Vector2i(800,600),
	
]

var current_resolution : Vector2i

func _ready() -> void:
	current_resolution = get_window().size
	pass

func get_resolution_from_list(index : int) -> void:
	var center_screen = DisplayServer.screen_get_position()*DisplayServer.screen_get_size()/2
	get_window().size = GraphicsManager.resolutions[index]
	get_window().position = center_screen - get_window().get_size_with_decorations()/2

##TODO: Implement this functions
@warning_ignore("unused_parameter")
func get_graphics_mode_change(index : int) -> void:
	
	pass
@warning_ignore("unused_parameter")
func get_shader_mode_change(index : int) -> void:
	pass
@warning_ignore("unused_parameter")
func get_lighting_mode_change(index : int) -> void:
	pass
@warning_ignore("unused_parameter")
func get_shadows_mode_change(index : int) -> void:
	pass
@warning_ignore("unused_parameter")
func get_terrain_mode_change(index : int) -> void:
	pass
@warning_ignore("unused_parameter")
func get_reflections_mode_change(index : int) -> void:
	pass
@warning_ignore("unused_parameter")
func get_effects_mode_change(index : int) -> void:
	pass
@warning_ignore("unused_parameter")
func get_texture_mode_change(index : int) -> void:
	pass
func get_post_processing_mode_change() -> void:
	pass
@warning_ignore("unused_parameter")
func get_physics_mode_change(index : int) -> void:
	pass
