extends Node



func create_configuration_settings_file() -> void:
	
	if FileAccess.file_exists("user://settings.cfg"):
		var config = ConfigFile.new()
		
		config.set_value("screen","fullscreen",true)
		# zero will be the current screen size
		config.set_value("screen","screen_size",0)
		
		config.set_value("sound","master",100)
		config.set_value("sound","ambient",50)
		config.set_value("sound","gui",50)
		
		
	
	pass
