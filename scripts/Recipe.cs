using Godot;
using Godot.Collections;

/*
    file: Recipe.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:57 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Container for making other goods
*/


namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Recipe : Resource
    {
        [Export]
        Array<Goods> Ingredients;
        [Export]
        Goods FinalGood;
        [Export]
        float CraftTime = 1.0f;
    }
}