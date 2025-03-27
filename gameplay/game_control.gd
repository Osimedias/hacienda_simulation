extends Control

@onready var terrain: StaticBody3D = $"../Terrain"

@onready var mini_map_texture: ColorRect = $Panel/HBoxContainer/MiniMapTexture

func _ready() -> void:
	print(DisplayServer.window_get_mode(0))
	await terrain.heightmap.changed
	mini_map_texture.material.set_shader_parameter("shader_parameter/splatmap",terrain.heightmap)

func _input(_event: InputEvent) -> void:
	
	if Input.is_key_pressed(KEY_F11):
		if DisplayServer.window_get_mode(0) != DisplayServer.WINDOW_MODE_FULLSCREEN:
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
		else:
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_MAXIMIZED)

func _on_min_pressed() -> void:
	if DisplayServer.window_get_mode(0) == DisplayServer.WINDOW_MODE_MAXIMIZED or DisplayServer.window_get_mode(0) == DisplayServer.WINDOW_MODE_WINDOWED:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_MINIMIZED)
		@warning_ignore("integer_division")
		$ConfirmationDialog.position = Vector2(get_window().size.x/2,get_window().size.y/2)
	else:
		print("lel")


func _on_max_pressed() -> void:
	if DisplayServer.window_get_mode(0) == DisplayServer.WINDOW_MODE_WINDOWED:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_MAXIMIZED)
		@warning_ignore("integer_division")
		$ConfirmationDialog.position = Vector2(get_window().size.x/2,get_window().size.y/2)
	else:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
		@warning_ignore("integer_division")
		$ConfirmationDialog.position = Vector2(get_window().size.x/2,get_window().size.y/2)


func _on_close_pressed() -> void:
	get_tree().quit()


func _on_objetives_pressed() -> void:
	if $ConfirmationDialog.visible == false:
		$ConfirmationDialog.show()
	else: $ConfirmationDialog.visible = true
