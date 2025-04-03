extends CheckButton



func _ready() -> void:
	button_pressed = DisplayServer.window_get_vsync_mode(0)


func _on_toggled(toggled_on: bool) -> void:
	if toggled_on:
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ENABLED)
	else:
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_DISABLED)
	print(DisplayServer.window_get_vsync_mode(0))
	pass # Replace with function body.
