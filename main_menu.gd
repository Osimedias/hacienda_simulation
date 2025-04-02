extends Control


func _ready() -> void:
	ConfigurationManager.create_configuration_settings_file()
	pass

func _on_start_game_pressed() -> void:
	get_tree().change_scene_to_file("res://world/world.tscn")
	pass # Replace with function body.


func _on_load_game_pressed() -> void:
	pass # Replace with function body.


func _on_options_pressed() -> void:
	get_tree().change_scene_to_file("res://options_main_menu.tscn")
	pass # Replace with function body.


func _on_exit_pressed() -> void:
	get_tree().quit()
	pass # Replace with function body.
