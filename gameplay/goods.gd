class_name Goods extends Resource


@export var name : StringName
@export_multiline var description : String
## Goods value in the market
@export var value : int






func json_to_resource(path : String) -> Goods:
	var goods : Goods
	
	var file = FileAccess.open(path,FileAccess.READ)
	
	return goods
	
