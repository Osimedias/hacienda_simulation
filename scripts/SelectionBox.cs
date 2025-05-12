using Godot;
using Godot.Collections;


namespace Trinketos.HaciendaSimulator
{
    public partial class SelectionBox : Node2D
    {
        [Export]
        string GroupSelectName = "Soliders";
        [Export]
        Color Color = Colors.RoyalBlue;
        [Export]
        Color LineColor = Colors.AliceBlue;
        [Export]
        Node3D World;

        private Camera3D _Camera;

        //private float timeElapsed = .5f;


        private bool _IsSelected = false;
        private Vector2 _SelectionStart = Vector2.Zero;
        private Rect2 _SelectionRect = new Rect2();

        private Array<Node3D> _SelectedUnitsCollection = new Array<Node3D>();
        public override void _Ready()
        {
            base._Ready();
            _Camera = GetViewport().GetCamera3D();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (_IsSelected)
            {
                Vector2 currentMousePosition = GetGlobalMousePosition();
                _SelectionRect = new Rect2(_SelectionStart, currentMousePosition - _SelectionStart).Abs();
                QueueRedraw();
            }
            else
            {
                _SelectionRect = new Rect2(Vector2.Zero, Vector2.Zero);
                QueueRedraw();
            }
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if (@event is InputEventMouseButton e)
            {
                if (e.ButtonIndex == MouseButton.Left)
                {
                    if (e.IsPressed())
                    {
                        _IsSelected = true;
                        _SelectionStart = GetGlobalMousePosition();
                        _SelectionRect.Position = _SelectionStart;
                        _SelectionRect.Size = Vector2.Zero;
                        _SelectedUnits();
                    }
                    if(_SelectedUnitsCollection.Count > 0)
                    {
                        Vector3 targetPosition = GetMousePosition();
                        foreach(Peasent unit in _SelectedUnitsCollection)
                        {
                            unit.GetNode<Agent>("Agent").SetMovementTarget(targetPosition);
                        }
                    }
                    else
                    {
                        if (_IsSelected)
                        {
                            _IsSelected = false;
                            _SelectionRect = new Rect2();
                        }
                    }

                }
                else if (e.ButtonIndex == MouseButton.Right)
                {
                    QueueRedraw();
                    if(_SelectedUnitsCollection.Count > 0)
                        _SelectedUnitsCollection.Clear();
                    _SelectionRect = new Rect2();
                }
            }
            else if (@event is InputEventMouseMotion)
            {
                if (_IsSelected)
                {
                    if (_SelectionRect.Size.Length() > 32)
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
            DrawRect(_SelectionRect, LineColor, false, 2.0f);
            DrawRect(_SelectionRect, Color, true, 1.0f);
        }

        private Vector3 GetMousePosition()
        {
            Vector3 from = _Camera.ProjectRayOrigin(GetGlobalMousePosition());
            Vector3 to = from + _Camera.ProjectRayNormal(GetGlobalMousePosition()) * 10000;
            PhysicsDirectSpaceState3D spaceState = _Camera.GetWorld3D().DirectSpaceState;
            PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);
            Dictionary result = spaceState.IntersectRay(query);
            if(result.Count > 0)
            {
                return (Vector3)result["position"];
            }
            return Vector3.Zero;
        }

        private void _SelectedUnits()
        {
            if (GetTree().GetNodeCountInGroup(GroupSelectName) <= 0)
            {
                GD.Print("There is not selectable units in the scene");
                return;
            }
            // Slow?
            foreach (Peasent unit in GetTree().GetNodesInGroup(GroupSelectName))
            {
                Vector2 unitPosition = _Camera.UnprojectPosition(unit.GlobalPosition);
                if (_SelectionRect.HasPoint(unitPosition))
                {
                    _SelectedUnitsCollection.Add(unit);
                }
            }
        }

    }
}
