using Godot;
using System;
using Trinketos.HaciendaSimulator;

public partial class VoxelGiGenerator : VoxelGI
{
	[Export]
	MeshInstance3D terrain;



	void OnTerrainGenerationFinish()
	{
		Size = terrain.GetAabb().Size;
		Bake();
	}
}
