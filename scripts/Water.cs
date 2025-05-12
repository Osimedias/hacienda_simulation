using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Water : StaticBody3D
    {
        [Export]
        MeshInstance3D waterMesh;
        [Export]
        CollisionShape3D waterShape;

        private PlaneMesh _plane;
        private HeightMapShape3D _heightMapShape;
        private Image _hgmp_tex;

        MapData mapData;

        public override void _Ready()
        {
            base._Ready();
            mapData = GetNode<MapData>("/root/MapData");
            _hgmp_tex = mapData.watermask.GetImage();
            GenerateWater();
        }
        public void GenerateWater()
        {
            int width = _hgmp_tex.GetWidth();
            int height = _hgmp_tex.GetHeight();
            _plane = new PlaneMesh();
            _plane.Size = new Vector2(width, height);
            _plane.SubdivideWidth = width;
            _plane.SubdivideDepth = height;
            waterMesh.Mesh = _plane;

            ShaderMaterial material = waterMesh.MaterialOverride as ShaderMaterial;
            material.SetShaderParameter("water_mask", mapData.watermask);
            _hgmp_tex = mapData.watermask.GetImage();
            _hgmp_tex.Resize(width, height, Image.Interpolation.Trilinear);
            _hgmp_tex.Convert(Image.Format.Rf);
            _heightMapShape = new HeightMapShape3D();
            _heightMapShape.UpdateMapDataFromImage(_hgmp_tex, 0, 60);
            waterShape.Shape = _heightMapShape;
        }
    }
}
