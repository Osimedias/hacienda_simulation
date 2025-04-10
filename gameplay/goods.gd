class_name Goods extends Resource


@export var name : StringName
@export_multiline var description : String
## Goods value in the market
@export var value : int



func json_to_resource(path : String) -> Goods:
	var goods : Goods = Goods.new()
	var data : Dictionary = {}
	if FileAccess.file_exists(path):
		var file = FileAccess.open(path,FileAccess.READ)
		var parsed = JSON.parse_string(file.get_as_text())
		
		if parsed is Dictionary:
			data = parsed
		else:
			print("Error reading file")
	
	if data.has("name"):
		goods.name = data["name"]
	if data.has("description"):
		goods.description = data["description"]
	if data.has("value"):
		goods.value = data["value"]
	
	return goods
	
