using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class NullWorkerGraphics : Node3D
    {
        public override void _Process(double delta)
        {
            base._Process(delta);
            RotateY(1.0f * (float)delta);
        }
    }
}
