using Godot;

/*
    file: World.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:25 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Main Scene for the Session.
*/
namespace Trinketos.HaciendaSimulator
{


    public partial class World : Node3D
    {
        [Export]
        SessionGui sessionGUI;
        public override void _Ready()
        {
            base._Ready();

        }
        void OnGUIChangeContext(string context)
        {
            sessionGUI.currentContext = context;
        }
    }
}
