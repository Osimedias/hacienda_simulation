class_name Player extends Node

@export var player_name : String

@export var current_worker_population : int
@export var current_houseing_space : int

@export var current_workers_strikes : int
@export var population_happiness : int
@export var money : int = 30
@export_range(0,100,1,"or_greater") var happines : int = 100
@export var land_own_by_you : int = 1


@warning_ignore("unused_signal")
signal is_not_food_in_the_hacienda
@warning_ignore("unused_signal")
signal is_not_money_in_the_hacienda
@warning_ignore("unused_signal")
signal peapole_is_leaving_the_hacienda
@warning_ignore("unused_signal")
signal happines_is_low
@warning_ignore("unused_signal")
signal is_not_goods_in_the_storehouse(goods : Goods)

@warning_ignore("unused_parameter")
func _process(_delta: float) -> void:
	happines = clampi(happines,0,100)
	current_houseing_space = clampi(current_houseing_space,0,300)
	current_worker_population = clampi(current_worker_population,0,current_houseing_space)
	money = clampi(money,0,15000000)
	if current_workers_strikes > 20 and land_own_by_you == 1:
		print("You lose the game xdxdxd and the workers are playing whit your head :D")
	
