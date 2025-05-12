using Godot;


namespace Trinketos.HaciendaSimulator
{
    public partial class Terrain : StaticBody3D
    {
        [Export]
        MeshInstance3D terrainMesh;
        [Export]
        CollisionShape3D terrainShape;

        public Texture2D splatMapTexture;
        public Texture2D heightMapTexture;
        public Texture2D colorMapTexture;
        public Texture2D forestmapDensity;
        private HeightMapShape3D _heightMapShape;

        // Subdivision need to be equal to the heightmapShape cells
        private PlaneMesh _plane;

        // Splatmap Image
        private Image _splmp_tex;
        // Heightmap Image
        private Image _hgmp_tex;
        // Colormap Image
        private Image _clrmp_tex;

        private Image _frsmp_tex;

        private Godot.Collections.Dictionary<Vector2I, float> heightCache = new Godot.Collections.Dictionary<Vector2I, float>(); // Buffer de alturas

        [Signal]
        public delegate void TerrainGenerateFinishEventHandler();

        public override void _Ready()
        {
            base._Ready();
            MapData mapData = GetNode<MapData>("/root/MapData");
            splatMapTexture = mapData.splatmap;
            heightMapTexture = mapData.heightmap;
            forestmapDensity = mapData.treeDistMask;
            GenerateTerrain();
        }


        public void GenerateTerrain()
        {
            GenerateMesh();
            GenerateHeightMapShape();
            //PlaceObjectBaseInGreenChannel("res://scenes/flora/tree.tscn",0.5f,0.3f);
            EmitSignal(SignalName.TerrainGenerateFinish);
        }

        public void GenerateMesh()
        {
            Image image = heightMapTexture.GetImage();
            int width = image.GetWidth();
            int height = image.GetHeight();
            GD.Print($"Terrain Size is ({width},{height})");
            _plane = new PlaneMesh();
            _plane.Size = new Vector2(width, height);

            // This will break??
            _plane.SubdivideWidth = width / 2;
            _plane.SubdivideDepth = height / 2;
            _plane.SubdivideWidth = width;
            _plane.SubdivideDepth = height;
            terrainMesh.Mesh = _plane;
            ShaderMaterial material = terrainMesh.MaterialOverride as ShaderMaterial;
            material.SetShaderParameter("heightmap", heightMapTexture);
            material.SetShaderParameter("splatmap", splatMapTexture);
        }

        public void GenerateHeightMapShape()
        {
            _hgmp_tex = heightMapTexture.GetImage();
            int width = _hgmp_tex.GetWidth();
            int height = _hgmp_tex.GetHeight();
            _hgmp_tex.Resize(width, height, Image.Interpolation.Trilinear);

            if(_hgmp_tex.GetFormat() != Image.Format.Rf)
            {
                _hgmp_tex.Convert(Image.Format.Rf);
            }
            _heightMapShape = new HeightMapShape3D();
            _heightMapShape.UpdateMapDataFromImage(_hgmp_tex, 0, 60);
            terrainShape.Shape = _heightMapShape;
        }

        // Not Working now :(
        public void PlaceObjectBaseInGreenChannel(string scene, float threshold, float frequency)
        {
            _frsmp_tex = forestmapDensity.GetImage();
            int width = _frsmp_tex.GetWidth();
            int height = _frsmp_tex.GetHeight();

            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    float green = _frsmp_tex.GetPixel(x,z).G;

                    if(green > threshold)
                    {
                       if(rng.Randf() < frequency)
                       {
                            InstanceObjectAtPosition(x,z,scene);
                       }
                    }
                }
            }
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
            //GetParent().CallDeferred("add_child",newInstance);
        }

        public float GetHeightAt(float x, float y, float maxHeight)
        {
            if(_hgmp_tex == null)
                _hgmp_tex = heightMapTexture.GetImage();
            int width = _hgmp_tex.GetWidth();
            int height = _hgmp_tex.GetHeight();
            Vector2I pos = new Vector2I(Mathf.FloorToInt(x), Mathf.FloorToInt(y));

            if (!heightCache.ContainsKey(pos))
            {
                pos.X = Mathf.Clamp(pos.X, 0, width - 2);
                pos.Y = Mathf.Clamp(pos.Y, 0, height - 2);

                Color c00 = _hgmp_tex.GetPixel(pos.X, pos.Y);
                heightCache[pos] = c00.R * maxHeight;
            }

            return heightCache[pos];
        }
    }
}
