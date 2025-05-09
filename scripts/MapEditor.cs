using System.Collections;
using Godot;

namespace Trinketos.HaciendaSimulator
{


    public partial class MapEditor : Node3D
    {
        [Export]
        Vector2I mapSize = new Vector2I(255, 255);

        [Export]
        Terrain terrain;
        [Export]
        Water water;
        // Called when the node enters the scene tree for the first time.

        Texture2D heightmap;
        Texture2D splatmap;
        Texture2D forestMask;
        Texture2D waterMask;

        Vector2 mousePosition;
        Vector3 cursorPosition;

        bool IsBrushMode = false;

        enum Brushes
        {
            Raise, Lower, Smooth, PaintColor
        }
        enum PaintColors
        {
            Red, Green, Blue, Alpha
        }

        Brushes brushes = Brushes.Raise;
        PaintColors paintColors = PaintColors.Green;

        int brushIntensity = 5;
        int brushSize = 32;

        public override void _Ready()
        {
            base._Ready();
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            Vector3 from = GetViewport().GetCamera3D().ProjectRayOrigin(mousePosition);
            Vector3 to = from + GetViewport().GetCamera3D().ProjectLocalRayNormal(mousePosition) * 10000;
            PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to, terrain.CollisionMask);
            PhysicsDirectSpaceState3D space = GetWorld3D().DirectSpaceState;
            var result = space.IntersectRay(query);

            if (result.ContainsKey("position") && IsBrushMode)
            {
                cursorPosition = (Vector3)result["position"];
            }
        }


        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);
            if (@event is InputEventMouseButton e)
            {
                if (e.IsPressed() && e.ButtonIndex == MouseButton.Left && IsBrushMode)
                {
                    mousePosition = e.Position;
                    switch (brushes)
                    {
                        case Brushes.Raise:
                            TerrainPainter.UpdateTerrainHeightmap(terrain.GetChild<MeshInstance3D>(0), terrain.GetChild<CollisionShape3D>(1).Shape as HeightMapShape3D, terrain.heightMapTexture, cursorPosition, brushIntensity, brushSize, TerrainPainter.BrushType.Raise);
                            break;
                        case Brushes.Lower:
                            TerrainPainter.UpdateTerrainHeightmap(terrain.GetChild<MeshInstance3D>(0), terrain.GetChild<CollisionShape3D>(1).Shape as HeightMapShape3D, terrain.heightMapTexture, cursorPosition, brushIntensity, brushSize, TerrainPainter.BrushType.Lower);
                            break;
                        case Brushes.Smooth:
                            TerrainPainter.UpdateTerrainHeightmap(terrain.GetChild<MeshInstance3D>(0), terrain.GetChild<CollisionShape3D>(1).Shape as HeightMapShape3D, terrain.heightMapTexture, cursorPosition, brushIntensity, brushSize, TerrainPainter.BrushType.Smooth);
                            break;
                        case Brushes.PaintColor:
                            switch (paintColors)
                            {
                                case PaintColors.Red:
                                    TerrainPainter.UpdateTerrainSplatmap(terrain.GetChild<MeshInstance3D>(0), terrain.splatMapTexture, cursorPosition, Colors.Red, brushSize, TerrainPainter.BrushType.Raise);
                                    break;
                                case PaintColors.Green:
                                    TerrainPainter.UpdateTerrainSplatmap(terrain.GetChild<MeshInstance3D>(0), terrain.splatMapTexture, cursorPosition, Colors.Green, brushSize, TerrainPainter.BrushType.Raise);
                                    break;
                                case PaintColors.Blue:
                                    TerrainPainter.UpdateTerrainSplatmap(terrain.GetChild<MeshInstance3D>(0), terrain.splatMapTexture, cursorPosition, Colors.Blue, brushSize, TerrainPainter.BrushType.Raise);
                                    break;
                                case PaintColors.Alpha:
                                    TerrainPainter.UpdateTerrainSplatmap(terrain.GetChild<MeshInstance3D>(0), terrain.splatMapTexture, cursorPosition, new Color(0.0f, 0.0f, 0.0f, 0.0f), brushSize, TerrainPainter.BrushType.Raise);
                                    break;
                                default:
                                    TerrainPainter.UpdateTerrainSplatmap(terrain.GetChild<MeshInstance3D>(0), terrain.splatMapTexture, cursorPosition, Colors.Black, 15, TerrainPainter.BrushType.Raise);
                                    break;
                            }
                            break;
                        default:
                            break;
                    }

                }
                else if (e.IsPressed() && e.ButtonIndex == MouseButton.Right && IsBrushMode)
                {
                    IsBrushMode = false;
                }
            }
        }
        void OnRaisePressed()
        {
            brushes = Brushes.Raise;
            IsBrushMode = true;
        }
        void OnLowerPressed()
        {
            brushes = Brushes.Lower;
            IsBrushMode = true;
        }
        void OnSmoothPressed()
        {
            brushes = Brushes.Smooth;
            IsBrushMode = true;
        }
        void OnPaintPressed()
        {
            brushes = Brushes.PaintColor;
            IsBrushMode = true;
        }
    }


}



