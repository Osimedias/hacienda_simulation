using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class WallCollider : StaticBody3D
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            CollisionShape3D collider = new CollisionShape3D();
            collider.Shape = new BoxShape3D { Size = new Vector3(2, 4, 2) };
            AddChild(collider);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}
