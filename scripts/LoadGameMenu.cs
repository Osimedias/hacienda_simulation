using Godot;

/*
    file: LoadGameMenu.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:35 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Loads save games from a list
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class LoadGameMenu : Control
    {
        void OnLoadPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/world.tscn");
        }
        void OnBackPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
        }
    }
}
