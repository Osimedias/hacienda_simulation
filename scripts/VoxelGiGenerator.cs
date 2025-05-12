using Godot;
using System;
using Trinketos.HaciendaSimulator;

public partial class VoxelGiGenerator : VoxelGI
{



    void OnTerrainGenerationFinish()
    {
        MeshInstance3D terrain = GetParent().GetNode<Terrain>("Terrain").GetChild<MeshInstance3D>(0);
        Size = terrain.GetAabb().Size;
        Bake();
    }
}
