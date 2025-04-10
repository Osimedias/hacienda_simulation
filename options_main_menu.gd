extends Control

@export var parent : Node3D

func _on_accept_pressed() -> void:
	print("Not implemented")
	parent.get_parent_node_3d().get_parent().get_node("Node3D/main_menu_background/Camera").current = true
	parent.get_parent_node_3d().get_parent().get_node("VBoxContainer").grab_focus()
	parent.get_parent_node_3d().get_parent().get_node("VBoxContainer").show()
	parent.set_process_unhandled_input(false)
	pass # Replace with function body.
