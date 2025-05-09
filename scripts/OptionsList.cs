using Godot;
using Godot.Collections;

/*
    file: OptionsList.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:05 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This scripts is for Manager the options of a MenuOptionButton.cs(less code)
*/
namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class OptionsList : Resource
    {
        [Export]
        public Dictionary<string, Variant> Elements;
    }
}
