extends Node

## Scenario data like heightmap texture etc.
@export var scenario_data : ScenarioData
## The terrain in this scene
@export var terrain : Terrain
## Where the entities spawn in the world(not implemented)
@export var entities_root : Node3D
## This need to match whit how the terrain is divided(not implemented)
@export var territories : int = 0

## entities root tree goes top to bottom
var entities_ids : PackedInt32Array
