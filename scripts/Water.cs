using Godot;
/*
    file: Water.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:24 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Creates a WaterBody for the Map.
    I use a Heightmap for leveling of the water.
*/

namespace Trinketos.HaciendaSimulator {
    public partial class Water : StaticBody3D
    {
        // Note will not be terrains biger that 1024*2.
        [Export(PropertyHint.Range,"64,4096")]
        int waterSize = 512;
        [Export]
        MeshInstance3D waterMesh;
        [Export]
        CollisionShape3D waterShape;
        [Export]
        Texture2D waterMask;

        private PlaneMesh _plane;
        private HeightMapShape3D _heightMapShape;
        private Image _hgmp_tex;

        public override void _Ready()
        {
            base._Ready();
            GenerateWater();
        }
        public void GenerateWater()
        {
            _plane = new PlaneMesh();
            _plane.Size = new Vector2(waterSize,waterSize);
            _plane.SubdivideWidth = waterSize;
            _plane.SubdivideDepth = waterSize;
            waterMesh.Mesh = _plane;

            ShaderMaterial material = waterMesh.MaterialOverride as ShaderMaterial;
            material.SetShaderParameter("water_mask",waterMask);

            _hgmp_tex = waterMask.GetImage();
            _hgmp_tex.Resize(waterSize,waterSize,Image.Interpolation.Trilinear);
            _hgmp_tex.Convert(Image.Format.Rf);
            _heightMapShape = new HeightMapShape3D();
            _heightMapShape.UpdateMapDataFromImage(_hgmp_tex,0,60);
            waterShape.Shape = _heightMapShape;
        }
    }
}