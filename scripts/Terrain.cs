using Godot;
/*
 *   file: Terrain.cs.
 *   author: Saúl Rodríguez Martínez (Trinketos)
 *   date: 1:10 PM 24/04/25
 *
 *   This code is part of Hacienda Simulation(Shity name xdxd).
 *   So the owner of this code is me Trinketos.
 *
 *   This Script is for generating the terrain and change the control textures.
 */

namespace Trinketos.HaciendaSimulator
{
    public partial class Terrain : StaticBody3D
    {
        // Note will not be terrains biger that 1024*2.
        [Export(PropertyHint.Range, "64,4096")]
        int terrainSize = 512;
        [Export]
        MeshInstance3D terrainMesh;
        [Export]
        CollisionShape3D terrainShape;

        public Texture2D splatMapTexture;
        public Texture2D heightMapTexture;
        public Texture2D colorMapTexture;


        private HeightMapShape3D _heightMapShape;

        // Subdivision need to be equal to the heightmapShape cells
        private PlaneMesh _plane;

        // Splatmap Image
        private Image _splmp_tex;
        // Heightmap Image
        private Image _hgmp_tex;
        // Colormap Image
        private Image _clrmp_tex;

        private Godot.Collections.Dictionary<Vector2I, float> heightCache = new Godot.Collections.Dictionary<Vector2I, float>(); // Buffer de alturas

        [Signal]
        public delegate void TerrainGenerateFinishEventHandler();

        public override void _Ready()
        {
            base._Ready();
            MapData mapData = GetNode<MapData>("/root/MapData");
            splatMapTexture = mapData.splatmap;
            heightMapTexture = mapData.heightmap;
            GenerateTerrain();
        }


        public void GenerateTerrain()
        {
            GenerateMesh();
            GenerateHeightMapShape();
            EmitSignal(SignalName.TerrainGenerateFinish);
        }

        public void GenerateMesh()
        {
            _plane = new PlaneMesh();
            _plane.Size = new Vector2(terrainSize, terrainSize);
            // This will break??
            _plane.SubdivideWidth = Mathf.Clamp(terrainSize / 4,4,128);
            _plane.SubdivideDepth = Mathf.Clamp(terrainSize / 4, 4, 128);
            terrainMesh.Mesh = _plane;
            ShaderMaterial material = terrainMesh.MaterialOverride as ShaderMaterial;
            material.SetShaderParameter("heightmap", heightMapTexture);
            material.SetShaderParameter("splatmap", splatMapTexture);
        }

        public void GenerateHeightMapShape()
        {
            _hgmp_tex = heightMapTexture.GetImage();
            _hgmp_tex.Resize(terrainSize, terrainSize, Image.Interpolation.Trilinear);
            if(_hgmp_tex.GetFormat() != Image.Format.Rf)
            {
                _hgmp_tex.Convert(Image.Format.Rf);
            }
            _heightMapShape = new HeightMapShape3D();
            _heightMapShape.UpdateMapDataFromImage(_hgmp_tex, 0, 60);
            terrainShape.Shape = _heightMapShape;
        }

        public void InstanceObjectAtPosition(float x, float z, string scene)
        {
            PackedScene newScene = GD.Load<PackedScene>(scene);
            if(newScene == null)
            {
                GD.PrintErr($"Error at loading scene: {scene}");
                return;
            }
            Node3D newInstance = newScene.Instantiate() as Node3D;
            if(newInstance == null)
            {
                GD.PrintErr("Error at instantiate object");
                return;
            }
            newInstance.GlobalPosition = new Vector3(x, GetHeightAt(x, z, 60), z);
            GetParent().AddChild(newInstance);
        }

        public float GetHeightAt(float x, float y, float maxHeight)
        {
            Vector2I pos = new Vector2I(Mathf.FloorToInt(x), Mathf.FloorToInt(y));

            if (!heightCache.ContainsKey(pos))
            {
                int width = _hgmp_tex.GetWidth();
                int height = _hgmp_tex.GetHeight();

                pos.X = Mathf.Clamp(pos.X, 0, width - 2);
                pos.Y = Mathf.Clamp(pos.Y, 0, height - 2);

                Color c00 = _hgmp_tex.GetPixel(pos.X, pos.Y);
                heightCache[pos] = c00.R * maxHeight; // Guardar en caché
            }

            return heightCache[pos];
        }
    }
}
