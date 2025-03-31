extends Area3D


## Using math or something i will dived this crap :D


## Zero is the "Mother Nature"
@export var own_by_id : int = 0

## Player color use as a 'background color', i will use a 'Decal' so your grapics card crash :D
@export var draw_player_color : Color = Color.ALICE_BLUE

@onready var decal_texture: SubViewport = $BackgroundColor/DecalTexture
@onready var color: ColorRect = $BackgroundColor/DecalTexture/Color


func _ready() -> void:
	$BackgroundColor.size = $CollisionShape3D.shape.size
	await  RenderingServer.frame_post_draw
	var texture : ViewportTexture = decal_texture.get_texture()
	$BackgroundColor.texture_albedo = texture
	$BackgroundColor.texture_emission = texture
	color.color = draw_player_color


func _process(_delta: float) -> void:
	color.color = draw_player_color
