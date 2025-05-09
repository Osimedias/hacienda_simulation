using Godot;

/*
    file: SessionGui.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:18 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Manager the Control by context.
*/
public partial class SessionGui : Control
{
    public string currentContext;

    [Signal]
    public delegate void ChangeContextEventHandler(string context);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
