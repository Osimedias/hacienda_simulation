using Godot;
using Godot.Collections;

/*
    file: SelectionBox.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:15 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    SelectionBox, selected units inside a rect2
*/

namespace Trinketos.HaciendaSimulator
{
	public partial class SelectionBox : Node2D
	{
		[Export]
		string GroupSelectName = "Soliders";
		[Export]
		Color Color = Colors.RoyalBlue;

		[Export]
		Node3D World;

		private Camera3D _Camera;


		private bool _IsSelected = false;
		private Vector2 _SelectionStart = Vector2.Zero;
		private Rect2 _SelectionRect = new Rect2();

		private Array<Node3D> _SelectedUnitsCollection;
        public override void _Ready()
        {
            base._Ready();
			_Camera = GetViewport().GetCamera3D();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
			if(_IsSelected)
			{
				Vector2 currentMousePosition = GetGlobalMousePosition();
				_SelectionRect = new Rect2(_SelectionStart,currentMousePosition - _SelectionStart).Abs();
				QueueRedraw();
			}
			else
			{
				_SelectionRect = new Rect2(Vector2.Zero,Vector2.Zero);
				QueueRedraw();
			}
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
			if(@event is InputEventMouseButton e)
			{
				if(e.ButtonIndex == MouseButton.Left)
				{
					if(e.IsPressed())
					{
						_IsSelected = true;
						_SelectionStart = GetGlobalMousePosition();
						_SelectionRect.Position = _SelectionStart;
						_SelectionRect.Size = Vector2.Zero;
						_SelectedUnits();
					}
					else
					{
						if(_IsSelected)
						{
							_IsSelected = false;
							_SelectionRect = new Rect2();
						}
					}
					
				}
				else if (e.ButtonIndex == MouseButton.Right)
				{
					QueueRedraw();
					_SelectedUnitsCollection.Clear();
					_SelectionRect = new Rect2();
				}
			}
			else if (@event is InputEventMouseMotion)
			{
				if(_IsSelected)
				{
					if(_SelectionRect.Size.Length() > 32)
					{
						QueueRedraw();
					}
					else
					{
						_SelectionRect = new Rect2();
						QueueRedraw();
					}
				}
			}
        }



        public override void _Draw()
        {
            base._Draw();
			DrawRect(_SelectionRect,Color,false,1.5f);
        }

		private void _SelectedUnits()
		{
			if(GetTree().GetNodeCountInGroup(GroupSelectName) <= 0)
			{
				GD.Print("There is not selectable units in the scene");
				return;
			}
			Array<Vector2> unitsPositions = [];
			int index = 0;
			// Slow?
			foreach(Node3D unit in GetTree().GetNodesInGroup(GroupSelectName))
			{
				unitsPositions.Add(_Camera.UnprojectPosition(unit.GlobalPosition));
				if(_SelectionRect.HasPoint(unitsPositions[index]))
				{
					_SelectedUnitsCollection.Add(unit);
				}
				index++;
			}
		}

	}
}