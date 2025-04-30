using Godot;


/*
    file: BuildingButton.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:58 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This code if for spawning a building into the world.
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class BuildingButton : Button
    {
        [Export]
        PackedScene Building;
        [Export]
        Node3D world;



        void OnButtonPressed()
        {
            Node3D building = (Node3D)Building.Instantiate();
            world.AddChild(building);
        }
    }
}
