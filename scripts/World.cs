using Godot;
using System;

/*
    file: World.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:25 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Main Scene for the Session.
*/
public partial class World : Node3D
{
	[Export]
	SessionGui sessionGUI;
	
	void OnGUIChangeContext(string context)
	{
		sessionGUI.currentContext = context;
	}
}
