extends ConfirmationDialog


func _ready() -> void:
	@warning_ignore("integer_division")
	position = Vector2(get_window().size.x/2,get_window().size.y/2)
