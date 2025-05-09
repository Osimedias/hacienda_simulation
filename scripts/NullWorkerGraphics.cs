using Godot;
/*
    file: NullWorkerGraphics.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:57 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This is the main class of all buildings in the game.
*/

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
