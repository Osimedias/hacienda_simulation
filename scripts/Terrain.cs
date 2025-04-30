using Godot;
/*
    file: Terrain.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:10 PM 24/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This Script is for generating the terrain and change the control textures.
*/

namespace Trinketos.HaciendaSimulator 
{
    public partial class Terrain : StaticBody3D
    {
        // Note will not be terrains biger that 1024*2.
        [Export(PropertyHint.Range,"64,4096")]
        int terrainSize = 512;
        [Export]
        MeshInstance3D terrainMesh;
        [Export]
        CollisionShape3D terrainShape;
        [Export]

        Texture2D splatMapTexture;
        [Export]
        Texture2D heightMapTexture;
        [Export]
        Texture2D colorMapTexture;


        private HeightMapShape3D _heightMapShape;

        // Subdivision need to be equal to the heightmapShape cells
        private PlaneMesh _plane;

        // Splatmap Image
        private Image _splmp_tex;
        // Heightmap Image
        private Image _hgmp_tex;
        // Colormap Image
        private Image _clrmp_tex;

        [Signal]
        public delegate void TerrainGenerateFinishEventHandler();

        public override void _Ready()
        {
            base._Ready();
        
            GenerateTerrain();
        }


        public void GenerateTerrain()
        {
            GenerateMesh();
            GenerateHeightMapShape();
            EmitSignal(SignalName.TerrainGenerateFinish);
        }

        public void TerrainPainter()
        {
        }

        public void GenerateMesh()
        {
            _plane = new PlaneMesh();
            _plane.Size = new Vector2(terrainSize,terrainSize);
            // This will break??
            _plane.SubdivideWidth = terrainSize;
            _plane.SubdivideDepth = terrainSize;
            terrainMesh.Mesh = _plane;
            ShaderMaterial material = terrainMesh.MaterialOverride as ShaderMaterial;
            material.SetShaderParameter("heightmap",heightMapTexture);
            material.SetShaderParameter("splatmap",splatMapTexture);
        }

        public void GenerateHeightMapShape()
        {
            _hgmp_tex = heightMapTexture.GetImage();
            _hgmp_tex.Resize(terrainSize,terrainSize,Image.Interpolation.Trilinear);
            _hgmp_tex.Convert(Image.Format.Rf);
            _heightMapShape = new HeightMapShape3D();
            _heightMapShape.UpdateMapDataFromImage(_hgmp_tex,0,60);
            terrainShape.Shape = _heightMapShape;
        }
    }
}