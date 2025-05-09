using Godot;

/*
    file: Goods.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:59 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    All Goods of the game example: food, wood, etc.
*/

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Goods : Resource
    {
        [Export]
        public Texture2D Icon;
        [Export]
        public StringName Name { get; set; }
        [Export(PropertyHint.MultilineText)]
        public string Description;
        [Export]
        public int Amount { get; set; }
        [Export]
        public int CellGUIPosition { get; set; }


        public Goods()
        {
            Name = "Goods";
            Description = "Put a description here";
            Amount = 0;
            CellGUIPosition = 0;
        }
    }
}
