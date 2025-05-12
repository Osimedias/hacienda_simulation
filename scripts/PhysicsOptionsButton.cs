using Godot;
using Godot.Collections;

namespace Trinketos.HaciendaSimulator
{
    public partial class PhysicsOptionsButton : OptionButton
    {
        [Export(PropertyHint.Enum, "GodotPhysics3D,JoltPhysics,Dummy")]
        int currentPhysicsEngine = 0;


        public override void _Ready()
        {
            base._Ready();
            AddItem("Godot Physics 3D", 0);
            AddItem("Jolt Physics", 1);
            AddItem("Disambled", 2);
        }


        void OnItemSelected(int index)
        {
            Array<string> values = ["GodotPhysics3D", "JoltPhysics", "Dummy"];
            ProjectSettings.SetSetting("physics/3d/physics_engine", values[index]);
        }
    }
}
