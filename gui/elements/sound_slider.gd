extends HSlider

@export_enum("Master","Music","SFX") var audio_bus : String = "Master"



var bus_index : int

func _ready() -> void:
	bus_index = AudioServer.get_bus_index(audio_bus)
	print(audio_bus +": " + str(db_to_linear(AudioServer.get_bus_volume_db(bus_index))))
	value_changed.connect(_on_value_changed)
	value = db_to_linear(AudioServer.get_bus_volume_db(bus_index))


func _on_value_changed(v: float) -> void:
	AudioServer.set_bus_volume_db(bus_index,linear_to_db(v))
