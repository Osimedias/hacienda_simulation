extends Node2D


#TODO: Hacer que la caja de selecion selecione algo.
# No se si Godot 4.4.1 tenga algun bug con los Raycast.
#error para recordar na mas:
#Space state is inaccessible right now,wait for iteration or physics process notification


@export var group : StringName
@export_flags_3d_physics var mask
@export var line_color : Color
@export var background_color : Color
@export var world : Node3D
var camera: Camera3D


var is_selecting : bool = false
var selection_start : Vector2 = Vector2.ZERO
var selection_rect : Rect2 = Rect2()

func _ready() -> void:
	camera = get_viewport().get_camera_3d()



func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.is_pressed():
				is_selecting = true
				selection_start = get_global_mouse_position()
				selection_rect.position = selection_start
				selection_rect.size = Vector2.ZERO
				_select_units()
			else:
				if is_selecting:
					is_selecting = false
					selection_rect = Rect2()
		elif event.button_index == MOUSE_BUTTON_RIGHT:
			queue_redraw()
			_clear_selection()
			selection_rect = Rect2()
			pass
	elif event is InputEventMouseMotion:
		if is_selecting:
			if selection_rect.size.length() > 32:
				queue_redraw()
			else:
				selection_rect = Rect2()
				queue_redraw()
	else:
		pass
	

func _process(_delta: float) -> void:
	if is_selecting:
		var current_mouse_position = get_global_mouse_position()
		selection_rect = Rect2(selection_start,current_mouse_position - selection_start).abs()
		queue_redraw()
	else:
		selection_rect = Rect2(Vector2.ZERO,Vector2.ZERO)
		queue_redraw()


func _draw() -> void:
	draw_rect(selection_rect,line_color,false,1.5)
	draw_rect(selection_rect,background_color)
	



func _select_units() -> void:
	
	pass

func _clear_selection() -> void:
	pass
