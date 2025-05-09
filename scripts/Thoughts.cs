using Godot;
using System;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Thoughts : Node3D
    {
        [Export]
        string[] ThoughtsList;
    }
}
