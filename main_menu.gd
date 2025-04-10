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
	$VBoxContainer.hide()
	$Node3D/sign2/Letters/Text/OptionsMainMenu.grab_focus()
	$Node3D/sign2.set_process_unhandled_input(true)
	$Node3D/OptionsCamera.current = true
	pass # Replace with function body.


func _on_exit_pressed() -> void:
	get_tree().quit()
	pass # Replace with function body.


func _on_player_name_pressed() -> void:
	$Back.show()
	$Back.disabled = false
	$VBoxContainer.hide()
	$Node3D/sign/Letters/Text/V/Name.grab_focus()
	$Node3D/sign.set_process_unhandled_input(true)
	$Node3D/PlayerNameCamera.current = true
	$Node3D/main_menu_background/Camera.current = false


func _on_back_pressed() -> void:
	$VBoxContainer.show()
	$Back.disabled = true
	$Back.hide()
	$VBoxContainer.grab_focus()
	$Node3D/sign.set_process_unhandled_input(false)
	var n = $Node3D/sign/Letters/Text/V/Name.text
	PlayerData.player_name = n
	$Node3D/PlayerNameCamera.current = false
	$Node3D/main_menu_background/Camera.current = true
