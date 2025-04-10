extends Control

@export var world_node : Node3D

@export var player : Player


@onready var pop_counter : Label = $PopulationCounter/Info
@onready var money_counter : Label = $MoneyCounter/Info

func _ready() -> void:
	population_counter_formating()
	money_counter.text = str(player.money)

# i can use signals or some thing else but i am lasy
func _process(_delta: float) -> void:
	population_counter_formating()
	pass

func population_counter_formating() -> void:
	pop_counter.text = str(player.current_worker_population) + "/" + str(player.current_houseing_space)
	pass

func _on_barracks_pressed() -> void:
	var barracks = preload("res://objects/structures/barracks.tscn").instantiate()
	barracks.name = "barracks"+str(barracks.get_rid().get_id())
	world_node.add_child(barracks)


func _on_tower_pressed() -> void:
	var tower = preload("res://objects/structures/tower_big.tscn").instantiate()
	tower.name = "tower"+str(tower.get_rid().get_id())
	world_node.add_child(tower)


func _on_land_owner_house_pressed() -> void:
	var land_owner_house = preload("res://objects/structures/land_owner_house.tscn").instantiate()
	land_owner_house.name = "land_owner_house_"+str(land_owner_house.get_rid().get_id())
	world_node.add_child(land_owner_house)


func _on_food_storehouse_pressed() -> void:
	var food_storehouse = preload("res://objects/structures/food_storehouse.tscn").instantiate()
	food_storehouse.name = "food_storehouse_"+str(food_storehouse.get_rid().get_id())
	world_node.add_child(food_storehouse)


func _on_agave_plantation_pressed() -> void:
	var agave_plantation = preload("res://objects/structures/agave_plantation.tscn").instantiate()
	agave_plantation.name = "food_storehouse_"+str(agave_plantation.get_rid().get_id())
	world_node.add_child(agave_plantation)
