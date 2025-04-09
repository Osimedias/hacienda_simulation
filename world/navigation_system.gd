extends Node3D
class_name NavigationSystem


const GRID_SIZE = 256
const CELL_SIZE = 2


var astar : AStar3D
var astar_capacity : int = 0  #Total number of points in the graph, usefull for size

func _ready() -> void:
	add_to_group("navigation_system")# optional
	astar = AStar3D.new()
	create_grid()


func create_grid() -> void:
	var total_points : int = (2 * GRID_SIZE + 1) * (2 * GRID_SIZE + 1)
	astar.reserve_space(total_points)
	
	var point_id = 0
	
	for x in range(-GRID_SIZE, GRID_SIZE + 1):
		for z in range(-GRID_SIZE,GRID_SIZE + 1):
			var point_position : Vector3 = Vector3(x * CELL_SIZE,0,z * CELL_SIZE)
			astar.add_point(point_id,point_position)
			point_id += 1
	
	point_id = 0
	for x in range(-GRID_SIZE, GRID_SIZE + 1):
		for z in range(-GRID_SIZE, GRID_SIZE + 1):
			if x < GRID_SIZE:
				astar.connect_points(point_id, point_id + 2 * GRID_SIZE + 1)
			if z < GRID_SIZE:
				astar.connect_points(point_id,point_id + 1)
			point_id += 1

func get_shortest_path(from_position : Vector3, to_position : Vector3) -> PackedVector3Array:
	var from_id = astar.get_closest_point(from_position)
	var to_id = astar.get_closest_point(to_position)
	
	if from_id == -1 or to_id == -1:
		print_debug("Error: Start or end position is out of bounds.")
		return []
	
	var path_ids = astar.get_id_path(from_id,to_id)
	
	if path_ids.is_empty():
		print_debug("Error: No path found between points.")
		return []
	
	var path_points : PackedVector3Array = []
	
	for id in path_ids:
		path_points.append(astar.get_point_position(id))
	return simplify_path(path_points, 8.0)




func simplify_path(path: PackedVector3Array, tolerance: float) -> PackedVector3Array:
	
	if path.size() < 3:
		return path
	
	var result : PackedVector3Array = []
	var stack = []
	stack.append(Vector2(0,path.size() - 1))
	
	var markers : PackedInt32Array = PackedInt32Array()
	markers.resize(path.size())
	markers[0] = 1
	markers[path.size() - 1] = 1
	
	while stack.size() > 0:
		var indices = stack.pop_back()
		var start_index = indices.x
		var end_index = indices.y
		
		var max_distance = 0
		var farthest_index = -1
		
		for i in range(start_index + 1, end_index):
			var distance = point_line_distance(path[i],path[start_index],path[end_index])
			if distance > max_distance:
				max_distance = distance
				farthest_index = i
		
		if max_distance > tolerance:
			markers[farthest_index] = 1
			stack.append(Vector2(start_index, farthest_index))
			stack.append(Vector2(farthest_index, end_index))
	for i in range(path.size()):
		if markers[i] == 1:
			result.append(path[i])
	return result



func point_line_distance(point: Vector3, line_start: Vector3, line_end: Vector3) -> float:
	var line = line_end - line_start
	var length_squared = line.length_squared()
	
	if length_squared == 0:
		return (point - line_start).length()
		
	var t = ((point - line_start).dot(line)) / length_squared
	t = clamp(t, 0.0, 1.0)
	var projection = line_start + t * line
	return (point - projection).length()
