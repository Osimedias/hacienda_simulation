extends SubViewport

@export var brush_position : Vector2i = Vector2i.ZERO :
	set(value):
		brush_position = value
		render_heightmap()
	get:
		return brush_position

@export var debug_tex : TextureRect
@onready var terrain_mesh: MeshInstance3D = $"../TerrainMesh"
@onready var brush: MeshInstance3D = $Brush


@export var camera : Camera3D

func _ready() -> void:
	render_heightmap()

func render_heightmap() -> void:
	await RenderingServer.frame_post_draw
	var img : Image = get_texture().get_image()
	img.convert(Image.FORMAT_RF)
	var tex = ImageTexture.create_from_image(img)
	debug_tex.texture = tex
	terrain_mesh.material_override.set_shader_parameter("height_map_mask",tex)
