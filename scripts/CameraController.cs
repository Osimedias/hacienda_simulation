using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class CameraController : Node3D
    {
        [Export(PropertyHint.Range, "10,150,hide_slide")]
        double Speed = 50.0;
        [Export]
        MeshInstance3D meshBounds;

        private const float DragSpeed = 0.01f;
        private float _screenRatio;
        private bool _dragging;
        private bool _draggingLeft;
        private Vector3 _rightVec, _forwardVec;


        private Aabb _bounds;

        public override void _Ready()
        {
            base._Ready();
            Vector2 screenSize = GetViewport().GetVisibleRect().Size;
            _screenRatio = screenSize.Y / screenSize.X;
            _GetMoveVectors();
            _bounds = meshBounds.GetAabb();
            GD.Print(_bounds);
        }

        private void _GetMoveVectors()
        {
            Vector3 offset = GetViewport().GetCamera3D().GlobalPosition - GlobalPosition;
            _rightVec = GetViewport().GetCamera3D().Transform.Basis.X;
            _forwardVec = new Vector3(offset.X, 0, offset.Z).Normalized();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);


            var direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

            GlobalPosition += GlobalPosition with { X = direction.X, Y = 0, Z = direction.Y } * (float)Speed * (float)delta;
            // Mathf.Clamp is not working
            // Mierda larga solo para no escribir lineas y lineas de codigo para esta mamada. No sirve por alguna razon antes si, pinche mierda
            //GlobalPosition = GlobalPosition with { X = Mathf.Clamp(GlobalPosition.X, _bounds.Size.X, _bounds.End.X), Z = Mathf.Clamp(GlobalPosition.Z, _bounds.Size.Z, _bounds.End.Z) };

        }

        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);

            if (@event is InputEventMouseButton e)
            {
                if (e.Pressed)
                {
                    _dragging = true;
                    _draggingLeft = e.ButtonIndex == MouseButton.Left;
                }
                else
                {
                    _dragging = false;
                }
            }
            else if (@event is InputEventMouseMotion m && _dragging)
            {
                if (_draggingLeft)
                {
                    GlobalPosition += _rightVec * -m.Relative.X * DragSpeed + _forwardVec * -m.Relative.Y * DragSpeed / _screenRatio;
                }
                else
                {
                    RotateY(-m.Relative.X * 0.5f * DragSpeed);
                    _GetMoveVectors();
                }

            }
        }

    }
}
