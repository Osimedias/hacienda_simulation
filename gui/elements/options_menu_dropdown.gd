extends HBoxContainer

## This is the text of the label
@export var list_name : String
## Type of Graphics List, example: if is Graphics Quality will apper Low,Medium,High,Extrem,Ultra and 
## if is Display Mode will be fullscreen,borderless etc
@export_enum(
	"Display Mode","Resolution","Graphics Quality","Shaders","Lighting","Shadows","Terrain","Reflections","Effects",
	"Texture Quality","Post-Processing","Physics","Models","Indirect Shadows"
	) var type : int = 0

@onready var label: Label = $Label
@onready var options_list: OptionButton = $OptionsList



func _ready() -> void:
	label.text = list_name
	match type:
		0:
			display_create_elements(options_list)
			
		1:
			resolution_create_elements(options_list)
			pass
		2:
			generic_level_of_detaly_create_elements(options_list)
		3:
			generic_level_of_detaly_create_elements(options_list)
		4:
			generic_level_of_detaly_create_elements(options_list)
		5:
			generic_level_of_detaly_create_elements(options_list)
		6:
			generic_level_of_detaly_create_elements(options_list)
		7:
			generic_level_of_detaly_create_elements(options_list)
		8:
			generic_level_of_detaly_create_elements(options_list)
		9:
			generic_level_of_detaly_create_elements(options_list)
		10:
			generic_level_of_detaly_create_elements(options_list)
		11:
			physics_create_element(options_list)
		12:
			generic_level_of_detaly_create_elements(options_list)
		13:
			generic_level_of_detaly_create_elements(options_list)




func _on_options_list_item_selected(index: int) -> void:
	match type:
		0:
			GraphicsManager.get_graphics_mode_change(index)
			pass
		1:
			GraphicsManager.get_resolution_from_list(index)
	pass # Replace with function body.

func display_create_elements(list : OptionButton) -> void:
	list.add_item("Windowed",0)
	list.add_item("Fullscreen",1)
	list.add_item("Borderless",2)
	list.selected = 1
	pass

func resolution_create_elements(list : OptionButton) -> void:
	var index : int
	index = clampi(index,0,GraphicsManager.resolutions.size())
	for r in GraphicsManager.resolutions:
		list.add_item("%sx%s" % [int(r.x),int(r.y)],index)
		index+=1


func generic_level_of_detaly_create_elements(list: OptionButton) -> void:
	list.add_item("Low",0)
	list.add_item("Meddium",1)
	list.add_item("High",2)
	list.add_item("Ultra",3)
	list.add_item("Extreme",4)
	list.selected = 4
	pass


func physics_create_element(list : OptionButton) -> void:
	list.add_item("Simple",0)
	list.add_item("Godot Physics",1)
	list.add_item("Jolt Physics",2)
	list.selected = 1
	pass
