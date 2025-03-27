@tool
class_name MiniMapRect extends Node2D

@export var terrain_splatmap_texture : Texture2D :
	set(value):
		terrain_splatmap_texture = value
		queue_redraw()
	get:
		return terrain_splatmap_texture

@export var rect : Rect2 : 
	set(value):
		rect = value
		queue_redraw()
	get:
		return rect

@export var line_width : float = 1.0 :
	set(value):
		line_width = value
		queue_redraw()
	get:
		return line_width

@export var color : Color :
	set(value):
		color = value
		queue_redraw()
	get:
		return color


func _draw() -> void:
	draw_rect(rect,color,false,line_width,false)
	draw_texture_rect(terrain_splatmap_texture,rect,false,Color.WHITE)
