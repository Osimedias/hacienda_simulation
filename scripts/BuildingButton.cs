using Godot;

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
