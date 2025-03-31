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

@export var props_meshes : Array[Mesh]

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
	#generate_objects(transform.origin,0.09,props_meshes,"rocks",4,8)


func _on_terrain_change() -> void:
	_generate_terrain()




# range_start/range_end are for perlin noise ranges from -1 to 1 for where to place objects
func generate_objects(player_position:Vector3, density: float, meshes: Array[Mesh], object_class_name: String, range_start:float, range_end:float):
	# Calculate player chunk position
	var player_chunk_x = int(player_position.x / terrain_mesh.get_aabb().size.x)
	var player_chunk_z = int(player_position.z / terrain_mesh.get_aabb().size.x)
	
	# Chunk based vegetation placement - vegetation node is child of chunk
	for x in range(player_chunk_x - 4, player_chunk_x + 4 + 1):
		for z in range(player_chunk_z - 4, player_chunk_z + 4 + 1):
			#if not loaded_chunks[str(x) + "," + str(z)].has_node(object_class_name):
			place_objects(x, z, density, meshes, object_class_name, range_start, range_end)

# Logic to displace vertices of chunks (called by TerrainChunk.create_chunk())
func _process_chunk_vertices(vertex: Vector3, chunk_position: Vector3) -> Vector3:
	var noise_value = texture.texture.noise.get_noise_3d(vertex.x + position.x * terrain_mesh.get_aabb().size.x, 0, vertex.z + position.z * terrain_mesh.get_aabb().size.z)
	var vertex_global_position = vertex + position * terrain_mesh.get_aabb().size

	# Apply combined falloff to noise
	return Vector3(
		vertex.x,
		(noise_value * noise_value if noise_value >= 0 else -noise_value * noise_value) * 128,
		vertex.z
	)

func place_objects(chunk_space_x, chunk_space_z, density:float, meshes: Array[Mesh], object_class_name: String, range_start:float, range_end:float):
	# Calculate the density grid size based on the density variable
	var density_grid_size = int(round(terrain_mesh.get_aabb().size.x * density))
	
	# Create a MultiMeshInstance3D for rocks in the chunk
	var rock_multimesh = MultiMeshInstance3D.new()
	rock_multimesh.name = object_class_name
	var multimesh = MultiMesh.new()
	rock_multimesh.multimesh = multimesh
	multimesh.transform_format = MultiMesh.TRANSFORM_3D
	multimesh.mesh = meshes.pick_random()
	
	var instance_transforms = []
	
	# Loop over the density grid and place rocks
	for x in range(density_grid_size):
		for z in range(density_grid_size):
			# Calculate the position in the original chunk grid
			var original_x = int(round(x / density))
			var original_z = int(round(z / density))
			
			var rock_position := _process_chunk_vertices(
				Vector3(original_x, 0, original_z),
				Vector3(chunk_space_x, 0, chunk_space_z)
			)
			if rock_position.y >= range_start and rock_position.y <= range_end: # CUSTOM RANGES
				instance_transforms.append(
					Transform3D(
						Basis().rotated(Vector3(randf_range(0, PI/3), randf_range(0, 2 * PI), randf_range(0, PI/3)).normalized(), randf_range(0, PI*2)).scaled(Vector3(1.,1.,1.)), # tmp rotated
						rock_position
					)
				)
	
	multimesh.instance_count = instance_transforms.size()
	for i in range(instance_transforms.size()):
		multimesh.set_instance_transform(i, instance_transforms[i])
	
	# Add the rock multimesh as a child of the chunk
	add_child(rock_multimesh)
