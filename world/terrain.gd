@tool
class_name Terrain extends StaticBody3D

@export_tool_button("Generate") var generate_terrain = _generate_terrain
@export var heightmap : ViewportTexture:
	set(value):
		heightmap = value
		terrain_change.emit()

@export var y_ratio : float = 60.0 :
	set(value):
		y_ratio = value
		_generate_terrain()

@export var navigation : NavigationRegion3D

@onready var terrain_mesh: MeshInstance3D = $TerrainMesh
@onready var material: ShaderMaterial = $TerrainMesh.material_override
@onready var collision_shape_3d: CollisionShape3D = $CollisionShape3D
@onready var heightmap_shape: HeightMapShape3D = $CollisionShape3D.shape
@onready var heightmap_viewport: SubViewport = $Heightmap
@onready var splatmap_viewport: SubViewport = $Splatmap
@onready var normalmap_viewport: SubViewport = $Normalmap
@onready var texture: TextureRect = $Heightmap/texture


signal terrain_change

func _ready() -> void:
	pass
	#_generate_terrain()


func _generate_terrain() -> void:
	await RenderingServer.frame_post_draw
	var image : Image = texture.get_texture().get_image()
	image.convert(Image.FORMAT_RF)
	image.resize(terrain_mesh.mesh.size.x,terrain_mesh.mesh.size.y)
	material.set_shader_parameter("set_shader_parameter/height_scale",y_ratio)
	terrain_mesh.material_override = material
	heightmap_shape.update_map_data_from_image(image,0.0,y_ratio)
	collision_shape_3d.shape = heightmap_shape
	navigation.bake_navigation_mesh(true)


func _on_terrain_change() -> void:
	_generate_terrain()
